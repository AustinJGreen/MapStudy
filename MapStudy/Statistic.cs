using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace MapStudy
{
    [DebuggerDisplay("{countryName}")]
    public class Statistic
    {
        [JsonProperty]
        private string countryName;
        [JsonProperty]
        private string capitalName;
        [JsonProperty]
        private int locationIndex;

        [JsonProperty]
        private DateTime lastCountryAttempt;
        [JsonProperty]
        private DateTime lastCapitalAttempt;
        [JsonProperty]
        private DateTime lastLocationAttempt;

        [JsonProperty]
        private int countryAttempts;
        [JsonProperty]
        private int capitalAttempts;
        [JsonProperty]
        private int locationAttempts;

        [JsonProperty]
        private int countryCorrect;
        [JsonProperty]
        private int capitalCorrect;
        [JsonProperty]
        private int locationCorrect;

        [JsonIgnore]
        public string CountryName => countryName;

        [JsonIgnore]
        public string CapitalName => capitalName;

        [JsonIgnore]
        public int LocationIndex => locationIndex;

        public void ProcessCountrySpelling(string spelling)
        {
            countryAttempts++;
            if (string.Compare(spelling, countryName, true) == 0)
            {
                countryCorrect++;
            }

            lastCountryAttempt = DateTime.Now;
        }

        public void ProcessCapitalSpelling(string spelling)
        {
            capitalAttempts++;
            if (string.Compare(spelling, capitalName, true) == 0)
            {
                capitalCorrect++;
            }

            lastCapitalAttempt = DateTime.Now;
        }

        public void ProcessLocation(int locationIndex)
        {
            locationAttempts++;
            if (this.locationIndex == locationIndex)
            {
                locationCorrect++;
            }

            lastLocationAttempt = DateTime.Now;
        }

        public float GetCountrySuccess()
        {
            if (countryAttempts == 0)
            {
                return 0f;
            }

            return countryCorrect / (float)countryAttempts;
        }

        public float GetCapitalSuccess()
        {
            if (capitalAttempts == 0)
            {
                return 0f;
            }

            return capitalCorrect / (float)capitalAttempts;
        }

        public float GetLocationSuccess()
        {
            if (locationAttempts == 0)
            {
                return 0f;
            }

            return locationCorrect / (float)locationAttempts;
        }

        public TimeSpan GetCountryAge()
        {
            return DateTime.Now.Subtract(lastCountryAttempt);
        }

        public TimeSpan GetCapitalAge()
        {
            return DateTime.Now.Subtract(lastCapitalAttempt);
        }

        public TimeSpan GetLocationAge()
        {
            return DateTime.Now.Subtract(lastLocationAttempt);
        }

        public bool HasCapital()
        {
            return !string.IsNullOrEmpty(capitalName);
        }

        public int GetStatCount()
        {
            return string.IsNullOrEmpty(capitalName) ? 2 : 3;
        }

        public float GetAccuracyGradient()
        {
            int items = string.IsNullOrEmpty(capitalName) ? 2 : 3;
            return (GetCountrySuccess() + GetCapitalSuccess() + GetLocationSuccess()) / items;
        }

        public double GetAge()
        {
            double lAge = GetLocationAge().TotalMinutes;
            double caAge = GetLocationAge().TotalMinutes;
            double coAge = GetLocationAge().TotalMinutes;

            return Math.Max(lAge, Math.Max(coAge, caAge));
        }

        public double Attempts()
        {
            int items = string.IsNullOrEmpty(capitalName) ? 2 : 3;
            return (locationAttempts + capitalAttempts + locationAttempts) / items;
        }

        private double GetCompletion(int correct, int attempts, int repCount = 15)
        {
            return Math.Min(1, attempts == 0 ? 0 : Math.Log(correct + 1, repCount + 1));
        }

        public double GetLocationCompletion()
        {
            return GetCompletion(locationCorrect, locationAttempts);
        }

        public double GetCountryCompletion()
        {
            return GetCompletion(countryCorrect, countryAttempts);
        }

        public double GetCapitalCompletion()
        {
            return GetCompletion(capitalCorrect, capitalAttempts);
        }

        public double[] GetCompletions()
        {
            if (HasCapital())
            {
                return new double[3]
                {
                    GetLocationCompletion(),
                    GetCountryCompletion(),
                    GetCapitalCompletion()
                };
            }
            else
            {
                return new double[2]
                {
                    GetLocationCompletion(),
                    GetCountryCompletion()
                };
            }
        }

        public double[] GetAccuracies()
        {
            if (HasCapital())
            {
                return new double[3]
                {
                    GetLocationSuccess(),
                    GetCountrySuccess(),
                    GetCapitalSuccess()
                };
            }
            else
            {
                return new double[2]
                {
                    GetLocationSuccess(),
                    GetCountrySuccess()
                };
            }
        }

        public double GetPercentLearned()
        {
            float a = GetAccuracyGradient();

            double coL = GetCountryCompletion();
            double caL = GetCapitalCompletion();
            double loL = GetLocationCompletion();

            double caWeight = string.IsNullOrEmpty(capitalName) ? 0 : 0.45;
            double coWeight = string.IsNullOrEmpty(capitalName) ? 0.60 : 0.35;
            double loWeight = string.IsNullOrEmpty(capitalName) ? 0.40 : 0.2;
            double learntWeightedAvg = (coL * coWeight) + (caL * caWeight) + (loL * loWeight);
            return (a * 0.75) + (learntWeightedAvg * 0.25);
        }

        public Grade GetLetterGrade()
        {
            double c = GetPercentLearned();
            double[] brackets = new double[] { 0.9, 0.8, 0.7, 0.6, 0 };

            int b = 0;
            for (int i  = 0; i < brackets.Length; i++)
            {
                if (c > brackets[i])
                {
                    b = i;
                    break;
                }
            }

            Grade[] grades = (Grade[])Enum.GetValues(typeof(Grade));
            return grades[b];
        }

        public override bool Equals(object obj)
        {
            if (obj is Statistic)
            {
                Statistic other = (Statistic)obj;
                return countryName.Equals(other.countryName) &&
                       capitalName.Equals(other.capitalName);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Statistic Clone()
        {
            Statistic clone = new Statistic(countryName, capitalName, locationIndex);
            clone.lastCountryAttempt = new DateTime(lastCountryAttempt.Ticks);
            clone.lastCapitalAttempt = new DateTime(lastCapitalAttempt.Ticks);
            clone.lastLocationAttempt = new DateTime(lastLocationAttempt.Ticks);

            clone.countryAttempts = countryAttempts;
            clone.capitalAttempts = capitalAttempts;
            clone.locationAttempts = locationAttempts;

            clone.countryCorrect = countryCorrect;
            clone.capitalCorrect = capitalCorrect;
            clone.locationCorrect = locationCorrect;
            return clone;
        }

        public Statistic(string countryName, string capitalName, int locationIndex)
        {
            this.countryName = countryName;
            this.capitalName = capitalName;
            this.locationIndex = locationIndex;
        }
    }
}
