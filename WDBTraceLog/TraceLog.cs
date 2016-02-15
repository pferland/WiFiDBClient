using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            this.file = new System.IO.StreamWriter(LogFile, true);

            if(PerRunRotate)
            {
                this.LogFile = "Trace" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".log";
            }
            else
            {
                this.LogFile = LogPath + "/Trace.log";
            }
            
        }

        public void Dispose()
        {
            file.Close();
        }

        public void WriteToLog(string message)
        {
            string line = "[" + DateTime.Now.ToString("yyyy-MM-dd") + "]" + "[" + DateTime.Now.ToString("HH:mm:ss") + "]" + "[" + message + "]";
            
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
