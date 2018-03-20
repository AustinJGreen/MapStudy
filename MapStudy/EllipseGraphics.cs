using Emgu.CV.Structure;
using System;
using System.Drawing;

namespace MapStudy
{
    public class EllipseGraphics : IGraphics
    {
        private Pt[] pts;
        private MapControl control;
        private Pt center;
        private int width;
        private int height;
        private Ellipse ellipse;

        public void Draw(Bgra color)
        {
            lock (control.Context)
            {
                control.Context.Draw(ellipse, color, -1);
            }
        }

        public float HitTest(PointF pt)
        {
            double angRad = Math.Atan2(pt.Y - center.Y, pt.X - center.X);
            //Find the pt on the ellipse at this angle

            double wRadi = width / 2.0;
            double hRadi = height / 2.0;

            double ePtX = center.X + (wRadi * Math.Cos(angRad));
            double ePtY = center.Y + (hRadi * Math.Sin(angRad));

            double exCross = (ePtX - center.X) * (ePtX - center.X);
            double eyCross = (ePtY - center.Y) * (ePtY - center.Y);
            //Compute distance from ePt to center
            double ePtDis = Math.Sqrt(exCross + eyCross);

            //Compute distance from pt to center
            double xCross = (pt.X - center.X) * (pt.X - center.X);
            double yCross = (pt.Y - center.Y) * (pt.Y - center.Y);

            double ptDis = Math.Sqrt(xCross + yCross);
            if (ePtDis >= ptDis)
            {
                return 1.0f;
            }
            else
            {
                return 0f;
            }
        }

        public bool Init()
        {
            if (pts.Length == 1)
            {
                center = pts[0];
                width = 40;
                height = 40;
            }
            else
            {
                int xMin = int.MaxValue;
                int yMin = int.MaxValue;
                int xMax = int.MinValue;
                int yMax = int.MinValue;
                int xTotal = 0;
                int yTotal = 0;
                for (int i = 0; i < pts.Length; i++)
                {
                    xTotal += (int)pts[i].X;
                    yTotal += (int)pts[i].Y;

                    xMin = Math.Min(xMin, (int)pts[i].X);
                    xMax = Math.Max(xMax, (int)pts[i].X);

                    yMin = Math.Min(yMin, (int)pts[i].Y);
                    yMax = Math.Max(yMax, (int)pts[i].Y);
                }

                center = new Pt(xTotal / pts.Length, yTotal / pts.Length);
                width = xMax - xMin;
                height = yMax - yMin;

                if (width == 0)
                {
                    width = height;
                }

                if (height == 0)
                {
                    height = width;
                }       
            }

            ellipse = new Ellipse(new PointF(center.X, center.Y), new SizeF(width, height), 90);
            return true;
        }

        public EllipseGraphics(MapControl control, Pt[] pts)
        {
            this.control = control;
            this.pts = pts;
        }
    }
}
