using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WiFiDBUploader
{
    public partial class WiFiDBUploadMainForm : Form
    {
        private WDBAPI.WDBAPI WDBAPIObj;
        private WDBCommon.WDBCommon WDBCommonObj;
        private int NextID = 0;
        private int ImportInternalID = 0;
        private List<KeyValuePair<int, string>> ImportIDs;
        private Timer timer1;
        private Timer timer2;

        private bool   AutoUploadFolder;
        private string AutoUploadFolderPath;
        private bool   ArchiveImports;
        private string ArchiveImportsFolderPath;
        private int    AutoCloseTimerSeconds;
        private int    TimerSeconds;
        private string DefaultImportNotes;
        private string DefaultImportTitle;
        private bool   UseDefaultImportValues;
        private string SQLiteFile;

        private List<ServerObj> ServerList;

        private string SelectedServer;

        private string ServerAddress;
        private string ApiPath;
        private string Username;
        private string ApiKey;
        private string ApiCompiledPath;

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

        public WiFiDBUploadMainForm()
        {
            //Debug.WriteLine("Start of Call: new WDBAPI.WDBAPI();");
            WDBAPIObj = new WDBAPI.WDBAPI();
            //Debug.WriteLine("End of Call: new WDBAPI.WDBAPI();");

            //Debug.WriteLine("Start of Call: LoadSettings()");
            LoadSettings();
            //Debug.WriteLine("End of Call: LoadSettings()");

            //Debug.WriteLine("Start of Call: new WDBCommon.WDBCommon(SQLiteFile);");
            WDBCommonObj = new WDBCommon.WDBCommon(SQLiteFile);
            //Debug.WriteLine("End of Call: new WDBCommon.WDBCommon(SQLiteFile);");

            //Debug.WriteLine("Start of Call: InitClasses()");
            InitClasses();
            //Debug.WriteLine("End of Call: InitClasses()");

            //Debug.WriteLine("Start of Call: InitializeComponent()");
            InitializeComponent();
            //Debug.WriteLine("End of Call: InitializeComponent()");

            //Debug.WriteLine("Start of Call: LoadDbDataIntoUI()");
            LoadDbDataIntoUI();
            //Debug.WriteLine("End of Call: LoadDbDataIntoUI()");

            //Debug.WriteLine("Start of Call: InitTimer();");
//            InitTimer();
            //Debug.WriteLine("End of Call: InitTimer();");

            //Debug.WriteLine("Start of Call: AutoUploadCheck();");
            AutoUploadCheck();
            //Debug.WriteLine("End of Call: AutoUploadCheck();");
        }

        private void LoadDbDataIntoUI()
        {
            List<WDBSQLite.ImportRow> ImportedRows = WDBCommonObj.GetImportRows();
            foreach( WDBSQLite.ImportRow Row in ImportedRows)
            {

                Debug.WriteLine("\n------------------\n------------------\nCreate ROW from SQL: " + Row.ImportID.ToString() + " |=| " + Row.Username + " |=| " + Row.ImportTitle + " |=| " + Row.Message + " |=| " + Row.Status + "\n------------------\n------------------\n");
                string[] row = { Row.ImportID.ToString(), Row.Username, Row.ImportTitle, Row.DateTime,
                    Row.FileSize, Row.FileName, Row.FileHash, Row.Status, Row.Message };

                var listViewItemNew = new ListViewItem(row);
                listView1.Items.Add(listViewItemNew);
                /* Was trying to update a row, when it should be inserted. This is why you dont code while being up for 30hrs.
                listViewItem.SubItems[0].Text = Row.ImportID.ToString();
                listViewItem.SubItems[1].Text = Row.Username;
                listViewItem.SubItems[2].Text = Row.ImportTitle;
                listViewItem.SubItems[3].Text = Row.DateTime;
                listViewItem.SubItems[4].Text = Row.FileSize;
                listViewItem.SubItems[5].Text = Row.FileName;
                listViewItem.SubItems[6].Text = Row.FileHash;
                listViewItem.SubItems[7].Text = Row.Status;
                listViewItem.SubItems[8].Text = Row.Message;
                */
            }
        }

        private void AutoUploadCheck()
        {
            if(AutoUploadFolder == true && SelectedServer != null)
            {
                StartFolderImport(AutoUploadFolderPath);
            }
            else
            {
                //Debug.WriteLine("Auto Upload Disabled, or No Server Selected.");
            }
        }
        
        public void WriteLog(string message)
        {
            string line = "[" + DateTime.Now.ToString("yyyy-MM-dd") + "]" +"[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "[" + message + "]";
            System.IO.StreamWriter file = new System.IO.StreamWriter(".\\logs\\Trace.log", true);
            file.WriteLine(line);
            file.Close();
        }

        private void InitClasses()
        {
            WDBCommonObj.AutoUploadFolder = AutoUploadFolder;
            WDBCommonObj.AutoUploadFolderPath = AutoUploadFolderPath;
            WDBCommonObj.ArchiveImports = ArchiveImports;
            WDBCommonObj.ArchiveImportsFolderPath = ArchiveImportsFolderPath;

            WDBCommonObj.DefaultImportNotes = DefaultImportNotes;
            WDBCommonObj.DefaultImportTitle = DefaultImportTitle;
            WDBCommonObj.UseDefaultImportValues = UseDefaultImportValues;

            WDBCommonObj.ServerAddress = ServerAddress;
            WDBCommonObj.ApiPath = ApiPath;
            WDBCommonObj.Username = Username;
            WDBCommonObj.ApiKey = ApiKey;
            WDBCommonObj.ApiCompiledPath = ApiCompiledPath;

            //Debug.WriteLine("Start of Call: WDBCommonObj.initApi();");
            WDBCommonObj.initApi();
        }

        private void InitTimer()
        {
            if (ApiCompiledPath != null)
            {
                //Debug.WriteLine("Not running background threads till there is a server selected. whats the point if there is no server?");
            }
            else
            {
                if (timer1 != null)
                {
                    //Debug.WriteLine("Stopping Import update background thread.");
                    timer1.Stop();
                    timer1.Dispose();
                    timer1 = null;
                }
                //Debug.WriteLine("Starting Import update background thread.");
                timer1 = new System.Windows.Forms.Timer();
                timer1.Tick += new EventHandler(CheckForUpdates);
                timer1.Interval = 10000; // in miliseconds
                timer1.Start();

                StartGetDaemonStats(); //prep the tables.

                if (timer2 != null)
                {
                    //Debug.WriteLine("Stopping Daemon update background thread.");
                    //Debug.WriteLine("Restart Timer 2");
                    timer2.Stop();
                    timer2.Dispose();
                    timer2 = null;
                }
                //Debug.WriteLine("Starting Daemon update background thread.");
                timer2 = new System.Windows.Forms.Timer();
                timer2.Tick += new EventHandler(CheckForDaemonUpdates);
                timer2.Interval = 4000; // in miliseconds
                timer2.Start();
            }
        }

        private void CreateRegistryKeys(Microsoft.Win32.RegistryKey rootKey)
        {
            Microsoft.Win32.RegistryKey ServersKey;
            rootKey.SetValue("DefaultImportTitle", "Generic Import Title");
            rootKey.SetValue("DefaultImportNotes", "Generic blable about the import, maybe some notes on where you drove or what you saw?");
            rootKey.SetValue("UseDefaultImportValues", "True");
            rootKey.SetValue("AutoUploadFolder", "True");
            rootKey.SetValue("AutoUploadFolderPath", "");
            rootKey.SetValue("ArchiveImports", "False");
            rootKey.SetValue("ArchiveImportsFolderPath", "");
            rootKey.SetValue("AutoCloseTimerSeconds", "30");
            
            ServersKey = rootKey.CreateSubKey("Servers");
        }

        private void LoadSettings()
        {
            Microsoft.Win32.RegistryKey rootKey;
            rootKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("Vistumbler").CreateSubKey("WiFiDB").CreateSubKey("Uploader");
            string[] SubKeys = rootKey.GetSubKeyNames();
            if (SubKeys.Count() == 0)
            {
                //Debug.WriteLine("Servers SubKey not found, creating Default Structure.");
                CreateRegistryKeys(rootKey);
            }
            Microsoft.Win32.RegistryKey ServerSubkeys = rootKey.CreateSubKey("Servers");

            foreach (string value in rootKey.GetValueNames())
            {
                //Debug.WriteLine(value);

                switch(value)
                {
                    case "AutoUploadFolder":
                    //    //Debug.WriteLine(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                        AutoUploadFolder = Convert.ToBoolean(rootKey.GetValue(value));
                        break;
                    case "AutoUploadFolderPath":
                        //Debug.WriteLine(value + " : " + rootKey.GetValue(value).ToString());
                        AutoUploadFolderPath = rootKey.GetValue(value).ToString();
                        break;
                    case "AutoCloseTimerSeconds":
                        //Debug.WriteLine(value + " : " + rootKey.GetValue(value).ToString());
                        AutoCloseTimerSeconds = Int32.Parse(rootKey.GetValue(value).ToString());
                        break;
                    case "ArchiveImports":
                        //Debug.WriteLine(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                        ArchiveImports = Convert.ToBoolean(rootKey.GetValue(value));
                        break;
                    case "ArchiveImportsFolderPath":
                        //Debug.WriteLine(value + " : " + rootKey.GetValue(value).ToString());
                        ArchiveImportsFolderPath = rootKey.GetValue(value).ToString();
                        break;
                    case "DefaultImportNotes":
                        //Debug.WriteLine(value + " : " + rootKey.GetValue(value).ToString());
                        DefaultImportNotes = rootKey.GetValue(value).ToString();
                        break;
                    case "DefaultImportTitle":
                        //Debug.WriteLine(value + " : " + rootKey.GetValue(value).ToString());
                        DefaultImportTitle = rootKey.GetValue(value).ToString();
                        break;
                    case "UseDefaultImportValues":
                        //Debug.WriteLine(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                        UseDefaultImportValues = Convert.ToBoolean(rootKey.GetValue(value));
                        break;
                    case "SQLiteFile":
                        //Debug.WriteLine(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                        SQLiteFile = rootKey.GetValue(value).ToString();
                        break;
                }
            }
            ServerList = new List<ServerObj>();
            int Increment = 0;
            foreach (string subitem in ServerSubkeys.GetSubKeyNames())
            {
                //Debug.WriteLine("-------------------\n"+subitem);
                Microsoft.Win32.RegistryKey ServerKey = ServerSubkeys.CreateSubKey(subitem);

                ServerObj Server = new ServerObj();
                //Debug.WriteLine("ServerAddress = "+ ServerKey.GetValue("ServerAddress").ToString());
                //Debug.WriteLine("ApiPath = " + ServerKey.GetValue("ApiPath").ToString());
                //Debug.WriteLine("Username = " + ServerKey.GetValue("Username").ToString());
                //Debug.WriteLine("ApiKey = " + ServerKey.GetValue("ApiKey").ToString());

                Server.ID = Increment;
                Server.ServerAddress = ServerKey.GetValue("ServerAddress").ToString();
                Server.ApiPath = ServerKey.GetValue("ApiPath").ToString();
                Server.Username = ServerKey.GetValue("Username").ToString();
                Server.ApiKey = ServerKey.GetValue("ApiKey").ToString();
                Server.Selected = Convert.ToBoolean(ServerKey.GetValue("Selected"));

                if(Server.Selected)
                {
                    ServerAddress = Server.ServerAddress.ToString();
                    SelectedServer = ServerAddress.Replace("https://", "").Replace("http://", "");
                    ApiPath = Server.ApiPath.ToString();
                    Username = Server.Username.ToString();
                    ApiKey = Server.ApiKey.ToString();
                    ApiCompiledPath = Server.ServerAddress + Server.ApiPath;
                }
                ServerList.Add(Server);
                Increment++;
            }
            

            if (ServerAddress == null)
            {
                MessageBox.Show("There is no selected server. Go to Settings-> WiFiDB Server. Select a server from the drop down, if there is none, add one with the +");
            }
        }

        private void WriteServerSettings()
        {
            /*
                Screw the app.config file, registry is easier to manage.
            */
            Microsoft.Win32.RegistryKey rootKey;
            rootKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("Vistumbler").CreateSubKey("WiFiDB").CreateSubKey("Uploader").CreateSubKey("Servers");

            List<ServerNameObj> VarNameList = new List<ServerNameObj>();
            foreach (ServerObj server in ServerList)
            {
                //Debug.WriteLine(server.ServerAddress);

                Microsoft.Win32.RegistryKey ServerKey = rootKey.CreateSubKey( server.ServerAddress.ToString().Replace("https://", "").Replace("http://", "") );
                ServerKey.SetValue("ServerAddress", server.ServerAddress);
                ServerKey.SetValue("ApiPath", server.ApiPath);
                ServerKey.SetValue("Username", server.Username);
                ServerKey.SetValue("ApiKey", server.ApiKey);
                ServerKey.SetValue("Selected", server.Selected);

                ServerNameObj nameObj = new ServerNameObj();
                nameObj.ServerName = server.ServerAddress.ToString().Replace("https://", "").Replace("http://", "");
                VarNameList.Add(nameObj);

            }

            var RegName = rootKey.GetSubKeyNames();
            List<ServerNameObj> RegNameList = new List<ServerNameObj>();

            foreach ( string subkey in RegName)
            {
                ServerNameObj nameObj = new ServerNameObj();
                nameObj.ServerName = subkey;
                RegNameList.Add(nameObj);
            }
            
            var list3 = RegNameList.Except(VarNameList, new IdComparer()).ToList();
            //Debug.WriteLine("Servers not in the list now:");
            foreach(ServerNameObj ServerName in list3)
            {
                //Debug.WriteLine(ServerName.ServerName);
                rootKey.DeleteSubKeyTree(ServerName.ServerName);
            }

            LoadSettings();
            InitClasses();
        }

        private void WriteGlobalSettings()
        {
            /*
                Screw the app.config file, registry is easier to manage.
            */
            Microsoft.Win32.RegistryKey rootKey;
            rootKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("Vistumbler").CreateSubKey("WiFiDB").CreateSubKey("Uploader");

            rootKey.SetValue("AutoUploadFolder", AutoUploadFolder);
            rootKey.SetValue("AutoCloseTimerSeconds", AutoCloseTimerSeconds);
            rootKey.SetValue("AutoUploadFolderPath", AutoUploadFolderPath);
            rootKey.SetValue("ArchiveImports", ArchiveImports);
            rootKey.SetValue("ArchiveImportsFolderPath", ArchiveImportsFolderPath);
            rootKey.SetValue("DefaultImportNotes", DefaultImportNotes);
            rootKey.SetValue("DefaultImportTitle", DefaultImportTitle);
            rootKey.SetValue("UseDefaultImportValues", UseDefaultImportValues);

            LoadSettings();
            InitClasses();
        }




        /*
            Background init Funtions.
        */
        private void CheckForDaemonUpdates(object sender, EventArgs e)
        {
            StartGetDaemonStats();
        }

        private void CheckForUpdates(object sender, EventArgs e)
        {
            //Debug.WriteLine("-------- BackGround Update Check Begin --------");
            foreach (ListViewItem item in listView1.Items)
            {
                //Debug.WriteLine(item.SubItems[8].Text);
                StartUpdateWiaitng(item.SubItems[6].Text);
            }
            //Debug.WriteLine("-------- BackGround Update Check End --------");
        }
        
        private void StartUpdateDaemonStats()
        {
            //Debug.WriteLine("Start Call: StartUpdateDaemonStats");
            QueryArguments args = new QueryArguments(NextID++, "");
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_UpdateDaemonStatsDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_GetDaemonListViewProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);
            //Debug.WriteLine("End Call: StartUpdateDaemonStats");
        }

        private void StartGetDaemonStats()
        {
            //Debug.WriteLine("Start Call: StartGetDaemonStats");
            QueryArguments args = new QueryArguments(NextID++, "");
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_GetDaemonStatsDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_GetDaemonListViewProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);
            //Debug.WriteLine("End Call: StartGetDaemonStats");
        }

        private void StartUpdateWiaitng(string query)
        {
            //Debug.WriteLine("Start Call: StartUpdateWaiting");
            QueryArguments args = new QueryArguments(NextID++, query);
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_UpdateWaitingDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_UpdateListViewProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);
            //Debug.WriteLine("End Call: StartUpdateWaiting");
        }

        private void StartFileImport(string query)
        {
            QueryArguments args = new QueryArguments(NextID++, query);
            
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_FileImportDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ImportProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);

        }

        private void StartFolderImport(string query)
        {
            QueryArguments args = new QueryArguments(NextID++, query);

            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_FolderImportDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ImportProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_ImportCompleted);
            backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);
        }

        


        //
        // Menu Click Function Items
        //

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ImportTitle;
            string ImportNotes;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "VS1 files (*.VS1)|*.VS1|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if(UseDefaultImportValues)
                    {
                        ImportTitle = DefaultImportTitle;
                        ImportNotes = DefaultImportNotes;
                    }else
                    {
                        //Ask For Import Title and Notes. They dont want to use the defaults.
                        ImportTitle = "Temp Title";
                        ImportNotes = "Temp Notes";
                    }
                    string response = WDBAPIObj.ApiImportFile(openFileDialog1.FileName, ImportTitle, ImportNotes);
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
                //Debug.WriteLine(folderBrowserDialog1.SelectedPath);
                StartFolderImport(folderBrowserDialog1.SelectedPath);
            }
        }

        private void wifidbSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WiFiDB_Settings SettingsForm = new WiFiDB_Settings();
            SettingsForm.ServerList = ServerList;
            SettingsForm.InitForm();

            if (SettingsForm.ShowDialog() == DialogResult.OK)
            {
                ServerList = SettingsForm.ServerList;
                this.SelectedServer = SettingsForm.SelectedServer;
                foreach (ServerObj server in ServerList)
                {
                    if (server.ServerAddress.ToString().Replace("https://", "").Replace("http://", "") == SelectedServer)
                    {
                        this.ServerAddress = server.ServerAddress;
                        this.ApiPath = server.ApiPath;
                        this.Username = server.Username;
                        this.ApiKey = server.ApiKey;
                        server.Selected = true;
                    }
                    else
                    {
                        server.Selected = false;
                    }
                }
                this.ApiCompiledPath = this.ServerAddress + this.ApiPath;
            }
            WriteServerSettings();
            InitTimer();
        }

        private void importSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Import_Settings ImportSettingsForm = new Import_Settings();
            ImportSettingsForm.ImportNotes = DefaultImportNotes;
            ImportSettingsForm.ImportTitle = DefaultImportTitle;
            ImportSettingsForm.UseImportDefaultValues = UseDefaultImportValues;

            if (ImportSettingsForm.ShowDialog() == DialogResult.OK)
            {
                this.DefaultImportTitle = ImportSettingsForm.ImportTitle;
                this.DefaultImportNotes = ImportSettingsForm.ImportNotes;
                this.UseDefaultImportValues = ImportSettingsForm.UseImportDefaultValues;
                WriteGlobalSettings();
            }
        }

        private void autoSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Auto_Upload_Settings AutoForm = new Auto_Upload_Settings();
            //Debug.WriteLine(AutoUploadFolder);
            AutoForm.AutoUploadFolder = AutoUploadFolder;
            AutoForm.AutoUploadFolderPath = AutoUploadFolderPath;
            AutoForm.ArchiveImports = ArchiveImports;
            AutoForm.ArchiveImportsFolderPath = ArchiveImportsFolderPath;
            AutoForm.AutoCloseTimerSeconds = AutoCloseTimerSeconds.ToString();

            if (AutoForm.ShowDialog() == DialogResult.OK)
            {
                //Debug.WriteLine("AutoForm.AutoUploadFolder: " + AutoForm.AutoUploadFolder);
                //Debug.WriteLine("AutoForm.AutoUploadFolderPath: " + AutoForm.AutoUploadFolderPath);
                //Debug.WriteLine("AutoForm.ArchiveImports: " + AutoForm.ArchiveImports);
                //Debug.WriteLine("AutoForm.ArchiveImportsFolderPath: " + AutoForm.ArchiveImportsFolderPath);

                AutoUploadFolder = Convert.ToBoolean(AutoForm.AutoUploadFolder);
                AutoUploadFolderPath = AutoForm.AutoUploadFolderPath;
                ArchiveImports = Convert.ToBoolean(AutoForm.ArchiveImports);
                ArchiveImportsFolderPath = AutoForm.ArchiveImportsFolderPath;
                AutoCloseTimerSeconds = Int32.Parse(AutoForm.AutoCloseTimerSeconds);

                //Debug.WriteLine("this.AutoUploadFolder: " + this.AutoUploadFolder);
                //Debug.WriteLine("this.AutoUploadFolderPath: " + this.AutoUploadFolderPath);
                //Debug.WriteLine("this.ArchiveImports: " + this.ArchiveImports);
                //Debug.WriteLine("this.ArchiveImportsFolderPath: " + this.ArchiveImportsFolderPath);

                WriteGlobalSettings();
            }
        }




        //
        // Do Work Functions
        //

        private void backgroundWorker_UpdateDaemonStatsDoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            //Debug.WriteLine(args.Query);
            WDBCommonObj.GetDaemonStatuses(args.Query, backgroundWorker);
            ////Debug.WriteLine(ImportIDs.ToString());
            //e.Result = "Waiting Done";
        }

        private void backgroundWorker_GetDaemonStatsDoWork(object sender, DoWorkEventArgs e)
        {
            //Debug.WriteLine(ServerAddress);
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            //Debug.WriteLine(args.Query);
            WDBCommonObj.GetDaemonStatuses(args.Query, backgroundWorker);
        }

        private void backgroundWorker_UpdateWaitingDoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            //Debug.WriteLine(args.Query);
            WDBCommonObj.GetHashStatus(args.Query, backgroundWorker);
            ////Debug.WriteLine(ImportIDs.ToString());
            //e.Result = "Waiting Done";
        }

        private void backgroundWorker_FolderImportDoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            //Debug.WriteLine(args.Query);
            ImportIDs = WDBCommonObj.ImportFolder(args.Query, DefaultImportTitle, DefaultImportNotes, backgroundWorker);
            if (ImportIDs.Count > 0)
            {
                Debug.WriteLine(ImportIDs[0].Value);
                if (ImportIDs[0].Value == "Error with API")
                {
                    MessageBox.Show("Error With API.");
                }else if (ImportIDs[0].Value == "Already Imported.")
                {
                    Debug.WriteLine("File Already Imported.");
//                    MessageBox.Show("Error With API.");
                }else
                {
                    args.Result = ImportIDs;
                    e.Result = args.Result;
                }
            }
        }
        
        private void backgroundWorker_FileImportDoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            //Debug.WriteLine(args.Query);
            ImportIDs.Add(new KeyValuePair <int, string >(ImportInternalID, WDBCommonObj.ImportFile(args.Query, DefaultImportTitle, DefaultImportNotes, backgroundWorker) ) ) ;
            e.Result = args.Result;
        }





        //
        // Progress Changed Functions
        //
        private void backgroundWorker_ImportProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            WDBSQLite.ImportRow ImportRowObj = new WDBSQLite.ImportRow();

            string[] stringSeparators = new string[] { "|~|" };
            string[] split = e.UserState.ToString().Split(stringSeparators, StringSplitOptions.None);
            
            string title = "";
            string user = "";
            string message = "";
            string ImportID = "";
            string filehash = "";

            Debug.WriteLine( e.UserState.ToString() );

            string[] stringSep1 = new string[] { ":" };
            string[] stringSep2 = new string[] { "-~-" };

            Debug.WriteLine("Split[0]" + split[0]);
            switch (split[0])
            {
                case "newrow":
                    FileInfo FileSizeString = new FileInfo(split[1]);
                    string Date_Time = DateTime.Now.ToString("yyyy-MM-dd    HH:mm:ss");
                    string StatusStr = "Uploading";
                    string MessageStr = "Uploading File to WiFiDB...";
                    
                    ImportRowObj.Username = Username;
                    ImportRowObj.ImportTitle = "";
                    ImportRowObj.DateTime = Date_Time;
                    ImportRowObj.FileSize = FileSizeString.Length.ToString();
                    ImportRowObj.FileName = split[1];
                    ImportRowObj.FileHash = split[2];
                    ImportRowObj.Status = StatusStr;
                    ImportRowObj.Message = MessageStr;

                    WDBCommonObj.InsertImportRow(ImportRowObj); // Insert import information into SQLite.

                    string[] row = { "", Username, "", Date_Time, FileSizeString.Length.ToString(), split[1], split[2], StatusStr, MessageStr };
                    Debug.WriteLine("\n------------------\n------------------\nNew ROW: " + "" + " |=| " + Username + " |=| " + "" + " |=| " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " |=| " 
                        + FileSizeString.Length.ToString() + " |=| " + split[1] + " |=| " + split[2] + " |=| " + "" + " |=| " + "Uploading File to WiFiDB..." + " |=| " + "Uploading" + " \n------------------\n------------------\n");

                    var listViewItemNew = new ListViewItem(row);
                    listView1.Items.Add(listViewItemNew);
                    break;
                case "error":
                    //Debug.WriteLine(split[0]);
                    string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);


                    string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);

                    //Debug.WriteLine(items_err[0]);
                    //Debug.WriteLine(SplitData[0]);
                    //Debug.WriteLine(SplitData[1]);

                    ListViewItem listViewItem1 = listView1.FindItemWithText(SplitData[1].TrimStart(' '));

                    //Debug.WriteLine(listViewItem1.SubItems[1].Text + " ==== " + listViewItem1.SubItems.Count);
                    listViewItem1.SubItems[8].Text = SplitData[0];
                    break;
                default:
                    foreach (string part in split)
                    {
                        //Debug.WriteLine(part + " \n--------------------\n");
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
                        //Debug.WriteLine(" \n--------------------\n");
                    }
                    //Debug.WriteLine(filehash);
                    ListViewItem listViewItem = listView1.FindItemWithText(filehash);
                    if(user != "" && filehash != "")
                    {
                        ImportRowObj.ImportID = Int32.Parse(ImportID);
                        ImportRowObj.ImportTitle = title;
                        ImportRowObj.FileHash = filehash;
                        ImportRowObj.Status = "Waiting";
                        ImportRowObj.Message = message;

                        WDBCommonObj.UpdateImportRow(ImportRowObj); // Update Import row information in SQLite.

                        Debug.WriteLine("\n------------------\n------------------\nUpdate ROW: " + ImportID + " |=| " + user + " |=| " + title + " |=| " + "Waiting" + " |=| " + message + "\n------------------\n------------------\n");
                        listViewItem.SubItems[0].Text = ImportID;
                        listViewItem.SubItems[1].Text = user;
                        listViewItem.SubItems[2].Text = title;
                        listViewItem.SubItems[7].Text = "Waiting";
                        listViewItem.SubItems[8].Text = message;
                    }
                    break;
            }
            ////Debug.WriteLine(e.ProgressPercentage.ToString());
        }

        private void backgroundWorker_UpdateListViewProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            WDBSQLite.ImportRow ImportRowObj = new WDBSQLite.ImportRow();

            string[] stringSeparators = new string[] { "|~|" };
            string[] stringSep1 = new string[] { ":" };
            string[] stringSep2 = new string[] { "-~-" };
            string[] split = e.UserState.ToString().Split(stringSeparators, StringSplitOptions.None);

            string title = "";
            string user = "";
            string message = "";
            string status = "";
            string ImportID = "";
            string filehash = "";
            //Debug.WriteLine("========== Update Listview Start ==========");
            //Debug.WriteLine(split[0]);
            switch (split[0])
            {
                case "error":
                    //Debug.WriteLine(split[0]);
                    string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);
                    string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);

                    //Debug.WriteLine(items_err[0]);
                    //Debug.WriteLine(SplitData[0]);
                    //Debug.WriteLine(SplitData[1]);

                    ListViewItem listViewItem1 = listView1.FindItemWithText(SplitData[1].TrimStart(' '));

                    //Debug.WriteLine(listViewItem1.SubItems[1].Text + " ==== " + listViewItem1.SubItems.Count);
                    listViewItem1.SubItems[8].Text = SplitData[0];
                    break;
                default:
                    //Debug.WriteLine(" \n--------- Start Parse ListView Update Return String -----------\n");

                    foreach (string part in split)
                    {
                        //Debug.WriteLine(" \n---------Part: " + part + "-----------\n");
                        string[] items_pre = part.Split('|');

                        foreach (var item in items_pre)
                        {
                            //Debug.WriteLine(" \n--------- Item: " + item + "-----------\n");
                            if(!item.Contains("-~-"))
                            {
                                switch (item.ToString())
                                {
                                    case "waiting":
                                        status = item;
                                        break;
                                    case "importing":
                                        status = item;
                                        break;
                                    case "finished":
                                        status = item;
                                        break;
                                }
                                //Debug.WriteLine("Message Loop Message: " + message + " ==== Item Value:" + item + " Part: " + part);
                                continue;
                            }
                            string[] items = item.Split(stringSep2, StringSplitOptions.None);
                            //Debug.WriteLine("---- Items and Values: " + items[0] + " :: Value: " + items[1]);
                            switch (items[0].ToString())
                            {
                                case "title":
                                    //Debug.WriteLine("Title? " + items[1]);
                                    title = items[1];
                                    break;
                                case "user":
                                    //Debug.WriteLine("user? " + items[1]);
                                    user = items[1];
                                    break;
                                case "id":
                                    //Debug.WriteLine("importnum? " + items[1]);
                                    ImportID = items[1];
                                    break;
                                case "message":
                                    //Debug.WriteLine("message? " + items[1]);
                                    message = items[1];
                                    break;
                                case "hash":
                                    //Debug.WriteLine("filehash? " + items[1]);
                                    filehash = items[1];
                                    break;
                                case "ap":
                                    //Debug.WriteLine("AP? " + items[1]);
                                    message = message + " - " + items[1];
                                    break;
                                case "tot":
                                    //Debug.WriteLine("This Of This? " + items[1]);
                                    message = message + " - " + items[1];
                                    break;
                            }
                        }
                    }
                    //Debug.WriteLine(filehash);
                    //Debug.WriteLine("End Parse Loop Message: " + message);
                    ListViewItem listViewItem = listView1.FindItemWithText(filehash);
                    if( (status == "finished") || ( (ImportID != "") && (message != "") ) )
                    {
                        ImportRowObj.ImportID = Int32.Parse(ImportID);
                        ImportRowObj.ImportTitle = title;
                        ImportRowObj.FileHash = filehash;
                        ImportRowObj.Status = status;
                        ImportRowObj.Message = message;

                        WDBCommonObj.UpdateImportRow(ImportRowObj); // Update Import row information in SQLite.

                        Debug.WriteLine("\n------------------\n------------------\nUpdate ROW: " + ImportID.ToString() + " |=| " + user + " |=| " + title + " |=| " + message + " |=| " + status + "\n------------------\n------------------\n");
                        listViewItem.SubItems[0].Text = ImportID;
                        listViewItem.SubItems[2].Text = title;
                        listViewItem.SubItems[7].Text = message;
                        listViewItem.SubItems[8].Text = status;
                    }
                    //Debug.WriteLine(" \n--------- End Parse ListView Update Return String -----------\n");
                    break;
            }
            //Debug.WriteLine("========== Update Listview End ==========");
        }
        
        private void backgroundWorker_GetDaemonListViewProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            string[] stringSeparators = new string[] { "|~|" };
            string[] split = e.UserState.ToString().Split(stringSeparators, StringSplitOptions.None);

            //Debug.WriteLine("========== Update Daemon ListView Start ==========");
            //Debug.WriteLine(e.UserState.ToString());
            //Debug.WriteLine(split[0]);

            string nodename = "";
            string pidfile = "";
            string pid = "";
            string pidmem = "";
            string pidtime = "";
            string cmd = "";
            string datetime_col = "";
            string[] stringSep1 = new string[] { ":" };
            string[] stringSep2 = new string[] { "-~-" };
            switch (split[0])
            {
                case "error":
                    //Debug.WriteLine(split[1]);
                    if(split[1] == "No_Daemons_Running")
                    {
                        //Debug.WriteLine("No Daemons running, do ListView CleanUp.");
                        if (this.listView2.Items.Count > 0)
                        {
                            foreach (ListViewItem item in this.listView2.Items)
                            {
                                item.Remove();
                            }
                        }else
                        {
                            //Debug.WriteLine("No rows, no need for cleanup...");
                        }
                    }
                    else
                    {
                        string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);
                        string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);
                        //Debug.WriteLine(items_err[0]);
                        //Debug.WriteLine(SplitData[0]);
                        //Debug.WriteLine(SplitData[1]);
                        //Debug.WriteLine( e.UserState.ToString() );
                    }
                    break;

                default:
                    
                    //Debug.WriteLine(" \n--------- Start Parse Daemon ListView Update Return String -----------\n");
                    int DaemonReturnCount = split.Count();
                    foreach (string part in split)
                    {
                        if(part == "")
                        {
                            DaemonReturnCount--;
                            continue;
                        }
                        //Debug.WriteLine(" \n---------Part: " + part + "-----------\n");
                        string[] items_pre = part.Split('|');

                        foreach (var item in items_pre)
                        {
                            //Debug.WriteLine(" \n--------- Item: " + item + "-----------\n");
                            if (!item.Contains("-~-"))
                            {
                                //Debug.WriteLine("Bad Message: " + item + " Part: " + part);
                                continue;
                            }
                            string[] items = item.Split(stringSep2, StringSplitOptions.None);
                            //Debug.WriteLine("---- Items and Values: " + items[0] + " :: Value: " + items[1]);
                            switch (items[0].ToString())
                            {
                                case "nodename":
                                    //Debug.WriteLine("nodename? " + items[1]);
                                    nodename = items[1];
                                    break;
                                case "pidfile":
                                    //Debug.WriteLine("pidfile? " + items[1]);
                                    pidfile = items[1];
                                    break;
                                case "pid":
                                    //Debug.WriteLine("pid? " + items[1]);
                                    pid = items[1];
                                    break;
                                case "pidtime":
                                    //Debug.WriteLine("runtime? " + items[1]);
                                    pidtime = items[1];
                                    break;
                                case "pidmem":
                                    //Debug.WriteLine("mem? " + items[1]);
                                    pidmem = items[1];
                                    break;
                                case "pidcmd":
                                    //Debug.WriteLine("cmd? " + items[1]);
                                    cmd = items[1];
                                    break;
                                case "date":
                                    //Debug.WriteLine("date anad time? " + items[1]);
                                    datetime_col = items[1];
                                    break;
                            }
                        }
                        if ((nodename != "") && (pid != "") && (pidfile != ""))
                        {
                            ListViewItem listViewItem = listView2.FindItemWithText(pidfile);
                            if ( listViewItem != null)
                            {
                                //Debug.WriteLine(listViewItem.SubItems[1].Text + " ==== " + listViewItem.SubItems.Count);
                                listViewItem.SubItems[0].Text = nodename;
                                listViewItem.SubItems[1].Text = pidfile;
                                listViewItem.SubItems[2].Text = pid;
                                listViewItem.SubItems[3].Text = pidtime;
                                listViewItem.SubItems[4].Text = pidmem;
                                listViewItem.SubItems[5].Text = cmd;
                                listViewItem.SubItems[6].Text = datetime_col;
                            }
                            else
                            {
                                string[] row = { nodename, pidfile, pid, pidtime.ToString(), pidmem.ToString(), cmd.ToString(), datetime_col.ToString() };
                                var listViewItemNew = new ListViewItem(row);
                                listView2.Items.Add(listViewItemNew);
                            }
                        }
                    }

                    //Check for rows that are not in the return, and remove them.
                    //Debug.WriteLine(DaemonReturnCount.ToString() + " --=--=-=-=-=-==-- " + listView2.Items.Count);
                    if ((listView2.Items.Count != DaemonReturnCount) && DaemonReturnCount != 0)
                    {
                        foreach (ListViewItem item in listView2.Items)
                        {
                            //Debug.WriteLine(e.UserState.ToString());
                            //Debug.WriteLine(item.SubItems[1].Text);

                            if(e.UserState.ToString().Contains(item.SubItems[1].Text))
                            {
                                //Debug.WriteLine(item.SubItems[1].Text + " Is in the return.");
                            }else
                            {
                                //Debug.WriteLine(item.SubItems[1].Text + " Is NOT in the return.");
                                //Debug.WriteLine("ListView CleanUp!");
                                item.Remove();
                            }
                        }
                    }
                    if(DaemonReturnCount == 0)
                    {
                        //Debug.WriteLine("DaemonReturnCount was 0...");
                        //Debug.WriteLine("UserStateString: " + e.UserState.ToString());
                    }
                    //Debug.WriteLine(" \n--------- End Parse Daemon ListView Update Return String -----------\n");
                    break;
            }
            //Debug.WriteLine("========== Update Daemon Listview End ==========");
        }


        /*
        //May not even be needed any more...
        private void backgroundWorker_UpvdateDaemonListViewProgressChanged(object sender, ProgressChangedEventArgs e)
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
            string[] stringSep1 = new string[] { ":" };
            string[] stringSep2 = new string[] { "-~-" };
            //Debug.WriteLine("========== Update Listview Start ==========");
            //Debug.WriteLine(split[0]);
            switch (split[0])
            {
                case "error":
                    //Debug.WriteLine(split[0]);
                    string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);


                    string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);

                    //Debug.WriteLine(items_err[0]);
                    //Debug.WriteLine(SplitData[0]);
                    //Debug.WriteLine(SplitData[1]);

                    ListViewItem listViewItem1 = listView1.FindItemWithText(SplitData[1].TrimStart(' '));

                    //Debug.WriteLine(listViewItem1.SubItems[1].Text + " ==== " + listViewItem1.SubItems.Count);
                    listViewItem1.SubItems[8].Text = SplitData[0];
                    break;
                default:
                    //Debug.WriteLine(" \n--------- Start Parse ListView Update Return String -----------\n");

                    foreach (string part in split)
                    {
                        //Debug.WriteLine(" \n---------Part: " + part + "-----------\n");
                        string[] items_pre = part.Split('|');

                        foreach (var item in items_pre)
                        {
                            //Debug.WriteLine(" \n--------- Item: " + item + "-----------\n");
                            if (!item.Contains("-~-"))
                            {
                                switch (item.ToString())
                                {
                                    case "waiting":
                                        status = item;
                                        break;
                                    case "importing":
                                        status = item;
                                        break;
                                    case "finished":
                                        status = item;
                                        break;
                                }
                                //Debug.WriteLine("Message Loop Message: " + message + " ==== Item Value:" + item + " Part: " + part);
                                continue;
                            }
                            string[] items = item.Split(stringSep2, StringSplitOptions.None);
                            //Debug.WriteLine("---- Items and Values: " + items[0] + " :: Value: " + items[1]);
                            switch (items[0].ToString())
                            {
                                case "title":
                                    //Debug.WriteLine("Title? " + items[1]);
                                    title = items[1];
                                    break;
                                case "user":
                                    //Debug.WriteLine("user? " + items[1]);
                                    user = items[1];
                                    break;
                                case "id":
                                    //Debug.WriteLine("importnum? " + items[1]);
                                    ImportID = items[1];
                                    break;
                                case "message":
                                    //Debug.WriteLine("message? " + items[1]);
                                    message = items[1];
                                    break;
                                case "hash":
                                    //Debug.WriteLine("filehash? " + items[1]);
                                    filehash = items[1];
                                    break;
                                case "ap":
                                    //Debug.WriteLine("AP? " + items[1]);
                                    message = message + " - " + items[1];
                                    break;
                                case "tot":
                                    //Debug.WriteLine("This Of This? " + items[1]);
                                    message = message + " - " + items[1];
                                    break;
                            }
                        }
                    }
                    //Debug.WriteLine(filehash);
                    //Debug.WriteLine("End Parse Loop Message: " + message);
                    ListViewItem listViewItem = listView1.FindItemWithText(filehash);
                    if ((status == "finished") || ((ImportID != "") && (message != "")))
                    {
                        //Debug.WriteLine(listViewItem.SubItems[1].Text + " ==== " + listViewItem.SubItems.Count);
                        listViewItem.SubItems[0].Text = ImportID;
                        listViewItem.SubItems[1].Text = user;
                        listViewItem.SubItems[2].Text = title;
                        listViewItem.SubItems[7].Text = message;
                        listViewItem.SubItems[8].Text = status;
                    }
                    //Debug.WriteLine(" \n--------- End Parse ListView Update Return String -----------\n");
                    break;
            }
            //Debug.WriteLine("========== Update Listview End ==========");
        }
        */

        //
        // Process Completed Functions
        //
        
        private void backgroundWorker1_ImportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //maybe do something?

            AutoCloseTimer AutoCloseTimerForm = new AutoCloseTimer();
            AutoCloseTimerForm.TimerSeconds = AutoCloseTimerSeconds.ToString();
            Debug.WriteLine(AutoCloseTimerForm.ShowDialog());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }



    public class ServerNameObj
    {
        public string ServerName { get; set; }
    }

    public class ServerObj
    {
        public int ID { get; set; }
        public string ServerAddress { get; set; }
        public string ApiPath { get; set; }
        public string Username { get; set; }
        public string ApiKey { get; set; }
        public bool Selected { get; set; }
    }

    public class IdComparer : IEqualityComparer<ServerNameObj>
    {
        public int GetHashCode(ServerNameObj co)
        {
            if (co == null)
            {
                return 0;
            }
            return co.ServerName.GetHashCode();
        }

        public bool Equals(ServerNameObj x1, ServerNameObj x2)
        {
            if (object.ReferenceEquals(x1, x2))
            {
                return true;
            }
            if (object.ReferenceEquals(x1, null) ||
                object.ReferenceEquals(x2, null))
            {
                return false;
            }
            return x1.ServerName == x2.ServerName;
        }
    }
}
