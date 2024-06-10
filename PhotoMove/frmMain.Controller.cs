using MetadataExtractor;
using PhotoMove.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Drawing;
using System.Text.Json;
using PhotoMove.Constants;

namespace PhotoMove
{
    public partial class frmMain
    {
        private void InitForm()
        {
            InitTimer();
            InitializeOtherComponents();
            InitComboBoxOutputFolderStructure();
            InitComboBoxCopyOrMoveExistFiles();
            InitListViews();
        }

        private void InitTimer()
        {
            timer1.Interval = 100; // Set timer interval to 1 second
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        private void InitializeOtherComponents()
        {
            grbCancel.Visible = false;
            grbProgress.Visible = false;
            txtPhotoFolder.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.SourceFolder)) ? string.Empty : PhotoMoveSettings.Default.SourceFolder.ToString();
            txtDestinationFolder.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.DestinationFolder)) ? string.Empty : PhotoMoveSettings.Default.DestinationFolder.ToString();
            txtDuplicatesFolderPath.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.DuplicatesFolder)) ? string.Empty : PhotoMoveSettings.Default.DuplicatesFolder.ToString();
            txtFolderForFilesWithNoExif.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.FilesWithNoExifDateCreatedFolder)) ? string.Empty : PhotoMoveSettings.Default.FilesWithNoExifDateCreatedFolder.ToString();

            // Check the radio button based on the stored value
            if (PhotoMoveSettings.Default.selectedSeperatorInFolderName == 0)
            {
                radNoSeperator.Checked = true;
            }
            else if (PhotoMoveSettings.Default.selectedSeperatorInFolderName == 1)
            {
                radUseDashesInFolderName.Checked = true;
            }
            else if (PhotoMoveSettings.Default.selectedSeperatorInFolderName == 2)
            {
                radUseUnderscoreInFolderName.Checked = true;
            }
        }

        private void InitComboBoxOutputFolderStructure()
        {
            cmbOutputFolderStructure.Items.Add("YMD (Single Folder)");
            cmbOutputFolderStructure.Items.Add("Yr, Mo, Day");
            cmbOutputFolderStructure.Items.Add("Yr, Mo");
            cmbOutputFolderStructure.Items.Add("Yr, Mo, Day, Camera Model");
            cmbOutputFolderStructure.Items.Add("Yr, Mo, Camera Model");
            cmbOutputFolderStructure.Items.Add("Camera Model, Yr, Mo, Day");
            cmbOutputFolderStructure.Items.Add("Camera Model, Yr, Mo");
            cmbOutputFolderStructure.Items.Add("Year Only");
            cmbOutputFolderStructure.Items.Add("Month Only");
            cmbOutputFolderStructure.Items.Add("Day Only");
            cmbOutputFolderStructure.Items.Add("Camera Model, Yr");

            // default selected option
            cmbOutputFolderStructure.SelectedIndex = 0;
        }

        private void InitComboBoxCopyOrMoveExistFiles()
        {
            cmbCopyMoveExistedFiles.Items.Add("Do Not Move or Copy");
            cmbCopyMoveExistedFiles.Items.Add("Add '-Copy###'. then Move or Copy");
            cmbCopyMoveExistedFiles.Items.Add("Overwrite the Existing File");
            cmbCopyMoveExistedFiles.Items.Add("Move to specified Duplicates Folder:");

            // default selected option
            cmbCopyMoveExistedFiles.SelectedIndex = 0;
        }

        private void InitListViews()
        {
            lvFilesWithValidExifDates.Columns.Add("File Type", -2, HorizontalAlignment.Left);
            lvFilesWithValidExifDates.Columns.Add("Count", -2, HorizontalAlignment.Left);

            lvFilesWithoutValidExifDates.Columns.Add("File Type", -2, HorizontalAlignment.Left);
            lvFilesWithoutValidExifDates.Columns.Add("Count", -2, HorizontalAlignment.Left);

            lvCameraModelsWithValidExifDates.Columns.Add("Camera Model", -2, HorizontalAlignment.Left);
            lvCameraModelsWithValidExifDates.Columns.Add("Count", -2, HorizontalAlignment.Left);

            lvCameraModelsWithoutValidExifDates.Columns.Add("Camera Model", -2, HorizontalAlignment.Left);
            lvCameraModelsWithoutValidExifDates.Columns.Add("Count", -2, HorizontalAlignment.Left);
        }

        private string GenerateUniqueCopyName(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath) ?? string.Empty;
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            int copyNumber = 1;
            while (File.Exists(Path.Combine(directory, $"{fileName}-Copy{copyNumber}{extension}")))
            {
                copyNumber++;
            }

            return $"{fileName}-Copy{copyNumber}{extension}";
        }

        private bool IsDuplicateFile(string sourceFilePath, string destinationFilePath)
        {
            try
            {
                if (radFileNamesMatch.Checked)
                {
                    // Check if file names match
                    if (Path.GetFileName(sourceFilePath) == Path.GetFileName(destinationFilePath))
                    {
                        return true;
                    }
                }
                else if (radAllExifAndExactFileContentsMatch.Checked)
                {
                    // Check if EXIF data match
                    var directories1 = ImageMetadataReader.ReadMetadata(sourceFilePath);
                    var directories2 = ImageMetadataReader.ReadMetadata(destinationFilePath);

                    if (directories1.ToString() == directories2.ToString())
                    {
                        return true;
                    }

                    // Check if file contents match
                    using (var md5 = MD5.Create())
                    {
                        using (var stream1 = File.OpenRead(sourceFilePath))
                        using (var stream2 = File.OpenRead(destinationFilePath))
                        {
                            var hash1 = md5.ComputeHash(stream1);
                            var hash2 = md5.ComputeHash(stream2);

                            if (BitConverter.ToString(hash1) == BitConverter.ToString(hash2))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void DoScanFiles()
        {
            int totalCount = 0;
            int exifFileCount = 0;
            int validDateCount = 0;
            int noValidDateCount = 0;

            var files = System.IO.Directory.GetFiles(userOptions.selectedFolderWithPhotosToProcess, "*.*", SearchOption.AllDirectories);

            pgbFindingFiles.Invoke((MethodInvoker)delegate
            {
                pgbFindingFiles.Minimum = 1;
                pgbFindingFiles.Maximum = files.Count();
                pgbFindingFiles.Value = 1;
                pgbFindingFiles.Step = 1;
            });

            foreach (var file in files)
            {
                // Check for cancellation
                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                // increase count
                totalCount++;
                lblTotalFiles.Invoke((MethodInvoker)delegate
                {
                    lblTotalFiles.Text = totalCount.ToString();
                });

                // new scan file
                ScanFile fileInfo = new(file);

                lblFindingFilesWithExif.Invoke((MethodInvoker)delegate
                {
                    lblFindingFilesWithExif.Text = file;
                });
                

                try
                {
                    fileInfo.ReadExifData();

                    if (fileInfo.isValidExif)
                    {
                        exifFileCount++;
                        lblContainEXIF.Invoke((MethodInvoker)delegate
                        {
                            lblContainEXIF.Text = exifFileCount.ToString();
                        });
                    }

                    if (fileInfo.isValidExif && fileInfo.isValidTakenDate)
                    {
                        validDateCount++;
                        lblHaveValidDate.Invoke((MethodInvoker)delegate
                        {
                            lblHaveValidDate.Text = validDateCount.ToString();
                        });
                    }

                    if (fileInfo.isValidExif && !fileInfo.isValidTakenDate)
                    {
                        noValidDateCount++;
                        lblHaveExifButNoValidDate.Invoke((MethodInvoker)delegate
                        {
                            lblHaveExifButNoValidDate.Text = noValidDateCount.ToString();
                        });
                    }
                }
                catch (Exception)
                {
                    // silent
                }
                finally
                {
                    scanFiles.Add(fileInfo);
                }

                // Perform the increment on the ProgressBar.
                pgbFindingFiles.Invoke((MethodInvoker)delegate
                {
                    pgbFindingFiles.PerformStep();
                });
            }

            tabFileOptions.Invoke((MethodInvoker)delegate
            {
                tabFileOptions.Visible = true;
            });

            RefreshFileTypesTab();
            RefreshCameraModelsTab();

            // store scan statistics
            scanStatistics.totalCount = totalCount;
            scanStatistics.exifFileCount = exifFileCount;
            scanStatistics.validDateCount = validDateCount;
            scanStatistics.noValidDateCount = noValidDateCount;
        }

        private void CopyOrMoveFiles()
        {
            int fileCount = 0;

            var filteredFiles = scanFiles.Where(x => userOptions.selectedFileTypes.Contains(x?.fileExtension ?? string.Empty)
                                                  && userOptions.selectedCameraModels.Contains(x?.cameraModel ?? string.Empty)
                                                  //&& x.isValidExif
                                                  //&& x.isValidTakenDate
                                                  );

            pgbCopyingOrMovingFiles.Invoke((MethodInvoker)delegate
            {
                pgbCopyingOrMovingFiles.Minimum = 1;
                pgbCopyingOrMovingFiles.Maximum = filteredFiles.Count();
                pgbCopyingOrMovingFiles.Value = 1;
                pgbCopyingOrMovingFiles.Step = 1;
            });

            Invoke((MethodInvoker)(() =>
            {
                grbCancel.Visible = true;
                grbProgress.Visible = true;
                lblCopyingFiles.BackColor = Color.Yellow;
                lblCopyingProgress.BackColor = Color.Yellow;
                lblCopyingProgress.Text = "Files Copied - File Operation currently in progress";
            }));

            foreach (var file in filteredFiles)
            {
                // Check for cancellation
                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                try
                {
                    // This photo has a valid date
                    string newFolderPath = GenerateFolder(file, userOptions.selectedDestinationFolder);
                    System.IO.Directory.CreateDirectory(newFolderPath);

                    // Copy the file to the new directory
                    string newFilePath = file.fileName != null ? Path.Combine(newFolderPath, file.fileName) : newFolderPath;

                    // check if the copying file exists in the destination folder
                    if (!IsDuplicateFile(newFilePath, file.filePath))
                    {
                        File.Copy(file.filePath, newFilePath);
                        file.isCopiedOrMoved = true;
                    }
                    else
                    {
                        switch (PhotoMoveSettings.Default.selectedCopyMoveExistedFiles)
                        {
                            case 0:
                                // Do Not Move or Copy
                                break;
                            case 1:
                                // Add '-Copy###'. then Move or Copy
                                string newFileName = GenerateUniqueCopyName(newFilePath);
                                File.Copy(file.filePath, Path.Combine(Path.GetDirectoryName(newFilePath) ?? string.Empty, newFileName));
                                file.isCopiedOrMoved = true;
                                break;
                            case 2:
                                // Overwrite the Existing File
                                File.Copy(file.filePath, newFilePath, true);
                                file.isCopiedOrMoved = true;
                                break;
                            case 3:
                                // Move to specified Duplicates Folder:
                                if (!string.IsNullOrEmpty(userOptions.selectedDuplicatesFolder))
                                {
                                    string newNewFolderPath = GenerateFolder(file, userOptions.selectedDuplicatesFolder);

                                    System.IO.Directory.CreateDirectory(newNewFolderPath);
                                    string newNewFilePath = Path.Combine(newNewFolderPath, Path.GetFileName(file.filePath));

                                    File.Copy(file.filePath, Path.Combine(newNewFolderPath, Path.GetFileName(newNewFilePath)));
                                    file.isCopiedOrMoved = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    if (file.isCopiedOrMoved)
                    {
                        // Update the TextBox with the number of files copied
                        fileCount++;
                        lblCopyingFiles.Invoke((MethodInvoker)delegate
                        {
                            lblCopyingFiles.Text = fileCount.ToString();
                        });
                    }

                    // Perform the increment on the ProgressBar.
                    pgbCopyingOrMovingFiles.Invoke((MethodInvoker)delegate
                    {
                        pgbCopyingOrMovingFiles.PerformStep();
                    });
                }
                catch (ImageProcessingException)
                {
                    // This file does not contain EXIF data or could not be processed
                }
            }

            Invoke((MethodInvoker)(() =>
            {
                grbCancel.Visible = false;
                lblCopyingFiles.BackColor = DefaultBackColor;
                lblCopyingProgress.BackColor = DefaultBackColor;
                lblCopyingProgress.Text = "Copy Operation Completed Successfully";
            }));
        }

        private void RefreshFileTypesTab()
        {
            var validFileTypeCounts = scanFiles.Where(x => x.isValidExif && x.isValidTakenDate)
                    .GroupBy(file => file.fileExtension)
                    .Select(group => new { FileType = group.Key, Count = group.Count() })
                    .OrderBy(x => x.FileType);

            int index = 0;
            foreach (var fileTypeCount in validFileTypeCounts.OrderBy(x => x.FileType))
            {
                lvFilesWithValidExifDates.Invoke((MethodInvoker)delegate
                {
                    ListViewItem item = new($"{fileTypeCount.FileType}", index);
                    item.Checked = true;
                    item.SubItems.Add($"{fileTypeCount.Count}");
                    lvFilesWithValidExifDates.Items.Add(item);
                });

                index++;
            }

            var invalidFileTypeCounts = scanFiles.Where(x => x.isValidExif && !x.isValidTakenDate)
                    .GroupBy(file => file.fileExtension)
                    .Select(group => new { FileType = group.Key, Count = group.Count() })
                    .OrderBy(x => x.FileType);

            index = 0;
            foreach (var fileTypeCount in invalidFileTypeCounts.OrderBy(x => x.FileType))
            {
                lvFilesWithoutValidExifDates.Invoke((MethodInvoker)delegate
                {
                    ListViewItem item = new($"{fileTypeCount.FileType}", index);
                    item.Checked = true;
                    item.SubItems.Add($"{fileTypeCount.Count}");
                    lvFilesWithoutValidExifDates.Items.Add(item);
                });

                index++;
            }
        }

        private void RefreshCameraModelsTab()
        {
            var validCameraModels = scanFiles
                    .Where(x => x.isValidTakenDate)
                    .GroupBy(file => file.cameraModel)
                    .Select(group => new { CameraModel = group.Key, Count = group.Count() })
                    .OrderBy(x => x.CameraModel);

            int index = 0;
            foreach (var eachCameraModel in validCameraModels.OrderBy(x => x.CameraModel))
            {
                var updatedCameraModel = (string.IsNullOrEmpty(eachCameraModel.CameraModel)) ? Exif.NoModelInfo : eachCameraModel.CameraModel;
                lvCameraModelsWithValidExifDates.Invoke((MethodInvoker)delegate
                {
                    ListViewItem item = new(updatedCameraModel, index);
                    item.Checked = true;
                    item.SubItems.Add($"{eachCameraModel.Count}");
                    lvCameraModelsWithValidExifDates.Items.Add(item);
                });

                index++;
            }

            var invalidCameraModels = scanFiles
                    .Where(x => !x.isValidTakenDate)
                    .GroupBy(file => file.cameraModel)
                    .Select(group => new { CameraModel = group.Key, Count = group.Count() })
                    .OrderBy(x => x.CameraModel);

            index = 0;
            foreach (var eachCameraModel in invalidCameraModels)
            {
                var updatedCameraModel = (string.IsNullOrEmpty(eachCameraModel.CameraModel)) ? Exif.NoModelInfo : eachCameraModel.CameraModel;
                lvCameraModelsWithoutValidExifDates.Invoke((MethodInvoker)delegate
                {
                    ListViewItem item = new(updatedCameraModel, index);
                    item.Checked = true;
                    item.SubItems.Add($"{eachCameraModel.Count}");
                    lvCameraModelsWithoutValidExifDates.Items.Add(item);
                });

                index++;
            }
        }

        private void ResetFindFilesStatistics()
        {
            // clear number of files
            lblTotalFiles.Text = 0.ToString();
            lblContainEXIF.Text = 0.ToString();
            lblHaveValidDate.Text = 0.ToString();
            lblHaveExifButNoValidDate.Text = 0.ToString();

            // clear lists in the tabs
            lvFilesWithValidExifDates.Items.Clear();
            lvFilesWithoutValidExifDates.Items.Clear();
            lvCameraModelsWithValidExifDates.Items.Clear();
            lvCameraModelsWithoutValidExifDates.Items.Clear();
        }

        private void GetUserOptions()
        {
            userOptions.selectedFolderWithPhotosToProcess = txtPhotoFolder.Text;
            userOptions.selectedDestinationFolder = txtDestinationFolder.Text;
            userOptions.selectedHowFolderAreStructured = cmbOutputFolderStructure.SelectedIndex;
            userOptions.selectedHowFilesAreDuplicates = (radFileNamesMatch.Checked) ? 0 : 1;
            userOptions.selectedHowToMoveOrCopyExistedFilesInTheDestinationFolder = cmbCopyMoveExistedFiles.SelectedIndex;
            userOptions.selectedDuplicatesFolder = txtDuplicatesFolderPath.Text;
            userOptions.checkedUseFileDateToMoveOrCopyToStructureFolder = chkUseFileDateToCopyOrMove.Checked;
            userOptions.checkedCopyOrMoveFilesWithNoExifDateCreatedToThisFolder = chkCopyOrMoveToThisFolder.Checked;
            userOptions.selectedFolderForFilesWithNoExifDateCreated = txtFolderForFilesWithNoExif.Text;

            // reset
            userOptions.selectedFileTypes.Clear();
            userOptions.selectedCameraModels.Clear();

            // add
            foreach (ListViewItem item in lvFilesWithValidExifDates.CheckedItems)
            {
                userOptions.selectedFileTypes.Add(item?.SubItems[0].Text ?? string.Empty);
            }

            foreach (ListViewItem item in lvCameraModelsWithValidExifDates.CheckedItems)
            {
                userOptions.selectedCameraModels.Add(item?.SubItems[0].Text ?? string.Empty);
            }

            // remove empty values
            userOptions.selectedFileTypes.RemoveAll(string.IsNullOrEmpty);
            userOptions.selectedCameraModels.RemoveAll(string.IsNullOrEmpty);

            userOptions.checkedUseDashesInFolderNames = radUseDashesInFolderName.Checked;
            userOptions.checkedUseUnderscoresInFolderNames = radUseUnderscoreInFolderName.Checked;
            userOptions.checkedNoSeperator = radNoSeperator.Checked;
        }

        private string GenerateFolder(ScanFile file, string folderPath)
        {
            string newFolderPath = string.Empty;
            string year = file.takenDate.Year.ToString();
            string month = file.takenDate.ToString("MM");
            string day = file.takenDate.ToString("dd");
            string cameraModel = file.cameraModel ?? Exif.NoModelInfo;

            // default seperator
            string seperator = string.Empty;

            // update seperator base on other checked options
            if (userOptions.checkedUseDashesInFolderNames)
            {
                seperator = "-";
            }

            if (userOptions.checkedUseUnderscoresInFolderNames)
            {
                seperator = "_";
            }

            switch (PhotoMoveSettings.Default.selectedOutputFolderStructure)
            {
                case 0:
                    // YMD (Single Folder)
                    newFolderPath = Path.Combine(folderPath, $"{year}{seperator}{month}{seperator}{day}");
                    break;
                case 1:
                    // year, month, day
                    newFolderPath = Path.Combine(folderPath,
                                                    year,
                                                    $"{year}{seperator}{month}",
                                                    $"{year}{seperator}{month}{seperator}{day}"
                                                    );
                    break;
                case 2:
                    // year, month
                    newFolderPath = Path.Combine(folderPath,
                                                    year,
                                                    $"{year}{seperator}{month}"
                                                    );
                    break;
                case 3:
                    // Yr, Mo, Day, Camera Model
                    newFolderPath = Path.Combine(folderPath,
                                                    year,
                                                    $"{year}{seperator}{month}",
                                                    $"{year}{seperator}{month}{seperator}{day}",
                                                    $"{year}{seperator}{month}{seperator}{day}{seperator}{cameraModel}"
                                                    );
                    break;
                case 4:
                    // Yr, Mo, Camera Model
                    newFolderPath = Path.Combine(folderPath,
                                                    year,
                                                    $"{year}{seperator}{month}",
                                                    $"{year}{seperator}{month}{seperator}{cameraModel}"
                                                    );
                    break;
                case 5:
                    // Camera Model, Yr, Mo, Day
                    newFolderPath = Path.Combine(folderPath,
                                                    cameraModel,
                                                    year,
                                                    $"{year}{seperator}{month}",
                                                    $"{year}{seperator}{month}{seperator}{day}"
                                                    );
                    break;
                case 6:
                    // Camera Model, Yr, Mo
                    newFolderPath = Path.Combine(folderPath,
                                                    cameraModel,
                                                    year,
                                                    $"{year}{seperator}{seperator}{seperator}{month}"
                                                    );
                    break;
                case 7:
                    // Year Only
                    newFolderPath = Path.Combine(folderPath, year);
                    break;
                case 8:
                    // Month Only
                    newFolderPath = Path.Combine(folderPath, month);
                    break;
                case 9:
                    // Day Only
                    newFolderPath = Path.Combine(folderPath, day);
                    break;
                case 10:
                    // Camera Model, Yr
                    newFolderPath = Path.Combine(folderPath, cameraModel, year);
                    break;
                default:
                    break;
            }

            return newFolderPath;
        }

        private List<ScanFileReport> FilteredQuery(Func<ScanFile, bool> filter)
        {
            var scanFilesReport = scanFiles
                                    .Where(filter)
                                    .Select((item, index) => new { Index = index + 1, Item = item })
                                    .Select(x => new ScanFileReport
                                    {
                                        Index = x.Index,
                                        File = x.Item.filePath,
                                        Make = x.Item.cameraMake,
                                        Model = string.IsNullOrEmpty(x.Item.cameraModel) ? Exif.NoModelInfo : x.Item.cameraModel,
                                        Date = x.Item.takenDate.ToString("yyyy:MM:dd"),
                                        Time = x.Item.takenDate.ToString("HH:mm:ss")
                                    })
                                    .ToList();

            return scanFilesReport;
        }

        private void DoScanFiles2()
        {
            var files = System.IO.Directory.GetFiles(userOptions.selectedFolderWithPhotosToProcess, "*.*", SearchOption.AllDirectories);
            List<ScanFile> scanFiles = new();

            foreach (var file in files)
            {
                ScanFile scanFile = new(file);

                try
                {
                    scanFile.ReadExifData();
                }
                catch (Exception)
                {

                }
                finally
                {
                    scanFiles.Add(scanFile);
                }
            }

            string json = JsonSerializer.Serialize(scanFiles);
            File.WriteAllText(userOptions.selectedDestinationFolder + @"\scanfiles.json", json);


            // breakpoint here
        }
    }
}
