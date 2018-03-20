using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace MapStudy
{
    public partial class StatsControl : UserControl
    {
        public StatsControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            PaddingPx = 20;

            if (!DesignMode)
            {
                Font = new Font("Muli Light", 11f, FontStyle.Regular);
            }
        }

        public int PaddingPx { get; set; }

        private Statistic[] stats;

        public void LoadStats(Statistic[] stats)
        {
            this.stats = stats;
        }

        protected void DrawLegend(Graphics gfx, Rectangle rect, Color color, string label)
        {
            SizeF textSize = gfx.MeasureString(label, Font);
            float widthLeft = rect.Width - textSize.Width;

            RectangleF colorRect = new RectangleF(rect.X, rect.Y, widthLeft, rect.Height);
            PointF labelPt = new PointF(rect.Right - textSize.Width, rect.Y); 

            using (SolidBrush fillBrush = new SolidBrush(color))
            using (SolidBrush borderBrush = new SolidBrush(Color.Black))
            using (Pen borderPen = new Pen(borderBrush, 1f))
            {
                gfx.FillRectangle(fillBrush, colorRect);
                gfx.DrawRectangle(borderPen, colorRect.X, colorRect.Y, colorRect.Width, colorRect.Height);
                gfx.DrawString(label, Font, borderBrush, labelPt);
            }
        }

        protected void DrawStat(Statistic statistic, Color[] colors)
        {
            System.Diagnostics.Debug.Assert(colors.Length == 3);
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Color[] colors = new Color[3] { Color.Red, Color.Green, Color.Blue };
            string[] labels = new string[3] { "Location", "Country", "Capital" };

            int pWidth = Width - (PaddingPx * 2);
            for (int i = 0; i < labels.Length; i++)
            {
                Rectangle rect = new Rectangle(PaddingPx + i * (pWidth / labels.Length), PaddingPx, pWidth / labels.Length, 20);             
                DrawLegend(e.Graphics, rect, colors[i], labels[i]);
            }

            //Sort stats from lowest to highest accuracy
            Statistic[] ranked = stats.OrderByDescending(t => t.GetAccuracyGradient()).ToArray();

            base.OnPaint(e);
        }
    }
}
