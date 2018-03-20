namespace MapStudy
{
    partial class InputBox
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

            timeoutClock.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.inputTxt1 = new System.Windows.Forms.TextBox();
            this.inputLbl1 = new System.Windows.Forms.Label();
            this.nxtBtn = new System.Windows.Forms.Button();
            this.playVoiceBtn = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // inputTxt1
            // 
            this.inputTxt1.BackColor = System.Drawing.SystemColors.Control;
            this.inputTxt1.Location = new System.Drawing.Point(17, 46);
            this.inputTxt1.Name = "inputTxt1";
            this.inputTxt1.Size = new System.Drawing.Size(246, 20);
            this.inputTxt1.TabIndex = 0;
            this.inputTxt1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputTxt1_KeyPress);
            // 
            // inputLbl1
            // 
            this.inputLbl1.AutoSize = true;
            this.inputLbl1.Location = new System.Drawing.Point(14, 21);
            this.inputLbl1.Name = "inputLbl1";
            this.inputLbl1.Size = new System.Drawing.Size(31, 13);
            this.inputLbl1.TabIndex = 1;
            this.inputLbl1.Text = "Input";
            // 
            // nxtBtn
            // 
            this.nxtBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.nxtBtn.Location = new System.Drawing.Point(17, 72);
            this.nxtBtn.Name = "nxtBtn";
            this.nxtBtn.Size = new System.Drawing.Size(246, 23);
            this.nxtBtn.TabIndex = 2;
            this.nxtBtn.Text = "Next Problem";
            this.nxtBtn.UseVisualStyleBackColor = true;
            this.nxtBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // playVoiceBtn
            // 
            this.playVoiceBtn.AutoSize = true;
            this.playVoiceBtn.Location = new System.Drawing.Point(52, 21);
            this.playVoiceBtn.Name = "playVoiceBtn";
            this.playVoiceBtn.Size = new System.Drawing.Size(64, 13);
            this.playVoiceBtn.TabIndex = 3;
            this.playVoiceBtn.TabStop = true;
            this.playVoiceBtn.Text = "Listen again";
            this.playVoiceBtn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.playVoiceBtn_LinkClicked);
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.playVoiceBtn);
            this.Controls.Add(this.nxtBtn);
            this.Controls.Add(this.inputLbl1);
            this.Controls.Add(this.inputTxt1);
            this.Name = "InputBox";
            this.Size = new System.Drawing.Size(280, 113);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inputTxt1;
        private System.Windows.Forms.Label inputLbl1;
        private System.Windows.Forms.Button nxtBtn;
        private System.Windows.Forms.LinkLabel playVoiceBtn;
        private Clock timeoutClock;
    }
}
