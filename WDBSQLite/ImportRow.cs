using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDBSQLite
{
    public class ImportRow
    {
        public int ID { get; set; }
        public int ImportID { get; set; }
        public string Username { get; set; }
        public string ImportTitle { get; set; }
        public string DateTime { get; set; }
        public string FileSize { get; set; }
        public string FileName { get; set; }
        public string FileHash { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
