namespace MapStudy
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.studyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startStudyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.locationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.capitalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countriesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.capitalsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpControl = new MapStudy.HelpControl();
            this.inputBox = new MapStudy.InputBox();
            this.mapControl = new MapStudy.MapControl();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapMenuItem,
            this.viewMenuItem,
            this.studyMenuItem,
            this.helpMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1036, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mapMenuItem
            // 
            this.mapMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadMenuItem});
            this.mapMenuItem.Name = "mapMenuItem";
            this.mapMenuItem.Size = new System.Drawing.Size(43, 20);
            this.mapMenuItem.Text = "Map";
            // 
            // loadMenuItem
            // 
            this.loadMenuItem.Name = "loadMenuItem";
            this.loadMenuItem.Size = new System.Drawing.Size(109, 22);
            this.loadMenuItem.Text = "Load...";
            this.loadMenuItem.Click += new System.EventHandler(this.loadMenuItem_Click);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statisticsMenuItem});
            this.viewMenuItem.Name = "viewMenuItem";
            this.viewMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewMenuItem.Text = "View";
            // 
            // statisticsMenuItem
            // 
            this.statisticsMenuItem.Name = "statisticsMenuItem";
            this.statisticsMenuItem.Size = new System.Drawing.Size(129, 22);
            this.statisticsMenuItem.Text = "Statistics...";
            this.statisticsMenuItem.Click += new System.EventHandler(this.statisticsMenuItem_Click);
            // 
            // studyMenuItem
            // 
            this.studyMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startStudyMenuItem,
            this.reviewToolStripMenuItem});
            this.studyMenuItem.Name = "studyMenuItem";
            this.studyMenuItem.Size = new System.Drawing.Size(48, 20);
            this.studyMenuItem.Text = "Learn";
            // 
            // startStudyMenuItem
            // 
            this.startStudyMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem,
            this.locationsToolStripMenuItem,
            this.countriesToolStripMenuItem,
            this.capitalsToolStripMenuItem});
            this.startStudyMenuItem.Name = "startStudyMenuItem";
            this.startStudyMenuItem.Size = new System.Drawing.Size(152, 22);
            this.startStudyMenuItem.Text = "Study";
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // locationsToolStripMenuItem
            // 
            this.locationsToolStripMenuItem.Name = "locationsToolStripMenuItem";
            this.locationsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.locationsToolStripMenuItem.Text = "Locations";
            this.locationsToolStripMenuItem.Click += new System.EventHandler(this.locationsToolStripMenuItem_Click);
            // 
            // countriesToolStripMenuItem
            // 
            this.countriesToolStripMenuItem.Name = "countriesToolStripMenuItem";
            this.countriesToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.countriesToolStripMenuItem.Text = "Countries";
            this.countriesToolStripMenuItem.Click += new System.EventHandler(this.countriesToolStripMenuItem_Click);
            // 
            // capitalsToolStripMenuItem
            // 
            this.capitalsToolStripMenuItem.Name = "capitalsToolStripMenuItem";
            this.capitalsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.capitalsToolStripMenuItem.Text = "Capitals";
            this.capitalsToolStripMenuItem.Click += new System.EventHandler(this.capitalsToolStripMenuItem_Click);
            // 
            // reviewToolStripMenuItem
            // 
            this.reviewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.countriesToolStripMenuItem1,
            this.capitalsToolStripMenuItem1});
            this.reviewToolStripMenuItem.Name = "reviewToolStripMenuItem";
            this.reviewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.reviewToolStripMenuItem.Text = "Review";
            // 
            // countriesToolStripMenuItem1
            // 
            this.countriesToolStripMenuItem1.Name = "countriesToolStripMenuItem1";
            this.countriesToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.countriesToolStripMenuItem1.Text = "Countries";
            this.countriesToolStripMenuItem1.Click += new System.EventHandler(this.countriesToolStripMenuItem1_Click);
            // 
            // capitalsToolStripMenuItem1
            // 
            this.capitalsToolStripMenuItem1.Name = "capitalsToolStripMenuItem1";
            this.capitalsToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.capitalsToolStripMenuItem1.Text = "Capitals";
            this.capitalsToolStripMenuItem1.Click += new System.EventHandler(this.capitalsToolStripMenuItem1_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpControl
            // 
            this.helpControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.helpControl.Location = new System.Drawing.Point(770, 171);
            this.helpControl.Name = "helpControl";
            this.helpControl.PaddingPx = 10;
            this.helpControl.Size = new System.Drawing.Size(254, 440);
            this.helpControl.TabIndex = 3;
            // 
            // inputBox
            // 
            this.inputBox.BackColor = System.Drawing.SystemColors.Control;
            this.inputBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.inputBox.Location = new System.Drawing.Point(770, 39);
            this.inputBox.Name = "inputBox";
            this.inputBox.PaddingPx = 10;
            this.inputBox.Size = new System.Drawing.Size(254, 125);
            this.inputBox.TabIndex = 2;
            // 
            // mapControl
            // 
            this.mapControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mapControl.Location = new System.Drawing.Point(12, 39);
            this.mapControl.Name = "mapControl";
            this.mapControl.Resizing = false;
            this.mapControl.Size = new System.Drawing.Size(751, 572);
            this.mapControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 623);
            this.Controls.Add(this.helpControl);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.mapControl);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(1200, 800);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "MainForm";
            this.Text = "Map Study";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapControl mapControl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mapMenuItem;
        private System.Windows.Forms.ToolStripMenuItem studyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startStudyMenuItem;
        private InputBox inputBox;
        private HelpControl helpControl;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem locationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem capitalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countriesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem capitalsToolStripMenuItem1;
    }
}