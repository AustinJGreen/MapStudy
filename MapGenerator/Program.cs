using HtmlAgilityPack;
using MapStudy;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MapStudyResources
{
    class Program
    {
        static bool GetAudioCambridge(string word, string dir)
        {
            string url = string.Format("https://dictionary.cambridge.org/us/search/english/direct/?q={0}", HttpUtility.UrlEncode(word.ToLower()));
            HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(url);
            wreq.Method = "GET";
            wreq.Host = "dictionary.cambridge.org";
            wreq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.73 Safari/537.36";
            wreq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            wreq.Headers["Accept-Language"] = "en-US,en;q=0.5";
            wreq.Headers["Accept-Encoding"] = "gzip, deflate, br";
            wreq.AutomaticDecompression = DecompressionMethods.GZip;

            string rhtml = null;

            try
            {
                using (HttpWebResponse wres = (HttpWebResponse)wreq.GetResponse())
                using (StreamReader rdr = new StreamReader(wres.GetResponseStream()))
                {
                    rhtml = rdr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return false;
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(rhtml);

            var node = document.DocumentNode;

            var voiceNode = node.SelectSingleNode("//*[@id=\"entryContent\"]/div[3]/div[3]/div[1]/div/div[2]/div/div/div/div[1]");
            if (voiceNode == null)
            {
                return false;
            }

            if (voiceNode.ChildNodes.Count(t => t.Name == "span") == 0)
            {
                return false;
            }

            var pronounciation = voiceNode.ChildNodes.Last(t => t.Name == "span");
            var voice = pronounciation.ChildNodes[0];
            if (voice.ChildNodes.Count < 3)
            {
                return false;
            }

            var voices = voice.ChildNodes[2];

            string oggFile = voices.GetAttributeValue("data-src-ogg", null);
            if (!string.IsNullOrWhiteSpace(oggFile))
            {
                string filename = Path.Combine(dir, string.Concat(word.ToLower(), ".ogg"));
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(oggFile, filename);
                }

                return true;
            }

            return false;
        }

        static bool GetAudioDictionaryCom(string word, string dir)
        {
            string url = string.Format("http://www.dictionary.com/browse/{0}", string.Join("-", word.ToLower().Split(' ')));
            HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(url);
            wreq.Method = "GET";
            wreq.Host = "www.dictionary.com";
            wreq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.73 Safari/537.36";
            wreq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            wreq.Headers["Accept-Language"] = "en-US,en;q=0.5";
            wreq.Headers["Accept-Encoding"] = "gzip, deflate, br";
            wreq.AutomaticDecompression = DecompressionMethods.GZip;

            string rhtml = null;

            try
            {
                using (HttpWebResponse wres = (HttpWebResponse)wreq.GetResponse())
                using (StreamReader rdr = new StreamReader(wres.GetResponseStream()))
                {
                    rhtml = rdr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return false;
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(rhtml);

            var node = document.DocumentNode;

            var audio = node.SelectSingleNode("//*[@id=\"source-luna\"]/div[1]/section/header/div[1]/div/audio");
            if (audio == null)
            {
                return false;
            }
            var audioSources = audio.ChildNodes[1];
            string oggFile = audioSources.GetAttributeValue("src", null);
            if (!string.IsNullOrWhiteSpace(oggFile))
            {
                using (WebClient client = new WebClient())
                {
                    string filename = Path.Combine(dir, string.Concat(word.ToLower(), ".ogg"));
                    client.DownloadFile(oggFile, filename);
                }

                return true;
            }

            return false;
        }

        static void GetAudio(string dir, string[] words)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            for (int i = 0; i < words.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(words[i]))
                {
                    string filename = Path.Combine(dir, string.Concat(words[i].ToLower(), ".ogg"));
                    if (!File.Exists(filename))
                    {
                        if (!GetAudioCambridge(words[i], dir))
                        {
                            if (!GetAudioDictionaryCom(words[i], dir))
                            {
                                Console.WriteLine("Couldn't get {0}", words[i]);
                            }
                        }
                    }
                }
            }
        }
        
        static void SaveMapData(MapData mapData)
        {

        }

        static void Main(string[] args)
        {
            Random rnd = new Random();

            string[] countries = new string[26]
            {
                "Mexico",
                "Guatemala",
                "Belize",
                "El Salvador",
                "Honduras",
                "Costa Rica",
                "Nicaragua",
                "Panama",
                "Cuba",
                "Jamaica",
                "Haiti",
                "Dominican Republic",
                "Colombia",
                "Venezuela",
                "Ecuador",
                "Guyana",
                "Suriname",
                "French Guiana",
                "Brazil",
                "Peru",
                "Bolivia",
                "Paraguay",
                "Argentina",
                "Uruguay",
                "Chile",
                "Bahamas"
            };

            string[] capitals = new string[26]
            {
                "Mexico City",
                "Guatemala City",
                "Belmopan",
                "San Salvador",
                "Tegucigalpa",
                "San Jose",
                "Managua",
                "Panama City",
                "Havana",
                "Kingston",
                "Port-au-Prince",
                "Santo Domingo",
                "Bogota",
                "Caracas",
                "Quito",
                "Georgetown",
                "Paramaribo",
                "Cayenne",
                "Brasilia",
                "Lima",
                "La Paz",
                "Asuncion",
                "Buenos Aires",
                "Montevideo",
                "Santiago",
                "Nassau"
            };

            Pt[][] pts = new Pt[26][]
            {
                new Pt[] { new Pt(351, 341) },
                new Pt[] { new Pt(705, 620) },
                new Pt[] { new Pt(756, 554) },
                new Pt[] { new Pt(752, 671) },
                new Pt[] { new Pt(812, 638) },
                new Pt[] { new Pt(899, 773) },
                new Pt[] { new Pt(875, 689) },
                new Pt[] { new Pt(1010, 824), new Pt(1109, 812) },
                new Pt[] { new Pt(1028, 407) },
                new Pt[] { new Pt(1100, 521) },
                new Pt[] { new Pt(1228, 511) },
                new Pt[] { new Pt(1295, 482) },
                new Pt[] { new Pt(1247, 941) },
                new Pt[] { new Pt(1535, 818) },
                new Pt[] { new Pt(1100, 1115) },
                new Pt[] { new Pt(1760, 884) },
                new Pt[] { new Pt(1868, 899) },
                new Pt[] { new Pt(1961, 917) },
                new Pt[] { new Pt(2030, 1355) },
                new Pt[] { new Pt(1226, 1376) },
                new Pt[] { new Pt(1571, 1553) },
                new Pt[] { new Pt(1739, 1724) },
                new Pt[] { new Pt(1610, 1973) },
                new Pt[] { new Pt(1853, 2055) },
                new Pt[] { new Pt(1445, 1762) },
                new Pt[] { new Pt(1097, 308) },
            };

            Type[] types = new Type[26]
            {
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(EllipseGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(FillGraphics),
                typeof(EllipseGraphics)
            };

            int[] zorders = new int[26];

            MapData africa = new MapData("South America", 26, "southamerica.jpg", countries, capitals, pts, types, zorders);

            //GetAudio(@"C:\Users\austi\Desktop\southamerica\", capitals);

            File.WriteAllText(@"C:\Users\austi\Desktop\southamerica.map", JsonConvert.SerializeObject(africa));
            //Console.ReadKey();
        }
    }
}
