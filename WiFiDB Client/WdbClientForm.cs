using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WDBAPI;

namespace WiFiDB_Uploader
{
    public partial class WdbClientForm : Form
    {
        WDBAPI.WDBAPI WDBAPIObj;
        public WdbClientForm()
        {
            InitializeComponent();
            listView1.View = View.Details;

            WDBAPIObj = new WDBAPI.WDBAPI();

            string[] row = { "1", "Pferland", "Test Import", "2016-01-05", "248.02kb", "WDB_Upload_20160105.vs1", "DUMMYHASHSUM", "N/A", "Not Importing" };
            var listViewItem = new ListViewItem(row);
            listView1.Items.Add(listViewItem);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void openSchedulePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Open Scheduling Page
        }

        private void uploadSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "VS1 files (*.VS1)|*.VS1|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string response = WDBAPIObj.ApiImportFile(openFileDialog1.FileName);
                    WDBAPIObj.ParseApiResponse(response);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            
        }

        private void getWaitingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void getFinishedListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string response = WDBAPIObj.ApiGetFinishedImports();

            WDBAPIObj.ParseApiResponse(response);
        }

        private void getImportingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string response = WDBAPIObj.ApiGetCurrentImporting();

            WDBAPIObj.ParseApiResponse(response);
        }

        private void getBadImportsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string response = WDBAPIObj.ApiGetBadIports();

            WDBAPIObj.ParseApiResponse(response);
        }

        private void selectFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
