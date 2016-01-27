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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AutoBrowseButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.AutoBrowse2Button = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.AutoCancelButton = new System.Windows.Forms.Button();
            this.AutoOKButton = new System.Windows.Forms.Button();
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
            this.AutoBrowseButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AutoBrowseButton);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(19, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(467, 90);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto Upload Folder";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AutoBrowse2Button);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Location = new System.Drawing.Point(19, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(466, 91);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Archive Imports";
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
            // AutoBrowse2Button
            // 
            this.AutoBrowse2Button.Location = new System.Drawing.Point(379, 38);
            this.AutoBrowse2Button.Name = "AutoBrowse2Button";
            this.AutoBrowse2Button.Size = new System.Drawing.Size(75, 23);
            this.AutoBrowse2Button.TabIndex = 5;
            this.AutoBrowse2Button.Text = "Browse";
            this.AutoBrowse2Button.UseVisualStyleBackColor = true;
            this.AutoBrowse2Button.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(87, 40);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(285, 20);
            this.textBox2.TabIndex = 4;
            // 
            // AutoCancelButton
            // 
            this.AutoCancelButton.Location = new System.Drawing.Point(411, 251);
            this.AutoCancelButton.Name = "AutoCancelButton";
            this.AutoCancelButton.Size = new System.Drawing.Size(75, 23);
            this.AutoCancelButton.TabIndex = 6;
            this.AutoCancelButton.Text = "Cancel";
            this.AutoCancelButton.UseVisualStyleBackColor = true;
            this.AutoCancelButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // AutoOKButton
            // 
            this.AutoOKButton.Location = new System.Drawing.Point(330, 251);
            this.AutoOKButton.Name = "AutoOKButton";
            this.AutoOKButton.Size = new System.Drawing.Size(75, 23);
            this.AutoOKButton.TabIndex = 7;
            this.AutoOKButton.Text = "OK";
            this.AutoOKButton.UseVisualStyleBackColor = true;
            this.AutoOKButton.Click += new System.EventHandler(this.button4_Click);
            // 
            // Auto_Upload_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 286);
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
    }
}