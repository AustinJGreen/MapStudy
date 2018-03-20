using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Diagnostics;

namespace MapStudy
{
    public partial class MapControl : UserControl
    {
        public MapControl()
        {
            InitializeComponent();

            DoubleBuffered = true;
            ResizeRedraw = true;
            Resizing = false;

            animClock = new Clock(100);
            animClock.OnTick += OnAnimTick;

            animEnd = new Bgra(227, 227, 227, 255);
            highlightTime = 1500;

            currentProblem = ProblemType.None;
            reviewing = -1;
            
        }

        public Image<Bgra, byte> Context => context;
        public Image<Bgra, byte> Data => data;

        public Teacher Teacher => teacher;

        public bool Resizing { get; set; }

        private MapData mapData;
        private HelpControl helpControl;
        private IGraphics[] graphics;

        private Bgra animStart;
        private Bgra animEnd;

        private int animCountry;
        private int animDirection = 0;
        private int highlightTime;

        //Current problem type
        private int reviewing;
        private ProblemType currentProblem;

        public void Clear()
        {
            if (teacher != null)
            {
                teacher.SaveUserData();
                teacher.Stop();
                SetProblem(ProblemType.None);
                StopHighlighting();
                animClock.Stop();
            }

            mapData = null;
            Invalidate();
        }

        public void Teach(params ProblemType[] types)
        {
            teacher.Start(types);
        }

        public void StopTeaching()
        {
            teacher.Stop();
        }

        public void LoadData(MapData mapData, InputBox input, HelpControl help)
        {
            StopHighlighting();
            this.mapData = mapData;

            if (mapData != null && !Resizing)
            {
                string curDir = Environment.CurrentDirectory;
                string imageFile = Path.Combine(curDir, "images", mapData.File);

                if (!File.Exists(imageFile))
                {
                    MessageBox.Show("Missing map image file.", "Missing Files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    context = new Image<Bgra, byte>(imageFile);
                    data = context.Clone();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error = {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                graphics = new IGraphics[mapData.Countries];
                for (int i = 0; i < mapData.Countries; i++)
                {
                    IGraphics type;
                    if (mapData.CountryTypes[i] == typeof(EllipseGraphics))
                    {
                        type = (IGraphics)Activator.CreateInstance(mapData.CountryTypes[i], new object[] { this, mapData.CountryPoints[i] });
                    }
                    else if (mapData.CountryTypes[i] == typeof(FillGraphics))
                    {
                        type = (IGraphics)Activator.CreateInstance(mapData.CountryTypes[i], new object[] { this, mapData.CountryPoints[i] });
                    }
                    else
                    {
                        type = null;
                    }

                    graphics[i] = type;
                    if (!graphics[i].Init())
                    {
                        MessageBox.Show(string.Format("Invalid graphics for (name={0}:index={1})", mapData.CountryNames[i], i), "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                helpControl = help;
                if (teacher == null)
                {
                    teacher = new Teacher(this, input, help);
                }

                help.LoadData(mapData);
                teacher.Load(mapData);
                teacher.LoadUserData();
                Invalidate();
            }
        }

        public void SaveData()
        {
            if (teacher != null)
            {
                teacher.SaveUserData();
            }
        }

        private byte Saturate(double value, byte min, byte max)
        {
            if (value < min)
            {
                return (byte)min;
            }
            else if (value > max)
            {
                return (byte)max;
            }
            else
            {
                return (byte)value;
            }
        }

        private double Lerp(double value, double to, double percent)
        {
            return value + ((to - value) * percent);
        }

        private Bgra Lerp(Bgra from, Bgra to, double percent)
        {
            byte a = Saturate(Lerp(from.Alpha, to.Alpha, percent), 0, 255);
            byte r = Saturate(Lerp(from.Red, to.Red, percent), 0, 255);
            byte g = Saturate(Lerp(from.Green, to.Green, percent), 0, 255);
            byte b = Saturate(Lerp(from.Blue, to.Blue, percent), 0, 255);

            return new Bgra(b, g, r, a);
        }

        public void HighlightCountry(int index, Bgra color)
        {
            animStart = color;
            animCountry = index;
            animDirection = 1;

            if (!animClock.Running)
            {
                animClock.Start();
            }
        }

        public void StopHighlighting()
        {
            animCountry = -1;
        }

        public void SetProblem(ProblemType problem)
        {
            reviewing = -1;
            this.currentProblem = problem;
        }

        public void StartReviewing(int type)
        {
            helpControl.Clear();
            StopHighlighting();
            reviewing = type;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (mapData != null && !DesignMode)
            {
                if (animCountry != -1)
                {
                    double percent = animClock.Elapsed / (double)highlightTime;
                    if (animDirection == 1)
                    {
                        Bgra current = Lerp(animStart, animEnd, percent);
                        graphics[animCountry].Draw(current);
                    }
                    else
                    {
                        Bgra current = Lerp(animEnd, animStart, percent);
                        graphics[animCountry].Draw(current);
                    }

                    if (animClock.Elapsed >= highlightTime - animClock.TickTime)
                    {
                        animClock.Reset();
                        animDirection = -animDirection;
                    }
                }
                else
                {
                    animClock.Stop();
                    animClock.Reset();
                    animDirection = 0;
                }

                if (context != null)
                {
                    lock (context)
                    {
                        try
                        {
                            Graphics gfx = e.Graphics;
                            gfx.DrawImage(context.Bitmap, 0, 0, Width, Height);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message, "Fatal");
                        }
                    }
                }
            }
            else
            {
                Graphics gfx = e.Graphics;
                gfx.Clear(BackColor);
            }
        }

        public int? GetCountry(Point pt)
        {
            if (context != null)
            {
                lock (context)
                {
                    float sX = context.Width / (float)Width;
                    float sY = context.Height / (float)Height;
                    PointF scaled = new PointF(pt.X * sX, pt.Y * sY);

                    Color color = Color.FromArgb(200, 239, 150, 0);

                    int? z = null;
                    int? hitIndex = null;
                    for (int i = 0; i < graphics.Length; i++)
                    {
                        float hit = graphics[i].HitTest(scaled);
                        if (hit > 0 && (!z.HasValue || mapData.ZOrders[i] > z))
                        {
                            z = mapData.ZOrders[i];
                            hitIndex = i;
                        }
                    }

                    return hitIndex;
                }
            }

            return null;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (currentProblem == ProblemType.Locate)
            {
                int? hitIndex = GetCountry(e.Location);
                if (hitIndex.HasValue)
                {
                    teacher.OnProblemInput(hitIndex.Value);
                }
            }
            else if (reviewing == 1 || reviewing == 2)
            {
                int? hitIndex = GetCountry(e.Location);
                if (hitIndex.HasValue)
                {
                    switch (reviewing)
                    {
                        case 1:
                            HighlightCountry(hitIndex.Value, new Bgra(91, 166, 38, 255));
                            teacher.PlayVorbis(0, string.Concat("sounds\\", mapData.CountryNames[hitIndex.Value].ToLower()));
                            helpControl.ShowHelp(hitIndex.Value, ProblemType.SpellCountry);
                            break;
                        case 2:
                            HighlightCountry(hitIndex.Value, new Bgra(24, 30, 217, 255));
                            teacher.PlayVorbis(0, string.Concat("sounds\\", mapData.CountryCapitals[hitIndex.Value].ToLower()));
                            helpControl.ShowHelp(hitIndex.Value, ProblemType.SpellCapital);
                            break;
                    }
                }
                else
                {
                    StopHighlighting();
                    helpControl.Clear();
                }
            }
        }

        private void OnAnimTick(object sender, EventArgs e)
        {
            if (!Resizing && context != null)
            {
                lock (context)
                {
                    context.Dispose();
                    context = data.Clone();
                }

                Invalidate();
            }
        }
    }
}
