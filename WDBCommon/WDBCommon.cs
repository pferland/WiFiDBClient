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
            //Debug.WriteLine("Start Function Call: Init API.");
            WDBAPIObj.ApiCompiledPath = ApiCompiledPath;
            WDBAPIObj.ApiKey = ApiKey;
            WDBAPIObj.Username = Username;
            WDBAPIObj.DefaultImportTitle = DefaultImportTitle;
            WDBAPIObj.DefaultImportNotes = DefaultImportNotes;
            WDBAPIObj.UseDefaultImportValues = UseDefaultImportValues;
            //Debug.WriteLine("End Function Call: Init API.");
        }

        public void GetHashStatus(string query, BackgroundWorker BgWk)
        {
            BgWk.ReportProgress(0, "");
            BgWk.ReportProgress(100, WDBAPIObj.ParseApiResponse(WDBAPIObj.CheckFileHash(query)));
        }

        public int IsFileImported(string query)
        {
            string response = WDBAPIObj.ParseApiResponse(WDBAPIObj.CheckFileHash(query));
            //Debug.WriteLine( "IsFileImported Response: " + response );

            if(response == "")
            {
                return 1;
            }else
            {
                return 0;
            }
        }

        public void GetDaemonStatuses(string query, BackgroundWorker BgWk)
        {
            //Debug.WriteLine("Active Server: " + WDBAPIObj.ApiCompiledPath);
            BgWk.ReportProgress(0, "");
            BgWk.ReportProgress(100, WDBAPIObj.ParseApiResponse(WDBAPIObj.ApiGetDaemonStatuses(query)));
        }

        public void GetWaiting(string query, BackgroundWorker BgWk)
        {
            
        }

        public List<KeyValuePair<int, string>> ImportFolder(string Path, string ImportTitle, string ImportNotes, BackgroundWorker BW)
        {
            var responses = new List<KeyValuePair<int, string>>();
            try
            {
                byte[] hashBytes;
                string hashish;
                var md5 = MD5.Create();

                string[] extensions = new string[] { "*.vs1", "*.vsz", "*.csv", "*.db3" };

                // Loop through the file extension types, find them in the provided folder, then import it.
                foreach (string ext in extensions)
                {
                    Debug.WriteLine("Extension that will be used: " + ext);
                    string[] files = Directory.GetFiles(Path, ext);
                    //Debug.WriteLine("The number of VS1 files: {0}.", files.Length);
                    if (files.Length > 0)
                    {
                        foreach (string file in files)
                        {
                            InternalImportID++;
                            //Debug.WriteLine(file);
                            using(var inputFileStream = File.Open(file, FileMode.Open))
                            {
                                hashBytes = md5.ComputeHash(inputFileStream);
                                hashish = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                            }

                            if (IsFileImported(hashish) == 1)
                            {
                                BW.ReportProgress(0, "newrow|~|" + file + "|~|" + hashish);
                                responses.Add(new KeyValuePair<int, string>(InternalImportID, WDBAPIObj.ParseApiResponse(WDBAPIObj.ApiImportFile(file, ImportTitle, ImportNotes ))));
                                BW.ReportProgress(0, responses.Last().Value.ToString());

                                ArchiveImportedFile(file);
                                //Debug.WriteLine(responses.Last().Value.ToString());
                            }
                            else
                            {
                                Debug.WriteLine("-------------> File " + file + " Already Imported.");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("The process failed: {0}", e.ToString());
            }
            return responses;
        }

        private void ArchiveImportedFile(string FilePath)
        {
            if(ArchiveImports && (ArchiveImportsFolderPath != "" || ArchiveImportsFolderPath != null) )
            {
                try
                {
                    string FileName = Path.GetFileName(FilePath);

                    string ArchivedFile = ArchiveImportsFolderPath + "\\" + FileName;

                    File.Move(FilePath, ArchivedFile);
                    Debug.WriteLine("{0} was moved to {1}.", FilePath, ArchivedFile);
                    
                }
                catch (Exception e)
                {
                    Debug.WriteLine("The process failed: {0}", e.ToString());
                }
            }
            else
            {
                Debug.WriteLine("Archve Imported Files is disabled.");
            }
        }


        public string ImportFile(string FilePath, string ImportTitle, string ImportNotes, BackgroundWorker BW)
        {
            string response;
            byte[] hashBytes;
            string hashish;
            var md5 = MD5.Create();
            InternalImportID++;
            //Debug.WriteLine(db3);
            using (var inputFileStream = File.Open(FilePath, FileMode.Open))
            {
                hashBytes = md5.ComputeHash(inputFileStream);
                hashish = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
            if(IsFileImported(hashish) == 1)
            {
                BW.ReportProgress(0, "newrow|~|" + FilePath + "|~|" + hashish);
                response = WDBAPIObj.ParseApiResponse(WDBAPIObj.ApiImportFile(FilePath, ImportTitle, ImportNotes));
                BW.ReportProgress(0, response);
                //Debug.WriteLine(responses.Last().Value.ToString());
            }else
            {
                response = "File is already imported.";
            }
            return response;
        }
    }
}
