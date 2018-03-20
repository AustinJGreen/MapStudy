using Emgu.CV;
using Emgu.CV.Structure;

namespace MapStudy
{
    partial class MapControl
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

            if (animClock != null)
            {
                animClock.Dispose();
            }

            if (context != null)
            {
                context.Dispose();
                context = null;
            }

            if (data != null)
            {
                data.Dispose();
                data = null;
            }

            if (teacher != null)
            {
                teacher.Dispose();
                teacher = null;
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
            this.SuspendLayout();
            // 
            // MapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MapControl";
            this.Size = new System.Drawing.Size(747, 578);
            this.ResumeLayout(false);

        }

        private Clock animClock;
        private Image<Bgra, byte> context;
        private Image<Bgra, byte> data;
        private Teacher teacher;
        #endregion
    }
}
