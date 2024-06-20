using PhotoMove.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace PhotoMove
{
    public partial class frmListScanFiles : Form
    {
        public frmListScanFiles()
        {
            InitializeComponent();
        }

        public void ShowData(List<ScanFileReport> scanFiles)
        {
            dgScanFiles.DataSource = Funcs.ToDataTable(scanFiles);
        }

        public void ShowSummaryData(List<ScanFileReport> scanFiles)
        {
            dgScanFiles.DataSource = Funcs.ToDataTableCustom(scanFiles, "Index", "Action", "File", "Date", "Destination");
        }
    }

    static class Funcs
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static DataTable ToDataTableCustom<T>(this IList<T> data, params string[] propertyNames)
        {
            DataTable table = new();

            foreach (string propertyName in propertyNames)
            {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(T))[propertyName];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (string propertyName in propertyNames)
                {
                    PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(T))[propertyName];
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
