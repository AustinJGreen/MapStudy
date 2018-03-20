namespace MapStudy
{
    partial class HelpControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.infoBox = new System.Windows.Forms.RichTextBox();
            this.infotitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // infoBox
            // 
            this.infoBox.BackColor = System.Drawing.SystemColors.Control;
            this.infoBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.infoBox.Location = new System.Drawing.Point(0, 36);
            this.infoBox.Name = "infoBox";
            this.infoBox.ReadOnly = true;
            this.infoBox.Size = new System.Drawing.Size(282, 381);
            this.infoBox.TabIndex = 0;
            this.infoBox.Text = "";
            // 
            // infotitle
            // 
            this.infotitle.AutoSize = true;
            this.infotitle.Location = new System.Drawing.Point(13, 11);
            this.infotitle.Name = "infotitle";
            this.infotitle.Size = new System.Drawing.Size(48, 13);
            this.infotitle.TabIndex = 1;
            this.infotitle.Text = "Info Title";
            // 
            // HelpControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infotitle);
            this.Controls.Add(this.infoBox);
            this.Name = "HelpControl";
            this.Size = new System.Drawing.Size(282, 464);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox infoBox;
        private System.Windows.Forms.Label infotitle;
    }
}
