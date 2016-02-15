namespace WiFiDBUploader
{
    partial class LoggingSettings
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

        public bool TraceLogEnable
        {
            get { return TraceLogEnableCheckBox.Checked; }
            set { TraceLogEnableCheckBox.Checked = value; }
        }
        
        public bool DEBUG
        {
            get { return DebugEnableCheckBox.Checked; }
            set { DebugEnableCheckBox.Checked = value; }
        }
        
        public bool PerRunRotate
        {
            get { return PerRunRotateTraceLogCheckBox.Checked; }
            set { PerRunRotateTraceLogCheckBox.Checked = value; }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DebugEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.TraceLogEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.PerRunRotateTraceLogCheckBox = new System.Windows.Forms.CheckBox();
            this.AutoOKButton = new System.Windows.Forms.Button();
            this.AutoCancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PerRunRotateTraceLogCheckBox);
            this.groupBox1.Controls.Add(this.TraceLogEnableCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(619, 79);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Trace Logging Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DebugEnableCheckBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(620, 52);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Debug Settings";
            // 
            // DebugEnableCheckBox
            // 
            this.DebugEnableCheckBox.AutoSize = true;
            this.DebugEnableCheckBox.Location = new System.Drawing.Point(7, 20);
            this.DebugEnableCheckBox.Name = "DebugEnableCheckBox";
            this.DebugEnableCheckBox.Size = new System.Drawing.Size(450, 17);
            this.DebugEnableCheckBox.TabIndex = 0;
            this.DebugEnableCheckBox.Text = "Enable Deugging Output (Will only do something if you are using Visual Studio to " +
    "Develop)";
            this.DebugEnableCheckBox.UseVisualStyleBackColor = true;
            // 
            // TraceLogEnableCheckBox
            // 
            this.TraceLogEnableCheckBox.AutoSize = true;
            this.TraceLogEnableCheckBox.Location = new System.Drawing.Point(7, 20);
            this.TraceLogEnableCheckBox.Name = "TraceLogEnableCheckBox";
            this.TraceLogEnableCheckBox.Size = new System.Drawing.Size(382, 17);
            this.TraceLogEnableCheckBox.TabIndex = 0;
            this.TraceLogEnableCheckBox.Text = "Enable Trace Logging (helps debug your issues when running the uploader)";
            this.TraceLogEnableCheckBox.UseVisualStyleBackColor = true;
            // 
            // PerRunRotateTraceLogCheckBox
            // 
            this.PerRunRotateTraceLogCheckBox.AutoSize = true;
            this.PerRunRotateTraceLogCheckBox.Location = new System.Drawing.Point(6, 43);
            this.PerRunRotateTraceLogCheckBox.Name = "PerRunRotateTraceLogCheckBox";
            this.PerRunRotateTraceLogCheckBox.Size = new System.Drawing.Size(589, 17);
            this.PerRunRotateTraceLogCheckBox.TabIndex = 1;
            this.PerRunRotateTraceLogCheckBox.Text = "Per Run Rotate Trace Logs (Will create a new Trace log for each Run of the Upload" +
    "er. Trace-yyyyMMdd-HHmmss.log )";
            this.PerRunRotateTraceLogCheckBox.UseVisualStyleBackColor = true;
            // 
            // AutoOKButton
            // 
            this.AutoOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.AutoOKButton.Location = new System.Drawing.Point(475, 156);
            this.AutoOKButton.Name = "AutoOKButton";
            this.AutoOKButton.Size = new System.Drawing.Size(75, 23);
            this.AutoOKButton.TabIndex = 11;
            this.AutoOKButton.Text = "OK";
            this.AutoOKButton.UseVisualStyleBackColor = true;
            // 
            // AutoCancelButton
            // 
            this.AutoCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AutoCancelButton.Location = new System.Drawing.Point(556, 156);
            this.AutoCancelButton.Name = "AutoCancelButton";
            this.AutoCancelButton.Size = new System.Drawing.Size(75, 23);
            this.AutoCancelButton.TabIndex = 10;
            this.AutoCancelButton.Text = "Cancel";
            this.AutoCancelButton.UseVisualStyleBackColor = true;
            // 
            // LoggingSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 194);
            this.Controls.Add(this.AutoOKButton);
            this.Controls.Add(this.AutoCancelButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "LoggingSettings";
            this.Text = "Debug and Logging Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox PerRunRotateTraceLogCheckBox;
        private System.Windows.Forms.CheckBox TraceLogEnableCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox DebugEnableCheckBox;
        private System.Windows.Forms.Button AutoOKButton;
        private System.Windows.Forms.Button AutoCancelButton;
    }
}