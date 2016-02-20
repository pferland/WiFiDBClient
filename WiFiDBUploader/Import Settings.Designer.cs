namespace WiFiDBUploader
{
    partial class Import_Settings
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
        
        public string ImportTitle
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
        
        public string ImportNotes
        {
            get { return richTextBox1.Text; }
            set { richTextBox1.Text = value; }
        }

        public bool UseImportDefaultValues
        {
            get { return checkBox1.Checked; }
            set { checkBox1.Checked = value; }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// Pfftt thats for pussies, live on the edge.
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ImportCancelButton = new System.Windows.Forms.Button();
            this.ImportOKButton = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(693, 399);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Default Import Values";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 373);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(231, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Use Default Import Values for Auto Uploads";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(48, 157);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(582, 210);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Notes";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(47, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(583, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            // 
            // ImportCancelButton
            // 
            this.ImportCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ImportCancelButton.Location = new System.Drawing.Point(627, 420);
            this.ImportCancelButton.Name = "ImportCancelButton";
            this.ImportCancelButton.Size = new System.Drawing.Size(75, 23);
            this.ImportCancelButton.TabIndex = 1;
            this.ImportCancelButton.Text = "Cancel";
            this.ImportCancelButton.UseVisualStyleBackColor = true;
            // 
            // ImportOKButton
            // 
            this.ImportOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ImportOKButton.Location = new System.Drawing.Point(546, 420);
            this.ImportOKButton.Name = "ImportOKButton";
            this.ImportOKButton.Size = new System.Drawing.Size(75, 23);
            this.ImportOKButton.TabIndex = 2;
            this.ImportOKButton.Text = "OK";
            this.ImportOKButton.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(47, 76);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(392, 17);
            this.checkBox2.TabIndex = 4;
            this.checkBox2.Text = "Use Auto Generated Date and Time For Title (Will over-ride Manually set Title)";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(621, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Use %DATETIME% in the Manual Title to have an AutoGenerated Date Time appended to" +
    " it. (format is YYYY-MM-DD HH:MM:SS)";
            // 
            // Import_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 455);
            this.ControlBox = false;
            this.Controls.Add(this.ImportOKButton);
            this.Controls.Add(this.ImportCancelButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "Import_Settings";
            this.Text = "Import_Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button ImportCancelButton;
        private System.Windows.Forms.Button ImportOKButton;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label3;
    }
}