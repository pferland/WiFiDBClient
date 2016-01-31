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
        
        public string ServerAddress
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string ApiPath
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }
        
        public string Username
        {
            get { return textBox3.Text; }
            set { textBox3.Text = value; }
        }

        public string ApiKey
        {
            get { return textBox4.Text; }
            set { textBox4.Text = value; }
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.ServerSettingsOKButton = new System.Windows.Forms.Button();
            this.ServerSettingsCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "WiFiDB Server";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(96, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(450, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "http://dev.randomintervals.com";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "API Path";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(96, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(450, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "/wfiidb/api/v2/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(96, 69);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(450, 20);
            this.textBox3.TabIndex = 5;
            this.textBox3.Text = "pferland";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "API Key";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(96, 97);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(450, 20);
            this.textBox4.TabIndex = 7;
            this.textBox4.Text = "GSn8NQeYzY8gq5Y8NFpf5gZZqH33kdBctEOwWzsOTmxCnrs4BYk32rgeNLNhLkzj";
            // 
            // ServerSettingsOKButton
            // 
            this.ServerSettingsOKButton.Location = new System.Drawing.Point(390, 161);
            this.ServerSettingsOKButton.Name = "ServerSettingsOKButton";
            this.ServerSettingsOKButton.Size = new System.Drawing.Size(75, 23);
            this.ServerSettingsOKButton.TabIndex = 8;
            this.ServerSettingsOKButton.Text = "OK";
            this.ServerSettingsOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ServerSettingsOKButton.UseVisualStyleBackColor = true;
            // 
            // ServerSettingsCancelButton
            // 
            this.ServerSettingsCancelButton.Location = new System.Drawing.Point(471, 161);
            this.ServerSettingsCancelButton.Name = "ServerSettingsCancelButton";
            this.ServerSettingsCancelButton.Size = new System.Drawing.Size(75, 23);
            this.ServerSettingsCancelButton.TabIndex = 9;
            this.ServerSettingsCancelButton.Text = "Cancel";
            this.ServerSettingsCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ServerSettingsCancelButton.UseVisualStyleBackColor = true;
            // 
            // WiFiDB_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 196);
            this.ControlBox = false;
            this.Controls.Add(this.ServerSettingsCancelButton);
            this.Controls.Add(this.ServerSettingsOKButton);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(610, 234);
            this.MinimumSize = new System.Drawing.Size(610, 234);
            this.Name = "WiFiDB_Settings";
            this.Text = "WiFiDB_Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button ServerSettingsCancelButton;
        private System.Windows.Forms.Button ServerSettingsOKButton;
    }
}