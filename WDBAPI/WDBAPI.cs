using System;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace WDBAPI
{
    public class WDBAPI
    {
        public NameValueCollection parameters = null;

        public string LogPath;

        public bool AutoUploadFolder;
        public string AutoUploadFolderPath;
        public bool ArchiveImports;
        public string ArchiveImportsFolderPath;

        public string DefaultImportNotes;
        public string DefaultImportTitle;
        public bool UseDefaultImportValues;

        public string ServerAddress;
        public string ApiPath;
        public string Username;
        public string ApiKey;
        public string ApiCompiledPath;
        private WDBTraceLog.TraceLog TraceLogObj;

        public WDBAPI(WDBTraceLog.TraceLog WDBTraceLogObj)
        {
            this.TraceLogObj = WDBTraceLogObj;
        }

        public string ApiGetWaitingImports()
        {
            //TraceLogObj.WriteToLog(ApiCompiledPath + "schedule.php  ---- Get Waiting");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //TraceLogObj.WriteToLog(this.parameters.Get("username"));
                //TraceLogObj.WriteToLog(this.parameters.Get("apikey"));
                this.parameters.Add("func", "waiting");

                var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
                //TraceLogObj.WriteToLog(response);
            }
            //TraceLogObj.WriteToLog("End Function Call: Get Waiting.");
            return response;
        }
        
        public string ApiGetFinishedImports()
        {
            //TraceLogObj.WriteToLog(ApiCompiledPath + "schedule.php  ---- Get Finished");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //TraceLogObj.WriteToLog(this.parameters.Get("username"));
                //TraceLogObj.WriteToLog(this.parameters.Get("apikey"));
                this.parameters.Add("func", "finished");

                var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            //TraceLogObj.WriteToLog("End Function Call: Get Finished.");
            return response;
        }
        
        public string ApiGetBadIports()
        {
            //TraceLogObj.WriteToLog(ApiCompiledPath + "schedule.php  ---- Get Bad Imports.");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //TraceLogObj.WriteToLog(this.parameters.Get("username"));
                //TraceLogObj.WriteToLog(this.parameters.Get("apikey"));
                this.parameters.Add("func", "bad");

                var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            //TraceLogObj.WriteToLog("End Function Call: Get Bad Imports.");
            return response;
        }
        
        public string ApiGetCurrentImporting()
        {
            //TraceLogObj.WriteToLog(ApiCompiledPath + "schedule.php  ---- Get Current Imports.");
            string response;
            //Console.WriteLine("Upload File: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //TraceLogObj.WriteToLog(this.parameters.Get("username"));
                //TraceLogObj.WriteToLog(this.parameters.Get("apikey"));
                this.parameters.Add("func", "importing");

                var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            //TraceLogObj.WriteToLog("End Function Call: Get Current Imports.");
            return response;
        }
        
        public string ApiGetDaemonStatuses(string query)
        {
            //TraceLogObj.WriteToLog(ApiCompiledPath + "schedule.php  ---- Get Daemon Stats");
            string response;
            //Console.WriteLine("Upload File: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //TraceLogObj.WriteToLog(this.parameters.Get("username"));
                //TraceLogObj.WriteToLog(this.parameters.Get("apikey"));
                this.parameters.Add("func", "daemonstatuses");
                if(query != "")
                {
                    this.parameters.Add("pidfile", query);
                }

                var responseBytes = client.UploadValues(ApiCompiledPath+"schedule.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            //TraceLogObj.WriteToLog("End Function Call: Get Daemon Stats.");
            return response;
        }

        public string ApiImportFile(string UploadFile, string ImportTitle, string ImportNotes)
        {
            //TraceLogObj.WriteToLog(ApiCompiledPath + "import.php  ---- Import File.");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //TraceLogObj.WriteToLog(this.parameters.Get("username"));
                //TraceLogObj.WriteToLog(this.parameters.Get("apikey"));
                byte[] hashBytes;
                string hashish;
                using (var inputFileStream = File.Open(UploadFile, FileMode.Open))
                {
                    var md5 = MD5.Create();
                    hashBytes = md5.ComputeHash(inputFileStream);
                    hashish = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                }


                this.parameters.Add("title", ImportTitle);
                this.parameters.Add("notes", ImportNotes);

                //parameters.Add("otherusers", "");
                this.parameters.Add("hash", hashish);
                client.QueryString = this.parameters;
                var responseBytes = client.UploadFile(ApiCompiledPath + "import.php", UploadFile);
                response = Encoding.ASCII.GetString(responseBytes);

                //TraceLogObj.WriteToLog("End Function Call: Import File.");
                //TraceLogObj.WriteToLog("Response: " + response);
                return response;
            }
        }

        private void InitParameters()
        {
            //TraceLogObj.WriteToLog("Start Function Call: Init Params.");
            this.parameters = new NameValueCollection();

            this.parameters.Add("output", "xml");
            this.parameters.Add("username", Username);
            this.parameters.Add("apikey", ApiKey);
            
            //TraceLogObj.WriteToLog("InitParameters ApiKey: " + ApiKey);
            //TraceLogObj.WriteToLog("End Function Call: Init Params.");
        }
        
        public string CheckFileHash(string FileHash)
        {
            //TraceLogObj.WriteToLog("Start Function Call: Check File Hash.");
            string response;
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //TraceLogObj.WriteToLog(this.parameters.Get("username"));
                TraceLogObj.WriteToLog(ApiCompiledPath + "import.php");
                this.parameters.Add("func", "check_hash");
                this.parameters.Add("hash", FileHash);
                var responseBytes = client.UploadValues(ApiCompiledPath + "import.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            //TraceLogObj.WriteToLog("End Function Call: Check File Hash.");
            return response;
        }

        public string ParseApiResponse( string response )
        {
            //TraceLogObj.WriteToLog("Start Function Call: Parse API Response.");
            string ret = "";
            if(response == "")
            {
                return "errorParsing";
            }
            //TraceLogObj.WriteToLog("RESPONSE: -------------------------- ");
            //TraceLogObj.WriteToLog(response);
            //TraceLogObj.WriteToLog(" -------------------------- ");
            //System.Threading.Thread.Sleep(2000);
            //TraceLogObj.WriteToLog(response);
            XElement xmlTree = XElement.Parse(response);
            //TraceLogObj.WriteToLog("Name: " + xmlTree.Name.ToString() + " - Value: " + xmlTree.Value.ToString());
            switch(xmlTree.Name.ToString())
            {
                case "error":
                    ret = "error|~|There was An error during Import-~-" + xmlTree.Value.ToString();
                    TraceLogObj.WriteToLog("There was An error during Import: " + xmlTree.Value.ToString());
                    break;

                case "scheduling":
                    //case "waiting":
                    foreach (var item in xmlTree.Elements())
                    {
                        switch(item.Name.ToString().ToLower())
                        {
                            case "unknown":
                                ret = ret + "Error|~|";
                                break;
                            case "waiting":
                                ret = ret + "|~|waiting|";
                                break;
                            case "importing":
                                ret = ret + "|~|importing|";
                                break;
                            case "finished":
                                ret = ret + "|~|finished|";
                                break;
                            case "bad":
                                ret = ret + "Error|~|";
                                break;
                        }
                        
                        //TraceLogObj.WriteToLog("Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                        foreach (var subitem in item.Elements())
                        {
                            ret = ret + subitem.Name.ToString() + "-~-" + subitem.Value.ToString() + "|";
                            //TraceLogObj.WriteToLog("Name: " + subitem.Name.ToString() + " --   Value: " + subitem.Value.ToString());
                        }
                    }
                    //TraceLogObj.WriteToLog("Parse Return: "+ret);
                    //    break;

                    break;
                case "daemons":
                    if(xmlTree.Value.ToString() != "No Daemons running.")
                    {
                        foreach (var item in xmlTree.Elements())
                        {
                            //TraceLogObj.WriteToLog("--Parent Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                            ret = ret + "|~|" + item.Name.ToString() + "|";
                            foreach (var subitem in item.Elements())
                            {
                                //TraceLogObj.WriteToLog("------Child Name: " + subitem.Name.ToString() + " --   Value: " + subitem.Value.ToString());
                                ret = ret + subitem.Name.ToString() + "-~-" + subitem.Value.ToString() + "|";
                            }
                        }
                    }else
                    {
                        ret = "error|~|No_Daemons_Running";
                    }
                    //TraceLogObj.WriteToLog("Ret: " + ret);
                    break;
                case "import":
                    foreach(var item in xmlTree.Elements())
                    {
                        ret = ret + "|~|import|"+ item.Name.ToString() + "-~-" + item.Value.ToString();
                        ////TraceLogObj.WriteToLog("Name: " + item.Name.ToString() + "-~-" + item.Value.ToString());
                    }
                    break;

                case "importing":
                    foreach (var item in xmlTree.Elements())
                    {
                        //TraceLogObj.WriteToLog("Type: " + item.GetType().ToString());
                        ret = ret+ "|~|importing|" + item.Name.ToString() + "-~-" + item.Value.ToString();
                        ////TraceLogObj.WriteToLog("Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                    }
                    break;

                case "finished":
                    foreach (var item in xmlTree.Elements())
                    {
                        ret = ret + "|~|finished|" + item.Name.ToString() + "-~-" + item.Value.ToString();
                        ////TraceLogObj.WriteToLog("Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                    }
                    break;


                case "bad":
                    foreach (var item in xmlTree.Elements())
                    {
                        ret = ret + "|~|bad|" + item.Name.ToString() + "-~-" + item.Value.ToString();
                        ////TraceLogObj.WriteToLog("Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                    }
                    break;

                default:
                    ret = "unknown|Unknown return: " + xmlTree.Value.ToString();
                    ////TraceLogObj.WriteToLog("Unknown return: " + xmlTree.Value.ToString());
                    break;
            }
            ////TraceLogObj.WriteToLog(ret);
            //TraceLogObj.WriteToLog("End Function Call: Parse API Response.");
            return ret;
        }

        public void WriteLog(string message)
        {
            string LogFile = LogPath + "/Trace.log";
            string line = "[" + DateTime.Now.ToString("yyyy-MM-dd") + "]" + "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "[" + message + "]";

            TraceLogObj.WriteToLog(line);

            System.IO.StreamWriter file = new System.IO.StreamWriter(LogFile, true);
            file.WriteLine(line);
            file.Close();
        }
    }
}
