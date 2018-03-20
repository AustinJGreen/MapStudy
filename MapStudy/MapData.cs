using System;

namespace MapStudy
{
    public class MapData
    {
        public string Name { get; set; }
        public int Countries { get; set; }
        public string File { get; set; }
        public string[] CountryNames { get; set; }
        public string[] CountryCapitals { get; set; }
        public Pt[][] CountryPoints { get; set; }
        public Type[] CountryTypes { get; set; }
        public int[] ZOrders { get; set; }

        public MapData(string name, int countries, string file, string[] countryNames, string[] countryCapitals, Pt[][] countryPoints, Type[] countryTypes, int[] zorders)
        {
            Name = name;
            Countries = countries;
            File = file;
            CountryNames = countryNames;
            CountryCapitals = countryCapitals;
            CountryPoints = countryPoints;
            CountryTypes = countryTypes;
            ZOrders = zorders;
        }
    }
}
