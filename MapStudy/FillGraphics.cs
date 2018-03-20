using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace MapStudy
{
    public class FillGraphics : IGraphics
    {
        private Pt[] points;
        private Contour<Point>[] contours;
        private MapControl control;

        public void Draw(Bgra color)
        {
            if (contours != null)
            {
                lock (control.Context)
                {
                    for (int i = 0; i < contours.Length; i++)
                    {
                        control.Context.Draw(contours[i], color, -1);
                    }
                }
            }
        }

        public float HitTest(PointF pt)
        {
            if (contours != null)
            {
                for (int i = 0; i < contours.Length; i++)
                {
                    if (contours[i].InContour(pt) > 0)
                    {
                        return 1f;
                    }
                }
            }

            return 0f;
        }

        public bool Init()
        {
            using (Image<Gray, byte> bw = control.Data.Convert<Gray, byte>())
            using (Image<Gray, byte> thres = bw.ThresholdBinary(new Gray(120), new Gray(255)))
            {
                bool foundContour = false;
                for (int i = 0; i < points.Length; i++)
                {
                    PointF ptf = new PointF(points[i].X, points[i].Y);
                    for (Contour<Point> cont = thres.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST); cont != null; cont = cont.HNext)
                    {
                        if (cont.Area > 0 && cont.InContour(ptf) > 0)
                        {
                            foundContour = true;
                            contours[i] = cont;
                            break;
                        }
                    }
                }

                if (!foundContour)
                {
                    return false;
                }
            }

            return true;
        }

        public FillGraphics(MapControl control, Pt[] points)
        {
            this.control = control;
            this.points = points;
            contours = new Contour<Point>[points.Length];
        }
    }
}
