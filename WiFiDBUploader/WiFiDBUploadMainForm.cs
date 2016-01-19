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

            InitTimer();
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

        private Timer timer1;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(CheckForUpdates);
            timer1.Interval = 10000; // in miliseconds
            timer1.Start();
        }

        private void CheckForUpdates(object sender, EventArgs e)
        {
            Debug.WriteLine("--------BackGround Update Check Begin--------");
            //BW.ReportProgress(0, "newrow|~|" + vs1 + "|~|" + hashish);

            foreach (ListViewItem item in listView1.Items)
            {
                Debug.WriteLine(item.SubItems[9].Text);
                switch(item.SubItems[9].Text)
                {
                    case "Waiting":
                        StartUpdateWiaitng(item.SubItems[6].Text);
                        break;
                    case "Importing":
                        StartUpdateImporting(item.SubItems[6].Text);
                        break;
                }
            }
            Debug.WriteLine("--------BackGround Update Check End--------");

            //string response = WDBAPIObj.ApiGetWaitingImports();
            //WDBAPIObj.ParseApiResponse(response);

        }

        public void StartUpdateWiaitng(string query)
        {
            QueryArguments args = new QueryArguments(NextID++, query);

            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_UpdateWaitingDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_UpdateListViewProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);
        }

        public void StartUpdateImporting(string query)
        {

        }

        public void StartFileImport(string query)
        {
            QueryArguments args = new QueryArguments(NextID++, query);
            
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_FileImportDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ImportProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);

        }

        public void StartFolderImport(string query)
        {
            QueryArguments args = new QueryArguments(NextID++, query);

            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_FolderImportDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ImportProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);
        }

        private void backgroundWorker_UpdateWaitingDoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            Debug.WriteLine(args.Query);
            WDBCommonObj.GetWaiting(args.Query, backgroundWorker);
            //Debug.WriteLine(ImportIDs.ToString());
            e.Result = "Waiting Done";
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

        private void backgroundWorker_ImportProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            string[] stringSeparators = new string[] { "|~|" };
            string[] split = e.UserState.ToString().Split(stringSeparators, StringSplitOptions.None);
            
            string title = "";
            string user = "";
            string message = "";
            string ImportID = "";
            string filehash = "";

            Debug.WriteLine(split[0]);

            string[] stringSep1 = new string[] { ":" };
            string[] stringSep2 = new string[] { "-~-" };

            switch (split[0])
            {
                case "newrow":
                    Debug.WriteLine(split[1]);
                    FileInfo f2 = new FileInfo(split[1]);

                    string[] row = { "", "pferland", "", DateTime.Now.ToString("yyyy-MM-dd"), f2.Length.ToString(), split[1], split[2], "", "", "Waiting" };
                    var listViewItemNew = new ListViewItem(row);
                    listView1.Items.Add(listViewItemNew);
                    break;
                case "error":
                    Debug.WriteLine(split[0]);
                    string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);


                    string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);

                    Debug.WriteLine(items_err[0]);
                    Debug.WriteLine(SplitData[0]);
                    Debug.WriteLine(SplitData[1]);

                    ListViewItem listViewItem1 = listView1.FindItemWithText(SplitData[1].TrimStart(' '));

                    Debug.WriteLine(listViewItem1.SubItems[1].Text + " ==== " + listViewItem1.SubItems.Count);
                    listViewItem1.SubItems[8].Text = SplitData[0];
                    break;
                default:
                    foreach (string part in split)
                    {
                        Debug.WriteLine(part + " \n--------------------\n");
                        string[] items_pre = part.Split('|');

                        foreach (var item in items_pre)
                        {
                            string[] items = item.Split(stringSep2, StringSplitOptions.None);
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
                    }
                    Debug.WriteLine(filehash);

                    ListViewItem listViewItem = listView1.FindItemWithText(filehash);

                    Debug.WriteLine(listViewItem.SubItems[1].Text + " ==== " + listViewItem.SubItems.Count);
                    listViewItem.SubItems[0].Text = ImportID;
                    listViewItem.SubItems[1].Text = user;
                    listViewItem.SubItems[2].Text = title;
                    listViewItem.SubItems[8].Text = message;
                    break;
            }
            //Debug.WriteLine(e.ProgressPercentage.ToString());
        }

        private void backgroundWorker_UpdateListViewProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            string[] stringSeparators = new string[] { "|~|" };
            string[] split = e.UserState.ToString().Split(stringSeparators, StringSplitOptions.None);

            string title = "";
            string user = "";
            string message = "";
            string status = "";
            string ImportID = "";
            string filehash = "";
            Debug.WriteLine(split[0]);

            string[] stringSep1 = new string[] { ":" };
            string[] stringSep2 = new string[] { "-~-" };

            switch (split[0])
            {
                case "newrow":
                    Debug.WriteLine(split[1]);
                    FileInfo f2 = new FileInfo(split[1]);

                    string[] row = { "", "pferland", "", DateTime.Now.ToString("yyyy-MM-dd"), f2.Length.ToString(), split[1], split[2], "", "", "Waiting" };
                    var listViewItemNew = new ListViewItem(row);
                    listView1.Items.Add(listViewItemNew);
                    break;
                case "error":
                    Debug.WriteLine(split[0]);
                    string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);

                    
                    string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);

                    Debug.WriteLine(items_err[0]);
                    Debug.WriteLine(SplitData[0]);
                    Debug.WriteLine(SplitData[1]);

                    ListViewItem listViewItem1 = listView1.FindItemWithText(SplitData[1].TrimStart(' '));

                    Debug.WriteLine(listViewItem1.SubItems[1].Text + " ==== " + listViewItem1.SubItems.Count);
                    listViewItem1.SubItems[8].Text = SplitData[0];
                    break;
                default:
                    foreach (string part in split)
                    {
                        Debug.WriteLine(part + " \n--------------------\n");
                        string[] items_pre = part.Split('|');

                        foreach (var item in items_pre)
                        {
                            string[] items = item.Split(stringSep2, StringSplitOptions.None);
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
                    }
                    Debug.WriteLine(filehash);

                    ListViewItem listViewItem = listView1.FindItemWithText(filehash);

                    Debug.WriteLine(listViewItem.SubItems[1].Text + " ==== " + listViewItem.SubItems.Count);
                    listViewItem.SubItems[0].Text = ImportID;
                    listViewItem.SubItems[1].Text = user;
                    listViewItem.SubItems[2].Text = title;
                    listViewItem.SubItems[8].Text = message;
                    break;
            }
            //Debug.WriteLine(e.ProgressPercentage.ToString());
        }

        private void backgroundWorker_ImportWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // maybe do something?
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


    }
}
