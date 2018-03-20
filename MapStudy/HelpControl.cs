using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapStudy
{
    public partial class HelpControl : UserControl
    {
        public HelpControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            PaddingPx = 10;

            if (!DesignMode)
            {
                infotitle.Font = new Font("Muli Light", 20f, FontStyle.Bold);
                infoBox.Font = new Font("Muli Light", 12f, FontStyle.Bold);
            }

            infotitle.Visible = false;
            infoBox.Visible = false;
        }

        private MapData mapData;

        public int PaddingPx { get; set; }

        public void LoadData(MapData mapData)
        {
            this.mapData = mapData;
        }

        public void ShowHelp(int country, ProblemType problem)
        {
            switch (problem)
            {
                case ProblemType.Locate:
                case ProblemType.SpellCountryVoice:
                case ProblemType.SpellCountry:
                    infotitle.Text = mapData.CountryNames[country];
                    Wikipedia.GetCountryInfo(mapData.CountryNames[country]).ContinueWith(SetContent);
                    infotitle.Visible = true;
                    infoBox.Visible = true;
                    break;
                case ProblemType.SpellCapital:
                case ProblemType.SpellCapitalVoice:
                    infotitle.Text = mapData.CountryCapitals[country];
                    Wikipedia.GetCapitalInfo(mapData.CountryCapitals[country]).ContinueWith(SetContent);
                    infotitle.Visible = true;
                    infoBox.Visible = true;
                    break;
            }
        }

        private void SetContent(Task<string> obj)
        {
            if (obj.IsCompleted && !obj.IsFaulted)
            {
                infoBox.Invoke(new MethodInvoker(delegate () { infoBox.Text = obj.Result; }));
            }
            else
            {
                infoBox.Clear();
            }
        }

        public void Clear()
        {
            infotitle.Visible = false;
            infoBox.Visible = false;
            infoBox.Clear();
        }

        public void Center()
        {
            infotitle.Location = new Point(PaddingPx, PaddingPx);
            infoBox.Location = new Point(0, infotitle.Bottom + PaddingPx);
            infoBox.Width = Width;
            infoBox.Height = Height - infoBox.Location.Y;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            Center();
            base.OnResize(e);
        }


    }
}
