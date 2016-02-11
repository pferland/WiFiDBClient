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

        public string ApiGetWaitingImports()
        {
            //Debug.WriteLine(ApiCompiledPath + "schedule.php  ---- Get Waiting");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //Debug.WriteLine(this.parameters.Get("username"));
                //Debug.WriteLine(this.parameters.Get("apikey"));
                this.parameters.Add("func", "waiting");

                var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
                //Debug.WriteLine(response);
            }
            //Debug.WriteLine("End Function Call: Get Waiting.");
            return response;
        }
        
        public string ApiGetFinishedImports()
        {
            //Debug.WriteLine(ApiCompiledPath + "schedule.php  ---- Get Finished");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //Debug.WriteLine(this.parameters.Get("username"));
                //Debug.WriteLine(this.parameters.Get("apikey"));
                this.parameters.Add("func", "finished");

                var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            //Debug.WriteLine("End Function Call: Get Finished.");
            return response;
        }
        
        public string ApiGetBadIports()
        {
            //Debug.WriteLine(ApiCompiledPath + "schedule.php  ---- Get Bad Imports.");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //Debug.WriteLine(this.parameters.Get("username"));
                //Debug.WriteLine(this.parameters.Get("apikey"));
                this.parameters.Add("func", "bad");

                var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            //Debug.WriteLine("End Function Call: Get Bad Imports.");
            return response;
        }
        
        public string ApiGetCurrentImporting()
        {
            //Debug.WriteLine(ApiCompiledPath + "schedule.php  ---- Get Current Imports.");
            string response;
            //Console.WriteLine("Upload File: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //Debug.WriteLine(this.parameters.Get("username"));
                //Debug.WriteLine(this.parameters.Get("apikey"));
                this.parameters.Add("func", "importing");

                var responseBytes = client.UploadValues(ApiCompiledPath + "schedule.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            //Debug.WriteLine("End Function Call: Get Current Imports.");
            return response;
        }
        
        public string ApiGetDaemonStatuses(string query)
        {
            //Debug.WriteLine(ApiCompiledPath + "schedule.php  ---- Get Daemon Stats");
            string response;
            //Console.WriteLine("Upload File: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //Debug.WriteLine(this.parameters.Get("username"));
                //Debug.WriteLine(this.parameters.Get("apikey"));
                this.parameters.Add("func", "daemonstatuses");
                if(query != "")
                {
                    this.parameters.Add("pidfile", query);
                }

                var responseBytes = client.UploadValues(ApiCompiledPath+"schedule.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            //Debug.WriteLine("End Function Call: Get Daemon Stats.");
            return response;
        }

        public string ApiImportFile(string UploadFile, string ImportTitle, string ImportNotes)
        {
            //Debug.WriteLine(ApiCompiledPath + "import.php  ---- Import File.");
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //Debug.WriteLine(this.parameters.Get("username"));
                //Debug.WriteLine(this.parameters.Get("apikey"));
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

                //Debug.WriteLine("End Function Call: Import File.");
                //Debug.WriteLine("Response: " + response);
                return response;
            }
        }

        private void InitParameters()
        {
            //Debug.WriteLine("Start Function Call: Init Params.");
            this.parameters = new NameValueCollection();

            this.parameters.Add("output", "xml");
            this.parameters.Add("username", Username);
            this.parameters.Add("apikey", ApiKey);
            
            //Debug.WriteLine("InitParameters ApiKey: " + ApiKey);
            //Debug.WriteLine("End Function Call: Init Params.");
        }
        
        public string CheckFileHash(string FileHash)
        {
            //Debug.WriteLine("Start Function Call: Check File Hash.");
            string response;
            using (WebClient client = new WebClient())
            {
                InitParameters();
                //Debug.WriteLine(this.parameters.Get("username"));
                //Debug.WriteLine(this.parameters.Get("apikey"));
                this.parameters.Add("func", "check_hash");
                this.parameters.Add("hash", FileHash);
                var responseBytes = client.UploadValues(ApiCompiledPath + "import.php", "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            //Debug.WriteLine("End Function Call: Check File Hash.");
            return response;
        }

        public string ParseApiResponse( string response )
        {
            //Debug.WriteLine("Start Function Call: Parse API Response.");
            string ret = "";
            if(response == "")
            {
                return "errorParsing";
            }
            //Debug.WriteLine("RESPONSE: -------------------------- ");
            //Debug.WriteLine(response);
            //Debug.WriteLine(" -------------------------- ");
            //System.Threading.Thread.Sleep(2000);
            //Debug.WriteLine(response);
            XElement xmlTree = XElement.Parse(response);
            //Debug.WriteLine("Name: " + xmlTree.Name.ToString() + " - Value: " + xmlTree.Value.ToString());
            switch(xmlTree.Name.ToString())
            {
                case "error":
                    ret = "error|~|There was An error during Import-~-" + xmlTree.Value.ToString();
                    Debug.WriteLine("There was An error during Import: " + xmlTree.Value.ToString());
                    break;

                case "scheduling":
                    //case "waiting":
                    foreach (var item in xmlTree.Elements())
                    {
                        switch(item.Name.ToString())
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
                        }
                        
                        //Debug.WriteLine("Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                        foreach (var subitem in item.Elements())
                        {
                            ret = ret + subitem.Name.ToString() + "-~-" + subitem.Value.ToString() + "|";
                            //Debug.WriteLine("Name: " + subitem.Name.ToString() + " --   Value: " + subitem.Value.ToString());
                        }
                    }
                    //Debug.WriteLine("Parse Return: "+ret);
                    //    break;

                    break;
                case "daemons":
                    if(xmlTree.Value.ToString() != "No Daemons running.")
                    {
                        foreach (var item in xmlTree.Elements())
                        {
                            //Debug.WriteLine("--Parent Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                            ret = ret + "|~|" + item.Name.ToString() + "|";
                            foreach (var subitem in item.Elements())
                            {
                                //Debug.WriteLine("------Child Name: " + subitem.Name.ToString() + " --   Value: " + subitem.Value.ToString());
                                ret = ret + subitem.Name.ToString() + "-~-" + subitem.Value.ToString() + "|";
                            }
                        }
                    }else
                    {
                        ret = "error|~|No_Daemons_Running";
                    }
                    //Debug.WriteLine("Ret: " + ret);
                    break;
                case "import":
                    foreach(var item in xmlTree.Elements())
                    {
                        ret = ret + "|~|import|"+ item.Name.ToString() + "-~-" + item.Value.ToString();
                        ////Debug.WriteLine("Name: " + item.Name.ToString() + "-~-" + item.Value.ToString());
                    }
                    break;

                case "importing":
                    foreach (var item in xmlTree.Elements())
                    {
                        //Debug.WriteLine("Type: " + item.GetType().ToString());
                        ret = ret+ "|~|importing|" + item.Name.ToString() + "-~-" + item.Value.ToString();
                        ////Debug.WriteLine("Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                    }
                    break;

                case "finished":
                    foreach (var item in xmlTree.Elements())
                    {
                        ret = ret + "|~|finished|" + item.Name.ToString() + "-~-" + item.Value.ToString();
                        ////Debug.WriteLine("Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                    }
                    break;


                case "bad":
                    foreach (var item in xmlTree.Elements())
                    {
                        ret = ret + "|~|bad|" + item.Name.ToString() + "-~-" + item.Value.ToString();
                        ////Debug.WriteLine("Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                    }
                    break;

                default:
                    ret = "unknown|Unknown return: " + xmlTree.Value.ToString();
                    ////Debug.WriteLine("Unknown return: " + xmlTree.Value.ToString());
                    break;
            }
            ////Debug.WriteLine(ret);
            //Debug.WriteLine("End Function Call: Parse API Response.");
            return ret;
        }
    }
}
