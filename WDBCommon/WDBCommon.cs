using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WDBAPI;

namespace WDBCommon
{
    public class WDBCommon
    {
        private WDBAPI.WDBAPI WDBAPIObj = new WDBAPI.WDBAPI();
        public int InternalImportID = 0;

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

        public void initApi()
        {
            WDBAPIObj.ApiCompiledPath = ApiCompiledPath;
            WDBAPIObj.ApiKey = ApiKey;
            WDBAPIObj.Username = Username;
            WDBAPIObj.DefaultImportTitle = DefaultImportTitle;
            WDBAPIObj.DefaultImportNotes = DefaultImportNotes;
        }

        public void GetHashStatus(string query, BackgroundWorker BgWk)
        {
            BgWk.ReportProgress(0, "");
            BgWk.ReportProgress(100, WDBAPIObj.ParseApiResponse(WDBAPIObj.CheckFileHash(query)));
        }

        public void GetDaemonStatuses(string query, BackgroundWorker BgWk)
        {
            BgWk.ReportProgress(0, "");
            BgWk.ReportProgress(100, WDBAPIObj.ParseApiResponse(WDBAPIObj.ApiGetDaemonStatuses(query)));
        }

        public void GetWaiting(string query, BackgroundWorker BgWk)
        {
            
        }

        public List<KeyValuePair<int, string>> ImportFolder(string Path, BackgroundWorker BW)
        {
            var responses = new List<KeyValuePair<int, string>>();
            try
            {

                byte[] hashBytes;
                string hashish;
                var md5 = MD5.Create();

                // IMPORT ALL VS1 FILES.
                string[] vs1s = Directory.GetFiles(Path, "*.vs1");
                //Debug.WriteLine("The number of VS1 files: {0}.", vs1s.Length);
                if(vs1s.Length > 0)
                {
                    foreach (string vs1 in vs1s)
                    {
                        InternalImportID++;
                        //Debug.WriteLine(vs1);
                        using (var inputFileStream = File.Open(vs1, FileMode.Open))
                        {
                            hashBytes = md5.ComputeHash(inputFileStream);
                            hashish = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                        }
                        BW.ReportProgress(0, "newrow|~|" + vs1 + "|~|" + hashish);
                        responses.Add(new KeyValuePair<int, string>(InternalImportID, WDBAPIObj.ParseApiResponse(WDBAPIObj.ApiImportFile(vs1) ) ) );
                        BW.ReportProgress(0, responses.Last().Value.ToString());
                        //Debug.WriteLine(responses.Last().Value.ToString());
                    }
                }

                // IMPORT ALL VSZ FILES.
                string[] vszs = Directory.GetFiles(Path, "*.vsz");
                //Debug.WriteLine("The number of VSZ files: {0}.", vszs.Length);
                if (vszs.Length > 0)
                {
                    foreach (string vsz in vszs)
                    {
                        InternalImportID++;
                        //Debug.WriteLine(vsz);
                        using (var inputFileStream = File.Open(vsz, FileMode.Open))
                        {
                            hashBytes = md5.ComputeHash(inputFileStream);
                            hashish = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                        }

                        BW.ReportProgress(0, "newrow|~|" + vsz + "|~|" + hashish);
                        responses.Add(new KeyValuePair<int, string>(InternalImportID, WDBAPIObj.ApiImportFile(vsz)));
                        BW.ReportProgress(0, responses.Last().Value.ToString());
                        //Debug.WriteLine(responses.Last().Value.ToString());
                    }
                }
                
                // IMPORT ALL CSV FILES.
                string[] csvs = Directory.GetFiles(Path, "*.csv");
                //Debug.WriteLine("The number of CSV files: {0}.", csvs.Length);
                if (csvs.Length > 0)
                {
                    foreach (string csv in csvs)
                    {
                        InternalImportID++;
                        //Debug.WriteLine(csv);
                        using (var inputFileStream = File.Open(csv, FileMode.Open))
                        {
                            hashBytes = md5.ComputeHash(inputFileStream);
                            hashish = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                        }
                        BW.ReportProgress(0, "newrow|~|" + csv + "|~|" + hashish);
                        responses.Add(new KeyValuePair<int, string>(InternalImportID, WDBAPIObj.ApiImportFile(csv)));
                        BW.ReportProgress(0, responses.Last().Value.ToString());
                        //Debug.WriteLine(responses.Last().Value.ToString());
                    }
                }
                
                // IMPORT ALL WarDrive 4 Android db3 FILES.
                string[] db3s = Directory.GetFiles(Path, "*.db3");
                //Debug.WriteLine("The number of db3 files: {0}.", db3s.Length);
                if (db3s.Length > 0)
                {
                    foreach (string db3 in db3s)
                    {
                        InternalImportID++;
                        //Debug.WriteLine(db3);
                        using (var inputFileStream = File.Open(db3, FileMode.Open))
                        {
                            hashBytes = md5.ComputeHash(inputFileStream);
                            hashish = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                        }
                        BW.ReportProgress(0, "newrow|~|" + db3 + "|~|" + hashish);
                        responses.Add(new KeyValuePair<int, string>(InternalImportID, WDBAPIObj.ApiImportFile(db3)));
                        BW.ReportProgress(0, responses.Last().Value.ToString());
                        //Debug.WriteLine(responses.Last().Value.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                //Debug.WriteLine("The process failed: {0}", e.ToString());
            }
            return responses;
        }

        public string ImportFile(string FilePath)
        {
            WDBAPIObj.ParseApiResponse(WDBAPIObj.ApiImportFile(FilePath));
            return "return";
        }
    }
}
