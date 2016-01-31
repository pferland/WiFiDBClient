namespace WiFiDBUploader
{
    partial class Auto_Upload_Settings
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

        #region Windows Form Designer generated code


        public bool AutoUploadFolder
        {
            get { return checkBox1.Checked; }
            set { }
        }

        public string AutoUploadFolderPath
        {
            get { return textBox1.Text; }
            set { }
        }

        public bool ArchiveImports
        {
            get { return checkBox2.Checked; }
            set { }
        }

        public string ArchiveImportsFolderPath
        {
            get { return textBox2.Text; }
            set { }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// again, live on the egde
        private void InitializeComponent()
        {
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AutoBrowseButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AutoBrowse2Button = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.AutoCancelButton = new System.Windows.Forms.Button();
            this.AutoOKButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();

            // 
            // Auto Upload Group Box
            // 
            this.groupBox1.Controls.Add(this.AutoBrowseButton);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(28, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(700, 138);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto Upload Folder";
            // 
            // Auto Upload Files CheckBox
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(130, 29);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Checked = AutoUploadFolder;
            this.checkBox1.Size = new System.Drawing.Size(290, 24);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Automatically Upload Files in Folder:";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Auto Upload Folder Path Visual
            // 
            this.textBox1.Location = new System.Drawing.Point(130, 77);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(426, 26);
            this.textBox1.Text = AutoUploadFolderPath;
            this.textBox1.TabIndex = 2;
            // 
            // AutoBrowseButton
            // 
            this.AutoBrowseButton.Location = new System.Drawing.Point(568, 77);
            this.AutoBrowseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AutoBrowseButton.Name = "AutoBrowseButton";
            this.AutoBrowseButton.Size = new System.Drawing.Size(112, 35);
            this.AutoBrowseButton.TabIndex = 3;
            this.AutoBrowseButton.Text = "Browse";
            this.AutoBrowseButton.UseVisualStyleBackColor = true;
            this.AutoBrowseButton.Click += new System.EventHandler(this.AutoBrowseButton_Click);

            // 
            // Group Box for Auto Archive
            // 
            this.groupBox2.Controls.Add(this.AutoBrowse2Button);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Location = new System.Drawing.Point(28, 183);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(699, 140);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Archive Imports";
            // 
            // Auto Archive Uploaded Files Browser Button
            // 
            this.AutoBrowse2Button.Location = new System.Drawing.Point(568, 58);
            this.AutoBrowse2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AutoBrowse2Button.Name = "AutoBrowse2Button";
            this.AutoBrowse2Button.Size = new System.Drawing.Size(112, 35);
            this.AutoBrowse2Button.TabIndex = 5;
            this.AutoBrowse2Button.Text = "Browse";
            this.AutoBrowse2Button.UseVisualStyleBackColor = true;
            this.AutoBrowse2Button.Click += new System.EventHandler(this.AutoBrowse2Button_Click);
            // 
            // Auto Archive Uploads Folder Path Visual
            // 
            this.textBox2.Location = new System.Drawing.Point(130, 62);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(426, 26);
            this.textBox2.TabIndex = 4;
            // 
            // Auto Archive Uploads CheckBox
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(130, 29);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(317, 24);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Automatically Archive Uploaded Files to:";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // AutoCancelButton
            // 
            this.AutoCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AutoCancelButton.Location = new System.Drawing.Point(616, 386);
            this.AutoCancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AutoCancelButton.Name = "AutoCancelButton";
            this.AutoCancelButton.Size = new System.Drawing.Size(112, 35);
            this.AutoCancelButton.TabIndex = 6;
            this.AutoCancelButton.Text = "Cancel";
            this.AutoCancelButton.UseVisualStyleBackColor = true;
            // 
            // AutoOKButton
            // 
            this.AutoOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.AutoOKButton.Location = new System.Drawing.Point(495, 386);
            this.AutoOKButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AutoOKButton.Name = "AutoOKButton";
            this.AutoOKButton.Size = new System.Drawing.Size(112, 35);
            this.AutoOKButton.TabIndex = 7;
            this.AutoOKButton.Text = "OK";
            this.AutoOKButton.UseVisualStyleBackColor = true;
            // 
            // Auto_Upload_Settings Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 440);
            this.ControlBox = false;
            this.Controls.Add(this.AutoOKButton);
            this.Controls.Add(this.AutoCancelButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Auto_Upload_Settings";
            this.Text = "Auto_Upload_Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button AutoBrowseButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button AutoBrowse2Button;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button AutoCancelButton;
        private System.Windows.Forms.Button AutoOKButton;
    }
}