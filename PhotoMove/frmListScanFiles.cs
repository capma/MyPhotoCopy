using PhotoMove.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace PhotoMove
{
    public partial class frmListScanFiles : Form
    {
        public frmListScanFiles()
        {
            InitializeComponent();
        }

        public void ShowData(List<ScanFile> scanFiles)
        {
            var bindingList = new BindingList<ScanFile>(scanFiles);
            dgScanFiles.DataSource = bindingList;
        }
    }
}
