using Emgu.CV.Structure;
using System.Drawing;

namespace MapStudy
{
    public interface IGraphics
    {
        void Draw(Bgra color);
        bool Init();
        float HitTest(PointF pt);
    }
}
