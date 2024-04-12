namespace PhotoMove
{
    partial class frmListScanFiles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgScanFiles = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgScanFiles).BeginInit();
            SuspendLayout();
            // 
            // dgScanFiles
            // 
            dgScanFiles.AllowUserToAddRows = false;
            dgScanFiles.AllowUserToDeleteRows = false;
            dgScanFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dgScanFiles.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dgScanFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgScanFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            dgScanFiles.Location = new System.Drawing.Point(0, 0);
            dgScanFiles.Name = "dgScanFiles";
            dgScanFiles.ReadOnly = true;
            dgScanFiles.RowHeadersVisible = false;
            dgScanFiles.Size = new System.Drawing.Size(1103, 535);
            dgScanFiles.TabIndex = 1;
            // 
            // frmListScanFiles
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1103, 535);
            Controls.Add(dgScanFiles);
            Name = "frmListScanFiles";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Scan Files";
            ((System.ComponentModel.ISupportInitialize)dgScanFiles).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgScanFiles;
    }
}