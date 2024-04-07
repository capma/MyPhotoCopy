namespace PhotoMove
{
    using MetadataExtractor.Formats.Exif;
    using MetadataExtractor;
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Linq;
    using System.Drawing;
    using System.Security.Cryptography;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Diagnostics;
    using PhotoMove.Models;
    //using System.Reflection;

    public partial class frmMain : Form
    {
        CancellationTokenSource cancellationTokenSource = new();
        List<ScanFile> scanFiles = new();
        UserOptions userOptions = new();
        private const string noModelInfo = "No Model Info";

        public frmMain()
        {
            InitializeComponent();
            InitializeOtherComponents();
            InitComboBoxOutputFolderStructure();
            InitComboBoxCopyOrMoveExistFiles();
            InitListViews();
        }

        #region Events
        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            // reset
            grbCopyOrMove.Enabled = false;

            FolderBrowserDialog folderBrowserDialog = new();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                txtPhotoFolder.Text = folderPath;
                PhotoMoveSettings.Default.SourceFolder = folderPath;
                PhotoMoveSettings.Default.Save();

                scanFiles = new();
            }
        }

        private void btnChooseOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                txtDestinationFolder.Text = folderPath;
                PhotoMoveSettings.Default.DestinationFolder = folderPath;
                PhotoMoveSettings.Default.Save();
            }
        }

        private void btnChooseDuplicatesFolderPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                txtDuplicatesFolderPath.Text = folderPath;
                PhotoMoveSettings.Default.DuplicatesFolder = folderPath;
                PhotoMoveSettings.Default.Save();
            }
        }

        private void btnChooseFolderForFilesWithNoExif_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                txtFolderForFilesWithNoExif.Text = folderPath;
                PhotoMoveSettings.Default.FilesWithNoExifDateCreatedFolder = folderPath;
                PhotoMoveSettings.Default.Save();
            }
        }

        private async void btnFindPhotos_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                ResetFindFilesStatistics();

                grbFindingPhottos.Visible = true;
                pgbFindingFiles.Visible = true;

                GetUserOptions();

                await Task.Run(() => DoScanFiles(), cancellationTokenSource.Token);

                grbFindingPhottos.Visible = false;
                pgbFindingFiles.Visible = false;
                grbCopyOrMove.Enabled = true;
            }
            catch (OperationCanceledException)
            {
                //break;
                grbFindingPhottos.Visible = false;
                pgbFindingFiles.Visible = false;
            }
        }

        private async void btnCopyToDestinationFolders_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                grbCancel.Visible = true;
                grbProgress.Visible = true;
                lblCopyingFiles.Text = "0";

                GetUserOptions();

                await Task.Run(() => CopyOrMoveFiles(), cancellationTokenSource.Token);

                grbCancel.Visible = false;
            }
            catch (OperationCanceledException)
            {
                //break;
                grbCancel.Visible = false;
            }
        }

        private void cmbCopyMoveExistedFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            grbDuplicatesFolder.Visible = (cmbCopyMoveExistedFiles.SelectedIndex == 3);
            PhotoMoveSettings.Default.selectedCopyMoveExistedFiles = cmbCopyMoveExistedFiles.SelectedIndex;
            PhotoMoveSettings.Default.Save();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout frmAbout = new frmAbout();
            frmAbout.ShowDialog();
        }

        private void btnCancelFindingPhotos_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void cmbOutputFolderStructure_SelectedIndexChanged(object sender, EventArgs e)
        {
            PhotoMoveSettings.Default.selectedOutputFolderStructure = cmbOutputFolderStructure.SelectedIndex;
            PhotoMoveSettings.Default.Save();
        }

        private void btnCancelCopyingOrMoving_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void btnUncheckAllFileTypes_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvFilesWithValidExifDates.CheckedItems)
            {
                lvi.Checked = false;
            }

            foreach (ListViewItem lvi in lvFilesWithoutValidExifDates.CheckedItems)
            {
                lvi.Checked = false;
            }
        }

        private void btnUncheckAllCameraModel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvCameraModelsWithoutValidExifDates.CheckedItems)
            {
                lvi.Checked = false;
            }

            foreach (ListViewItem lvi in lvCameraModelsWithValidExifDates.CheckedItems)
            {
                lvi.Checked = false;
            }
        }

        private void btnShowListScanFiles_Click(object sender, EventArgs e)
        {
            frmListScanFiles childForm = new();
            childForm.ShowData(scanFiles);
            childForm.ShowDialog();
        }

        #endregion

        #region Methods
        private void InitializeOtherComponents()
        {
            grbCancel.Visible = false;
            grbProgress.Visible = false;
            txtPhotoFolder.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.SourceFolder)) ? string.Empty : PhotoMoveSettings.Default.SourceFolder.ToString();
            txtDestinationFolder.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.DestinationFolder)) ? string.Empty : PhotoMoveSettings.Default.DestinationFolder.ToString();
            txtDuplicatesFolderPath.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.DuplicatesFolder)) ? string.Empty : PhotoMoveSettings.Default.DuplicatesFolder.ToString();
            txtFolderForFilesWithNoExif.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.FilesWithNoExifDateCreatedFolder)) ? string.Empty : PhotoMoveSettings.Default.FilesWithNoExifDateCreatedFolder.ToString();
        }

        private void InitComboBoxOutputFolderStructure()
        {
            cmbOutputFolderStructure.Items.Add("YMD (Single Folder)");
            cmbOutputFolderStructure.Items.Add("Yr, Mo, Day");
            cmbOutputFolderStructure.Items.Add("Yr, Mo");

            //cmbOutputFolderStructure.Items.Add("Yr, Mo, Day, Camera Model");
            //cmbOutputFolderStructure.Items.Add("Yr, Mo, Camera Model");
            //cmbOutputFolderStructure.Items.Add("Camera Model, Yr, Mo, Day");
            //cmbOutputFolderStructure.Items.Add("Camera Model, Yr, Mo");
            //cmbOutputFolderStructure.Items.Add("Year Only");
            //cmbOutputFolderStructure.Items.Add("Month Only");
            //cmbOutputFolderStructure.Items.Add("Day Only");
            //cmbOutputFolderStructure.Items.Add("Camera Model, Yr");

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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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

                // new scan file
                ScanFile fileInfo = new ScanFile();

                // read general info
                fileInfo.filePath = file;
                fileInfo.fileName = Path.GetFileName(file);
                fileInfo.fileExtension = Path.GetExtension(file);

                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(file);
                    var Ifd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
                    //var metadataDirectory = directories.OfType<FileMetadataDirectory>().FirstOrDefault();

                    if (Ifd0Directory != null)
                    //if (metadataDirectory != null)
                    //if (HasExifData(fileInfo.filePath))
                    //if (HasExifData(directories))
                    {
                        // This file contains EXIF data
                        fileInfo.isValidExif = true;
                        exifFileCount++;
                        lblContainEXIF.Invoke((MethodInvoker)delegate
                        {
                            lblContainEXIF.Text = exifFileCount.ToString();
                        });

                        // Try to get the "Date Taken" property
                        //var originalDate = Ifd0Directory?.GetDescription(ExifDirectoryBase.TagDateTime);
                        //var originalDate2 = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault()?
                        //                        .GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

                        //var originDate = originalDate ?? originalDate2;
                        //DateTime? takenDate = GetValidDateTime(originDate) ?? null;

                        // read EXIF info
                        ReadExifInfo(directories, fileInfo);
                        //ReadExifInfo(Ifd0Directory, fileInfo);

                        var originDate = fileInfo.ExifSubIfdDirectory_takenDate
                                            ?? fileInfo.ExifIfd0Directory_takenDate
                                            //?? fileInfo.ExifThumbnailDirectory_takenDate
                                            ;
                        DateTime? takenDate = GetValidDateTime(originDate) ?? null;

                        //DateTime? takenDate = GetValidDateTime(fileInfo.ExifIfd0Directory_takenDate) ?? null;

                        if (takenDate != null)
                        {
                            // This photo has a valid date
                            fileInfo.isValidTakenDate = true;
                            validDateCount++;

                            //fileInfo.takenDate = (DateTime)Ifd0Directory?.GetDateTime(ExifDirectoryBase.TagDateTime);
                            //fileInfo.takenDate = (DateTime)directories.OfType<ExifSubIfdDirectory>().FirstOrDefault()?
                            //                        .GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);

                            fileInfo.takenDate = (DateTime)takenDate;

                            //fileInfo.cameraModel = Ifd0Directory?.GetDescription(ExifDirectoryBase.TagModel) ?? string.Empty;
                            //fileInfo.cameraModel = directories.OfType<ExifIfd0Directory>().FirstOrDefault()?
                            //                            .GetDescription(ExifDirectoryBase.TagModel) ?? string.Empty;

                            lblHaveValidDate.Invoke((MethodInvoker)delegate
                            {
                                lblHaveValidDate.Text = validDateCount.ToString();
                            });
                        }
                        else
                        {
                            // This photo has EXIF data but no valid date
                            noValidDateCount++;

                            lblHaveExifButNoValidDate.Invoke((MethodInvoker)delegate
                            {
                                lblHaveExifButNoValidDate.Text = noValidDateCount.ToString();
                            });
                        }

                        lblFindingFilesWithExif.Invoke((MethodInvoker)delegate
                        {
                            lblFindingFilesWithExif.Text = file;
                        });
                    }
                    else
                    {
                        // empty EXIF
                        noValidDateCount++;

                        lblHaveExifButNoValidDate.Invoke((MethodInvoker)delegate
                        {
                            lblHaveExifButNoValidDate.Text = noValidDateCount.ToString();
                        });
                    }
                }
                catch (ImageProcessingException)
                {
                    // This file does not contain EXIF data or could not be processed
                    noValidDateCount++;

                    lblHaveExifButNoValidDate.Invoke((MethodInvoker)delegate
                    {
                        lblHaveExifButNoValidDate.Text = noValidDateCount.ToString();
                    });
                }
                catch (Exception)
                { 
                    // continue
                }
                finally
                {
                    scanFiles.Add(fileInfo);
                }

                lblTotalFiles.Invoke((MethodInvoker)delegate
                {
                    lblTotalFiles.Text = totalCount.ToString();
                });

                // Perform the increment on the ProgressBar.
                pgbFindingFiles.Invoke((MethodInvoker)delegate
                {
                    pgbFindingFiles.PerformStep();
                });

                // Update the status strip
                toolStripStatusLabel1.Text = $"Time: {stopwatch.Elapsed.TotalSeconds} seconds elapsed.";
                statusStrip1.Invoke((MethodInvoker)delegate
                {
                    statusStrip1.Refresh();
                });
            }

            tabFileOptions.Invoke((MethodInvoker)delegate
            {
                tabFileOptions.Visible = true;
            });

            RefreshFileTypesTab();
            RefreshCameraModelsTab();

            stopwatch.Stop();
        }

        

        private void CopyOrMoveFiles()
        {
            int fileCount = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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
                    string newFilePath = Path.Combine(newFolderPath, file.fileName);

                    bool isCopied = false;

                    // check if the copying file exists in the destination folder
                    if (!IsDuplicateFile(newFilePath, file.filePath))
                    {
                        File.Copy(file.filePath, newFilePath);
                        isCopied = true;
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
                                isCopied = true;
                                break;
                            case 2:
                                // Overwrite the Existing File
                                File.Copy(file.filePath, newFilePath, true);
                                isCopied = true;
                                break;
                            case 3:
                                // Move to specified Duplicates Folder:
                                if (!string.IsNullOrEmpty(userOptions.selectedDuplicatesFolder))
                                {
                                    string newNewFolderPath = GenerateFolder(file, userOptions.selectedDuplicatesFolder);

                                    System.IO.Directory.CreateDirectory(newNewFolderPath);
                                    string newNewFilePath = Path.Combine(newNewFolderPath, Path.GetFileName(file.filePath));

                                    File.Copy(file.filePath, Path.Combine(newNewFolderPath, Path.GetFileName(newNewFilePath)));
                                    isCopied = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    if (isCopied)
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

                // Update the status strip
                toolStripStatusLabel1.Text = $"Time: {stopwatch.Elapsed.TotalSeconds} seconds elapsed.";
                statusStrip1.Invoke((MethodInvoker)delegate
                {
                    statusStrip1.Refresh();
                });
            }

            Invoke((MethodInvoker)(() =>
            {
                grbCancel.Visible = false;
                lblCopyingFiles.BackColor = DefaultBackColor;
                lblCopyingProgress.BackColor = DefaultBackColor;
                lblCopyingProgress.Text = "Copy Operation Completed Successfully";
            }));

            stopwatch.Stop();
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

            var invalidFileTypeCounts = scanFiles.Where(x => !x.isValidExif || !x.isValidTakenDate)
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
                    .Where(x => !string.IsNullOrEmpty(x.cameraModel))
                    .GroupBy(file => file.cameraModel)
                    .Select(group => new { CameraModel = group.Key, Count = group.Count() })
                    .OrderBy(x => x.CameraModel);

            int index = 0;
            foreach (var eachCameraModel in validCameraModels.OrderBy(x => x.CameraModel))
            {
                lvCameraModelsWithValidExifDates.Invoke((MethodInvoker)delegate
                {
                    ListViewItem item = new($"{eachCameraModel.CameraModel}", index);
                    item.Checked = true;
                    item.SubItems.Add($"{eachCameraModel.Count}");
                    lvCameraModelsWithValidExifDates.Items.Add(item);
                });

                index++;
            }

            var invalidCameraModels = scanFiles
                    .Where(x => string.IsNullOrEmpty(x.cameraModel))
                    .GroupBy(file => file.cameraModel)
                    .Select(group => new { CameraModel = group.Key, Count = group.Count() })
                    .OrderBy(x => x.CameraModel);

            index = 0;
            foreach (var eachCameraModel in invalidCameraModels)
            {
                lvCameraModelsWithoutValidExifDates.Invoke((MethodInvoker)delegate
                {
                    ListViewItem item = new(noModelInfo, index);
                    item.Checked = true;
                    item.SubItems.Add($"{eachCameraModel.Count}");
                    lvCameraModelsWithoutValidExifDates.Items.Add(item);
                });

                index++;
            }
        }

        private DateTime? GetValidDateTime(string input)
        {
            DateTime temp;
            string[] formats =
            {
                "dd/MM/yyyy HH:mm:ss", "dd-MM-yyyy HH:mm:ss", "dd.MM.yyyy HH:mm:ss", "dd MM yyyy HH:mm:ss", // DMY formats
                "yyyy/MM/dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy.MM.dd HH:mm:ss", "yyyy MM dd HH:mm:ss", // YMD formats
                "MM/dd/yyyy HH:mm:ss", "MM-dd-yyyy HH:mm:ss", "MM.dd.yyyy HH:mm:ss", "MM dd yyyy HH:mm:ss", // MDY formats
                "yyyy:MM:dd HH:mm:ss" // ISO 8601 format
            };

            if (DateTime.TryParseExact(input, formats, null, System.Globalization.DateTimeStyles.None, out temp))
                return temp;
            else
                return null;
        }

        private void ReadExifInfo(ExifIfd0Directory exifIfd0Directory, ScanFile fileInfo)
        {
            if (exifIfd0Directory != null)
            {
                if (exifIfd0Directory.ContainsTag(ExifDirectoryBase.TagDateTime))
                {
                    fileInfo.ExifIfd0Directory_takenDate = exifIfd0Directory?.GetDescription(ExifDirectoryBase.TagDateTime);
                }

                if (exifIfd0Directory.ContainsTag(ExifDirectoryBase.TagModel))
                {
                    fileInfo.ExifIfd0Directory_cameraModel = exifIfd0Directory?.GetDescription(ExifDirectoryBase.TagModel);
                    fileInfo.cameraModel = fileInfo.ExifIfd0Directory_cameraModel;
                }
            }
        }

        private void ReadExifInfo(IReadOnlyList<MetadataExtractor.Directory> directories, ScanFile fileInfo)
        {
            var exifSubIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            if (exifSubIfdDirectory != null)
            {
                if (exifSubIfdDirectory.ContainsTag(ExifDirectoryBase.TagDateTimeOriginal))
                {
                    fileInfo.ExifSubIfdDirectory_takenDate = exifSubIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
                }

                //if (exifSubIfdDirectory.ContainsTag(ExifDirectoryBase.TagModel))
                //{
                //    fileInfo.ExifSubIfdDirectory_cameraModel = exifSubIfdDirectory?.GetDescription(ExifDirectoryBase.TagModel);
                //}

            }

            var exifIfd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
            if (exifIfd0Directory != null)
            {
                if (exifIfd0Directory.ContainsTag(ExifDirectoryBase.TagDateTime))
                {
                    fileInfo.ExifIfd0Directory_takenDate = exifIfd0Directory?.GetDescription(ExifDirectoryBase.TagDateTime);
                }

                if (exifIfd0Directory.ContainsTag(ExifDirectoryBase.TagModel))
                {
                    fileInfo.ExifIfd0Directory_cameraModel = exifIfd0Directory?.GetDescription(ExifDirectoryBase.TagModel);
                    fileInfo.cameraModel = fileInfo.ExifIfd0Directory_cameraModel;
                }
            }

            //var exifThumbnailDirectory = directories.OfType<ExifThumbnailDirectory>().FirstOrDefault();
            //if (exifThumbnailDirectory != null && exifThumbnailDirectory.ContainsTag(ExifDirectoryBase.TagDateTime))
            //{
            //    if (exifThumbnailDirectory.ContainsTag(ExifDirectoryBase.TagDateTime))
            //    {
            //        fileInfo.ExifThumbnailDirectory_takenDate = exifThumbnailDirectory?.GetDescription(ExifDirectoryBase.TagDateTime);
            //    }

            //    if (exifThumbnailDirectory.ContainsTag(ExifDirectoryBase.TagModel))
            //    {
            //        fileInfo.ExifThumbnailDirectory_cameraModel = exifThumbnailDirectory?.GetDescription(ExifDirectoryBase.TagModel);
            //    }
            //}

            //fileInfo.cameraModel = fileInfo.ExifSubIfdDirectory_cameraModel
            //                            ?? fileInfo.ExifIfd0Directory_cameraModel
            //                            ?? fileInfo.ExifThumbnailDirectory_cameraModel
            //                            ;
        }

        private void ResetFindFilesStatistics()
        {
            lblTotalFiles.Text = 0.ToString();
            lblContainEXIF.Text = 0.ToString();
            lblHaveValidDate.Text = 0.ToString();
            lblHaveExifButNoValidDate.Text = 0.ToString();
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

            userOptions.checkedUseDashesInFolderNames = chkUseDashesInFolderName.Checked;
            userOptions.checkedNoSeperator = chkNoSeperator.Checked;
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

        private string GenerateFolder(ScanFile file, string folderPath)
        {
            //string destinationFolderPath = txtDestinationFolder.Text;
            string newFolderPath = string.Empty;
            string year = file.takenDate.Year.ToString();
            string month = file.takenDate.ToString("MM");
            string day = file.takenDate.ToString("dd");

            // Create the YMD (Single Folder) folders
            if (PhotoMoveSettings.Default.selectedOutputFolderStructure == 0)
            {
                newFolderPath = Path.Combine(folderPath, $"{year}{month}{day}");
            }
            // Create the year, month, day folders
            else if (PhotoMoveSettings.Default.selectedOutputFolderStructure == 1)
            {
                newFolderPath = Path.Combine(folderPath, year, $"{year}_{month}", $"{year}_{month}_{day}");
            }
            // Create the year, month folders
            else if (PhotoMoveSettings.Default.selectedOutputFolderStructure == 2)
            {
                newFolderPath = Path.Combine(folderPath, year, $"{year}_{month}");
            }

            return newFolderPath;

        }

        //private bool HasExifData(string filePath)
        private bool HasExifData(IReadOnlyList<MetadataExtractor.Directory> directories)
        {
            //var directories = ImageMetadataReader.ReadMetadata(filePath);

            // Check if any of the directories is an Exif directory
            foreach (var directory in directories)
            {
                if (directory is ExifSubIfdDirectory || directory is ExifIfd0Directory || directory is ExifThumbnailDirectory)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion



    }
}
