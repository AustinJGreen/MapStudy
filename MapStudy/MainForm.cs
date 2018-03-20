using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapStudy
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            PaddingPx = 12;

            loadingAnim = new LoadingControl();
            loadingAnim.Location = new Point(mapControl.Location.X + (mapControl.Width / 2) - (loadingAnim.Width / 2), mapControl.Location.Y + (mapControl.Height / 2) - (loadingAnim.Height / 2));
            loadingAnim.Size = new Size(50, 50);
            loadingAnim.Visible = false;

            Controls.Add(loadingAnim);
            loadingAnim.BringToFront();
        }

        private LoadingControl loadingAnim;

        public int PaddingPx { get; set; }

        public void ResizeControls()
        {
            int inputSize = 300;

            mapControl.Location = new Point(PaddingPx, menuStrip1.Bottom + PaddingPx);
            mapControl.Size = new Size(Width - inputSize - (PaddingPx * 4), Height - (PaddingPx * 2) - menuStrip1.Height - mapControl.Location.Y);
            inputBox.Location = new Point(Width - (PaddingPx * 2) - inputSize, menuStrip1.Bottom + PaddingPx);
            inputBox.Size = new Size(inputSize, Math.Max((Height / 3) - (PaddingPx * 2) - menuStrip1.Height, 150));
            helpControl.Location = new Point(Width - (PaddingPx * 2) - inputSize, inputBox.Bottom + PaddingPx);
            helpControl.Size = new Size(inputSize, Height - inputBox.Bottom - (PaddingPx * 3) - menuStrip1.Height);
        }

        private void loadMenuItem_Click(object sender, EventArgs e)
        {
            if (mapControl.Visible)
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    string map_path = Path.Combine(Environment.CurrentDirectory, "maps");
                    ofd.InitialDirectory = Directory.Exists(map_path) ? map_path : Environment.CurrentDirectory;

                    ofd.CheckFileExists = true;
                    ofd.CheckPathExists = true;
                    ofd.SupportMultiDottedExtensions = true;
                    ofd.Filter = "Map Files (*.map)|*.map";
                    ofd.Multiselect = false;
                    ofd.AutoUpgradeEnabled = true;

                    DialogResult dr = ofd.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        mapControl.Visible = false;
                        mapControl.Clear();

                        string contents = File.ReadAllText(ofd.FileName);

                        loadingAnim.Start();
                        Task.Run(delegate ()
                        {
                            MapData mapData = JsonConvert.DeserializeObject<MapData>(contents);
                            if (mapData.ZOrders == null)
                            {
                                mapData.ZOrders = new int[mapData.Countries];
                            }

                            mapControl.LoadData(mapData, inputBox, helpControl);
                        }).ContinueWith(OnEndLoad);
                    }
                }
            }
        }

        private void OnEndLoad(Task obj)
        {
            if (loadingAnim != null)
            {
                loadingAnim.Stop();
            }

            if (mapControl != null)
            {
                mapControl.Invoke(new MethodInvoker(delegate () { mapControl.Visible = true; }));
                mapControl.Invalidate();
            }
        }

        private void statisticsMenuItem_Click(object sender, EventArgs e)
        {
            if (mapControl.Teacher != null)
            {
                using (StatisticsForm statsForm = new StatisticsForm())
                {
                    Dictionary<DateTime, Statistic[]> stats = mapControl.Teacher.GetHistoryStatistics();
                    statsForm.LoadStats(stats);
                    statsForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Please load a map.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (About about = new About())
            {
                about.ShowDialog();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mapControl.SaveData();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (loadingAnim != null)
            {
                loadingAnim.Location = new Point(mapControl.Location.X + (mapControl.Width / 2) - (loadingAnim.Width / 2), mapControl.Location.Y + (mapControl.Height / 2) - (loadingAnim.Height / 2));
            }

            ResizeControls();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            Text = string.Format("MapStudy {0}.{1}.{2}", ver.Major, ver.Minor, ver.Revision);
            ResizeControls();
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            mapControl.Resizing = true;
            base.OnResizeBegin(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            mapControl.Resizing = false;
            mapControl.Invalidate();
            base.OnResizeEnd(e);
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapControl.Teacher != null)
            {
                mapControl.Teach(ProblemType.Locate, ProblemType.SpellCountry, ProblemType.SpellCapital);
            }
            else
            {
                MessageBox.Show("Please load a map before starting to study.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void locationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapControl.Teacher != null)
            {
                mapControl.Teach(ProblemType.Locate);
            }
            else
            {
                MessageBox.Show("Please load a map before starting to study.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void countriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapControl.Teacher != null)
            {
                mapControl.Teach(ProblemType.SpellCountry);
            }
            else
            {
                MessageBox.Show("Please load a map before starting to study.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void capitalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapControl.Teacher != null)
            {
                mapControl.Teach(ProblemType.SpellCapital);
            }
            else
            {
                MessageBox.Show("Please load a map before starting to study.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void countriesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (mapControl.Teacher != null)
            {
                inputBox.SetReviewMode();
                mapControl.StartReviewing(1);
            }
            else
            {
                MessageBox.Show("Please load a map before starting to review.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void capitalsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (mapControl.Teacher != null)
            {
                inputBox.SetReviewMode();
                mapControl.StartReviewing(2);
            }
            else
            {
                MessageBox.Show("Please load a map before starting to review.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
