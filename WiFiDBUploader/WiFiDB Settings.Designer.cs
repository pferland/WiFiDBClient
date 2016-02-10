using System.Collections.Generic;
using System.Diagnostics;

namespace WiFiDBUploader
{
    partial class WiFiDB_Settings
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

        public void InitForm()
        {
            foreach (ServerObj Server in ServerList)
            {
                //Debug.WriteLine("Form Drop Down: " + Server.ServerAddress.ToString().Replace("https://", "").Replace("http://", ""));
                this.comboBox1.Items.AddRange(new object[] { Server.ServerAddress.ToString().Replace("https://", "").Replace("http://", "") });
                if (Server.Selected)
                {
                    comboBox1.SelectedIndex = comboBox1.FindStringExact(Server.ServerAddress.ToString().Replace("https://", "").Replace("http://", ""));
                }
            }
        }
        
        private string _SelectedServer;
        private List<ServerObj> _ServerList;

        public List<ServerObj> ServerList
        {
            get { return _ServerList; }
            set { _ServerList = value; }
        }

        public string SelectedServer
        {
            get { return _SelectedServer; }
            set { _SelectedServer = value; }
        }
        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServerSettingsOKButton = new System.Windows.Forms.Button();
            this.ServerSettingsCancelButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ServerSettingsOKButton
            // 
            this.ServerSettingsOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ServerSettingsOKButton.Location = new System.Drawing.Point(321, 39);
            this.ServerSettingsOKButton.Name = "ServerSettingsOKButton";
            this.ServerSettingsOKButton.Size = new System.Drawing.Size(75, 23);
            this.ServerSettingsOKButton.TabIndex = 8;
            this.ServerSettingsOKButton.Text = "OK";
            this.ServerSettingsOKButton.UseVisualStyleBackColor = true;
            // 
            // ServerSettingsCancelButton
            // 
            this.ServerSettingsCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ServerSettingsCancelButton.Location = new System.Drawing.Point(402, 39);
            this.ServerSettingsCancelButton.Name = "ServerSettingsCancelButton";
            this.ServerSettingsCancelButton.Size = new System.Drawing.Size(75, 23);
            this.ServerSettingsCancelButton.TabIndex = 9;
            this.ServerSettingsCancelButton.Text = "Cancel";
            this.ServerSettingsCancelButton.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(80, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(298, 21);
            this.comboBox1.TabIndex = 11;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Server List: ";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(385, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AddServer_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(413, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(22, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "-";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.RemoveServer_Click);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(441, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(36, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "Edit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.EditServer_Click);
            // 
            // WiFiDB_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 79);
            this.ControlBox = false;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.ServerSettingsCancelButton);
            this.Controls.Add(this.ServerSettingsOKButton);
            this.Name = "WiFiDB_Settings";
            this.Text = "Select Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ServerSettingsCancelButton;
        private System.Windows.Forms.Button ServerSettingsOKButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}