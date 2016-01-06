using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;
using System.Diagnostics;

namespace WDBAPI
{
    public class WDBAPI
    {
        public string address = "https://live.wifidb.net/wifidb/api/import.php";

        public void ApiPostFile(string[] Query, string UploadFile)
        {   
            using (WebClient client = new WebClient())
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("username", "pferland");
                parameters.Add("apikey", "");
                parameters.Add("title", "123");
                parameters.Add("otherusers", "xyz");
                parameters.Add("notes", "xyz");
                parameters.Add("hash", "xyz");
                client.QueryString = parameters;
                var responseBytes = client.UploadFile(address, "C:\\GitHub\\Czar-Incidents\\Reports\\2016-01-01_04-02-45.pdf");
                string response = Encoding.ASCII.GetString(responseBytes);

                Debug.WriteLine(response);
            }
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
