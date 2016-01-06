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
            string[] array = {""};
            WDBAPIObj.ApiPostFile(array, "");
        }
    }
}
