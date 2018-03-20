using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace MapStudy
{
    public static class Wikipedia
    {
        public static async Task<string> GetCountryInfo(string country)
        {
            string url = string.Format("https://en.wikipedia.org/wiki/geography_of_{0}", string.Join("_", country.Split(' ')));
            HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(url);
            wreq.Method = "GET";
            wreq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
            wreq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            wreq.Headers["Accept-Encoding"] = "gzip";
            wreq.Headers["Accept-Language"] = "en-US,en;q=0.8";
            wreq.AutomaticDecompression = DecompressionMethods.GZip;

            string rhtml = null;

            try
            {
                using (HttpWebResponse wres = (HttpWebResponse)await wreq.GetResponseAsync())
                using (StreamReader rdr = new StreamReader(wres.GetResponseStream()))
                {
                    rhtml = await rdr.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Info");
                return null;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(rhtml);
            var docNode = doc.DocumentNode;

            StringBuilder info = new StringBuilder();
            var pronounciation = docNode.SelectSingleNode("//*[@id=\"mw-content-text\"]");
            if (pronounciation != null)
            {
                for (int i = 0; i < pronounciation.ChildNodes.Count; i++)
                {
                    if (pronounciation.ChildNodes[i].Name == "p")
                    {
                        string[] lines = Regex.Unescape(pronounciation.ChildNodes[i].InnerText).Split('.', '?', '!');
                        for (int j = 0; j < lines.Length; j++)
                        {
                            string lower = HttpUtility.HtmlDecode(lines[j].Replace("&#160;", " ")).ToLower().Trim();
                            if (lower.StartsWith(country.ToLower()) || lower.StartsWith("it"))
                            {
                                info.Append(HttpUtility.HtmlDecode(Regex.Unescape(lines[j])).Trim());
                                if (!lines[j].EndsWith("."))
                                {
                                    info.Append('.');
                                }

                                info.Append(' ');
                            }
                        }
                    }
                }
            }

            string all = info.ToString();
            return info.ToString();
        }

        public static async Task<string> GetCapitalInfo(string capital)
        {
            string url = string.Format("https://en.wikipedia.org/wiki/{0}", string.Join("_", capital.Split(' ')));
            HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(url);
            wreq.Method = "GET";
            wreq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
            wreq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            wreq.Headers["Accept-Encoding"] = "gzip";
            wreq.Headers["Accept-Language"] = "en-US,en;q=0.8";
            wreq.AutomaticDecompression = DecompressionMethods.GZip;

            string rhtml = null;

            try
            {
                using (HttpWebResponse wres = (HttpWebResponse)await wreq.GetResponseAsync())
                using (StreamReader rdr = new StreamReader(wres.GetResponseStream()))
                {
                    rhtml = await rdr.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Info");
                return null;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(rhtml);
            var docNode = doc.DocumentNode;

            StringBuilder info = new StringBuilder();
            var pronounciation = docNode.SelectSingleNode("//*[@id=\"mw-content-text\"]");
            if (pronounciation != null)
            {
                for (int i = 0; i < pronounciation.ChildNodes.Count; i++)
                {
                    if (pronounciation.ChildNodes[i].Name == "p")
                    {
                        string[] lines = Regex.Unescape(pronounciation.ChildNodes[i].InnerText).Split('.', '?', '!');
                        for (int j = 0; j < lines.Length; j++)
                        {
                            string lower = HttpUtility.HtmlDecode(lines[j].Replace("&#160;", " ")).ToLower().Trim();
                            if (lower.StartsWith(capital.ToLower()) || lower.StartsWith("it"))
                            {
                                info.Append(HttpUtility.HtmlDecode(Regex.Unescape(lines[j])).Trim());
                                if (!lines[j].EndsWith("."))
                                {
                                    info.Append('.');
                                }

                                info.Append(' ');
                            }
                        }
                    }
                }
            }

            string all = info.ToString();
            return info.ToString();
        }
    }
}
