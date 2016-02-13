
using System;

namespace WiFiDBUploader
{
    partial class AutoCloseTimer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Timer timer1;


        private string _AutoCloseSeconds;
        public string TimerSeconds
        {
            set { AutoCloseSecondsLabel.Text = value; _AutoCloseSeconds = value; }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AutoCloseSecondsLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AutoCloseSecondsLabel
            // 
            this.AutoCloseSecondsLabel.AutoSize = true;
            this.AutoCloseSecondsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoCloseSecondsLabel.Location = new System.Drawing.Point(207, 8);
            this.AutoCloseSecondsLabel.Name = "AutoCloseSecondsLabel";
            this.AutoCloseSecondsLabel.Size = new System.Drawing.Size(92, 39);
            this.AutoCloseSecondsLabel.TabIndex = 3;
            this.AutoCloseSecondsLabel.Text = "100s";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "WiFiDB Uploader will close in: ";
            // 
            // AutoCloseTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 56);
            this.Controls.Add(this.AutoCloseSecondsLabel);
            this.Controls.Add(this.label1);
            this.Name = "AutoCloseTimer";
            this.Text = "Auto Close";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AutoCloseTimer_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AutoCloseSecondsLabel;
        private System.Windows.Forms.Label label1;
    }
}