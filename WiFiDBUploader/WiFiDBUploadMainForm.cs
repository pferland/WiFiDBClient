using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WDBAPI;
using WDBCommon;

namespace WiFiDBUploader
{
    public partial class WiFiDBUploadMainForm : Form
    {
        WDBAPI.WDBAPI WDBAPIObj;
        WDBCommon.WDBCommon WDBCommonObj;
        public int NextID = 0;
        public int ImportInternalID = 0;
        public List<KeyValuePair<int, string>> ImportIDs;

        public WiFiDBUploadMainForm()
        {
            InitializeComponent();
            listView1.View = View.Details;

            WDBAPIObj = new WDBAPI.WDBAPI();
            WDBCommonObj = new WDBCommon.WDBCommon();

            /*string[] row = { "1", "Pferland", "Test Import", "2016-01-05", "248.02kb", "WDB_Upload_20160105.vs1", "DUMMYHASHSUM", "N/A", "Not Importing" };
            var listViewItem = new ListViewItem(row);
            listView1.Items.Add(listViewItem);*/
        }

        private struct QueryArguments
        {
            public QueryArguments(int QueryID, string Query)
                : this()
            {
                this.QueryID = QueryID;
                this.Query = Query;
            }

            public int QueryID { get; set; }
            public string Query { get; set; }
            public List<KeyValuePair<int, string>> Result { get; set; }
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void importFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Debug.WriteLine(folderBrowserDialog1.SelectedPath);
                StartFolderImport(folderBrowserDialog1.SelectedPath);
            }
        }

        public void StartFileImport(string query)
        {
            QueryArguments args = new QueryArguments(NextID++, query);
            
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_FileImportDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);

        }

        public void StartFolderImport(string query)
        {
            QueryArguments args = new QueryArguments(NextID++, query);

            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_FolderImportDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);
        }

        private void backgroundWorker_FolderImportDoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            Debug.WriteLine(args.Query);
            ImportIDs = WDBCommonObj.ImportFolder(args.Query, backgroundWorker);
            Debug.WriteLine(ImportIDs.ToString());
            args.Result = ImportIDs;
            e.Result = args.Result;
        }
        
        private void backgroundWorker_FileImportDoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            Debug.WriteLine(args.Query);
            ImportIDs.Add(new KeyValuePair <int, string >(ImportInternalID, WDBCommonObj.ImportFile(args.Query) ) ) ;
            e.Result = args.Result;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            string[] stringSeparators = new string[] { "|~|" };
            string[] split = e.UserState.ToString().Split(stringSeparators, StringSplitOptions.None);
            
            string title = "";
            string user = "";
            string message = "";
            string ImportID = "";
            string filehash = "";
            if (split[0] == "newrow")
            {
                Debug.WriteLine(split[1]);
                FileInfo f2 = new FileInfo(split[1]);

                string[] row = { "", "pferland", "", DateTime.Now.ToString("yyyy-MM-dd"), f2.Length.ToString(), split[1], split[2], "" , ""};
                var listViewItemNew = new ListViewItem(row);
                listView1.Items.Add(listViewItemNew);
            }
            else
            {

                foreach (string part in split)
                {
                    Debug.WriteLine(part + " \n--------------------\n");
                    string[] items_pre = part.Split('|');

                    foreach (var item in items_pre)
                    {
                        string[] stringSep = new string[] { "-~-" };
                        string[] items = item.Split(stringSep, StringSplitOptions.None);
                        switch (items[0])
                        {
                            case "title":
                                title = items[1];
                                break;
                            case "user":
                                user = items[1];
                                break;
                            case "importnum":
                                ImportID = items[1];
                                break;
                            case "message":
                                message = items[1];
                                break;
                            case "filehash":
                                filehash = items[1];
                                break;
                        }
                    }
                    Debug.WriteLine(" \n--------------------\n");
                    /*Debug.WriteLine(items_pre[1] + "\n--------------------\n" +items_pre.Length);

                    string[] stringSep = new string[] { "-~-" };
                    string[] items = items_pre[1].Split(stringSep, StringSplitOptions.None);
                    Debug.WriteLine(items[0]);
                    switch(items[0])
                    {
                        case "title":
                            title = items[1];
                            break;
                        case "user":
                            user = items[1];
                            break;
                        case "importnum":
                            ImportID = items[1];
                            break;
                        case "message":
                            message = items[1];
                            break;
                        case "filehash":
                            filehash = items[1];
                            break;
                    }*/
                }
                Debug.WriteLine(filehash);

                ListViewItem listViewItem = listView1.FindItemWithText(filehash);
                
                Debug.WriteLine(listViewItem.SubItems[1].Text + " ==== " + listViewItem.SubItems.Count);
                listViewItem.SubItems[0].Text = ImportID;
                listViewItem.SubItems[1].Text = user;
                listViewItem.SubItems[2].Text = title;
                listViewItem.SubItems[8].Text = message;
            }
            //Debug.WriteLine(e.ProgressPercentage.ToString());
        }

        private void backgroundWorker_ImportWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //List<KeyValuePair<int, string>> args = e.Result as List<KeyValuePair<int, string>>;
            //Debug.WriteLine("List of Imports:");

            /*foreach (KeyValuePair<int, string> item in args)
            {
                //Debug.WriteLine(item.Value.ToString());
                string[] stringSeparators = new string[] { "|-~-|" };
                string[] split = item.Value.ToString().Split(stringSeparators, StringSplitOptions.None);
                foreach(string part in split)
                {
                    Debug.WriteLine(part);
                }
                string[] row = { "1", "Pferland", "Test Import", "2016-01-05", "248.02kb", "WDB_Upload_20160105.vs1", "DUMMYHASHSUM", "N/A", "Not Importing" };
                var listViewItem = new ListViewItem(row);
                listView1.Items.Add(listViewItem);
            }*/
            }

        }
}
