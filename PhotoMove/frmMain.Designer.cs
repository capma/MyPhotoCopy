﻿using System.Drawing;
using System.Windows.Forms;

namespace PhotoMove
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            chkIncludeSubFolders = new CheckBox();
            btnChooseFolder = new Button();
            txtPhotoFolder = new TextBox();
            groupBox2 = new GroupBox();
            btnChooseOutputFolder = new Button();
            txtDestinationFolder = new TextBox();
            groupBox3 = new GroupBox();
            pgbFindingFiles = new ProgressBar();
            grbFindingPhottos = new GroupBox();
            btnCancelFindingPhotos = new Button();
            lblFindingFilesWithExif = new Label();
            label4 = new Label();
            lblTitleHaveExifButNoValidDate = new Label();
            lblHaveExifButNoValidDate = new Label();
            lblTitleHaveValidDate = new Label();
            lblHaveValidDate = new Label();
            lblTitleContainEXIF = new Label();
            lblContainEXIF = new Label();
            lblTitleTotalFiles = new Label();
            lblTotalFiles = new Label();
            btnFindPhotos = new Button();
            groupBox4 = new GroupBox();
            grbCancel = new GroupBox();
            btnCancel = new Button();
            grbProgress = new GroupBox();
            lblCopyingProgress = new Label();
            lblCopyingFiles = new Label();
            chkAlwaysShowSummaryReport = new CheckBox();
            btnShowSummaryReport = new Button();
            btnMoveToDestinationFolders = new Button();
            btnCopyToDestinationFolders = new Button();
            groupBox5 = new GroupBox();
            cmbOutputFolderStructure = new ComboBox();
            groupBox6 = new GroupBox();
            radAllExifAndExactFileContentsMatch = new RadioButton();
            radFileNamesMatch = new RadioButton();
            groupBox7 = new GroupBox();
            grbDuplicatesFolder = new GroupBox();
            txtDuplicatesFolderPath = new TextBox();
            btnChooseDuplicatesFolderPath = new Button();
            label1 = new Label();
            cmbCopyMoveExistedFiles = new ComboBox();
            groupBox8 = new GroupBox();
            button2 = new Button();
            textBox2 = new TextBox();
            checkBox2 = new CheckBox();
            checkBox1 = new CheckBox();
            tabFileOptions = new TabControl();
            tabPage1 = new TabPage();
            splitContainer1 = new SplitContainer();
            clbFilesWithValidExifDates = new CheckedListBox();
            label6 = new Label();
            clbFilesWithoutValidExifDates = new CheckedListBox();
            label7 = new Label();
            label2 = new Label();
            tabPage2 = new TabPage();
            splitContainer2 = new SplitContainer();
            clbCameraModelsWithValidExifDates = new CheckedListBox();
            label5 = new Label();
            clbCameraModelsWithoutValidExifDates = new CheckedListBox();
            label8 = new Label();
            label3 = new Label();
            tabPage3 = new TabPage();
            checkBox4 = new CheckBox();
            checkBox3 = new CheckBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            pgbCopyingOrMovingFiles = new ProgressBar();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            grbFindingPhottos.SuspendLayout();
            groupBox4.SuspendLayout();
            grbCancel.SuspendLayout();
            grbProgress.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox7.SuspendLayout();
            grbDuplicatesFolder.SuspendLayout();
            groupBox8.SuspendLayout();
            tabFileOptions.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tabPage3.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkIncludeSubFolders);
            groupBox1.Controls.Add(btnChooseFolder);
            groupBox1.Controls.Add(txtPhotoFolder);
            groupBox1.Location = new Point(12, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(538, 87);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Step 1: Choose Folder with Photos to Process:";
            // 
            // chkIncludeSubFolders
            // 
            chkIncludeSubFolders.AutoSize = true;
            chkIncludeSubFolders.Checked = true;
            chkIncludeSubFolders.CheckState = CheckState.Checked;
            chkIncludeSubFolders.Location = new Point(337, 54);
            chkIncludeSubFolders.Name = "chkIncludeSubFolders";
            chkIncludeSubFolders.Size = new Size(129, 19);
            chkIncludeSubFolders.TabIndex = 2;
            chkIncludeSubFolders.Text = "Include Sub Folders";
            chkIncludeSubFolders.UseVisualStyleBackColor = true;
            // 
            // btnChooseFolder
            // 
            btnChooseFolder.Location = new Point(6, 51);
            btnChooseFolder.Name = "btnChooseFolder";
            btnChooseFolder.Size = new Size(282, 23);
            btnChooseFolder.TabIndex = 1;
            btnChooseFolder.Text = "Click Here to Choose Folder to Search";
            btnChooseFolder.UseVisualStyleBackColor = true;
            btnChooseFolder.Click += btnChooseFolder_Click;
            // 
            // txtPhotoFolder
            // 
            txtPhotoFolder.BackColor = Color.White;
            txtPhotoFolder.Location = new Point(6, 22);
            txtPhotoFolder.Name = "txtPhotoFolder";
            txtPhotoFolder.ReadOnly = true;
            txtPhotoFolder.Size = new Size(526, 23);
            txtPhotoFolder.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnChooseOutputFolder);
            groupBox2.Controls.Add(txtDestinationFolder);
            groupBox2.Location = new Point(12, 131);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(538, 90);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Step 2: Set Destination Folder Under Which Date Sorted Folders Will Be Made:";
            // 
            // btnChooseOutputFolder
            // 
            btnChooseOutputFolder.Location = new Point(6, 51);
            btnChooseOutputFolder.Name = "btnChooseOutputFolder";
            btnChooseOutputFolder.Size = new Size(282, 23);
            btnChooseOutputFolder.TabIndex = 1;
            btnChooseOutputFolder.Text = "Click Here to Choose Output Folder";
            btnChooseOutputFolder.UseVisualStyleBackColor = true;
            btnChooseOutputFolder.Click += btnChooseOutputFolder_Click;
            // 
            // txtDestinationFolder
            // 
            txtDestinationFolder.BackColor = Color.White;
            txtDestinationFolder.Location = new Point(6, 22);
            txtDestinationFolder.Name = "txtDestinationFolder";
            txtDestinationFolder.ReadOnly = true;
            txtDestinationFolder.Size = new Size(526, 23);
            txtDestinationFolder.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(pgbFindingFiles);
            groupBox3.Controls.Add(grbFindingPhottos);
            groupBox3.Controls.Add(lblTitleHaveExifButNoValidDate);
            groupBox3.Controls.Add(lblHaveExifButNoValidDate);
            groupBox3.Controls.Add(lblTitleHaveValidDate);
            groupBox3.Controls.Add(lblHaveValidDate);
            groupBox3.Controls.Add(lblTitleContainEXIF);
            groupBox3.Controls.Add(lblContainEXIF);
            groupBox3.Controls.Add(lblTitleTotalFiles);
            groupBox3.Controls.Add(lblTotalFiles);
            groupBox3.Controls.Add(btnFindPhotos);
            groupBox3.Location = new Point(12, 240);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(538, 236);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Step 3: Find Photos to Move or Copy:";
            // 
            // pgbFindingFiles
            // 
            pgbFindingFiles.Location = new Point(6, 22);
            pgbFindingFiles.Name = "pgbFindingFiles";
            pgbFindingFiles.Size = new Size(159, 23);
            pgbFindingFiles.TabIndex = 13;
            pgbFindingFiles.Visible = false;
            // 
            // grbFindingPhottos
            // 
            grbFindingPhottos.Controls.Add(btnCancelFindingPhotos);
            grbFindingPhottos.Controls.Add(lblFindingFilesWithExif);
            grbFindingPhottos.Controls.Add(label4);
            grbFindingPhottos.Location = new Point(6, 129);
            grbFindingPhottos.Name = "grbFindingPhottos";
            grbFindingPhottos.Size = new Size(526, 100);
            grbFindingPhottos.TabIndex = 12;
            grbFindingPhottos.TabStop = false;
            grbFindingPhottos.Visible = false;
            // 
            // btnCancelFindingPhotos
            // 
            btnCancelFindingPhotos.Location = new Point(6, 64);
            btnCancelFindingPhotos.Name = "btnCancelFindingPhotos";
            btnCancelFindingPhotos.Size = new Size(87, 31);
            btnCancelFindingPhotos.TabIndex = 14;
            btnCancelFindingPhotos.Text = "Cancel";
            btnCancelFindingPhotos.UseVisualStyleBackColor = true;
            btnCancelFindingPhotos.Click += btnCancelFindingPhotos_Click;
            // 
            // lblFindingFilesWithExif
            // 
            lblFindingFilesWithExif.BackColor = Color.White;
            lblFindingFilesWithExif.BorderStyle = BorderStyle.FixedSingle;
            lblFindingFilesWithExif.Location = new Point(6, 37);
            lblFindingFilesWithExif.Name = "lblFindingFilesWithExif";
            lblFindingFilesWithExif.Size = new Size(513, 23);
            lblFindingFilesWithExif.TabIndex = 13;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 15);
            label4.Name = "label4";
            label4.Size = new Size(166, 15);
            label4.TabIndex = 12;
            label4.Text = "Finding Files With Exif Data in:";
            // 
            // lblTitleHaveExifButNoValidDate
            // 
            lblTitleHaveExifButNoValidDate.BorderStyle = BorderStyle.FixedSingle;
            lblTitleHaveExifButNoValidDate.Location = new Point(243, 103);
            lblTitleHaveExifButNoValidDate.Name = "lblTitleHaveExifButNoValidDate";
            lblTitleHaveExifButNoValidDate.Size = new Size(276, 23);
            lblTitleHaveExifButNoValidDate.TabIndex = 8;
            lblTitleHaveExifButNoValidDate.Text = "Have Exif Data But No Valid Date";
            lblTitleHaveExifButNoValidDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblHaveExifButNoValidDate
            // 
            lblHaveExifButNoValidDate.BackColor = Color.White;
            lblHaveExifButNoValidDate.BorderStyle = BorderStyle.FixedSingle;
            lblHaveExifButNoValidDate.Location = new Point(171, 103);
            lblHaveExifButNoValidDate.Name = "lblHaveExifButNoValidDate";
            lblHaveExifButNoValidDate.Size = new Size(66, 23);
            lblHaveExifButNoValidDate.TabIndex = 7;
            lblHaveExifButNoValidDate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitleHaveValidDate
            // 
            lblTitleHaveValidDate.BorderStyle = BorderStyle.FixedSingle;
            lblTitleHaveValidDate.Location = new Point(243, 76);
            lblTitleHaveValidDate.Name = "lblTitleHaveValidDate";
            lblTitleHaveValidDate.Size = new Size(276, 23);
            lblTitleHaveValidDate.TabIndex = 6;
            lblTitleHaveValidDate.Text = "Have Valid Date Created in File's Internal Exif Data";
            lblTitleHaveValidDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblHaveValidDate
            // 
            lblHaveValidDate.BackColor = Color.White;
            lblHaveValidDate.BorderStyle = BorderStyle.FixedSingle;
            lblHaveValidDate.Location = new Point(171, 76);
            lblHaveValidDate.Name = "lblHaveValidDate";
            lblHaveValidDate.Size = new Size(66, 23);
            lblHaveValidDate.TabIndex = 5;
            lblHaveValidDate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitleContainEXIF
            // 
            lblTitleContainEXIF.BorderStyle = BorderStyle.FixedSingle;
            lblTitleContainEXIF.Location = new Point(243, 49);
            lblTitleContainEXIF.Name = "lblTitleContainEXIF";
            lblTitleContainEXIF.Size = new Size(276, 23);
            lblTitleContainEXIF.TabIndex = 4;
            lblTitleContainEXIF.Text = "Contain EXIF Information of Some Type";
            lblTitleContainEXIF.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblContainEXIF
            // 
            lblContainEXIF.BackColor = Color.White;
            lblContainEXIF.BorderStyle = BorderStyle.FixedSingle;
            lblContainEXIF.Location = new Point(171, 49);
            lblContainEXIF.Name = "lblContainEXIF";
            lblContainEXIF.Size = new Size(66, 23);
            lblContainEXIF.TabIndex = 3;
            lblContainEXIF.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTitleTotalFiles
            // 
            lblTitleTotalFiles.BorderStyle = BorderStyle.FixedSingle;
            lblTitleTotalFiles.Location = new Point(243, 22);
            lblTitleTotalFiles.Name = "lblTitleTotalFiles";
            lblTitleTotalFiles.Size = new Size(276, 23);
            lblTitleTotalFiles.TabIndex = 2;
            lblTitleTotalFiles.Text = "Total Files Checked In All Folders";
            lblTitleTotalFiles.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalFiles
            // 
            lblTotalFiles.BackColor = Color.White;
            lblTotalFiles.BorderStyle = BorderStyle.FixedSingle;
            lblTotalFiles.Location = new Point(171, 22);
            lblTotalFiles.Name = "lblTotalFiles";
            lblTotalFiles.Size = new Size(66, 23);
            lblTotalFiles.TabIndex = 1;
            lblTotalFiles.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnFindPhotos
            // 
            btnFindPhotos.Location = new Point(6, 22);
            btnFindPhotos.Name = "btnFindPhotos";
            btnFindPhotos.Size = new Size(159, 23);
            btnFindPhotos.TabIndex = 0;
            btnFindPhotos.Text = "Find Photos";
            btnFindPhotos.UseVisualStyleBackColor = true;
            btnFindPhotos.Click += btnFindPhotos_Click;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(grbCancel);
            groupBox4.Controls.Add(grbProgress);
            groupBox4.Controls.Add(chkAlwaysShowSummaryReport);
            groupBox4.Controls.Add(btnShowSummaryReport);
            groupBox4.Controls.Add(btnMoveToDestinationFolders);
            groupBox4.Controls.Add(btnCopyToDestinationFolders);
            groupBox4.Location = new Point(12, 493);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(538, 172);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Step 4: Copy or Move Photos to Date Sorted Folders";
            // 
            // grbCancel
            // 
            grbCancel.Controls.Add(pgbCopyingOrMovingFiles);
            grbCancel.Controls.Add(btnCancel);
            grbCancel.Location = new Point(6, 22);
            grbCancel.Name = "grbCancel";
            grbCancel.Size = new Size(526, 62);
            grbCancel.TabIndex = 5;
            grbCancel.TabStop = false;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(6, 22);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(111, 26);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // grbProgress
            // 
            grbProgress.Controls.Add(lblCopyingProgress);
            grbProgress.Controls.Add(lblCopyingFiles);
            grbProgress.Location = new Point(6, 76);
            grbProgress.Name = "grbProgress";
            grbProgress.Size = new Size(526, 62);
            grbProgress.TabIndex = 4;
            grbProgress.TabStop = false;
            // 
            // lblCopyingProgress
            // 
            lblCopyingProgress.BackColor = Color.FromArgb(255, 255, 128);
            lblCopyingProgress.BorderStyle = BorderStyle.FixedSingle;
            lblCopyingProgress.Location = new Point(123, 19);
            lblCopyingProgress.Name = "lblCopyingProgress";
            lblCopyingProgress.Size = new Size(397, 23);
            lblCopyingProgress.TabIndex = 9;
            lblCopyingProgress.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCopyingFiles
            // 
            lblCopyingFiles.BackColor = Color.FromArgb(255, 255, 128);
            lblCopyingFiles.BorderStyle = BorderStyle.FixedSingle;
            lblCopyingFiles.Location = new Point(6, 19);
            lblCopyingFiles.Name = "lblCopyingFiles";
            lblCopyingFiles.Size = new Size(111, 23);
            lblCopyingFiles.TabIndex = 8;
            lblCopyingFiles.TextAlign = ContentAlignment.MiddleRight;
            // 
            // chkAlwaysShowSummaryReport
            // 
            chkAlwaysShowSummaryReport.AutoSize = true;
            chkAlwaysShowSummaryReport.Location = new Point(232, 147);
            chkAlwaysShowSummaryReport.Name = "chkAlwaysShowSummaryReport";
            chkAlwaysShowSummaryReport.Size = new Size(287, 19);
            chkAlwaysShowSummaryReport.TabIndex = 3;
            chkAlwaysShowSummaryReport.Text = "Always show summary report after Copy or Move";
            chkAlwaysShowSummaryReport.UseVisualStyleBackColor = true;
            // 
            // btnShowSummaryReport
            // 
            btnShowSummaryReport.Location = new Point(6, 144);
            btnShowSummaryReport.Name = "btnShowSummaryReport";
            btnShowSummaryReport.Size = new Size(208, 23);
            btnShowSummaryReport.TabIndex = 2;
            btnShowSummaryReport.Text = "Show Summary Report";
            btnShowSummaryReport.UseVisualStyleBackColor = true;
            // 
            // btnMoveToDestinationFolders
            // 
            btnMoveToDestinationFolders.Location = new Point(258, 35);
            btnMoveToDestinationFolders.Name = "btnMoveToDestinationFolders";
            btnMoveToDestinationFolders.Size = new Size(208, 23);
            btnMoveToDestinationFolders.TabIndex = 1;
            btnMoveToDestinationFolders.Text = "MOVE to Destination Folders";
            btnMoveToDestinationFolders.UseVisualStyleBackColor = true;
            // 
            // btnCopyToDestinationFolders
            // 
            btnCopyToDestinationFolders.Location = new Point(6, 35);
            btnCopyToDestinationFolders.Name = "btnCopyToDestinationFolders";
            btnCopyToDestinationFolders.Size = new Size(208, 23);
            btnCopyToDestinationFolders.TabIndex = 0;
            btnCopyToDestinationFolders.Text = "COPY to Destination Folders";
            btnCopyToDestinationFolders.UseVisualStyleBackColor = true;
            btnCopyToDestinationFolders.Click += btnCopyToDestinationFolders_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(cmbOutputFolderStructure);
            groupBox5.Location = new Point(592, 27);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(415, 56);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "Output Folder Structure";
            // 
            // cmbOutputFolderStructure
            // 
            cmbOutputFolderStructure.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOutputFolderStructure.FormattingEnabled = true;
            cmbOutputFolderStructure.Location = new Point(6, 22);
            cmbOutputFolderStructure.Name = "cmbOutputFolderStructure";
            cmbOutputFolderStructure.Size = new Size(403, 23);
            cmbOutputFolderStructure.TabIndex = 1;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(radAllExifAndExactFileContentsMatch);
            groupBox6.Controls.Add(radFileNamesMatch);
            groupBox6.Location = new Point(592, 89);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(415, 74);
            groupBox6.TabIndex = 5;
            groupBox6.TabStop = false;
            groupBox6.Text = "Files are Duplicates If:";
            // 
            // radAllExifAndExactFileContentsMatch
            // 
            radAllExifAndExactFileContentsMatch.AutoSize = true;
            radAllExifAndExactFileContentsMatch.Checked = true;
            radAllExifAndExactFileContentsMatch.Location = new Point(15, 47);
            radAllExifAndExactFileContentsMatch.Name = "radAllExifAndExactFileContentsMatch";
            radAllExifAndExactFileContentsMatch.Size = new Size(224, 19);
            radAllExifAndExactFileContentsMatch.TabIndex = 1;
            radAllExifAndExactFileContentsMatch.TabStop = true;
            radAllExifAndExactFileContentsMatch.Text = "All Exif and Exact File Contents Match";
            radAllExifAndExactFileContentsMatch.UseVisualStyleBackColor = true;
            // 
            // radFileNamesMatch
            // 
            radFileNamesMatch.AutoSize = true;
            radFileNamesMatch.Location = new Point(15, 22);
            radFileNamesMatch.Name = "radFileNamesMatch";
            radFileNamesMatch.Size = new Size(120, 19);
            radFileNamesMatch.TabIndex = 0;
            radFileNamesMatch.TabStop = true;
            radFileNamesMatch.Text = "File Names Match";
            radFileNamesMatch.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(grbDuplicatesFolder);
            groupBox7.Controls.Add(cmbCopyMoveExistedFiles);
            groupBox7.Location = new Point(592, 169);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(415, 107);
            groupBox7.TabIndex = 6;
            groupBox7.TabStop = false;
            groupBox7.Text = "If File to be Moved/Copied exists in the destination folder:";
            // 
            // grbDuplicatesFolder
            // 
            grbDuplicatesFolder.Controls.Add(txtDuplicatesFolderPath);
            grbDuplicatesFolder.Controls.Add(btnChooseDuplicatesFolderPath);
            grbDuplicatesFolder.Controls.Add(label1);
            grbDuplicatesFolder.Location = new Point(6, 51);
            grbDuplicatesFolder.Name = "grbDuplicatesFolder";
            grbDuplicatesFolder.Size = new Size(403, 47);
            grbDuplicatesFolder.TabIndex = 4;
            grbDuplicatesFolder.TabStop = false;
            // 
            // txtDuplicatesFolderPath
            // 
            txtDuplicatesFolderPath.BackColor = Color.White;
            txtDuplicatesFolderPath.Location = new Point(92, 15);
            txtDuplicatesFolderPath.Name = "txtDuplicatesFolderPath";
            txtDuplicatesFolderPath.ReadOnly = true;
            txtDuplicatesFolderPath.Size = new Size(269, 23);
            txtDuplicatesFolderPath.TabIndex = 2;
            // 
            // btnChooseDuplicatesFolderPath
            // 
            btnChooseDuplicatesFolderPath.Location = new Point(367, 15);
            btnChooseDuplicatesFolderPath.Name = "btnChooseDuplicatesFolderPath";
            btnChooseDuplicatesFolderPath.Size = new Size(30, 23);
            btnChooseDuplicatesFolderPath.TabIndex = 3;
            btnChooseDuplicatesFolderPath.Text = ">";
            btnChooseDuplicatesFolderPath.UseVisualStyleBackColor = true;
            btnChooseDuplicatesFolderPath.Click += btnChooseDuplicatesFolderPath_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(80, 15);
            label1.TabIndex = 1;
            label1.Text = "Duplicates To:";
            // 
            // cmbCopyMoveExistedFiles
            // 
            cmbCopyMoveExistedFiles.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCopyMoveExistedFiles.FormattingEnabled = true;
            cmbCopyMoveExistedFiles.Location = new Point(6, 22);
            cmbCopyMoveExistedFiles.Name = "cmbCopyMoveExistedFiles";
            cmbCopyMoveExistedFiles.Size = new Size(403, 23);
            cmbCopyMoveExistedFiles.TabIndex = 0;
            cmbCopyMoveExistedFiles.SelectedIndexChanged += cmbCopyMoveExistedFiles_SelectedIndexChanged;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(button2);
            groupBox8.Controls.Add(textBox2);
            groupBox8.Controls.Add(checkBox2);
            groupBox8.Controls.Add(checkBox1);
            groupBox8.Location = new Point(592, 282);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(415, 100);
            groupBox8.TabIndex = 7;
            groupBox8.TabStop = false;
            groupBox8.Text = "Option for Files with No Exif Date Created:";
            // 
            // button2
            // 
            button2.Location = new Point(373, 68);
            button2.Name = "button2";
            button2.Size = new Size(30, 23);
            button2.TabIndex = 3;
            button2.Text = ">";
            button2.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.White;
            textBox2.Location = new Point(35, 69);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(332, 23);
            textBox2.TabIndex = 2;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Checked = true;
            checkBox2.CheckState = CheckState.Checked;
            checkBox2.Location = new Point(15, 44);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(173, 19);
            checkBox2.TabIndex = 1;
            checkBox2.Text = "Copy or Move to this Folder";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(15, 22);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(298, 19);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Use File Date to Move or Copy to Structured Folders";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // tabFileOptions
            // 
            tabFileOptions.Controls.Add(tabPage1);
            tabFileOptions.Controls.Add(tabPage2);
            tabFileOptions.Controls.Add(tabPage3);
            tabFileOptions.Location = new Point(592, 389);
            tabFileOptions.Name = "tabFileOptions";
            tabFileOptions.SelectedIndex = 0;
            tabFileOptions.Size = new Size(422, 276);
            tabFileOptions.TabIndex = 8;
            tabFileOptions.Visible = false;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(splitContainer1);
            tabPage1.Controls.Add(label2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(414, 248);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "File Types";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(3, 26);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(clbFilesWithValidExifDates);
            splitContainer1.Panel1.Controls.Add(label6);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(clbFilesWithoutValidExifDates);
            splitContainer1.Panel2.Controls.Add(label7);
            splitContainer1.Size = new Size(408, 219);
            splitContainer1.SplitterDistance = 204;
            splitContainer1.TabIndex = 11;
            // 
            // clbFilesWithValidExifDates
            // 
            clbFilesWithValidExifDates.Dock = DockStyle.Fill;
            clbFilesWithValidExifDates.FormattingEnabled = true;
            clbFilesWithValidExifDates.Location = new Point(0, 14);
            clbFilesWithValidExifDates.Name = "clbFilesWithValidExifDates";
            clbFilesWithValidExifDates.Size = new Size(204, 205);
            clbFilesWithValidExifDates.TabIndex = 3;
            // 
            // label6
            // 
            label6.Dock = DockStyle.Top;
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(204, 14);
            label6.TabIndex = 2;
            label6.Text = "Files With Valid Exif Dates:";
            // 
            // clbFilesWithoutValidExifDates
            // 
            clbFilesWithoutValidExifDates.Dock = DockStyle.Fill;
            clbFilesWithoutValidExifDates.FormattingEnabled = true;
            clbFilesWithoutValidExifDates.Location = new Point(0, 14);
            clbFilesWithoutValidExifDates.Name = "clbFilesWithoutValidExifDates";
            clbFilesWithoutValidExifDates.Size = new Size(200, 205);
            clbFilesWithoutValidExifDates.TabIndex = 4;
            // 
            // label7
            // 
            label7.Dock = DockStyle.Top;
            label7.Location = new Point(0, 0);
            label7.Name = "label7";
            label7.Size = new Size(200, 14);
            label7.TabIndex = 3;
            label7.Text = "Files Without Valid Exif Dates:";
            // 
            // label2
            // 
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(3, 3);
            label2.Name = "label2";
            label2.Size = new Size(408, 23);
            label2.TabIndex = 0;
            label2.Text = "Copy or Move These Files:";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(splitContainer2);
            tabPage2.Controls.Add(label3);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(414, 248);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Camera Models";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(3, 26);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(clbCameraModelsWithValidExifDates);
            splitContainer2.Panel1.Controls.Add(label5);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(clbCameraModelsWithoutValidExifDates);
            splitContainer2.Panel2.Controls.Add(label8);
            splitContainer2.Size = new Size(408, 219);
            splitContainer2.SplitterDistance = 204;
            splitContainer2.TabIndex = 12;
            // 
            // clbCameraModelsWithValidExifDates
            // 
            clbCameraModelsWithValidExifDates.Dock = DockStyle.Fill;
            clbCameraModelsWithValidExifDates.FormattingEnabled = true;
            clbCameraModelsWithValidExifDates.Location = new Point(0, 14);
            clbCameraModelsWithValidExifDates.Name = "clbCameraModelsWithValidExifDates";
            clbCameraModelsWithValidExifDates.Size = new Size(204, 205);
            clbCameraModelsWithValidExifDates.TabIndex = 3;
            // 
            // label5
            // 
            label5.Dock = DockStyle.Top;
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Size = new Size(204, 14);
            label5.TabIndex = 2;
            label5.Text = "Files With Valid Exif Dates:";
            // 
            // clbCameraModelsWithoutValidExifDates
            // 
            clbCameraModelsWithoutValidExifDates.Dock = DockStyle.Fill;
            clbCameraModelsWithoutValidExifDates.FormattingEnabled = true;
            clbCameraModelsWithoutValidExifDates.Location = new Point(0, 14);
            clbCameraModelsWithoutValidExifDates.Name = "clbCameraModelsWithoutValidExifDates";
            clbCameraModelsWithoutValidExifDates.Size = new Size(200, 205);
            clbCameraModelsWithoutValidExifDates.TabIndex = 4;
            // 
            // label8
            // 
            label8.Dock = DockStyle.Top;
            label8.Location = new Point(0, 0);
            label8.Name = "label8";
            label8.Size = new Size(200, 14);
            label8.TabIndex = 3;
            label8.Text = "Files Without Valid Exif Dates:";
            // 
            // label3
            // 
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(3, 3);
            label3.Name = "label3";
            label3.Size = new Size(408, 23);
            label3.TabIndex = 1;
            label3.Text = "Copy or Move These Camera Models:";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(checkBox4);
            tabPage3.Controls.Add(checkBox3);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(414, 248);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Other Options";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(11, 43);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(280, 19);
            checkBox4.TabIndex = 1;
            checkBox4.Text = "No Seperator. Do not use Dashes or Underscores";
            checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(11, 18);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(271, 19);
            checkBox3.TabIndex = 0;
            checkBox3.Text = "Use Dashes. Not Underscores. In Folder Names";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1023, 24);
            menuStrip1.TabIndex = 9;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(93, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // pgbCopyingOrMovingFiles
            // 
            pgbCopyingOrMovingFiles.Location = new Point(123, 22);
            pgbCopyingOrMovingFiles.Name = "pgbCopyingOrMovingFiles";
            pgbCopyingOrMovingFiles.Size = new Size(390, 23);
            pgbCopyingOrMovingFiles.TabIndex = 14;
            pgbCopyingOrMovingFiles.Visible = false;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1023, 671);
            Controls.Add(tabFileOptions);
            Controls.Add(groupBox8);
            Controls.Add(groupBox7);
            Controls.Add(groupBox6);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PhotoMove";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            grbFindingPhottos.ResumeLayout(false);
            grbFindingPhottos.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            grbCancel.ResumeLayout(false);
            grbProgress.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox7.ResumeLayout(false);
            grbDuplicatesFolder.ResumeLayout(false);
            grbDuplicatesFolder.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            tabFileOptions.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private CheckBox chkIncludeSubFolders;
        private Button btnChooseFolder;
        private TextBox txtPhotoFolder;
        private GroupBox groupBox2;
        private TextBox txtDestinationFolder;
        private Button btnChooseOutputFolder;
        private GroupBox groupBox3;
        private Button btnFindPhotos;
        private GroupBox groupBox4;
        private Label lblTitleTotalFiles;
        private Label lblTotalFiles;
        private Label lblTitleHaveExifButNoValidDate;
        private Label lblHaveExifButNoValidDate;
        private Label lblTitleHaveValidDate;
        private Label lblHaveValidDate;
        private Label lblTitleContainEXIF;
        private Label lblContainEXIF;
        private Button btnMoveToDestinationFolders;
        private Button btnCopyToDestinationFolders;
        private CheckBox chkAlwaysShowSummaryReport;
        private Button btnShowSummaryReport;
        private GroupBox groupBox5;
        private ComboBox cmbOutputFolderStructure;
        private GroupBox groupBox6;
        private RadioButton radAllExifAndExactFileContentsMatch;
        private RadioButton radFileNamesMatch;
        private GroupBox grbProgress;
        private Label lblCopyingProgress;
        private Label lblCopyingFiles;
        private GroupBox grbCancel;
        private Button btnCancel;
        private GroupBox groupBox7;
        private ComboBox cmbCopyMoveExistedFiles;
        private Button btnChooseDuplicatesFolderPath;
        private TextBox txtDuplicatesFolderPath;
        private Label label1;
        private GroupBox groupBox8;
        private CheckBox checkBox2;
        private CheckBox checkBox1;
        private Button button2;
        private TextBox textBox2;
        private TabControl tabFileOptions;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private GroupBox grbDuplicatesFolder;
        private TabPage tabPage3;
        private Label label2;
        private Label label3;
        private CheckBox checkBox4;
        private CheckBox checkBox3;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private SplitContainer splitContainer1;
        private CheckedListBox clbFilesWithValidExifDates;
        private Label label6;
        private CheckedListBox clbFilesWithoutValidExifDates;
        private Label label7;
        private GroupBox grbFindingPhottos;
        private Button btnCancelFindingPhotos;
        private Label lblFindingFilesWithExif;
        private Label label4;
        private ProgressBar pgbFindingFiles;
        private SplitContainer splitContainer2;
        private CheckedListBox clbCameraModelsWithValidExifDates;
        private Label label5;
        private CheckedListBox clbCameraModelsWithoutValidExifDates;
        private Label label8;
        private ProgressBar pgbCopyingOrMovingFiles;
    }
}
