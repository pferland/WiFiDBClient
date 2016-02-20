using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WiFiDBUploader
{
    public partial class Import_Settings : Form
    {
        public Import_Settings()
        {
            InitializeComponent();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                textBox1.ReadOnly = true;
            }else
            {
                textBox1.ReadOnly = false;
            }
        }
    }
}
