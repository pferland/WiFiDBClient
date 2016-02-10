using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDBSQLite
{
    public class WDBSQLite
    {
        private SQLiteConnection conn;
        SQLiteCommand cmd;
        public WDBSQLite(string Path)
        {
            if (!File.Exists(Path))
            {
                this.conn = CreateDB(Path);
            }else
            {
                this.conn = new SQLiteConnection("data source=" + Path);
                conn.Open();
            }
        }

        public void Close()
        {
            this.conn.Dispose();
        }

        private SQLiteConnection CreateDB(string DbPath)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;

            SQLiteConnection.CreateFile(DbPath);
            Debug.Write("Created Database: " + DbPath);

            conn = new SQLiteConnection("data source=" + DbPath);
            conn.Open();

            cmd = new SQLiteCommand(conn);
            
            cmd.CommandText = @"CREATE TABLE ImportView (
ID INTEGER PRIMARY KEY AUTOINCREMENT,
Username VARCHAR(255),
ImportTitle VARCHAR(255),
Date_Time VARCHAR(255),
FileSize VARCHAR(255),
FileName VARCHAR(255),
FileHash VARCHAR(255),
Status VARCHAR(255),
Message VARCHAR(255)

)";
            cmd.ExecuteNonQuery();
            Debug.Write("Successfully created `ImportView` Table");
            
            cmd.Dispose();

            return conn;
        }

        public void InsertImportRow(ImportRow ImportRowObj)
        {
            cmd = new SQLiteCommand(conn);
            cmd.CommandText = @"INSERT INTO `ImportView` 
(`Username`, `ImportTitle`, `Date_Time`, `FileSize`, `FileName`, `FileHash`, `Status`, `Message`) 
VALUES ( ?, ?, ?, ?, ?, ?, ?, ?)";
            var Username = cmd.CreateParameter();
            Username.Value = ImportRowObj.Username;
            cmd.Parameters.Add(Username);

            var ImportTitle = cmd.CreateParameter();
            ImportTitle.Value = ImportRowObj.ImportTitle;
            cmd.Parameters.Add(ImportTitle);

            var Date_Time = cmd.CreateParameter();
            Date_Time.Value = ImportRowObj.DateTime ;
            cmd.Parameters.Add(Date_Time);

            var FileSize = cmd.CreateParameter();
            FileSize.Value = ImportRowObj.FileSize ;
            cmd.Parameters.Add(FileSize);

            var FileName = cmd.CreateParameter();
            FileName.Value = ImportRowObj.FileName ;
            cmd.Parameters.Add(FileName);

            var FileHash = cmd.CreateParameter();
            FileHash.Value = ImportRowObj.FileHash ;
            cmd.Parameters.Add(FileHash);

            var Status = cmd.CreateParameter();
            Status.Value = ImportRowObj.Status ;
            cmd.Parameters.Add(Status);

            var Message = cmd.CreateParameter();
            Message.Value = ImportRowObj.Message ;
            cmd.Parameters.Add(Message);

            Debug.WriteLine(cmd.ExecuteNonQuery());

            cmd.Dispose();
        }
    }
}
