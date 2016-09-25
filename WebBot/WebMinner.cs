using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Threading;

namespace HansWebCrawler
{
    public class WebMinner
    {
        public WebDataBase Database;
        public static string StartAddress;
        public static int ThreadCount = 0;
        public static int LostSiteCount = 0;
        public static string OutputConsole;
        public static List<string> BannedSite; // robots

        static List<Regex> _Filters = new List<Regex>()
        {
            new Regex("/documents/"),
                new Regex("\\.pdf.*"),
                new Regex("\\.doc.*"),
                new Regex("\\.docx.*"),
                new Regex("\\.ico.*"),
                new Regex("\\.gif"),
        };
        static int _WorkingThreadsCount = 0;
        static Mutex _WorkingThreadMutex = new Mutex();
        static int _MaxThread = 200;
        static int _RequestTimeout = 0;
        static int _Dept = 1;
        static WebDataBase _Database;
        static List<string> _TakenSite;

        public WebMinner(string address, int timeout = 200000)
        {
            StartAddress = address;
            Database = new WebDataBase(address);
            _RequestTimeout = timeout;
        }

        public void Mining(int dept, int threadLimit = 100)
        {
            _TakenSite = new List<string>() { StartAddress };
            LostSiteCount = 0;
            ThreadCount = 0;
            var iteration = 0;
            OutputConsole = "";
            _MaxThread = threadLimit;
            _WorkingThreadsCount = 0;
            _Dept = dept;
            _Database = Database;
            BannedSite = ParseRobots.GetRobotsFile(StartAddress); // robots
            Mining(StartAddress, -1, iteration);
            while (_WorkingThreadsCount != 0)
                Thread.Sleep(100);
        }

        private static void Mining(string address, int parentId, int iteration)
        {
            ThreadCount++;
            _WorkingThreadsCount++;
            var data = GetDataFromSite(address);
            if (data == "")
            {
                _WorkingThreadsCount--;
                return;
            }
            _Database.MarkAddressAsVisited(address);
            var newAddresses = GetAllAddressesWithContentFromData(data, address);
            parentId = _Database.AddNewRowAddressAndContent(address, GetSiteTitleFromData(data), newAddresses, parentId);
            if (++iteration > _Dept)
            {
                _WorkingThreadsCount--;
                return;
            }
            foreach (var newAddress in newAddresses)
            {
                if (!newAddress.Contains(StartAddress))
                    continue;
                if (FilterSite(newAddress))
                    continue;
                _WorkingThreadMutex.WaitOne();
                if (_TakenSite.Contains(newAddress))
                {
                    _WorkingThreadMutex.ReleaseMutex();
                    continue;
                }
                _TakenSite.Add(newAddress);
                while (_WorkingThreadsCount > _MaxThread) ; // wait for lowering the limit
                new Thread(() => Mining(newAddress, parentId, iteration)).Start();
                _WorkingThreadMutex.ReleaseMutex();
            }
            SaveDataToFile(address, data);
            data = null;
            _WorkingThreadsCount--;
        }

        private static bool FilterSite(string address)
        {
            foreach (var filter in _Filters) // filter for documents
                if (filter.IsMatch(address))
                    return true;
            foreach (var banned in BannedSite) //robots
                if (address.Contains(banned))
                    return true;
            return false;
        }

        private static string GetSiteTitleFromData(string data)
        {
            var match = Regex.Match(data, "<title>(.*)</title>");
            if (match.Success)
                return match.Groups[1].Value;
            return "";
        }

        private static List<string> GetAllAddressesFromData(string data, string address = "")
        {
            List<string> addresses = new List<string>();
            var match = Regex.Match(data, "<a href=\\\"([^mailto#][-A-Za-z0-9,.:;?@_~/&]*)\\\"");
            while (match.Success)
            {
                var value = match.Groups[1].Value;
                if (!value.StartsWith("http"))
                    value = StartAddress + value;
                addresses.Add(value);
                match = match.NextMatch();
            }
            return addresses;
        }

        private static List<string> GetAllAddressesWithContentFromData(string data, string address = "")
        {
            List<string> addresses = new List<string>();
            var match = Regex.Match(data, "<a href=\"([^mailto#][-A-Za-z0-9,.:;?@_~/&]*)\".*?(/>|</a>)");
            while (match.Success)
            {
                var value = match.Groups[1].Value;
                if (!value.StartsWith("http"))
                {
                    if (value.IndexOf('/') < 1)
                        value = value.Remove(0, 1);
                    value = StartAddress + value;
                }
                addresses.Add(value);
                var match2 = Regex.Match(match.Value, "title=\"(.*?)\"");
                if (match2.Success)
                    addresses.Add(match2.Groups[1].Value);
                else
                    addresses.Add("");
                match2 = Regex.Match(match.Value, ">(.*?)</a>");
                if (match2.Success)
                    addresses.Add(match2.Groups[1].Value);
                else
                    addresses.Add("");
                match = match.NextMatch();
            }
            return addresses;
        }

        private static string GetDataFromSite(string address)
        {
            var req = WebRequest.Create(address);
            req.Timeout = _RequestTimeout;
            req.UseDefaultCredentials = true;
            try
            {
                using (var webResposne = req.GetResponse())
                    using (var webStream = webResposne.GetResponseStream())
                        using (StreamReader webReader = new StreamReader(webStream))
                            return webReader.ReadToEnd();
            }
            catch (WebException err)
            {
                LostSiteCount++;
                OutputConsole += "Couldn't get resposne from " + address + " with error: " + err.Message + "\r\n";
                return "";
            }
        }

        private static void SaveDataToFile(string address, string data)
        {
            var limitLength = Directory.GetCurrentDirectory().Length;
            string newPath = address.Substring(7);
            // For Windows filesystem there is a limit for a number of characters in path name to 248.
            if (newPath.Length > (247 - limitLength))
                newPath = newPath.Substring(0, 246 - limitLength);
            foreach (var illegalCharacter in Path.GetInvalidFileNameChars())
                    newPath.Replace(illegalCharacter, ';');
            Directory.CreateDirectory(newPath);
            // ToDo - compute data or clean it from trash and leave only text.
            data = Regex.Replace(data, "(<(.*?)<body (.*?)>)", "");
            data = Regex.Replace(data, "(<script.*?</script>)", "");
            data = Regex.Replace(data, "(<br>)", "\n");
            data = Regex.Replace(data, "(<.*?>)", "");
            data = data.Replace("&nbsp;", " ");
            data = Regex.Replace(data, "( {2,})", "\n");
            File.WriteAllLines(newPath + "/txt", new string[] { data });
        }
    }
}
