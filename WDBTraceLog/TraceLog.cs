using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDBTraceLog
{
    public class TraceLog
    {
        private string LogPath;
        private bool PerRunRotate;
        private System.IO.StreamWriter file;
        private string LogFile;
        public bool DEBUG = false;
        public bool TraceLogEnable;

        public TraceLog(string LogPath, bool TraceLogEnable, bool PerRunRotate)
        {
            this.TraceLogEnable = TraceLogEnable;
            this.PerRunRotate = PerRunRotate;
            this.LogPath = LogPath;
            if(TraceLogEnable)
            {
                if (PerRunRotate)
                {
                    this.LogFile = LogPath + "Trace-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".log";
                }
                else
                {
                    this.LogFile = LogPath + "/Trace.log";
                }
                try
                {
                    //Debug.WriteLine(LogFile);
                    this.file = new System.IO.StreamWriter(LogFile, true);
                }
                catch(Exception e)
                {
                    Directory.CreateDirectory(LogPath);
                    this.file = new System.IO.StreamWriter(LogFile, true);
                }
                this.file.AutoFlush = true;
            }
        }

        public void Dispose()
        {
            file.Close();
        }

        public void WriteToLog(string ThreadName, string ObjectName, string MethodName, string message)
        {
            string line = "[" + DateTime.Now.ToString("yyyy-MM-dd") + "]" + "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "[" + ThreadName + "]" + "[" + ObjectName + "]" + "[" + MethodName + "]" + "[" + message + "]";
            if (DEBUG)
            {
                Debug.WriteLine(line);
            }
            
            if (TraceLogEnable)
            {
                file.WriteLine(line);
            }
        }
    }
}
