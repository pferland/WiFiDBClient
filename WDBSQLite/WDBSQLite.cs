using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WDBSQLite
{
    public class WDBSQLite
    {
        public SQLiteConnection conn;
        private WDBTraceLog.TraceLog TraceLogObj;
        public string ObjectName = "WDBSQLite";
        private string SQLFile;
        private string _ThreadName;
        public string ThreadName
        {
            get { return _ThreadName; }
            set { _ThreadName = value; }
        }

        public WDBSQLite(string Path, string UI, string LogPath, WDBTraceLog.TraceLog WDBTraceLogObj)
        {
            SQLFile = Path;
            WDBTraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "Start Call: WDBSQLite()");
            this.TraceLogObj = WDBTraceLogObj;
            if (!File.Exists(Path))
            {
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Start of Create SQLite DB. " + Path);
                if (UI.ToLower() == "client")
                {
                    this.conn = CreateClientDB(Path);
                }
                else if (UI.ToLower() == "uploader")
                {
                    this.conn = CreateUploadDB(Path);
                }
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "End of Create SQLite DB." + Path);
            }
            else
            {
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "SQLite data source=" + Path);
                conn = new SQLiteConnection("data source=" + Path);
                conn.Open();
            }
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "End Call: WDBSQLite()");
        }

        public void Close()
        {
            this.conn.Dispose();
        }


        public void Dispose(bool SaveDB = false)
        {
            Close();
            if(!SaveDB)
            {
                File.Delete(SQLFile);
            }
        }

        private SQLiteConnection CreateDB(string DbFile)
        {
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "Start Call: CreateDB");
            try
            {
                SQLiteConnection.CreateFile(DbFile);
            }
            catch (Exception e)
            {
                string DbPath = Path.GetDirectoryName(DbFile);
                TraceLogObj.WriteToLog(_ThreadName, ObjectName, GetCurrentMethod(), "Failed to Create SQLite DB: " + e.Message.ToString());
                Directory.CreateDirectory(DbPath);
                SQLiteConnection.CreateFile(DbFile);
            }
            Debug.Write("Created Database: " + DbFile);

            conn = new SQLiteConnection("data source=" + DbFile);
            conn.Open();
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "End Call: CreateDB");
            return conn;
        }

        private SQLiteConnection CreateUploadDB(string DbPath)
        {
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "Start Call: CreateUploadDB");
            SQLiteConnection conn;
            SQLiteCommand cmd;

            conn = CreateDB(DbPath);

            cmd = new SQLiteCommand(conn);
            cmd.CommandText = @"CREATE TABLE ImportView (
ID INTEGER PRIMARY KEY AUTOINCREMENT,
ImportID INT,
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
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "End Call: CreateUploadDB");
            return conn;
        }

        private SQLiteConnection CreateClientDB(string DbPath)
        {
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "Start Call: CreateClientDB");
            SQLiteConnection conn;
            SQLiteCommand cmd;

            conn = CreateDB(DbPath);

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
            TraceLogObj.WriteToLog(ThreadName, ObjectName, GetCurrentMethod(), "End Call: CreateClientDB");
            return conn;
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