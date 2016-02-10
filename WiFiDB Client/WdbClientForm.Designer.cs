using System.Windows.Forms;

namespace WiFiDB_Uploader
{
    partial class WdbClientForm
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
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schedulingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getWaitingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getFinishedListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getImportingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getBadImportsListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.User = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImportTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.HashSum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CurrentImportingSSID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openSchedulePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.statusStrip1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 757);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1920, 31);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(150, 25);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(179, 26);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // menuStrip2
            // 
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.schedulingToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip2.Size = new System.Drawing.Size(1920, 35);
            this.menuStrip2.TabIndex = 2;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(88, 29);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(161, 30);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // schedulingToolStripMenuItem
            // 
            this.schedulingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getWaitingListToolStripMenuItem,
            this.getFinishedListToolStripMenuItem,
            this.getImportingListToolStripMenuItem,
            this.getBadImportsListToolStripMenuItem});
            this.schedulingToolStripMenuItem.Name = "schedulingToolStripMenuItem";
            this.schedulingToolStripMenuItem.Size = new System.Drawing.Size(111, 29);
            this.schedulingToolStripMenuItem.Text = "Scheduling";
            // 
            // getWaitingListToolStripMenuItem
            // 
            this.getWaitingListToolStripMenuItem.Name = "getWaitingListToolStripMenuItem";
            this.getWaitingListToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.getWaitingListToolStripMenuItem.Text = "Get Waiting List";
            this.getWaitingListToolStripMenuItem.Click += new System.EventHandler(this.getWaitingListToolStripMenuItem_Click);
            // 
            // getFinishedListToolStripMenuItem
            // 
            this.getFinishedListToolStripMenuItem.Name = "getFinishedListToolStripMenuItem";
            this.getFinishedListToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.getFinishedListToolStripMenuItem.Text = "Get Finished List";
            this.getFinishedListToolStripMenuItem.Click += new System.EventHandler(this.getFinishedListToolStripMenuItem_Click);
            // 
            // getImportingListToolStripMenuItem
            // 
            this.getImportingListToolStripMenuItem.Name = "getImportingListToolStripMenuItem";
            this.getImportingListToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.getImportingListToolStripMenuItem.Text = "Get Importing List";
            this.getImportingListToolStripMenuItem.Click += new System.EventHandler(this.getImportingListToolStripMenuItem_Click);
            // 
            // getBadImportsListToolStripMenuItem
            // 
            this.getBadImportsListToolStripMenuItem.Name = "getBadImportsListToolStripMenuItem";
            this.getBadImportsListToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.getBadImportsListToolStripMenuItem.Text = "Get Bad Imports List";
            this.getBadImportsListToolStripMenuItem.Click += new System.EventHandler(this.getBadImportsListToolStripMenuItem_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.User,
            this.ImportTitle,
            this.Date,
            this.Size,
            this.FileName,
            this.HashSum,
            this.CurrentImportingSSID,
            this.Status});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(4, 5);
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1904, 679);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 50;
            // 
            // User
            // 
            this.User.Text = "User";
            this.User.Width = 200;
            // 
            // ImportTitle
            // 
            this.ImportTitle.Text = "Import Title";
            this.ImportTitle.Width = 200;
            // 
            // Date
            // 
            this.Date.Text = "Date";
            this.Date.Width = 100;
            // 
            // Size
            // 
            this.Size.Text = "Size";
            this.Size.Width = 100;
            // 
            // FileName
            // 
            this.FileName.Text = "File Name";
            this.FileName.Width = 220;
            // 
            // HashSum
            // 
            this.HashSum.Text = "File Hash Sum";
            this.HashSum.Width = 150;
            // 
            // CurrentImportingSSID
            // 
            this.CurrentImportingSSID.Text = "Current Importing SSID";
            this.CurrentImportingSSID.Width = 150;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSchedulePageToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(86, 34);
            // 
            // openSchedulePageToolStripMenuItem
            // 
            this.openSchedulePageToolStripMenuItem.Name = "openSchedulePageToolStripMenuItem";
            this.openSchedulePageToolStripMenuItem.Size = new System.Drawing.Size(85, 30);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 35);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1920, 722);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(1912, 689);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // WdbClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 788);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip2);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "WdbClientForm";
            this.Text = "WiFiDB Client";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader User;
        private System.Windows.Forms.ColumnHeader ImportTitle;
        private System.Windows.Forms.ColumnHeader Date;
        private new System.Windows.Forms.ColumnHeader Size;
        private System.Windows.Forms.ColumnHeader FileName;
        private System.Windows.Forms.ColumnHeader HashSum;
        private System.Windows.Forms.ColumnHeader CurrentImportingSSID;
        private System.Windows.Forms.ColumnHeader Status;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem openSchedulePageToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private ToolStripMenuItem schedulingToolStripMenuItem;
        private ToolStripMenuItem getWaitingListToolStripMenuItem;
        private ToolStripMenuItem getFinishedListToolStripMenuItem;
        private ToolStripMenuItem getImportingListToolStripMenuItem;
        private ToolStripMenuItem getBadImportsListToolStripMenuItem;
    }
}

