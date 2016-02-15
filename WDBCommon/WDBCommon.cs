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

        public string LogPath;

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
            WriteLog("Start of Call: new WDBCommon.WDBCommon(Path, \"Uploader\")");
            WDBSQLite = new WDBSQLite.WDBSQLite(Path, "uploader", LogPath);
            WriteLog("End of Call: new WDBCommon.WDBCommon(Path, \"Uploader\")");
        }

        public void initApi()
        {
            //WriteLog("Start Function Call: Init API.");
            WDBAPIObj.ApiCompiledPath = ApiCompiledPath;
            WDBAPIObj.ApiKey = ApiKey;
            WDBAPIObj.Username = Username;
            WDBAPIObj.DefaultImportTitle = DefaultImportTitle;
            WDBAPIObj.DefaultImportNotes = DefaultImportNotes;
            WDBAPIObj.UseDefaultImportValues = UseDefaultImportValues;
            WDBAPIObj.LogPath = LogPath;
            //WriteLog("End Function Call: Init API.");
        }

        ///
        /// Common Fucntions
        ///

        public int IsFileImported(string query, bool LocalOnly)
        {
            //Check the Local SQLite DB for the File hash, If it is found, return now.
            if(CheckSQLFileHash(query) == 0)
            {
                if(!LocalOnly)
                {
                    // If the filehash was not found in the Local SQLite DB, check the WDB API.
                    if (CheckFileHash(query) == 0)
                    {
                        return 0; //Hash not found.
                    }
                    else
                    {
                        return 1; //Hash found.
                    }
                }else
                {
                    return 0;
                }
            } else
            {
                return 0; //Hash Found
            }            
        }


        ///
        /// WiFiDB API Functions
        ///
        public int CheckFileHash(string query)
        {
            string APIResponse = WDBAPIObj.CheckFileHash(query);
            WriteLog("IsFileImported Response: " + APIResponse);
            string response = WDBAPIObj.ParseApiResponse(APIResponse);


            string[] stringSeparators = new string[] { "|~|" };
            string[] split = response.Split(stringSeparators, StringSplitOptions.None);

            if (split[0] == "error")
            {
                return -1;
            }

            WriteLog("split[0].ToLower(): " + split[0].ToLower());
            if (split[1].ToLower() == "")
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public void GetHashStatus(string query, BackgroundWorker BgWk)
        {
            BgWk.ReportProgress(0, "");
            BgWk.ReportProgress(100, WDBAPIObj.ParseApiResponse(WDBAPIObj.CheckFileHash(query)));
        }
        
        public void GetDaemonStatuses(string query, BackgroundWorker BgWk)
        {
            //WriteLog("Active Server: " + WDBAPIObj.ApiCompiledPath);
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
                WriteLog("Auto Import Folder: " + Path + " -> " + Directory.Exists(Path));
                if (Directory.Exists(Path)) 
                {
                    WriteLog("Auto Import Folder: " + Path);
                    var md5 = MD5.Create();

                    string[] extensions = new string[] { "*.vs1", "*.vsz", "*.csv", "*.db3" };

                    // Loop through the file extension types, find them in the provided folder, then import it.
                    foreach (string ext in extensions)
                    {
                        WriteLog("Extension that will be used: " + ext);
                        string[] files = Directory.GetFiles(Path, ext);
                        //WriteLog("The number of VS1 files: {0}.", files.Length);
                        if (files.Length > 0)
                        {
                            foreach (string file in files)
                            {
                                responses.Add(new KeyValuePair<int, string>(InternalImportID, ImportFile(file, ImportTitle, ImportNotes, BW)));
                            }
                        }
                    }
                }else
                {
                    responses.Add(new KeyValuePair<int, string>(InternalImportID, "error|~|Auto Import Folder Not Found."));
                }
            }
            catch (Exception e)
            {
                WriteLog("The process failed: " + e.ToString());
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
                    WriteLog(FilePath + " was moved to " + ArchivedFile);
                    
                }
                catch (Exception e)
                {
                    WriteLog("The process failed: " + e.ToString());
                }
            }
            else
            {
                WriteLog("Archve Imported Files is disabled.");
            }
        }

        public string ImportFile(string FilePath, string ImportTitle, string ImportNotes, BackgroundWorker BW)
        {
            string response;
            byte[] hashBytes;
            string hashish;
            var md5 = MD5.Create();
            InternalImportID++;
            //WriteLog(db3);
            using (var inputFileStream = File.Open(FilePath, FileMode.Open))
            {
                hashBytes = md5.ComputeHash(inputFileStream);
                hashish = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
            int IsFileImportedResult = IsFileImported(hashish, true);

            WriteLog("IsFileImportedResult :" + IsFileImportedResult);
            if (IsFileImportedResult == 0)
            {
                BW.ReportProgress(0, "newrow|~|" + FilePath + "|~|" + hashish);
                string RawResponse = WDBAPIObj.ApiImportFile(FilePath, ImportTitle, ImportNotes);

                WriteLog(RawResponse);

                response = WDBAPIObj.ParseApiResponse(RawResponse);

                WriteLog("Parse Response Result: " + response);

                BW.ReportProgress(0, response+"::"+FilePath);
                ArchiveImportedFile(FilePath);
            }
            else
            {
                response = "error|~|Already Imported.-~-" + FilePath + "::" + hashish;
            }
            WriteLog("File Import Response return: " + response);
            return response;
        }




        ///
        /// SQLite Functions.
        ///

        public int CheckSQLFileHash(string query)
        {
            SQLiteCommand cmd;
            cmd = new SQLiteCommand(WDBSQLite.conn);
            int ret = 0;
            cmd.CommandText = @"SELECT `ID` FROM `ImportView` WHERE `FileHash` = ?";

            WriteLog("FileHash query: " + query);

            var filehash = cmd.CreateParameter();
            filehash.Value = query;
            cmd.Parameters.Add(filehash);

            SQLiteDataReader reader;
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if(Int32.Parse(reader["ID"].ToString()) > 0)
                {
                    ret = 1;
                }
            }
            cmd.Dispose();
            return ret;
        }

        public List<ImportRow> GetImportRows()
        {
            string ImportID;
            SQLiteCommand cmd;
            SQLiteDataReader reader;

            List<ImportRow> ImportRows = new List<ImportRow>();
            cmd = new SQLiteCommand(WDBSQLite.conn);
            cmd.CommandText = @"SELECT * FROM `ImportView`";
            
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ImportRow ImportRowObj = new ImportRow();
                if (reader["ImportID"].ToString() == "")
                {
                    ImportID = "0";
                }else
                {
                    ImportID = reader["ImportID"].ToString();
                }
                WriteLog("ImportID For IntParse: "+ImportID);
                ImportRowObj.ImportID = Int32.Parse(ImportID);
                ImportRowObj.Username = reader["Username"].ToString();
                ImportRowObj.ImportTitle = reader["ImportTitle"].ToString();
                ImportRowObj.DateTime = reader["Date_Time"].ToString();
                ImportRowObj.FileSize = reader["FileSize"].ToString();
                ImportRowObj.FileName = reader["FileName"].ToString();
                ImportRowObj.FileHash = reader["FileHash"].ToString();
                ImportRowObj.Status = reader["Status"].ToString();
                ImportRowObj.Message = reader["Message"].ToString();

                ImportRows.Add(ImportRowObj);

            }
            return ImportRows;
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

            WriteLog("cmd.ExecuteNonQuery(): " + cmd.ExecuteNonQuery().ToString());

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
            MessageParm.Value = ImportRowObj.Message;
            cmd.Parameters.Add(MessageParm);

            var FileHashParm = cmd.CreateParameter();
            FileHashParm.Value = ImportRowObj.FileHash.ToUpper();
            cmd.Parameters.Add(FileHashParm);

            WriteLog("cmd.ExecuteNonQuery(): " + cmd.ExecuteNonQuery().ToString());

            cmd.Dispose();
        }





        public void WriteLog(string message)
        {
            string LogFile = LogPath + "/Trace.log";
            string line = "[" + DateTime.Now.ToString("yyyy-MM-dd") + "]" + "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "[" + message + "]";

            Debug.WriteLine(line);

            //System.IO.StreamWriter file = new System.IO.StreamWriter(LogFile, true);
            //file.WriteLine(line);
            //file.Close();
        }


    }
}
