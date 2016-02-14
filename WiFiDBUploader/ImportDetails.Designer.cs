namespace WiFiDBUploader
{
    partial class ImportDetails
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
        }

        public string ImportNotes
        {
            get { return richTextBox1.Text; }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ImportOKButton = new System.Windows.Forms.Button();
            this.ImportCancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(639, 297);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import Values";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(51, 74);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(582, 210);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Notes";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(50, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(583, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Title";
            // 
            // ImportOKButton
            // 
            this.ImportOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ImportOKButton.Location = new System.Drawing.Point(495, 315);
            this.ImportOKButton.Name = "ImportOKButton";
            this.ImportOKButton.Size = new System.Drawing.Size(75, 23);
            this.ImportOKButton.TabIndex = 5;
            this.ImportOKButton.Text = "OK";
            this.ImportOKButton.UseVisualStyleBackColor = true;
            // 
            // ImportCancelButton
            // 
            this.ImportCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ImportCancelButton.Location = new System.Drawing.Point(576, 315);
            this.ImportCancelButton.Name = "ImportCancelButton";
            this.ImportCancelButton.Size = new System.Drawing.Size(75, 23);
            this.ImportCancelButton.TabIndex = 4;
            this.ImportCancelButton.Text = "Cancel";
            this.ImportCancelButton.UseVisualStyleBackColor = true;
            // 
            // ImportDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 355);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ImportOKButton);
            this.Controls.Add(this.ImportCancelButton);
            this.Name = "ImportDetails";
            this.Text = "ImportDetails";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ImportCancelButton;
        private System.Windows.Forms.Button ImportOKButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}