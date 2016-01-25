using System;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Xml.Linq;

namespace WDBAPI
{
    public class WDBAPI
    {
        public NameValueCollection parameters = null;

        public string ApiGetWaitingImports()
        {
            string address = "http://dev.randomintervals.com/wifidb/api/v2/schedule.php";
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                this.parameters.Add("func", "waiting");

                var responseBytes = client.UploadValues(address, "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
                //Debug.WriteLine(response);
            }
           return response;
        }


        public string ApiGetFinishedImports()
        {
            string address = "http://dev.randomintervals.com//wifidb/api/v2/schedule.php";
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                this.parameters.Add("func", "finished");

                var responseBytes = client.UploadValues(address, "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            return response;
        }


        public string ApiGetBadIports()
        {
            string address = "http://dev.randomintervals.com//wifidb/api/v2/schedule.php";
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                this.parameters.Add("func", "bad");

                var responseBytes = client.UploadValues(address, "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            return response;
        }


        public string ApiGetCurrentImporting()
        {
            string address = "http://dev.randomintervals.com//wifidb/api/v2/schedule.php";
            string response;
            //Console.WriteLine("Upload File: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                this.parameters.Add("func", "importing");

                var responseBytes = client.UploadValues(address, "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            return response;
        }
        

        public string ApiGetDaemonStatuses(string query)
        {
            string address = "http://dev.randomintervals.com//wifidb/api/v2/schedule.php";
            string response;
            //Console.WriteLine("Upload File: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                this.parameters.Add("func", "daemonstatuses");
                if(query != "")
                {
                    this.parameters.Add("pidfile", query);
                }

                var responseBytes = client.UploadValues(address, "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            return response;
        }

        public string ApiImportFile(string UploadFile, bool CheckHash = false)
        {
            string address = "http://dev.randomintervals.com//wifidb/api/import.php";
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                InitParameters();
                byte[] hashBytes;
                string hashish;
                using (var inputFileStream = File.Open(UploadFile, FileMode.Open))
                {
                    var md5 = MD5.Create();
                    hashBytes = md5.ComputeHash(inputFileStream);
                    hashish = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                }
                this.parameters.Add("title", "C%23 upload Test");

                //parameters.Add("otherusers", "");
                this.parameters.Add("notes", "C# upload Test");
                this.parameters.Add("hash", hashish);
                client.QueryString = this.parameters;
                var responseBytes = client.UploadFile(address, UploadFile);
                response = Encoding.ASCII.GetString(responseBytes);
                return response;
            }
        }

        private void InitParameters()
        {
            if (this.parameters == null)
            {
                this.parameters = new NameValueCollection();
                this.parameters.Add("output", "xml");
                this.parameters.Add("username", "pferland");
                this.parameters.Add("apikey", "GSn8NQeYzY8gq5Y8NFpf5gZZqH33kdBctEOwWzsOTmxCnrs4BYk32rgeNLNhLkzj");
            }
            else
            {
                this.parameters.Remove("title");
                this.parameters.Remove("notes");
                this.parameters.Remove("hash");
                this.parameters.Remove("func");
            }
        }

        public string CheckFileHash(string FileHash)
        {
            string address = "http://dev.randomintervals.com//wifidb/api/v2/import.php";
            string response;
            using (WebClient client = new WebClient())
            {
                InitParameters();
                this.parameters.Add("func", "check_hash");
                this.parameters.Add("hash", FileHash);
                var responseBytes = client.UploadValues(address, "POST", this.parameters);
                response = Encoding.ASCII.GetString(responseBytes);
            }
            return response;
        }

        public string ParseApiResponse( string response )
        {
            string ret = "";
            if(response == "")
            {
                return "errorParsing";
            }
            //Debug.WriteLine("RESPONSE: -------------------------- ");
            //Debug.WriteLine(response);
            //Debug.WriteLine(" -------------------------- ");
            System.Threading.Thread.Sleep(2000);
            XElement xmlTree = XElement.Parse(response);
            //Debug.WriteLine("Name: " + xmlTree.Name.ToString() + " - Value: " + xmlTree.Value.ToString());
            switch(xmlTree.Name.ToString())
            {
                case "error":
                    ret = "error|~|There was An error during Import-~-" + xmlTree.Value.ToString();
                    ////Debug.WriteLine("There was An error during Import: " + xmlTree.Value.ToString());
                    break;

                case "scheduling":
                    //case "waiting":
                    foreach (var item in xmlTree.Elements())
                    {
                        switch(item.Name.ToString())
                        {
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
                    foreach(var item in xmlTree.Elements())
                    {
                        //Debug.WriteLine("--Parent Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                        ret = ret + "|~|" + item.Name.ToString() + "|";
                        foreach (var subitem in item.Elements())
                        {
                            //Debug.WriteLine("------Child Name: " + subitem.Name.ToString() + " --   Value: " + subitem.Value.ToString());
                            ret = ret + subitem.Name.ToString() + "-~-" + subitem.Value.ToString() + "|";
                        }
                    }
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
            return ret;
        }
    }
}
