using Emgu.CV.Structure;
using NAudio.Vorbis;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace MapStudy
{
    public class Teacher : IDisposable
    {
        //Random variable
        private Random random;
        private ProblemType[] problemTypes;
        private volatile int soundInstances = 0;

        [JsonIgnore]
        private ManualResetEvent soundHandle;

        [JsonIgnore]
        private MapData mapData;

        //UI
        [JsonIgnore]
        private MapControl control; //The map

        [JsonIgnore]
        private InputBox inputBox; //The input box

        [JsonIgnore]
        private HelpControl helpControl; //The help control

        //Current problem
        [JsonIgnore]
        private int currentCountry;

        [JsonIgnore]
        private Problem current;

        //Sigmoid like function that tells how the student is doing.
        //The closer to 0 that means the player is doing worse
        //The closer to 1 that means the player is doing better 
        [JsonIgnore]
        private double mx = 0; //The x point of the derivative function

        [JsonIgnore]
        private double momentum = 0.5; //Momentum should'nt be saved, it's per session

        [JsonIgnore]
        private Dictionary<DateTime, Statistic[]> history;

        //Each of the country's current/last individual statistic
        [JsonIgnore]
        private Statistic[] statistics;

        public Statistic[] GetSessionStatistics()
        {
            return statistics;
        }

        public Dictionary<DateTime, Statistic[]> GetHistoryStatistics()
        {
            return history;
        }

        public void UpdateMomentum(int direction)
        {
            int dir = Math.Sign(direction);
            mx += dir * (1.0 / (mapData.Countries * 3)); //3 things to remember
            momentum = 1.0 / (1 + Math.Exp(-mx));
        }
        
        public void UpdateStatistics(object input)
        {
            switch (current.Type)
            {
                case ProblemType.Locate:
                    statistics[currentCountry].ProcessLocation((int)input);
                    break;
                case ProblemType.SpellCapitalVoice:
                case ProblemType.SpellCapital:
                    statistics[currentCountry].ProcessCapitalSpelling((string)input);
                    break;
                case ProblemType.SpellCountryVoice:
                case ProblemType.SpellCountry:
                    statistics[currentCountry].ProcessCountrySpelling((string)input);
                    break;
            }
        }

        private void OnInput(object sender, string e)
        {
            OnProblemInput(e);
        }

        private void OnNext(object sender, EventArgs e)
        {
            NextProblem();
        }

        private void OnPlayAgain(object sender, EventArgs e)
        {
            //Play voice from current problem (if availible) again
            switch (current.Type)
            {
                case ProblemType.Locate:
                    PlayVorbis(0, "common\\locate", string.Concat("sounds\\", mapData.CountryNames[currentCountry].ToLower()));
                    break;
                case ProblemType.SpellCapitalVoice:
                    PlayVorbis(0, "common\\spell", string.Concat("sounds\\", mapData.CountryCapitals[currentCountry].ToLower()));
                    break;
                case ProblemType.SpellCountryVoice:
                    PlayVorbis(0, "common\\spell", string.Concat("sounds\\", mapData.CountryNames[currentCountry].ToLower()));
                    break;
            }
        }

        public void OnProblemInput(object data)
        {
            if (current == null)
            {
                return;
            }

            //Clear map from sending input
            control.SetProblem(ProblemType.None);

            //Test if correct, if correct, move on to next problem
            //If incorrect, show correct answer
            UpdateStatistics(data);
            if (current.IsCorrect(data))
            {
                //Correct!
                control.HighlightCountry(currentCountry, new Bgra(91, 166, 38, 255));

                //Update momentum and momentum rate
                UpdateMomentum(1);
                inputBox.SetComplete(true, 0);
            }
            else
            {
                switch (current.Type)
                {
                    case ProblemType.Locate:
                        control.HighlightCountry((int)current.Answer, new Bgra(24, 30, 217, 255));
                        break;
                    case ProblemType.SpellCountryVoice:
                    case ProblemType.SpellCountry:    
                        break;
                    case ProblemType.SpellCapitalVoice:
                    case ProblemType.SpellCapital:
                        break;
                }

                //Update momentum and momentum rate
                UpdateMomentum(-1);

                int timeStudySeconds = (int)Math.Round(10 * (1 - momentum));
                inputBox.SetComplete(false, timeStudySeconds);

                //TODO: Load help info
                helpControl.ShowHelp(currentCountry, current.Type);
            }  
        }

        public double ExpRandom(int a, int b, double rate)
        {
            double exprate = Math.Exp(-rate * a);
            return -Math.Log(exprate - random.NextDouble() * (exprate - Math.Exp(-rate * b))) / rate;
        }

        public int GetCountry()
        {
            Statistic[] testCopy = new Statistic[statistics.Length];
            Array.Copy(statistics, testCopy, statistics.Length);

            int[] countryKeys = new int[mapData.Countries];
            double[] ranking = new double[mapData.Countries];
            for (int i = 0; i < mapData.Countries; i++)
            {
                countryKeys[i] = i;
                ranking[i] = statistics[i].GetPercentLearned();
            }

            Array.Sort(ranking, countryKeys);
            int index = (int)ExpRandom(0, mapData.Countries - 1, momentum);
            return countryKeys[index];
        }

        public bool HasCapital(int country)
        {
            return !string.IsNullOrEmpty(mapData.CountryCapitals[country]);
        }
        
        public ProblemType GetProblem(int country)
        {
            Statistic s = statistics[country];
            TimeSpan lAge = s.GetLocationAge();

            //Sort by completion, then by accuracy
            List<int> types = new List<int>();
            for (int i = 0; i < problemTypes.Length; i++)
            {
                if ((problemTypes[i] == ProblemType.SpellCapital ||
                    problemTypes[i] == ProblemType.SpellCapitalVoice) &&
                    !s.HasCapital())
                {
                    continue;
                }
                else
                {
                    int type = 0;
                    switch (problemTypes[i])
                    {
                        case ProblemType.Locate:
                            type = 0;
                            break;
                        case ProblemType.SpellCountry:
                        case ProblemType.SpellCountryVoice:
                            type = 1;
                            break;
                        case ProblemType.SpellCapital:
                        case ProblemType.SpellCapitalVoice:
                            type = 2;
                            break;
                        default:
                            continue;
                        
                    }

                    if (!types.Contains(type))
                    {
                        types.Add(type);
                    }
                }
            }

            //int[] types = new int[Math.Min(s.HasCapital() ? 3 : 2, problemTypes.Length)];
            //for (int i = 0; i < problemTypes.Length; i++)
            //{
            //    types[i] = (int)problemTypes[i];
            //}

            double[] completions = s.GetCompletions();
            double[] accuracies = s.GetAccuracies();
            int[] problems = types.OrderBy(t => completions[t]).ThenBy(a => accuracies[a]).ToArray();
            switch (problems[0])
            {
                case 0:
                    return ProblemType.Locate;
                case 1:
                    {
                        if (s.GetCountrySuccess() < 0.5)
                        {
                            return ProblemType.SpellCountryVoice;
                        }
                        else
                        {
                            return ProblemType.SpellCountry;
                        }
                    }
                case 2:
                    {
                        if (s.GetCapitalSuccess() < 0.5)
                        {
                            return ProblemType.SpellCapitalVoice;
                        }
                        else
                        {
                            return ProblemType.SpellCapital;
                        }
                    }
                default:
                    throw new Exception("Take cover, the world just ended.");
            }
        }

        public void NextProblem() 
        {
            //Steps
            //- Locate { voice of country }
            //- Spell { voice of country } [highlight country]
            //- Spell { voice of capital } [highlight country]
            //- Label { highlight country } 
            control.StopHighlighting();
            helpControl.Clear();

            //Get country to work on
            currentCountry = GetCountry();

            //Pick problem
            ProblemType problem = GetProblem(currentCountry);
            if (inputBox != null)
            {
                inputBox.SetProblem(problem, mapData.CountryNames[currentCountry]);
            }

            control.SetProblem(problem);
            switch (problem)
            {
                case ProblemType.Locate:
                    current = new Problem(problem, currentCountry);
                    PlayVorbis(0, "common\\locate", string.Concat("sounds\\", mapData.CountryNames[currentCountry].ToLower()));
                    break;
                case ProblemType.SpellCountryVoice:
                    current = new Problem(problem, mapData.CountryNames[currentCountry]);      
                    control.HighlightCountry(currentCountry, new Bgra(91, 166, 38, 255));
                    PlayVorbis(0, "common\\spell", string.Concat("sounds\\", mapData.CountryNames[currentCountry].ToLower()));
                    break;
                case ProblemType.SpellCountry:
                    current = new Problem(problem, mapData.CountryNames[currentCountry]);
                    control.HighlightCountry(currentCountry, new Bgra(91, 166, 38, 255));
                    break;
                case ProblemType.SpellCapitalVoice:
                    current = new Problem(problem, mapData.CountryCapitals[currentCountry]);
                    control.HighlightCountry(currentCountry, new Bgra(24, 30, 217, 255));
                    PlayVorbis(0, "common\\spell", string.Concat("sounds\\", mapData.CountryCapitals[currentCountry].ToLower()));
                    break;
                case ProblemType.SpellCapital:
                    current = new Problem(problem, mapData.CountryCapitals[currentCountry]);
                    control.HighlightCountry(currentCountry, new Bgra(24, 30, 217, 255));
                    break;
            }
        }

        public void Start(params ProblemType[] types)
        {
            this.problemTypes = types;
            NextProblem();
        }

        public void Stop()
        {
            control.SetProblem(ProblemType.None);
            inputBox.SetProblem(ProblemType.None);
            helpControl.Clear();
        }

        public void PlayVorbis(int index, params string[] names)
        {
            if (soundInstances < 4 && index < names.Length)
            {
                string filename = Path.Combine(Environment.CurrentDirectory, string.Concat(names[index], ".ogg"));
                if (!File.Exists(filename))
                {
                    return;
                }

                var vorbisStream = new VorbisWaveReader(filename);
                var waveOut = new WaveOutEvent();

                Thread currentSound = new Thread(delegate ()
                {
                    soundHandle.Reset();
                    waveOut.Init(vorbisStream);
                    waveOut.Play();

                    SpinWait.SpinUntil(() => waveOut.PlaybackState == PlaybackState.Stopped);

                    waveOut.Dispose();
                    vorbisStream.Dispose();

                    if (index + 1 < names.Length)
                    {
                        PlayVorbis(index + 1, names);
                    }
                    else
                    {
                        soundHandle.Set();
                    }

                    soundInstances--;
                });

                currentSound.IsBackground = true;
                currentSound.Start();
                soundInstances++;
            }
        }

        public void Load(MapData mapData)
        {
            this.mapData = mapData;
            statistics = new Statistic[mapData.CountryNames.Length];
            for (int i = 0; i < statistics.Length; i++)
            {
                statistics[i] = new Statistic(mapData.CountryNames[i], mapData.CountryCapitals[i], i);
            }
        }

        public void SaveUserData()
        {
            string dir = Path.Combine(Environment.CurrentDirectory, "profile");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string filename = Path.Combine(dir, string.Format("{0}.json", mapData.Name));

            history.Add(DateTime.Now, statistics);     
            string stats = JsonConvert.SerializeObject(history);

            File.WriteAllText(filename, stats);
        }

        public void LoadUserData()
        {
            string dir = Path.Combine(Environment.CurrentDirectory, "profile");
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch
                {
                    return;
                }
            }

            string filename = Path.Combine(dir, string.Format("{0}.json", mapData.Name));
            if (File.Exists(filename))
            {
                string stats = File.ReadAllText(filename);
                history = JsonConvert.DeserializeObject<Dictionary<DateTime, Statistic[]>>(stats);

                //Get last
                DateTime curTime = DateTime.Now;

                DateTime? best = null;
                Statistic[] last = null;
                foreach(KeyValuePair<DateTime, Statistic[]> observation in history)
                {
                    if (!best.HasValue || curTime.Subtract(observation.Key) < curTime.Subtract(best.Value))
                    {
                        best = observation.Key;
                        last = observation.Value;
                    }
                }

                if (last != null)
                {
                    Statistic[] currentSession = new Statistic[last.Length];
                    for (int j = 0; j < currentSession.Length; j++)
                    {
                        currentSession[j] = last[j].Clone();
                    }

                    statistics = currentSession;
                }
                else
                {
                    history.Clear();
                }
            }
        }

        public void Dispose()
        {
            if (soundHandle != null)
            {
                soundHandle.Dispose();
                soundHandle = null;
            }
        }

        public Teacher(MapControl control, InputBox inputBox, HelpControl helpControl)
        {
            //http://www.thedailybeast.com/articles/2014/07/12/repetition-doesn-t-work-better-ways-to-train-your-memory.html
            this.control = control;
            this.inputBox = inputBox;
            this.helpControl = helpControl;

            inputBox.OnInput += OnInput;
            inputBox.OnNext += OnNext;
            inputBox.OnPlayAgain += OnPlayAgain;
            
            random = new Random();
            history = new Dictionary<DateTime, Statistic[]>();
            soundHandle = new ManualResetEvent(true);    
        }
    }
}
