using System;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

namespace WDBAPI
{
    public class WDBAPI
    {
        public NameValueCollection parameters = null;

        public string LogPath;
        private string _ThreadName;
        public string ThreadName
        {
            get { return _ThreadName; }
            set { _ThreadName = value; }
        }
        public string ObjectName = "WDBAPI";

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
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "Start Call: ApiGetWaitingImports");
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), ApiCompiledPath + "schedule.php  ---- Get Waiting");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("username"));
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("apikey"));
                this.parameters.Add("func", "waiting");
                try
                {
                    var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                    response = Encoding.ASCII.GetString(responseBytes);
                }catch(Exception e)
                {
                    response = "Error";
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), e.Message);
                }
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), response);
            }
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "End Call: ApiGetWaitingImports");
            return response;
        }
        
        public string ApiGetFinishedImports()
        {
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "Start Call: ApiGetFinishedImports");
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), ApiCompiledPath + "schedule.php  ---- Get Finished");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("username"));
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("apikey"));
                this.parameters.Add("func", "finished");
                try
                {
                    var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                    response = Encoding.ASCII.GetString(responseBytes);
                }
                catch (Exception e)
                {
                    response = "Error";
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), e.Message);
                }
            }
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "End Function Call: ApiGetFinishedImports.");
            return response;
        }
        
        public string ApiGetBadIports()
        {
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "Start Call: ApiGetBadIports");
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), ApiCompiledPath + "schedule.php  ---- Get Bad Imports.");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("username"));
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("apikey"));
                this.parameters.Add("func", "bad");
                try
                {
                    var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                    response = Encoding.ASCII.GetString(responseBytes);
                }
                catch (Exception e)
                {
                    response = "Error";
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), e.Message);
                }
            }
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "End Call: ApiGetBadIports");
            return response;
        }
        
        public string ApiGetCurrentImporting()
        {
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "Start Call: ApiGetCurrentImporting");
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), ApiCompiledPath + "schedule.php  ---- Get Current Imports.");
            string response;
            //Console.WriteLine("Upload File: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("username"));
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("apikey"));
                this.parameters.Add("func", "importing");
                try
                {
                    var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                    response = Encoding.ASCII.GetString(responseBytes);
                }
                catch (Exception e)
                {
                    response = "Error";
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), e.Message);
                }
            }
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "End Call: ApiGetCurrentImporting");
            return response;
        }
        
        public string ApiGetDaemonStatuses(string query)
        {
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "Start Call: ApiGetDaemonStatuses");
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), ApiCompiledPath + "schedule.php  ---- Get Daemon Stats");
            
            string response;
            //Console.WriteLine("Upload File: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("username"));
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("apikey"));
                this.parameters.Add("func", "daemonstatuses");
                if(query != "")
                {
                    this.parameters.Add("pidfile", query);
                }
                try
                {
                    var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                    response = Encoding.ASCII.GetString(responseBytes);
                    return response;
                }
                catch(Exception e)
                {
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "UploadValues Exception: " + e.Message);
                }
            }
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "End Call: ApiGetDaemonStatuses");
            return "Error";
        }

        public string ApiImportFile(string UploadFile, string ImportTitle, string ImportNotes)
        {
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "Start Call: ApiImportFile");
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), ApiCompiledPath + "import.php  ---- Import File: " + UploadFile);
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("username"));
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("apikey"));
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

                try
                {
                    client.QueryString = this.parameters;
                    var responseBytes = client.UploadFile(ApiCompiledPath + "import.php", UploadFile);
                    response = Encoding.ASCII.GetString(responseBytes);
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Response: " + response);
                }
                catch (Exception e)
                {
                    response = "Error";
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), e.Message);
                }


                TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "End Call: ApiImportFile");
                return response;
            }
        }

        private void InitParameters()
        {
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Start Function Call: Init Params.");
            this.parameters = new NameValueCollection();

            this.parameters.Remove("output");
            this.parameters.Remove("username");
            this.parameters.Remove("apikey");

            this.parameters.Add("output", "xml");
            this.parameters.Add("username", Username);
            this.parameters.Add("apikey", ApiKey);
            
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "InitParameters ApiKey: " + ApiKey);
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "End Function Call: Init Params.");
        }
        
        public string CheckFileHash(string FileHash)
        {
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Start Function Call: Check File Hash.");
            string response;
            using (WebClient client = new WebClient())
            {
                InitParameters();
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), this.parameters.Get("username"));
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), ApiCompiledPath + "import.php");
                this.parameters.Add("func", "check_hash");
                this.parameters.Add("hash", FileHash);
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Number of Params for CheckFileHash: " + this.parameters.Count);
                try
                {
                    var responseBytes = client.UploadValues(ApiCompiledPath + "import.php", "POST", this.parameters);
                    response = Encoding.ASCII.GetString(responseBytes);
                }
                catch(Exception e)
                {
                    response = "Error";
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "UploadValues Exception: " + e.Message);
                }
                
            }
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "End Function Call: Check File Hash.");
            return response;
        }

        public string ParseApiResponse( string response )
        {
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Start Function Call: Parse API Response.");
            string ret = "";
            if(response == "")
            {
                return "errorParsing";
            }else if(response == "Error")
            {
                return "errorParsing";
            }
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "RESPONSE: -------------------------- ");
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Parse Response: " + response);
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), " -------------------------- ");
            //System.Threading.Thread.Sleep(2000);
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), response);
            XElement xmlTree = XElement.Parse(response);
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Name: " + xmlTree.Name.ToString() + " - Value: " + xmlTree.Value.ToString());
            switch(xmlTree.Name.ToString())
            {
                case "error":

                    ret = "error|~|There was An error during Import-~-" + xmlTree.Value.ToString();
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "There was An error during Import: " + xmlTree.Value.ToString());
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
                        
                        TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                        foreach (var subitem in item.Elements())
                        {
                            ret = ret + subitem.Name.ToString() + "-~-" + subitem.Value.ToString() + "|";
                            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Name: " + subitem.Name.ToString() + " --   Value: " + subitem.Value.ToString());
                        }
                    }
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Parse Return: "+ret);
                    //    break;

                    break;
                case "daemons":
                    if(xmlTree.Value.ToString() != "No Daemons running.")
                    {
                        foreach (var item in xmlTree.Elements())
                        {
                            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "--Parent Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                            ret = ret + "|~|" + item.Name.ToString() + "|";
                            foreach (var subitem in item.Elements())
                            {
                                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "------Child Name: " + subitem.Name.ToString() + " --   Value: " + subitem.Value.ToString());
                                ret = ret + subitem.Name.ToString() + "-~-" + subitem.Value.ToString() + "|";
                            }
                        }
                    }else
                    {
                        ret = "error|~|No_Daemons_Running";
                    }
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Ret: " + ret);
                    break;
                case "import":
                    foreach(var item in xmlTree.Elements())
                    {
                        ret = ret + "|~|import|"+ item.Name.ToString() + "-~-" + item.Value.ToString();
                        TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Name: " + item.Name.ToString() + "-~-" + item.Value.ToString());
                    }
                    break;

                case "importing":
                    foreach (var item in xmlTree.Elements())
                    {
                        TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Type: " + item.GetType().ToString());
                        ret = ret+ "|~|importing|" + item.Name.ToString() + "-~-" + item.Value.ToString();
                        TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                    }
                    break;

                case "finished":
                    foreach (var item in xmlTree.Elements())
                    {
                        ret = ret + "|~|finished|" + item.Name.ToString() + "-~-" + item.Value.ToString();
                        TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                    }
                    break;


                case "bad":
                    foreach (var item in xmlTree.Elements())
                    {
                        ret = ret + "|~|bad|" + item.Name.ToString() + "-~-" + item.Value.ToString();
                        TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                    }
                    break;

                default:
                    ret = "unknown|Unknown return: " + xmlTree.Value.ToString();
                    TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Unknown return: " + xmlTree.Value.ToString());
                    break;
            }
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), ret);
            TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "End Function Call: Parse API Response.");
            return ret;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
    }
}
