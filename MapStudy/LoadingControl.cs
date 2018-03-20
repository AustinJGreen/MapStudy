using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MapStudy
{
    public partial class LoadingControl : UserControl
    {
        public LoadingControl()
        {
            InitializeComponent();
            clock = new Clock(60);
            clock.OnTick += OnTick;

            Thickness = 5f;
            DoubleBuffered = true;

            gradAngle = 0;
            startAngle = 0;
            sweepAngle = 45f;
            SpeedPx = 500;
        }

        private int dir = 1;
        private float gradAngle;
        private float startAngle;
        private float sweepAngle;

        public float Thickness { get; set; }
        public int SpeedPx { get; set; }

        private void OnTick(object sender, EventArgs e)
        {
            startAngle += (1 / 30f) * SpeedPx;
            startAngle %= 360;

            gradAngle += (1 / 30f) * 360f;
            gradAngle %= 360;

            sweepAngle += (1 / 30f) * SpeedPx * dir;
            if (sweepAngle > 360)
            {
                sweepAngle = 360;
                dir = -dir;
            }
            else if (sweepAngle <= 0)
            {
                dir = -dir;
                sweepAngle = 0;
            }

            Invalidate();
        }

        public void Start()
        {
            clock.Start();

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () { Visible = true; }));
            }
            else
            {
                Visible = true;
            }         
        }

        public void Stop()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () { Visible = false; }));
            }
            else
            {
                Visible = false;
            }

            clock.Stop();
        }

        private void DrawAnimation(Graphics gfx)
        {
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (LinearGradientBrush lgb = new LinearGradientBrush(rect, Color.YellowGreen, Color.LimeGreen, gradAngle))
            using (Pen pen = new Pen(lgb, Thickness))
            {
                gfx.FillPie(lgb, rect, startAngle, sweepAngle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            DrawAnimation(gfx);
            base.OnPaint(e);
        }
    }
}
