using System.Diagnostics;

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
            set{ checkBox1.Checked = value;}
        }

        public string AutoUploadFolderPath
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string AutoCloseTimerSeconds
        {
            get { return AutoUploadTimerTextBox.Text; }
            set { AutoUploadTimerTextBox.Text = value; }
        }

        public bool AutoCloseEnable
        {
            get { return checkBox3.Checked; }
            set { checkBox3.Checked = value; }
        }

        public bool ArchiveImports
        {
            get { return checkBox2.Checked; }
            set { checkBox2.Checked = value; }
        }

        public string ArchiveImportsFolderPath
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
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
            this.label1 = new System.Windows.Forms.Label();
            this.AutoUploadTimerTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AutoBrowse2Button = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.AutoCancelButton = new System.Windows.Forms.Button();
            this.AutoOKButton = new System.Windows.Forms.Button();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(87, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(195, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Automatically Upload Files in Folder:";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(87, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(285, 20);
            this.textBox1.TabIndex = 2;
            // 
            // AutoBrowseButton
            // 
            this.AutoBrowseButton.Location = new System.Drawing.Point(379, 50);
            this.AutoBrowseButton.Name = "AutoBrowseButton";
            this.AutoBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.AutoBrowseButton.TabIndex = 3;
            this.AutoBrowseButton.Text = "Browse";
            this.AutoBrowseButton.UseVisualStyleBackColor = true;
            this.AutoBrowseButton.Click += new System.EventHandler(this.AutoBrowseButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.AutoUploadTimerTextBox);
            this.groupBox1.Controls.Add(this.AutoBrowseButton);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(19, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(467, 136);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto Upload Folder";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(261, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Seconds til close the UI after finishing the auto upload";
            // 
            // AutoUploadTimerTextBox
            // 
            this.AutoUploadTimerTextBox.Location = new System.Drawing.Point(355, 106);
            this.AutoUploadTimerTextBox.Name = "AutoUploadTimerTextBox";
            this.AutoUploadTimerTextBox.Size = new System.Drawing.Size(27, 20);
            this.AutoUploadTimerTextBox.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AutoBrowse2Button);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Location = new System.Drawing.Point(19, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(466, 91);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Archive Imports";
            // 
            // AutoBrowse2Button
            // 
            this.AutoBrowse2Button.Location = new System.Drawing.Point(379, 38);
            this.AutoBrowse2Button.Name = "AutoBrowse2Button";
            this.AutoBrowse2Button.Size = new System.Drawing.Size(75, 23);
            this.AutoBrowse2Button.TabIndex = 5;
            this.AutoBrowse2Button.Text = "Browse";
            this.AutoBrowse2Button.UseVisualStyleBackColor = true;
            this.AutoBrowse2Button.Click += new System.EventHandler(this.AutoBrowse2Button_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(87, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(285, 20);
            this.textBox2.TabIndex = 4;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(87, 19);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(215, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Automatically Archive Uploaded Files to:";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // AutoCancelButton
            // 
            this.AutoCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AutoCancelButton.Location = new System.Drawing.Point(411, 251);
            this.AutoCancelButton.Name = "AutoCancelButton";
            this.AutoCancelButton.Size = new System.Drawing.Size(75, 23);
            this.AutoCancelButton.TabIndex = 6;
            this.AutoCancelButton.Text = "Cancel";
            this.AutoCancelButton.UseVisualStyleBackColor = true;
            // 
            // AutoOKButton
            // 
            this.AutoOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.AutoOKButton.Location = new System.Drawing.Point(330, 251);
            this.AutoOKButton.Name = "AutoOKButton";
            this.AutoOKButton.Size = new System.Drawing.Size(75, 23);
            this.AutoOKButton.TabIndex = 7;
            this.AutoOKButton.Text = "OK";
            this.AutoOKButton.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(87, 86);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(200, 17);
            this.checkBox3.TabIndex = 6;
            this.checkBox3.Text = "Enable Auto Close After Auto Upload";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // Auto_Upload_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 286);
            this.ControlBox = false;
            this.Controls.Add(this.AutoOKButton);
            this.Controls.Add(this.AutoCancelButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AutoUploadTimerTextBox;
        private System.Windows.Forms.CheckBox checkBox3;
    }
}