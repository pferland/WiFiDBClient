using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WiFiDBUploader
{
    public partial class AutoCloseTimer : Form
    {
        public AutoCloseTimer()
        {
            InitializeComponent();
            StartTimer();
        }

        private void StartTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(TimeTick);
            timer1.Interval = 1000; // in miliseconds
            timer1.Start();
        }

        public void TimeTick(object sender, EventArgs e)
        {
            Debug.WriteLine("TimerSeconds: " + _AutoCloseSeconds);
            int calc = Int32.Parse(_AutoCloseSeconds);
            calc--;
            if (calc == 0)
            {
                timer1.Stop();
                timer1.Dispose();
                Application.Exit();
            }else
            {
                _AutoCloseSeconds = calc.ToString();
                AutoCloseSecondsLabel.Text = calc.ToString() + "s";
            }
        }

        private void AutoCloseTimer_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            timer1.Dispose();
        }
    }
}
