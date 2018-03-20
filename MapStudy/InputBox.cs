using System;
using System.Drawing;
using System.Windows.Forms;

namespace MapStudy
{
    public partial class InputBox : UserControl
    {
        private Random random;

        private string[] negatives = new string[]
        {
            "Almost right.",
            "Not quite.",
            "Your answer is incorrect :(",
            "Not the best answer.",
        };

        private string[] positives = new string[]
        {
            "You're the best!",
            "Correct!",
            "Precisely!",
        };

        public InputBox()
        {
            InitializeComponent();
            PaddingPx = 10;

            if (!DesignMode)
            {
                inputLbl1.Font = new Font("Muli Light", 11f, FontStyle.Regular);
                inputTxt1.Font = new Font("Muli Light", 16f, FontStyle.Bold);
                playVoiceBtn.Font = new Font("Muli Light", 8f, FontStyle.Regular);

                inputLbl1.Visible = false;
                inputTxt1.Visible = false;
                playVoiceBtn.Visible = false;
                nxtBtn.Visible = false;
            }

            random = new Random();
            timeoutClock = new Clock(1000);
            timeoutClock.OnTick += OnTick;
        }

        private int secondsLeft = 0;

        private void OnTick(object sender, EventArgs e)
        {
            if (secondsLeft == 0)
            {
                timeoutClock.Stop();
                if (nxtBtn.InvokeRequired)
                {
                    nxtBtn.Invoke(new MethodInvoker(delegate ()
                    {
                        nxtBtn.Text = "Next Problem";
                        nxtBtn.Enabled = true;
                    }));
                }
                else
                {
                    nxtBtn.Text = "Next Problem";
                    nxtBtn.Enabled = true;
                }
            }
            else
            {
                secondsLeft--;
                if (nxtBtn.InvokeRequired)
                {
                    nxtBtn.Invoke(new MethodInvoker(delegate ()
                    {
                        nxtBtn.Text = string.Format("Study ({0} sec).", secondsLeft);
                    }));
                }
                else
                {
                    nxtBtn.Text = string.Format("Study ({0} sec).", secondsLeft);
                }
            }
        }

        public int PaddingPx { get; set; }

        public event EventHandler<string> OnInput;

        public event EventHandler OnPlayAgain;

        public event EventHandler OnNext;

        public void SetReviewMode()
        {
            inputLbl1.Text = string.Format("Click on any country to review it.");
            inputLbl1.Visible = true;
            inputTxt1.Visible = false;
            inputTxt1.Enabled = false;
            nxtBtn.Visible = false;
            playVoiceBtn.Visible = false;
        }

        public void SetProblem(ProblemType problem, string value = null)
        {
            switch (problem)
            {
                case ProblemType.Locate:
                    inputLbl1.Text = string.Format("Locate {0}", value ?? "the country.");
                    inputLbl1.Visible = true;
                    inputTxt1.Visible = false;
                    inputTxt1.Enabled = false;
                    nxtBtn.Visible = false;
                    playVoiceBtn.Visible = true;
                    break;
                case ProblemType.SpellCapital:
                    inputLbl1.Text = string.Format("Spell the capital of this country.");
                    inputLbl1.Visible = true;
                    inputTxt1.Visible = true;
                    inputTxt1.Enabled = true;
                    nxtBtn.Visible = false;
                    playVoiceBtn.Visible = false;
                    break;
                case ProblemType.SpellCapitalVoice:
                    inputLbl1.Text = string.Format("Spell the capital of this country.");
                    inputLbl1.Visible = true;
                    inputTxt1.Visible = true;
                    inputTxt1.Enabled = true;
                    nxtBtn.Visible = false;
                    playVoiceBtn.Visible = true;
                    break;
                case ProblemType.SpellCountry:
                    inputLbl1.Text = string.Format("Spell the name of this country.");
                    inputLbl1.Visible = true;
                    inputTxt1.Visible = true;
                    inputTxt1.Enabled = true;
                    nxtBtn.Visible = false;
                    playVoiceBtn.Visible = false;
                    break;
                case ProblemType.SpellCountryVoice:
                    inputLbl1.Text = string.Format("Spell the name of this country.");
                    inputLbl1.Visible = true;
                    inputTxt1.Visible = true;
                    inputTxt1.Enabled = true;
                    nxtBtn.Visible = false;
                    playVoiceBtn.Visible = true;
                    break;
                case ProblemType.None:
                    inputLbl1.Visible = false;
                    inputTxt1.Visible = false;
                    inputTxt1.Enabled = false;
                    nxtBtn.Visible = false;
                    playVoiceBtn.Visible = false;
                    break;
            }
        }

        public void SetComplete(bool correct, int timeoutSeconds)
        {
            //Show the next button and a congratulations or a helpful message if incorrect
            if (correct)
            {
                inputLbl1.Text = positives[random.Next(positives.Length)];
            }
            else
            {
                inputLbl1.Text = negatives[random.Next(positives.Length)];
            }

            if (timeoutSeconds == 0)
            {
                nxtBtn.Visible = true;
            }
            else
            {
                secondsLeft = timeoutSeconds;
                nxtBtn.Text = string.Format("Study ({0} sec).", secondsLeft);
                nxtBtn.Enabled = false;
                nxtBtn.Visible = true;
                timeoutClock.Start();
            }
        }

        public void Center()
        {
            inputLbl1.Location = new Point(PaddingPx, PaddingPx);
            playVoiceBtn.Location = new Point(Width - playVoiceBtn.Width - PaddingPx, PaddingPx);

            nxtBtn.Location = new Point(PaddingPx, Height - nxtBtn.Height - PaddingPx);
            nxtBtn.Width = Width - (nxtBtn.Location.X * 2);

            inputTxt1.Location = new Point(PaddingPx + 5, nxtBtn.Location.Y - inputTxt1.Height - PaddingPx);
            inputTxt1.Width = Width - (inputTxt1.Location.X * 2);
            
        }

        protected override void OnResize(EventArgs e)
        {
            Center();
            base.OnResize(e);
        }

        private void inputTxt1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(inputTxt1.Text) && e.KeyChar == (char)Keys.Enter)
            {
                if (OnInput != null)
                {
                    inputTxt1.Enabled = false;
                    OnInput(this, inputTxt1.Text);
                }
            }
        }

        private void nextBtn_Click(object sender, EventArgs e)
        { 
            if (OnNext != null)
            {
                nxtBtn.Visible = false;
                inputTxt1.Clear();

                OnNext(this, EventArgs.Empty);
            }
        }

        private void playVoiceBtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (OnPlayAgain != null)
            {
                OnPlayAgain(this, EventArgs.Empty);
            }
        }
    }
}
