namespace PhotoMove
{
    using System;
    using System.Windows.Forms;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PhotoMove.Models;
    using System.Diagnostics;

    public partial class frmMain : Form
    {
        Stopwatch stopwatch = new Stopwatch();
        CancellationTokenSource cancellationTokenSource = new();
        List<ScanFile> scanFiles = new();
        UserOptions userOptions = new();
        ScanStatistics scanStatistics = new();

        public frmMain()
        {
            InitializeComponent();
            InitForm();
        }

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
            scanFiles = new();
            scanStatistics = new();

            try
            {
                // Reset and start the Stopwatch and Timer
                stopwatch.Reset();
                stopwatch.Start();
                timer1.Stop();
                timer1.Start();

                ResetFindFilesStatistics();

                grbFindingPhottos.Visible = true;
                pgbFindingFiles.Visible = true;
                btnShowListOfNoExifDateFiles.Visible = false;
                btnShowListOfValidExifDateFiles.Visible = false;

                GetUserOptions();

                await Task.Run(() => DoScanFiles(), cancellationTokenSource.Token);
                //await Task.Run(() => DoScanFilesUsingExifTool(), cancellationTokenSource.Token);

                grbFindingPhottos.Visible = false;
                pgbFindingFiles.Visible = false;
                grbCopyOrMove.Enabled = true;

                btnShowListOfNoExifDateFiles.Visible = (scanStatistics.noValidDateCount > 0);
                btnShowListOfValidExifDateFiles.Visible = (scanStatistics.validDateCount > 0);
            }
            catch (OperationCanceledException)
            {
                //break;
                grbFindingPhottos.Visible = false;
                pgbFindingFiles.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally 
            {
                stopwatch.Stop();
                timer1.Stop();
            }
        }

        private async void btnCopyToDestinationFolders_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Reset and start the Stopwatch and Timer
                stopwatch.Reset();
                stopwatch.Start();
                timer1.Stop();
                timer1.Start();

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
            finally 
            {
                stopwatch.Stop();
                timer1.Stop();
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

        private void btnShowListOfValidExifDateFiles_Click(object sender, EventArgs e)
        {
            frmListScanFiles childForm = new();
            childForm.Text = "Valid Exif Date Files";
            var filteredFiles = FilteredQuery(x => x.isValidExif && x.isValidTakenDate);
            childForm.ShowData(filteredFiles);
            this.Hide();
            childForm.FormClosed += frmListScanFiles_FormClosed; // Attach the FormClosed event handler
            childForm.ShowDialog();
        }

        private void btnShowListOfNoExifDateFiles_Click(object sender, EventArgs e)
        {
            frmListScanFiles childForm = new();
            childForm.Text = "Files With No Exif Date";
            var filteredFiles = FilteredQuery(x => !x.isValidTakenDate);
            //var filteredFiles = FilteredQuery(x => !x.isValidTakenDate);
            childForm.ShowData(filteredFiles);
            this.Hide();
            childForm.FormClosed += frmListScanFiles_FormClosed; // Attach the FormClosed event handler
            childForm.ShowDialog();
        }

        private void btnShowSummaryReport_Click(object sender, EventArgs e)
        {
            frmListScanFiles childForm = new();
            var filteredFiles = FilteredQuery(x => x.isProcessed);
            childForm.ShowSummaryData(filteredFiles);
            this.Hide();
            childForm.FormClosed += frmListScanFiles_FormClosed; // Attach the FormClosed event handler
            childForm.ShowDialog();
        }

        private void frmListScanFiles_FormClosed(object? sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton? radioButton = sender as RadioButton;

            if (radioButton != null)
            {
                if (radioButton.Checked)
                {
                    // Store the value of the checked radio button in the variable
                    PhotoMoveSettings.Default.selectedSeperatorInFolderName = radioButton.TabIndex;
                    PhotoMoveSettings.Default.Save();
                }
            }
        }

        private void timer1_Tick(object? sender, EventArgs e)
        {
            // Update the status strip
            toolStripStatusLabel1.Text = $"Time: {stopwatch.Elapsed.ToString()} seconds elapsed.";
        }
    }
}
