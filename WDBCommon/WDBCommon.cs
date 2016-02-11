using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WDBAPI;
using WDBSQLite;
namespace WDBCommon
{
    public class WDBCommon
    {
        private WDBAPI.WDBAPI WDBAPIObj = new WDBAPI.WDBAPI();
        private WDBSQLite.WDBSQLite WDBSQLite;
        
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


        public WDBCommon(string Path)
        {
            Debug.WriteLine("Start of Call: new WDBCommon.WDBCommon(Path, \"Uploader\")");
            WDBSQLite = new WDBSQLite.WDBSQLite(Path, "uploader");
            Debug.WriteLine("End of Call: new WDBCommon.WDBCommon(Path, \"Uploader\")");
        }

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
            //Check the Local SQLite DB for the File hash, If it is found, return now.



            // If the filehash was not found in the Local SQLite DB, check the WDB API.
            string response = WDBAPIObj.ParseApiResponse(WDBAPIObj.CheckFileHash(query));
            Debug.WriteLine( "IsFileImported Response: " + response );

            string[] stringSeparators = new string[] { "|~|" };
            string[] split = response.Split(stringSeparators, StringSplitOptions.None);
            
            if(split[0] == "error")
            {
                return -1;
            }

            Debug.WriteLine("split[0].ToLower(): " + split[0].ToLower());
            if (split[1].ToLower() == "")
            {
                return 0;
            }else
            {
                return 1;
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
                            responses.Add(new KeyValuePair<int, string>(InternalImportID, ImportFile(file, ImportTitle, ImportNotes, BW)));
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
            int IsFileImportedResult = IsFileImported(hashish);
            Debug.WriteLine("IsFileImportedResult :" + IsFileImportedResult);
            if (IsFileImportedResult == 0)
            {
                BW.ReportProgress(0, "newrow|~|" + FilePath + "|~|" + hashish);
                response = WDBAPIObj.ParseApiResponse(WDBAPIObj.ApiImportFile(FilePath, ImportTitle, ImportNotes));
                BW.ReportProgress(0, response);
                //Debug.WriteLine(responses.Last().Value.ToString());
                ArchiveImportedFile(FilePath);
            }
            else
            {
                response = "Already imported or error.";
            }
            return response;
        }
        
        public void InsertImportRow(ImportRow ImportRowObj)
        {
            SQLiteCommand cmd;
            cmd = new SQLiteCommand(WDBSQLite.conn);
            cmd.CommandText = @"INSERT INTO `ImportView` 
(`Username`, `Date_Time`, `FileSize`, `FileName`, `FileHash`, `Status`, `Message`) 
VALUES ( ?, ?, ?, ?, ?, ?, ?)";
            var Username = cmd.CreateParameter();
            Username.Value = ImportRowObj.Username;
            cmd.Parameters.Add(Username);

            var Date_Time = cmd.CreateParameter();
            Date_Time.Value = ImportRowObj.DateTime;
            cmd.Parameters.Add(Date_Time);

            var FileSize = cmd.CreateParameter();
            FileSize.Value = ImportRowObj.FileSize;
            cmd.Parameters.Add(FileSize);

            var FileName = cmd.CreateParameter();
            FileName.Value = ImportRowObj.FileName;
            cmd.Parameters.Add(FileName);

            var FileHash = cmd.CreateParameter();
            FileHash.Value = ImportRowObj.FileHash;
            cmd.Parameters.Add(FileHash);

            var Status = cmd.CreateParameter();
            Status.Value = ImportRowObj.Status;
            cmd.Parameters.Add(Status);

            var Message = cmd.CreateParameter();
            Message.Value = ImportRowObj.Message;
            cmd.Parameters.Add(Message);

            Debug.WriteLine(cmd.ExecuteNonQuery());

            cmd.Dispose();
        }

        public void UpdateImportRow(ImportRow ImportRowObj)
        {
            SQLiteCommand cmd;
            cmd = new SQLiteCommand(WDBSQLite.conn);
            cmd.CommandText = @"UPDATE `ImportView` SET 
`ImportID` = ?,
`ImportTitle` = ?,
`Status` = ?,
`Message` = ?
WHERE `FileHash` = ?";

            var ImportIDParm = cmd.CreateParameter();
            ImportIDParm.Value = ImportRowObj.ImportID;
            cmd.Parameters.Add(ImportIDParm);

            var ImportTitleParm = cmd.CreateParameter();
            ImportTitleParm.Value = ImportRowObj.ImportTitle;
            cmd.Parameters.Add(ImportTitleParm);

            var StatusParm = cmd.CreateParameter();
            StatusParm.Value = ImportRowObj.Status;
            cmd.Parameters.Add(StatusParm);

            var MessageParm = cmd.CreateParameter();
            ImportTitleParm.Value = ImportRowObj.ImportTitle;
            cmd.Parameters.Add(ImportTitleParm);

            var FileHashParm = cmd.CreateParameter();
            FileHashParm.Value = ImportRowObj.FileHash.ToUpper();
            cmd.Parameters.Add(FileHashParm);

            Debug.WriteLine(cmd.ExecuteNonQuery());

            cmd.Dispose();
        }

    }
}
