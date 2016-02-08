namespace WiFiDBUploader
{
    partial class EditServer
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

        private string _ApiPath;
        private string _Username;
        private string _ApiKey;
        private string _ServerAddress;

        public void InitForm()
        {
            textBox1.Text = _ServerAddress;
            textBox2.Text = _ApiPath;
            textBox3.Text = _Username;
            textBox4.Text = _ApiKey;
        }

        public string ServerAddress
        {
            get { return textBox1.Text; }
            set { _ServerAddress = value; }
        }

        public string ApiPath
        {
            get { return textBox2.Text; }
            set { _ApiPath = value; }
        }

        public string ApiKey
        {
            get { return textBox4.Text; }
            set { _ApiKey = value; }
        }

        public string Username
        {
            get { return textBox3.Text; }
            set { _Username = value; }
        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EditSettingsCancelButton = new System.Windows.Forms.Button();
            this.EditSettingsOKButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(607, 169);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Details";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(107, 122);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(450, 20);
            this.textBox4.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "WiFiDB Server";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(107, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(450, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "API Path";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "API Key";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(107, 65);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(450, 20);
            this.textBox2.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(107, 94);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(450, 20);
            this.textBox3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username";
            // 
            // EditSettingsCancelButton
            // 
            this.EditSettingsCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.EditSettingsCancelButton.Location = new System.Drawing.Point(544, 187);
            this.EditSettingsCancelButton.Name = "EditSettingsCancelButton";
            this.EditSettingsCancelButton.Size = new System.Drawing.Size(75, 23);
            this.EditSettingsCancelButton.TabIndex = 18;
            this.EditSettingsCancelButton.Text = "Cancel";
            this.EditSettingsCancelButton.UseVisualStyleBackColor = true;
            // 
            // EditSettingsOKButton
            // 
            this.EditSettingsOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.EditSettingsOKButton.Location = new System.Drawing.Point(463, 187);
            this.EditSettingsOKButton.Name = "EditSettingsOKButton";
            this.EditSettingsOKButton.Size = new System.Drawing.Size(75, 23);
            this.EditSettingsOKButton.TabIndex = 17;
            this.EditSettingsOKButton.Text = "OK";
            this.EditSettingsOKButton.UseVisualStyleBackColor = true;
            // 
            // EditServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 225);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.EditSettingsCancelButton);
            this.Controls.Add(this.EditSettingsOKButton);
            this.Name = "EditServer";
            this.Text = "Edit Server";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button EditSettingsCancelButton;
        private System.Windows.Forms.Button EditSettingsOKButton;
    }
}