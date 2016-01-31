using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WiFiDBUploader
{
    public partial class Auto_Upload_Settings : Form
    {
        public Auto_Upload_Settings()
        {
            InitializeComponent();
        }
        
        private void AutoBrowse2Button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                //Debug.WriteLine(folderBrowserDialog1.SelectedPath);
                this.ArchiveImportsFolderPath = folderBrowserDialog1.SelectedPath;
            }
        }

        private void AutoBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                //Debug.WriteLine(folderBrowserDialog1.SelectedPath);
                this.AutoUploadFolderPath = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
