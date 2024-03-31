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

    public partial class frmMain : Form
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        //CancellationToken cancellationToken;

        public frmMain()
        {
            InitializeComponent();
            InitializeOtherComponents();
            InitComboBoxOutputFolderStructure();
            InitComboBoxCopyOrMoveExistFiles();

            //cancellationTokenSource = new CancellationTokenSource();
        }

        #region Events
        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                txtPhotoFolder.Text = folderPath;
                PhotoMoveSettings.Default.SourceFolder = folderPath;
                PhotoMoveSettings.Default.Save();
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

        private async void btnFindPhotos_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                grbFindingPhottos.Visible = true;
                pgbFindingFiles.Visible = true;
                clbFilesWithValidExifDates.Items.Clear();
                clbFilesWithoutValidExifDates.Items.Clear();

                await Task.Run(() => FindFiles(), cancellationTokenSource.Token);

                grbFindingPhottos.Visible = false;
                pgbFindingFiles.Visible = false;
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
                await Task.Run(() => CopyOrMoveFiles(), cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                //break;
                pgbFindingFiles.Visible = false;
            }
        }

        private void cmbCopyMoveExistedFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            grbDuplicatesFolder.Visible = (cmbCopyMoveExistedFiles.SelectedIndex == 3);
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
        #endregion

        #region Methods
        private void InitializeOtherComponents()
        {
            grbCancel.Visible = false;
            grbProgress.Visible = false;
            txtPhotoFolder.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.SourceFolder)) ? string.Empty : PhotoMoveSettings.Default.SourceFolder.ToString();
            txtDestinationFolder.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.DestinationFolder)) ? string.Empty : PhotoMoveSettings.Default.DestinationFolder.ToString();
            txtDuplicatesFolderPath.Text = (string.IsNullOrEmpty(PhotoMoveSettings.Default.DuplicatesFolder)) ? string.Empty : PhotoMoveSettings.Default.DuplicatesFolder.ToString();
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
            string directory = Path.GetDirectoryName(filePath);
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

        private void FindFiles()
        {
            string folderPath = txtPhotoFolder.Text;
            List<string> filesWithValidExifDates = new List<string>();
            List<string> filesWithInvalidExifDates = new List<string>();
            int totalCount = 0;
            int exifFileCount = 0;
            int validDateCount = 0;
            int noValidDateCount = 0;

            var files = System.IO.Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

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

                totalCount++;

                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(file);

                    var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().OrderByDescending(x => x.TagCount).FirstOrDefault();
                    if (subIfdDirectory != null)
                    {
                        // This file contains EXIF data
                        exifFileCount++;
                        lblContainEXIF.Invoke((MethodInvoker)delegate
                        {
                            lblContainEXIF.Text = exifFileCount.ToString();
                        });

                        var originalDate = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);

                        // Try to get the "Date Taken" property
                        if (originalDate != null)
                        {
                            // This photo has a valid date
                            validDateCount++;
                            filesWithValidExifDates.Add(file);

                            lblHaveValidDate.Invoke((MethodInvoker)delegate
                            {
                                lblHaveValidDate.Text = validDateCount.ToString();
                            });
                        }
                        else
                        {
                            // This photo has EXIF data but no valid date
                            noValidDateCount++;
                            filesWithInvalidExifDates.Add(file);

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
                }
                catch (ImageProcessingException)
                {
                    // This file does not contain EXIF data or could not be processed
                    noValidDateCount++;
                    filesWithInvalidExifDates.Add(file);

                    lblHaveExifButNoValidDate.Invoke((MethodInvoker)delegate
                    {
                        lblHaveExifButNoValidDate.Text = noValidDateCount.ToString();
                    });
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
            }

            Invoke((MethodInvoker)(() =>
            {
                //lblTotalFiles.Text = files.Length.ToString();
                //lblContainEXIF.Text = exifFileCount.ToString();
                //lblHaveValidDate.Text = validDateCount.ToString();
                //lblHaveExifButNoValidDate.Text = noValidDateCount.ToString();
                tabFileOptions.Visible = true;
            }));



            var validFileTypeCounts = filesWithValidExifDates
                    .GroupBy(file => Path.GetExtension(file))
                    .Select(group => new { FileType = group.Key, Count = group.Count() })
                    .OrderByDescending(x => x.Count);

            foreach (var fileTypeCount in validFileTypeCounts)
            {
                //clbFilesWithValidExifDates.Items.Add($"{fileTypeCount.FileType} ({fileTypeCount.Count})", true);

                clbFilesWithValidExifDates.Invoke((MethodInvoker)delegate
                {
                    clbFilesWithValidExifDates.Items.Add($"{fileTypeCount.FileType} ({fileTypeCount.Count})", true);
                });
            }

            var invalidFileTypeCounts = filesWithInvalidExifDates
                    .GroupBy(file => Path.GetExtension(file))
                    .Select(group => new { FileType = group.Key, Count = group.Count() })
                    .OrderByDescending(x => x.Count);

            foreach (var fileTypeCount in invalidFileTypeCounts)
            {
                //clbFilesWithoutValidExifDates.Items.Add($"{fileTypeCount.FileType} ({fileTypeCount.Count})", true);
                clbFilesWithoutValidExifDates.Invoke((MethodInvoker)delegate
                {
                    clbFilesWithoutValidExifDates.Items.Add($"{fileTypeCount.FileType} ({fileTypeCount.Count})", true);
                });
            }
        }

        private void CopyOrMoveFiles()
        {
            string sourceFolderPath = txtPhotoFolder.Text;
            string destinationFolderPath = txtDestinationFolder.Text;
            string duplicatesFolderPath = txtDuplicatesFolderPath.Text;

            var files = System.IO.Directory.GetFiles(sourceFolderPath, "*.*", SearchOption.AllDirectories);
            int fileCount = 0;

            grbCancel.Visible = true;
            grbProgress.Visible = true;
            lblCopyingFiles.BackColor = Color.Yellow;
            lblCopyingProgress.BackColor = Color.Yellow;
            lblCopyingProgress.Text = "Files Copied - File Operation currently in progress";

            foreach (var file in files)
            {
                // Check for cancellation
                cancellationTokenSource.Token.ThrowIfCancellationRequested();

                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(file);

                    var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().OrderByDescending(x => x.TagCount).FirstOrDefault();
                    var dateTaken = subIfdDirectory?.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);

                    if (subIfdDirectory != null && dateTaken != null)
                    {
                        // This photo has a valid date

                        string year = dateTaken.Value.Year.ToString();
                        //string month = dateTaken.Value.ToString("MMMM");
                        string month = dateTaken.Value.ToString("MM");
                        string day = dateTaken.Value.ToString("dd");

                        string newFolderPath = string.Empty;

                        // Create the YMD (Single Folder) folders
                        if (cmbOutputFolderStructure.SelectedIndex == 0)
                        {
                            newFolderPath = Path.Combine(destinationFolderPath, $"{year}{month}{day}");
                        }
                        // Create the year, month, day folders
                        else if (cmbOutputFolderStructure.SelectedIndex == 1)
                        {
                            newFolderPath = Path.Combine(destinationFolderPath, year, $"{year}_{month}", $"{year}_{month}_{day}");
                        }
                        // Create the year, month folders
                        else if (cmbOutputFolderStructure.SelectedIndex == 2)
                        {
                            newFolderPath = Path.Combine(destinationFolderPath, year, $"{year}_{month}");
                        }

                        System.IO.Directory.CreateDirectory(newFolderPath);

                        // Copy the file to the new directory
                        string newFilePath = Path.Combine(newFolderPath, Path.GetFileName(file));

                        // check if the copying file exists in the destination folder
                        if (!IsDuplicateFile(newFilePath, file))
                        {
                            File.Copy(file, newFilePath);
                        }
                        else
                        {
                            switch (cmbCopyMoveExistedFiles.SelectedIndex)
                            {
                                case 0:
                                    // Do Not Move or Copy
                                    break;
                                case 1:
                                    // Add '-Copy###'. then Move or Copy
                                    string newFileName = GenerateUniqueCopyName(newFilePath);
                                    File.Copy(file, Path.Combine(Path.GetDirectoryName(newFilePath), newFileName));
                                    break;
                                case 2:
                                    // Overwrite the Existing File
                                    File.Copy(file, newFilePath, true);
                                    break;
                                case 3:
                                    // Move to specified Duplicates Folder:
                                    if (!string.IsNullOrEmpty(duplicatesFolderPath))
                                    {
                                        string newNewFolderPath = string.Empty;

                                        // Create the YMD (Single Folder) folders
                                        if (cmbOutputFolderStructure.SelectedIndex == 0)
                                        {
                                            newNewFolderPath = Path.Combine(duplicatesFolderPath, $"{year}{month}{day}");
                                        }
                                        // Create the year, month, day folders
                                        else if (cmbOutputFolderStructure.SelectedIndex == 1)
                                        {
                                            newNewFolderPath = Path.Combine(duplicatesFolderPath, year, $"{year}_{month}", $"{year}_{month}_{day}");
                                        }
                                        // Create the year, month folders
                                        else if (cmbOutputFolderStructure.SelectedIndex == 2)
                                        {
                                            newNewFolderPath = Path.Combine(duplicatesFolderPath, year, $"{year}_{month}");
                                        }

                                        System.IO.Directory.CreateDirectory(newNewFolderPath);
                                        string newNewFilePath = Path.Combine(newNewFolderPath, Path.GetFileName(file));

                                        //File.Copy(file, Path.Combine(duplicatesFolderPath, Path.GetFileName(newFilePath)));
                                        File.Copy(file, Path.Combine(newNewFolderPath, Path.GetFileName(newNewFilePath)));
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                        // Update the TextBox with the number of files copied
                        fileCount++;
                        lblCopyingFiles.Text = fileCount.ToString();
                        Application.DoEvents(); // This line allows the form to redraw itself to show the updated TextBox

                    }
                }
                catch (ImageProcessingException)
                {
                    // This file does not contain EXIF data or could not be processed
                }
            }

            grbCancel.Visible = false;
            lblCopyingFiles.BackColor = DefaultBackColor;
            lblCopyingProgress.BackColor = DefaultBackColor;
            lblCopyingProgress.Text = "Copy Operation Completed Successfully";
        }

        #endregion
    }
}
