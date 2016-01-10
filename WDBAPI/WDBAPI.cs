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
        public string address = "http://dev.randomintervals.com//wifidb/api/import.php";

        public string ApiGetWaitingImports()
        {


            return "";
        }


        public string ApiGetFinishedIports()
        {


            return "";
        }


        public string ApiGetCurrentImporting()
        {

            return "";
        }


        public string ApiImportFile(string[] Query, string UploadFile, bool CheckHash = false)
        {
            string response;
            //Console.WriteLine("Upload FIle: " + UploadFile);
            using (WebClient client = new WebClient())
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("output", "xml");
                byte[] hashBytes;
                string hashish;
                using (var inputFileStream = File.Open(UploadFile, FileMode.Open))
                {
                    var md5 = MD5.Create();
                    hashBytes = md5.ComputeHash(inputFileStream);
                    hashish = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                }

                if (CheckHash)
                {
                    parameters.Add("func", "check_hash");
                    parameters.Add("hash", hashish);
                    var responseBytes = client.UploadValues(address, "POST", parameters);
                    response = Encoding.ASCII.GetString(responseBytes);
                }
                else
                {
                    parameters.Add("username", "pferland");
                    parameters.Add("apikey", "GSn8NQeYzY8gq5Y8NFpf5gZZqH33kdBctEOwWzsOTmxCnrs4BYk32rgeNLNhLkzj");
                    parameters.Add("title", "C%23 upload Test");

                    //parameters.Add("otherusers", "");
                    parameters.Add("notes", "C# upload Test");
                    parameters.Add("hash", hashish);
                    client.QueryString = parameters;
                    var responseBytes = client.UploadFile(address, UploadFile);
                    response = Encoding.ASCII.GetString(responseBytes);
                }
                return response;
            }
        }

        public int ParseApiResponse( string response )
        {
            int ret = 0;

            Debug.WriteLine(" RESPONSE: -------------------------- ");
            Debug.WriteLine(response);
            Debug.WriteLine(" -------------------------- ");

            XElement xmlTree = XElement.Parse(response);
            //Console.WriteLine("Name: " + xmlTree.Name.ToString() + " - Value: " + xmlTree.Value.ToString());

            switch(xmlTree.Name.ToString())
            {
                case "error":
                    Debug.WriteLine("There was An error during Import: " + xmlTree.Value.ToString());
                    break;

                case "import":
                    foreach(var item in xmlTree.Elements())
                    {
                        Debug.WriteLine("Name: " + item.Name.ToString() + " --   Value: " + item.Value.ToString());
                    }
                    
                    //Debug.WriteLine("The FIle is being imported: " +   .Value.ToString());
                    break;

                case "imported":
                    Debug.WriteLine("Imported: " + xmlTree.Value.ToString());
                    break;

                case "waiting":
                    Debug.WriteLine("Waiting to Import: " + xmlTree.Value.ToString());
                    break;

                case "bad":
                    Debug.WriteLine("Bad Import: " + xmlTree.Value.ToString());
                    break;

                default:
                    Debug.WriteLine("Unknown return: " + xmlTree.Value.ToString());
                    break;
            }


            return ret;
        }

        public void ApiPostStrings(string Query)
        {
            using (WebClient client = new WebClient())
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("value1", "123");
                parameters.Add("value2", "xyz");
                var responseBytes = client.UploadValues(address, parameters);
                string response = Encoding.ASCII.GetString(responseBytes);
            }
        }
    }
}
