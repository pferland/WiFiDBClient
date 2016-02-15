using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WDBSQLite;

namespace WiFiDBUploader
{
    public partial class WiFiDBUploadMainForm : Form
    {
        private WDBAPI.WDBAPI WDBAPIObj;
        private WDBCommon.WDBCommon WDBCommonObj;
        private WDBTraceLog.TraceLog WDBTraceLogObj;
        private Timer timer1;
        private Timer timer2;
        private List<ServerObj> ServerList;

        private int    NextID = 0;

        private bool   AutoUploadFolder;
        private string AutoUploadFolderPath;

        private bool   ArchiveImports;
        private string ArchiveImportsFolderPath;

        private int    AutoCloseTimerSeconds;
        private bool   AutoCloseEnable;

        private string DefaultImportNotes;
        private string DefaultImportTitle;
        private bool   UseDefaultImportValues;

        private string SQLiteDBFile;
        private string SQLiteDBPath;
        private string SQLiteFile;

        private bool   ImportUpdateThreadEnable;
        private bool   DaemonUpdateThreadEnable;
        private int    ImportUpdateThreadSeconds;
        private int    DaemonUpdateThreadSeconds;

        private string LogPath;
        private string LogFile;
        private bool TraceLogEnable;
        private bool PerRunRotate;
        private bool DEBUG;

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
            LoadSettings();
            InitClasses();
            InitializeComponent();
            LoadDbDataIntoUI();
            InitTimer();
            AutoUploadCheck();
        }

        private void LoadDbDataIntoUI()
        {
            List<ImportRow> ImportedRows = WDBCommonObj.GetImportRows();
            foreach ( ImportRow Row in ImportedRows)
            {
                TraceLogObj.WriteToLog("\n------------------\n------------------\nCreate ROW from SQL: " + Row.ImportID.ToString() + " |=| " + Row.Username + " |=| " + Row.ImportTitle + " |=| " + Row.Message + " |=| " + Row.Status + "\n------------------\n------------------\n");
                string[] row = { Row.ImportID.ToString(), Row.Username, Row.ImportTitle, Row.DateTime,
                    Row.FileSize, Row.FileName, Row.FileHash, Row.Status, Row.Message };

                var listViewItemNew = new ListViewItem(row);
                listView1.Items.Add(listViewItemNew);
            }
        }

        private void AutoUploadCheck()
        {
            if(AutoUploadFolder == true && SelectedServer != null)
            {
                StartFolderImport(AutoUploadFolderPath);
            }
        }
        
        private void InitClasses()
        {
            WDBTraceLogObj = new WDBTraceLog.TraceLog(LogPath, TraceLogEnable, PerRunRotate);
            WDBAPIObj = new WDBAPI.WDBAPI(WDBTraceLogObj);
            WDBCommonObj = new WDBCommon.WDBCommon(SQLiteFile, WDBAPIObj, WDBTraceLogObj);
            
            WDBCommonObj.AutoUploadFolder = AutoUploadFolder;
            WDBCommonObj.AutoUploadFolderPath = AutoUploadFolderPath;
            WDBCommonObj.ArchiveImports = ArchiveImports;
            WDBCommonObj.ArchiveImportsFolderPath = ArchiveImportsFolderPath;
            WDBCommonObj.LogPath = LogPath;

            WDBCommonObj.DefaultImportNotes = DefaultImportNotes;
            WDBCommonObj.DefaultImportTitle = DefaultImportTitle;
            WDBCommonObj.UseDefaultImportValues = UseDefaultImportValues;

            WDBCommonObj.ServerAddress = ServerAddress;
            WDBCommonObj.ApiPath = ApiPath;
            WDBCommonObj.Username = Username;
            WDBCommonObj.ApiKey = ApiKey;
            WDBCommonObj.ApiCompiledPath = ApiCompiledPath;
            
            WDBCommonObj.initApi();
        }

        private void InitTimer()
        {
            if (ApiCompiledPath == null)
            {
                TraceLogObj.WriteToLog("Not running background threads till there is a server selected. whats the point if there is no server?");
            }
            else
            {
                if(ImportUpdateThreadEnable)
                {
                    if (timer1 != null)
                    {
                        //TraceLogObj.WriteToLog("Stopping Import update background thread.");
                        timer1.Stop();
                        timer1.Dispose();
                        timer1 = null;
                    }
                    //TraceLogObj.WriteToLog("Starting Import update background thread.");
                    timer1 = new System.Windows.Forms.Timer();
                    timer1.Tick += new EventHandler(CheckForUpdates);
                    timer1.Interval = (ImportUpdateThreadSeconds * 1000); // in miliseconds
                    timer1.Start();

                    
                }

                if(DaemonUpdateThreadEnable)
                {
                    StartGetDaemonStats(); //prep the tables.
                    if (timer2 != null)
                    {
                        //TraceLogObj.WriteToLog("Stopping Daemon update background thread.");
                        //TraceLogObj.WriteToLog("Restart Timer 2");
                        timer2.Stop();
                        timer2.Dispose();
                        timer2 = null;
                    }
                    //TraceLogObj.WriteToLog("Starting Daemon update background thread.");
                    timer2 = new System.Windows.Forms.Timer();
                    timer2.Tick += new EventHandler(CheckForDaemonUpdates);
                    timer2.Interval = (DaemonUpdateThreadSeconds * 1000); // in miliseconds
                    timer2.Start();
                }
            }
        }

        private void CreateRegistryKeys(Microsoft.Win32.RegistryKey rootKey)
        {
            Microsoft.Win32.RegistryKey ServersKey;
            Microsoft.Win32.RegistryKey DefaultServerKey;

            rootKey.SetValue("DefaultImportTitle", "Generic Import Title");
            rootKey.SetValue("DefaultImportNotes", "Generic blable about the import, maybe some notes on where you drove or what you saw?");
            rootKey.SetValue("UseDefaultImportValues", "False");
            rootKey.SetValue("AutoUploadFolder", "False");
            rootKey.SetValue("AutoUploadFolderPath", "");
            rootKey.SetValue("ArchiveImports", "False");
            rootKey.SetValue("ArchiveImportsFolderPath", "");
            rootKey.SetValue("AutoCloseEnable", "False");
            rootKey.SetValue("AutoCloseTimerSeconds", "30");
            rootKey.SetValue("SQLiteDBPath", ".\\DB\\");
            rootKey.SetValue("SQLiteDBFile", "Uploader.db3");
            rootKey.SetValue("ImportUpdateThreadEnable", "True");
            rootKey.SetValue("ImportUpdateThreadSeconds", 30);
            rootKey.SetValue("DaemonUpdateThreadEnable", "True");
            rootKey.SetValue("DaemonUpdateThreadSeconds", 60);
            rootKey.SetValue("TraceLogEnable", "False");
            rootKey.SetValue("DEBUG", "False");
            
            ServersKey = rootKey.CreateSubKey("Servers");
            DefaultServerKey = ServersKey.CreateSubKey("api.wifidb.net");

            DefaultServerKey.SetValue("ServerAddress", "https://api.wifidb.net");
            DefaultServerKey.SetValue("ApiPath", "/v2/");
            DefaultServerKey.SetValue("Username", "AnonCoward");
            DefaultServerKey.SetValue("ApiKey", "");
            DefaultServerKey.SetValue("Selected", "True");
            
        }

        private void LoadSettings()
        {
            Microsoft.Win32.RegistryKey rootKey;
            rootKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("Vistumbler").CreateSubKey("WiFiDB").CreateSubKey("Uploader");
            string[] SubKeys = rootKey.GetSubKeyNames();

            if (SubKeys.Count() == 0)
            {
                //TraceLogObj.WriteToLog("Servers SubKey not found, creating Default Structure.");
                CreateRegistryKeys(rootKey);
                LoadSettings();
            }
            else
            {
                Microsoft.Win32.RegistryKey ServerSubkeys = rootKey.CreateSubKey("Servers");

                foreach (string value in rootKey.GetValueNames())
                {
                    //TraceLogObj.WriteToLog(value);

                    switch (value)
                    {
                        case "AutoUploadFolder":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            AutoUploadFolder = Convert.ToBoolean(rootKey.GetValue(value));
                            break;
                        case "AutoUploadFolderPath":
                            //TraceLogObj.WriteToLog(value + " : " + rootKey.GetValue(value).ToString());
                            AutoUploadFolderPath = rootKey.GetValue(value).ToString();
                            break;
                        case "AutoCloseTimerSeconds":
                            //TraceLogObj.WriteToLog(value + " : " + rootKey.GetValue(value).ToString());
                            AutoCloseTimerSeconds = Int32.Parse(rootKey.GetValue(value).ToString());
                            break;
                        case "AutoCloseEnable":
                            //TraceLogObj.WriteToLog(value + " : " + rootKey.GetValue(value).ToString());
                            AutoCloseEnable = Convert.ToBoolean(rootKey.GetValue(value));
                            break;
                        case "ArchiveImports":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            ArchiveImports = Convert.ToBoolean(rootKey.GetValue(value));
                            break;
                        case "ArchiveImportsFolderPath":
                            //TraceLogObj.WriteToLog(value + " : " + rootKey.GetValue(value).ToString());
                            ArchiveImportsFolderPath = rootKey.GetValue(value).ToString();
                            break;
                        case "DefaultImportNotes":
                            //TraceLogObj.WriteToLog(value + " : " + rootKey.GetValue(value).ToString());
                            DefaultImportNotes = rootKey.GetValue(value).ToString();
                            break;
                        case "DefaultImportTitle":
                            //TraceLogObj.WriteToLog(value + " : " + rootKey.GetValue(value).ToString());
                            DefaultImportTitle = rootKey.GetValue(value).ToString();
                            break;
                        case "UseDefaultImportValues":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            UseDefaultImportValues = Convert.ToBoolean(rootKey.GetValue(value));
                            break;
                        case "SQLiteDBFile":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            SQLiteDBFile = rootKey.GetValue(value).ToString();
                            break;
                        case "SQLiteDBPath":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            SQLiteDBPath = rootKey.GetValue(value).ToString();
                            break;
                        case "LogPath":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            LogPath = rootKey.GetValue(value).ToString();
                            break;
                        case "ImportUpdateThreadEnable":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            ImportUpdateThreadEnable = Convert.ToBoolean(rootKey.GetValue(value));
                            break;
                        case "DaemonUpdateThreadEnable":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            DaemonUpdateThreadEnable = Convert.ToBoolean(rootKey.GetValue(value));
                            break;
                        case "ImportUpdateThreadSeconds":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            ImportUpdateThreadSeconds =  Int32.Parse(rootKey.GetValue(value).ToString());
                            break;
                        case "DaemonUpdateThreadSeconds":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            DaemonUpdateThreadSeconds = Int32.Parse(rootKey.GetValue(value).ToString());
                            break;
                        case "TraceLogEnable":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            TraceLogEnable = Convert.ToBoolean(rootKey.GetValue(value));
                            break;
                        case "DEBUG":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            DEBUG = Convert.ToBoolean(rootKey.GetValue(value));
                            break;
                        case "PerRunRotate":
                            //TraceLogObj.WriteToLog(value + " : " + Convert.ToBoolean(rootKey.GetValue(value)));
                            PerRunRotate = Convert.ToBoolean(rootKey.GetValue(value));
                            break;
                    }
                }
                ServerList = new List<ServerObj>();
                int Increment = 0;
                foreach (string subitem in ServerSubkeys.GetSubKeyNames())
                {
                    //TraceLogObj.WriteToLog("-------------------\n"+subitem);
                    Microsoft.Win32.RegistryKey ServerKey = ServerSubkeys.CreateSubKey(subitem);

                    ServerObj Server = new ServerObj();
                    //TraceLogObj.WriteToLog("ServerAddress = "+ ServerKey.GetValue("ServerAddress").ToString());
                    //TraceLogObj.WriteToLog("ApiPath = " + ServerKey.GetValue("ApiPath").ToString());
                    //TraceLogObj.WriteToLog("Username = " + ServerKey.GetValue("Username").ToString());
                    //TraceLogObj.WriteToLog("ApiKey = " + ServerKey.GetValue("ApiKey").ToString());

                    Server.ID = Increment;
                    Server.ServerAddress = ServerKey.GetValue("ServerAddress").ToString();
                    Server.ApiPath = ServerKey.GetValue("ApiPath").ToString();
                    Server.Username = ServerKey.GetValue("Username").ToString();
                    Server.ApiKey = ServerKey.GetValue("ApiKey").ToString();
                    Server.Selected = Convert.ToBoolean(ServerKey.GetValue("Selected"));

                    if (Server.Selected)
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
                //TraceLogObj.WriteToLog(server.ServerAddress);

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
            //TraceLogObj.WriteToLog("Servers not in the list now:");
            foreach(ServerNameObj ServerName in list3)
            {
                //TraceLogObj.WriteToLog(ServerName.ServerName);
                rootKey.DeleteSubKeyTree(ServerName.ServerName);
            }

            LoadSettings();
            InitClasses();
            InitTimer();
        }

        private void WriteGlobalSettings()
        {
            /* Screw the app.config file, registry is easier to manage. */
            Microsoft.Win32.RegistryKey rootKey;
            rootKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("Vistumbler").CreateSubKey("WiFiDB").CreateSubKey("Uploader");

            rootKey.SetValue("AutoUploadFolder", AutoUploadFolder);
            rootKey.SetValue("AutoCloseTimerSeconds", AutoCloseTimerSeconds);
            rootKey.SetValue("AutoCloseEnable", AutoCloseEnable);
            rootKey.SetValue("AutoUploadFolderPath", AutoUploadFolderPath);
            rootKey.SetValue("ArchiveImports", ArchiveImports);
            rootKey.SetValue("ArchiveImportsFolderPath", ArchiveImportsFolderPath);
            rootKey.SetValue("DefaultImportNotes", DefaultImportNotes);
            rootKey.SetValue("DefaultImportTitle", DefaultImportTitle);
            rootKey.SetValue("SQLiteFile", SQLiteFile);
            rootKey.SetValue("LogPath", LogPath);
            rootKey.SetValue("ImportUpdateThreadEnable", ImportUpdateThreadEnable);
            rootKey.SetValue("DaemonUpdateThreadEnable", DaemonUpdateThreadEnable);
            rootKey.SetValue("ImportUpdateThreadSeconds", ImportUpdateThreadSeconds);
            rootKey.SetValue("DaemonUpdateThreadSeconds", DaemonUpdateThreadSeconds);
            rootKey.SetValue("TraceLogEnable", TraceLogEnable);
            rootKey.SetValue("DEBUGEnable", DEBUG);
            rootKey.SetValue("PerRunRotate", PerRunRotate);

            LoadSettings();
            InitClasses();
            InitTimer();
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
            //TraceLogObj.WriteToLog("-------- BackGround Update Check Begin --------");
            foreach (ListViewItem item in listView1.Items)
            {
                //TraceLogObj.WriteToLog(item.SubItems[8].Text);
                StartUpdateWiaitng(item.SubItems[6].Text);
            }
            //TraceLogObj.WriteToLog("-------- BackGround Update Check End --------");
        }
        
        private void StartUpdateDaemonStats()
        {
            //TraceLogObj.WriteToLog("Start Call: StartUpdateDaemonStats");
            QueryArguments args = new QueryArguments(NextID++, "");
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_UpdateDaemonStatsDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_GetDaemonListViewProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);
            //TraceLogObj.WriteToLog("End Call: StartUpdateDaemonStats");
        }

        private void StartGetDaemonStats()
        {
            TraceLogObj.WriteToLog("Start Call: StartGetDaemonStats");
            QueryArguments args = new QueryArguments(NextID++, "");
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_GetDaemonStatsDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_GetDaemonListViewProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);
            TraceLogObj.WriteToLog("End Call: StartGetDaemonStats");
        }

        private void StartUpdateWiaitng(string query)
        {
            TraceLogObj.WriteToLog("Start Call: StartUpdateWaiting");
            QueryArguments args = new QueryArguments(NextID++, query);
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_UpdateWaitingDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_UpdateListViewProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_ImportWorkerCompleted);
            backgroundWorker1.RunWorkerAsync(args);
            TraceLogObj.WriteToLog("End Call: StartUpdateWaiting");
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

        private void StartFolderImport(string query, bool ManualRun = false)
        {
            QueryArguments args = new QueryArguments(NextID++, query);

            BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker_FolderImportDoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ImportProgressChanged);
            if(!ManualRun)
            {
                backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_ImportCompleted);
            }
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
                        ImportDetails ImportDetailsForm = new ImportDetails();

                        if (ImportDetailsForm.ShowDialog() == DialogResult.OK)
                        {
                            ImportTitle = ImportDetailsForm.ImportTitle;
                            ImportNotes = ImportDetailsForm.ImportNotes;
                        }else
                        {
                            ImportTitle = DefaultImportTitle;
                            ImportNotes = DefaultImportNotes;
                        }
                    }
                    //string response = WDBAPIObj.ApiImportFile(openFileDialog1.FileName, ImportTitle, ImportNotes);
                    //WDBAPIObj.ParseApiResponse(response);
                    string Query = openFileDialog1.FileName + "|" + ImportTitle + "|" + ImportNotes;
                    TraceLogObj.WriteToLog(Query);
                    StartFileImport(Query);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void importFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ImportTitle;
            string ImportNotes;
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                //TraceLogObj.WriteToLog(folderBrowserDialog1.SelectedPath);
                if (UseDefaultImportValues)
                {
                    ImportTitle = DefaultImportTitle;
                    ImportNotes = DefaultImportNotes;
                }
                else
                {
                    //Ask For Import Title and Notes. They dont want to use the defaults.

                    ImportDetails ImportDetailsForm = new ImportDetails();

                    if (ImportDetailsForm.ShowDialog() == DialogResult.OK)
                    {
                        ImportTitle = ImportDetailsForm.ImportTitle;
                        ImportNotes = ImportDetailsForm.ImportNotes;
                    }
                    else
                    {
                        ImportTitle = DefaultImportTitle;
                        ImportNotes = DefaultImportNotes;
                    }
                }
                string Query = folderBrowserDialog1.SelectedPath + "|" + ImportTitle + "|" + ImportNotes;

                StartFolderImport(Query, true);
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
            //TraceLogObj.WriteToLog(AutoUploadFolder);
            AutoForm.AutoUploadFolder = AutoUploadFolder;
            AutoForm.AutoUploadFolderPath = AutoUploadFolderPath;
            AutoForm.ArchiveImports = ArchiveImports;
            AutoForm.ArchiveImportsFolderPath = ArchiveImportsFolderPath;
            AutoForm.AutoCloseTimerSeconds = AutoCloseTimerSeconds.ToString();
            AutoForm.AutoCloseEnable = AutoCloseEnable;

            if (AutoForm.ShowDialog() == DialogResult.OK)
            {
                //TraceLogObj.WriteToLog("AutoForm.AutoUploadFolder: " + AutoForm.AutoUploadFolder);
                //TraceLogObj.WriteToLog("AutoForm.AutoUploadFolderPath: " + AutoForm.AutoUploadFolderPath);
                //TraceLogObj.WriteToLog("AutoForm.ArchiveImports: " + AutoForm.ArchiveImports);
                //TraceLogObj.WriteToLog("AutoForm.ArchiveImportsFolderPath: " + AutoForm.ArchiveImportsFolderPath);

                AutoUploadFolder = Convert.ToBoolean(AutoForm.AutoUploadFolder);
                AutoUploadFolderPath = AutoForm.AutoUploadFolderPath;
                ArchiveImports = Convert.ToBoolean(AutoForm.ArchiveImports);
                ArchiveImportsFolderPath = AutoForm.ArchiveImportsFolderPath;
                AutoCloseTimerSeconds = Int32.Parse(AutoForm.AutoCloseTimerSeconds);
                AutoCloseEnable = AutoForm.AutoCloseEnable;

                //TraceLogObj.WriteToLog("this.AutoUploadFolder: " + this.AutoUploadFolder);
                //TraceLogObj.WriteToLog("this.AutoUploadFolderPath: " + this.AutoUploadFolderPath);
                //TraceLogObj.WriteToLog("this.ArchiveImports: " + this.ArchiveImports);
                //TraceLogObj.WriteToLog("this.ArchiveImportsFolderPath: " + this.ArchiveImportsFolderPath);

                WriteGlobalSettings();
            }
        }

        private void backgroundThreadSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BGThreadsSettings BGThreadsSettingsForm = new BGThreadsSettings();
            BGThreadsSettingsForm.DaemonUpdateThreadSeconds = DaemonUpdateThreadSeconds.ToString();
            BGThreadsSettingsForm.ImportUpdateThreadSeconds = ImportUpdateThreadSeconds.ToString();
            BGThreadsSettingsForm.ImportUpdateThreadEnable = ImportUpdateThreadEnable;
            BGThreadsSettingsForm.DaemonUpdateThreadEnable = DaemonUpdateThreadEnable;
            if(BGThreadsSettingsForm.ShowDialog() == DialogResult.OK)
            {
                DaemonUpdateThreadSeconds = Int32.Parse(BGThreadsSettingsForm.DaemonUpdateThreadSeconds);
                ImportUpdateThreadSeconds = Int32.Parse(BGThreadsSettingsForm.ImportUpdateThreadSeconds);
                ImportUpdateThreadEnable = BGThreadsSettingsForm.ImportUpdateThreadEnable;
                DaemonUpdateThreadEnable = BGThreadsSettingsForm.DaemonUpdateThreadEnable;

                WriteGlobalSettings();
                LoadSettings();
                InitTimer();
            }
        }
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loggingSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoggingSettings LoggingSettingsForm = new LoggingSettings();
            LoggingSettingsForm.TraceLogEnable = TraceLogEnable;
            LoggingSettingsForm.DEBUG = DEBUG;
            LoggingSettingsForm.PerRunRotate = PerRunRotate;

            if(LoggingSettingsForm.ShowDialog() == DialogResult.OK)
            {
                TraceLogEnable = LoggingSettingsForm.TraceLogEnable;
                DEBUG = LoggingSettingsForm.DEBUG;
                PerRunRotate = LoggingSettingsForm.PerRunRotate;

                WriteGlobalSettings();
                LoadSettings();
                InitTimer();
            }
        }

        //
        // Do Work Functions
        //

        private void backgroundWorker_UpdateDaemonStatsDoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            //TraceLogObj.WriteToLog(args.Query);
            WDBCommonObj.GetDaemonStatuses(args.Query, backgroundWorker);
            ////TraceLogObj.WriteToLog(ImportIDs.ToString());
            //e.Result = "Waiting Done";
        }

        private void backgroundWorker_GetDaemonStatsDoWork(object sender, DoWorkEventArgs e)
        {
            //TraceLogObj.WriteToLog(ServerAddress);
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            //TraceLogObj.WriteToLog(args.Query);
            WDBCommonObj.GetDaemonStatuses(args.Query, backgroundWorker);
        }

        private void backgroundWorker_UpdateWaitingDoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            //TraceLogObj.WriteToLog(args.Query);
            WDBCommonObj.GetHashStatus(args.Query, backgroundWorker);
            ////TraceLogObj.WriteToLog(ImportIDs.ToString());
            //e.Result = "Waiting Done";
        }

        private void backgroundWorker_FolderImportDoWork(object sender, DoWorkEventArgs e)
        {
            List<KeyValuePair<int, string>> ImportIDs;
            var backgroundWorker = sender as BackgroundWorker;
            QueryArguments args = (QueryArguments)e.Argument;
            //TraceLogObj.WriteToLog(args.Query);
            ImportIDs = WDBCommonObj.ImportFolder(args.Query, DefaultImportTitle, DefaultImportNotes, backgroundWorker);
            if (ImportIDs.Count > 0)
            {
                TraceLogObj.WriteToLog(ImportIDs[0].Value);
                if (ImportIDs[0].Value == "Error with API")
                {
                    MessageBox.Show("Error With API.");
                }else if (ImportIDs[0].Value == "Already Imported.")
                {
                    TraceLogObj.WriteToLog("File Already Imported.");
                    //MessageBox.Show("Error With API.");
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
            TraceLogObj.WriteToLog(args.Query);

            string[] splits = args.Query.Split('|');

            //splits[0] == ImportFile
            //splits[1] == ImportTitle
            //splits[2] == ImportNotes
            string ImportFileResult = WDBCommonObj.ImportFile(splits[0], splits[1], splits[2], backgroundWorker);
            TraceLogObj.WriteToLog("Import File Return: " + ImportFileResult);
            
            if (ImportFileResult == "Error with API")
            {
                //MessageBox.Show("Error With API.");
            }
            else if (ImportFileResult == "Already Imported.")
            {
                TraceLogObj.WriteToLog("File Already Imported.");
                //MessageBox.Show("File Already Imported.");
            }/*
            else
            {
                ImportIDs.Add(new KeyValuePair<int, string>(ImportInternalID, ImportFileResult));
            }
            
            args.Result = ImportIDs;
            e.Result = args.Result;
            */
        }


        private void InsertNewListViewRow(string[] split, string Type)
        {
            WDBSQLite.ImportRow ImportRowObj = new WDBSQLite.ImportRow();

            string Date_Time = DateTime.Now.ToString("yyyy-MM-dd    HH:mm:ss");
            string StatusStr;
            string MessageStr;
            string FileSizeString;
            string FileHash;
            string FilePath;

            if (Type == "NewRow")
            {
                FileSizeString = new FileInfo(split[1]).Length.ToString();
                FilePath = split[1];
                FileHash = split[2].ToUpper();
                StatusStr = "Uploading";
                MessageStr = "Uploading File to WiFiDB...";
            }
            else if(Type == "Error")
            {
                string[] stringSep1 = new string[] { "::" };
                string[] stringSep2 = new string[] { "-~-" };
                string[] stringSep3 = new string[] { ": " };

                string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);
                string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);
                string[] SplitData1 = SplitData[0].Split(stringSep3, StringSplitOptions.None);

                FilePath = SplitData[1];
                FileHash = SplitData1[1].ToUpper();
                FileSizeString = new FileInfo(FilePath).Length.ToString();
                
                StatusStr = "Error";
                MessageStr = split[1];
            }else
            {
                string[] stringSep1 = new string[] { "::" };
                string[] stringSep2 = new string[] { "-~-" };
                string[] stringSep3 = new string[] { ": " };

                string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);
                string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);
                string[] SplitData1 = SplitData[1].Split(stringSep3, StringSplitOptions.None);

                FilePath = SplitData[1];
                FileHash = SplitData1[1].ToUpper();
                FileSizeString = new FileInfo(FilePath).Length.ToString();

                StatusStr = "Error";
                MessageStr = split[1];
            }

            ImportRowObj.Username = Username;
            ImportRowObj.ImportTitle = "";
            ImportRowObj.DateTime = Date_Time;
            ImportRowObj.FileSize = FileSizeString;
            ImportRowObj.FileName = FilePath;
            ImportRowObj.FileHash = FileHash;
            ImportRowObj.Status = StatusStr;
            ImportRowObj.Message = MessageStr;

            WDBCommonObj.InsertImportRow(ImportRowObj); // Insert import information into SQLite.

            string[] row = { "", Username, "", Date_Time, FileSizeString, FilePath, FileHash, StatusStr, MessageStr };
            TraceLogObj.WriteToLog("\n------------------\n------------------\n" + Type + ": " + " |=| " + Username + " |=| " + " |=| " + Date_Time + " |=| "
                + FileSizeString + " |=| " + FilePath + " |=| " + FileHash + " |=| " + " |=| " + StatusStr + " |=| " + MessageStr + " \n------------------\n------------------\n");

            var listViewItemNew = new ListViewItem(row);
            listView1.Items.Add(listViewItemNew);
        }


        //
        // Progress Changed Functions
        //
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
            string FileSizeString;
            string FileHash;
            string FilePath;
            string StatusStr;
            string MessageStr;


            TraceLogObj.WriteToLog("e.UserState: " + e.UserState.ToString() );

            string[] stringSep1 = new string[] { "::" };
            string[] stringSep2 = new string[] { "-~-" };
            string[] stringSep3 = new string[] { ": " };

            TraceLogObj.WriteToLog("Split[0]" + split[0]);
            switch (split[0].ToLower())
            {
                case "newrow":
                    InsertNewListViewRow(split, "NewRow");
                    break;
                case "error":
                    //TraceLogObj.WriteToLog(split[0]);
                    
                    string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);
                    string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);
                    string[] SplitData1 = SplitData[1].Split(stringSep3, StringSplitOptions.None);

                    FilePath = SplitData[1];
                    FileHash = SplitData1[0].ToUpper();
                    FileSizeString = new FileInfo(FilePath).Length.ToString();

                    StatusStr = "Error";
                    MessageStr = split[1];


                    /*
                    string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);
                    string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);
                    */

                    TraceLogObj.WriteToLog("split[0]: " + split[0]);
                    TraceLogObj.WriteToLog("items_err[0]: " + items_err[0]);
                    TraceLogObj.WriteToLog("SplitData[0]: " + SplitData[1]); //FilePath
                    TraceLogObj.WriteToLog("SplitData[1]: " + SplitData1[0]); //FileHash
                    
                    ListViewItem listViewItem1 = listView1.FindItemWithText(FileHash);

                    //TraceLogObj.WriteToLog(listViewItem1.SubItems[1].Text + " ==== " + listViewItem1.SubItems.Count);

                    listViewItem1.SubItems[7].Text = StatusStr;
                    listViewItem1.SubItems[8].Text = MessageStr;
                    
                    //InsertNewListViewRow(split, "Error");
                    break;
                default:
                    foreach (string part in split)
                    {
                        TraceLogObj.WriteToLog(part + " \n--------------------\n");
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
                        //TraceLogObj.WriteToLog(" \n--------------------\n");
                    }
                    TraceLogObj.WriteToLog("Update Row FileHash: " + filehash);
                    WDBSQLite.ImportRow ImportRowObj = new WDBSQLite.ImportRow();
                    ListViewItem listViewItem = listView1.FindItemWithText(filehash);
                    if(user != "" && filehash != "")
                    {
                        ImportRowObj.ImportID = Int32.Parse(ImportID);
                        ImportRowObj.ImportTitle = title;
                        ImportRowObj.FileHash = filehash;
                        ImportRowObj.Status = "Waiting";
                        ImportRowObj.Message = message;

                        WDBCommonObj.UpdateImportRow(ImportRowObj); // Update Import row information in SQLite.

                        TraceLogObj.WriteToLog("\n------------------\n------------------\nUpdate ROW: " + ImportID + " |=| " + user + " |=| " + title + " |=| " + "Waiting" + " |=| " + message + "\n------------------\n------------------\n");
                        listViewItem.SubItems[0].Text = ImportID;
                        listViewItem.SubItems[1].Text = user;
                        listViewItem.SubItems[2].Text = title;
                        listViewItem.SubItems[7].Text = "Waiting";
                        listViewItem.SubItems[8].Text = message;
                    }
                    break;
            }
            ////TraceLogObj.WriteToLog(e.ProgressPercentage.ToString());
        }

        private void backgroundWorker_UpdateListViewProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            WDBSQLite.ImportRow ImportRowObj = new WDBSQLite.ImportRow();

            string[] stringSeparators = new string[] { "|~|" };
            string[] stringSep1 = new string[] { "::" };
            string[] stringSep2 = new string[] { "-~-" };
            string[] split = e.UserState.ToString().Split(stringSeparators, StringSplitOptions.None);

            string title = "";
            string user = "";
            string message = "";
            string status = "";
            string ImportID = "";
            string filehash = "";
            //TraceLogObj.WriteToLog("========== Update Listview Start ==========");
            //TraceLogObj.WriteToLog(split[0]);
            switch (split[0])
            {
                case "error":
                    //TraceLogObj.WriteToLog(split[0]);
                    string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);
                    string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);

                    //TraceLogObj.WriteToLog(items_err[0]);
                    //TraceLogObj.WriteToLog(SplitData[0]);
                    //TraceLogObj.WriteToLog(SplitData[1]);

                    ListViewItem listViewItem1 = listView1.FindItemWithText(SplitData[1].TrimStart(' '));

                    //TraceLogObj.WriteToLog(listViewItem1.SubItems[1].Text + " ==== " + listViewItem1.SubItems.Count);
                    listViewItem1.SubItems[8].Text = SplitData[0];
                    break;
                default:
                    //TraceLogObj.WriteToLog(" \n--------- Start Parse ListView Update Return String -----------\n");

                    foreach (string part in split)
                    {
                        //TraceLogObj.WriteToLog(" \n---------Part: " + part + "-----------\n");
                        string[] items_pre = part.Split('|');

                        foreach (var item in items_pre)
                        {
                            //TraceLogObj.WriteToLog(" \n--------- Item: " + item + "-----------\n");
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
                                //TraceLogObj.WriteToLog("Message Loop Message: " + message + " ==== Item Value:" + item + " Part: " + part);
                                continue;
                            }
                            string[] items = item.Split(stringSep2, StringSplitOptions.None);
                            //TraceLogObj.WriteToLog("---- Items and Values: " + items[0] + " :: Value: " + items[1]);
                            switch (items[0].ToString())
                            {
                                case "title":
                                    //TraceLogObj.WriteToLog("Title? " + items[1]);
                                    title = items[1];
                                    break;
                                case "user":
                                    //TraceLogObj.WriteToLog("user? " + items[1]);
                                    user = items[1];
                                    break;
                                case "id":
                                    //TraceLogObj.WriteToLog("importnum? " + items[1]);
                                    ImportID = items[1];
                                    break;
                                case "message":
                                    //TraceLogObj.WriteToLog("message? " + items[1]);
                                    message = items[1];
                                    break;
                                case "hash":
                                    //TraceLogObj.WriteToLog("filehash? " + items[1]);
                                    filehash = items[1];
                                    break;
                                case "ap":
                                    //TraceLogObj.WriteToLog("AP? " + items[1]);
                                    message = message + " - " + items[1];
                                    break;
                                case "tot":
                                    //TraceLogObj.WriteToLog("This Of This? " + items[1]);
                                    message = message + " - " + items[1];
                                    break;
                            }
                        }
                    }
                    //TraceLogObj.WriteToLog(filehash);
                    //TraceLogObj.WriteToLog("End Parse Loop Message: " + message);
                    ListViewItem listViewItem = listView1.FindItemWithText(filehash);
                    if( (status == "finished") || ( (ImportID != "") && (message != "") ) )
                    {
                        ImportRowObj.ImportID = Int32.Parse(ImportID);
                        ImportRowObj.ImportTitle = title;
                        ImportRowObj.FileHash = filehash;
                        ImportRowObj.Status = status;
                        ImportRowObj.Message = message;

                        WDBCommonObj.UpdateImportRow(ImportRowObj); // Update Import row information in SQLite.

                        TraceLogObj.WriteToLog("\n------------------\n------------------\nUpdate ROW: " + ImportID.ToString() + " |=| " + user + " |=| " + title + " |=| " + message + " |=| " + status + "\n------------------\n------------------\n");
                        listViewItem.SubItems[0].Text = ImportID;
                        listViewItem.SubItems[2].Text = title;
                        listViewItem.SubItems[7].Text = message;
                        listViewItem.SubItems[8].Text = status;
                    }
                    //TraceLogObj.WriteToLog(" \n--------- End Parse ListView Update Return String -----------\n");
                    break;
            }
            //TraceLogObj.WriteToLog("========== Update Listview End ==========");
        }
        
        private void backgroundWorker_GetDaemonListViewProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            string[] stringSeparators = new string[] { "|~|" };
            string[] split = e.UserState.ToString().Split(stringSeparators, StringSplitOptions.None);

            //TraceLogObj.WriteToLog("========== Update Daemon ListView Start ==========");
            //TraceLogObj.WriteToLog(e.UserState.ToString());
            //TraceLogObj.WriteToLog(split[0]);

            string nodename = "";
            string pidfile = "";
            string pid = "";
            string pidmem = "";
            string pidtime = "";
            string cmd = "";
            string datetime_col = "";
            string[] stringSep1 = new string[] { "::" };
            string[] stringSep2 = new string[] { "-~-" };
            switch (split[0])
            {
                case "error":
                    //TraceLogObj.WriteToLog(split[1]);
                    if(split[1] == "No_Daemons_Running")
                    {
                        //TraceLogObj.WriteToLog("No Daemons running, do ListView CleanUp.");
                        if (this.listView2.Items.Count > 0)
                        {
                            foreach (ListViewItem item in this.listView2.Items)
                            {
                                item.Remove();
                            }
                        }else
                        {
                            //TraceLogObj.WriteToLog("No rows, no need for cleanup...");
                        }
                    }
                    else
                    {
                        string[] items_err = split[1].Split(stringSep2, StringSplitOptions.None);
                        string[] SplitData = items_err[1].Split(stringSep1, StringSplitOptions.None);
                        //TraceLogObj.WriteToLog(items_err[0]);
                        //TraceLogObj.WriteToLog(SplitData[0]);
                        //TraceLogObj.WriteToLog(SplitData[1]);
                        //TraceLogObj.WriteToLog( e.UserState.ToString() );
                    }
                    break;

                default:
                    
                    //TraceLogObj.WriteToLog(" \n--------- Start Parse Daemon ListView Update Return String -----------\n");
                    int DaemonReturnCount = split.Count();
                    foreach (string part in split)
                    {
                        if(part == "")
                        {
                            DaemonReturnCount--;
                            continue;
                        }
                        //TraceLogObj.WriteToLog(" \n---------Part: " + part + "-----------\n");
                        string[] items_pre = part.Split('|');

                        foreach (var item in items_pre)
                        {
                            //TraceLogObj.WriteToLog(" \n--------- Item: " + item + "-----------\n");
                            if (!item.Contains("-~-"))
                            {
                                //TraceLogObj.WriteToLog("Bad Message: " + item + " Part: " + part);
                                continue;
                            }
                            string[] items = item.Split(stringSep2, StringSplitOptions.None);
                            //TraceLogObj.WriteToLog("---- Items and Values: " + items[0] + " :: Value: " + items[1]);
                            switch (items[0].ToString())
                            {
                                case "nodename":
                                    //TraceLogObj.WriteToLog("nodename? " + items[1]);
                                    nodename = items[1];
                                    break;
                                case "pidfile":
                                    //TraceLogObj.WriteToLog("pidfile? " + items[1]);
                                    pidfile = items[1];
                                    break;
                                case "pid":
                                    //TraceLogObj.WriteToLog("pid? " + items[1]);
                                    pid = items[1];
                                    break;
                                case "pidtime":
                                    //TraceLogObj.WriteToLog("runtime? " + items[1]);
                                    pidtime = items[1];
                                    break;
                                case "pidmem":
                                    //TraceLogObj.WriteToLog("mem? " + items[1]);
                                    pidmem = items[1];
                                    break;
                                case "pidcmd":
                                    //TraceLogObj.WriteToLog("cmd? " + items[1]);
                                    cmd = items[1];
                                    break;
                                case "date":
                                    //TraceLogObj.WriteToLog("date anad time? " + items[1]);
                                    datetime_col = items[1];
                                    break;
                            }
                        }
                        if ((nodename != "") && (pid != "") && (pidfile != ""))
                        {
                            ListViewItem listViewItem = listView2.FindItemWithText(pidfile);
                            if ( listViewItem != null)
                            {
                                //TraceLogObj.WriteToLog(listViewItem.SubItems[1].Text + " ==== " + listViewItem.SubItems.Count);
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
                    //TraceLogObj.WriteToLog(DaemonReturnCount.ToString() + " --=--=-=-=-=-==-- " + listView2.Items.Count);
                    if ((listView2.Items.Count != DaemonReturnCount) && DaemonReturnCount != 0)
                    {
                        foreach (ListViewItem item in listView2.Items)
                        {
                            //TraceLogObj.WriteToLog(e.UserState.ToString());
                            //TraceLogObj.WriteToLog(item.SubItems[1].Text);

                            if(e.UserState.ToString().Contains(item.SubItems[1].Text))
                            {
                                //TraceLogObj.WriteToLog(item.SubItems[1].Text + " Is in the return.");
                            }else
                            {
                                //TraceLogObj.WriteToLog(item.SubItems[1].Text + " Is NOT in the return.");
                                //TraceLogObj.WriteToLog("ListView CleanUp!");
                                item.Remove();
                            }
                        }
                    }
                    if(DaemonReturnCount == 0)
                    {
                        //TraceLogObj.WriteToLog("DaemonReturnCount was 0...");
                        //TraceLogObj.WriteToLog("UserStateString: " + e.UserState.ToString());
                    }
                    //TraceLogObj.WriteToLog(" \n--------- End Parse Daemon ListView Update Return String -----------\n");
                    break;
            }
            //TraceLogObj.WriteToLog("========== Update Daemon Listview End ==========");
        }
        
        //
        // Process Completed Functions
        //
        
        private void backgroundWorker1_ImportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //maybe do something?
            if(AutoCloseEnable)
            {
                AutoCloseTimer AutoCloseTimerForm = new AutoCloseTimer();
                AutoCloseTimerForm.TimerSeconds = AutoCloseTimerSeconds.ToString();
                AutoCloseTimerForm.ShowDialog();
            }
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
