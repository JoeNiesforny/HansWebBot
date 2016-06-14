using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
        //public static List<string> BannedSite; // useless for "http://bg.pg.edu.pl"

        static int _WorkingThreadsCount = 0;
        static int _MaxThread = 100;
        static int _RequestTimeout = 200000;

        public WebMinner(string address, int timeout = 200000)
        {
            StartAddress = address;
            Database = new WebDataBase(address);
            _RequestTimeout = timeout;
        }

        public void Mining(int dept, int threadLimit = 100)
        {
            LostSiteCount = 0;
            ThreadCount = 0;
            var iteration = 0;
            OutputConsole = "";
            _MaxThread = threadLimit;
            //BannedSite = ParseRobots.GetRobotsFile(StartAddress); // useless for "http://bg.pg.edu.pl"
            Mining(StartAddress, -1, iteration, dept, Database);
        }

        private static void Mining(string address, int parentId, int iteration, int dept, WebDataBase database)
        {
            ThreadCount++;
            _WorkingThreadsCount++;
            var data = GetDataFromSite(address);
            if (data == "")
            {
                _WorkingThreadsCount--;
                return;
            }

            var title = GetSiteTitleFromData(data);
            var newAddresses = GetAllAddressesFromData(data, address);
            parentId = database.AddNewRow(address, title, newAddresses, parentId);
            database.CheckAddressAsVisited(address);

            if (++iteration > dept)
            {
                _WorkingThreadsCount--;
                return;
            }

            var threads = new List<Thread>();
            foreach (var newAddress in newAddresses)
            {
                if (!newAddress.Contains(StartAddress))
                    continue;

                //foreach (var site in BannedSite) // useless for "http://bg.pg.edu.pl"
                //    if (newAddress.StartsWith(site))
                //        continue;

                while (_WorkingThreadsCount > _MaxThread); // wait for lowering the limit

                var thread = new Thread(() => Mining(newAddress, parentId, iteration, dept, database));
                thread.Start();
                threads.Add(thread);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            _WorkingThreadsCount--;
        }

        private static string GetSiteTitleFromData(string data)
        {
            var match = Regex.Match(data, "<title>(.*)</title>");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return "";
        }

        private static string GetSiteReadMeFromData(string data)
        {
            var match = Regex.Match(data, "<title>(.*)</title>");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return "";
        }

        private static List<string> GetAllAddressesFromData(string data, string address = "")
        {
            List<string> addresses = new List<string>();
            var match = Regex.Match(data, "<a href=\\\"([^mailto#][-A-Za-z0-9,.:;?@_~/&]*[^pdf])\\\"");
            while (match.Success)
            {
                var value = match.Groups[1].Value;
                if (!value.StartsWith("http"))
                    value = address + value;
                addresses.Add(value);
                match = match.NextMatch();
            }
            return addresses;
        }

        private static string GetHeaderFromSite(string address)
        {
            var req = WebRequest.Create(address);
            req.UseDefaultCredentials = true;
            var webResposne = req.GetResponse();
            return webResposne.Headers.ToString();
        }

        private static string GetDataFromSite(string address)
        {
            var req = WebRequest.Create(address);
            req.Timeout = _RequestTimeout;
            req.UseDefaultCredentials = true;
            try
            {
                var webResposne = req.GetResponse();
                var webStream = webResposne.GetResponseStream();
                StreamReader webReader = new StreamReader(webStream);
                return webReader.ReadToEnd();
            }
            catch (WebException err)
            {
                LostSiteCount++;
                OutputConsole += "Couldn't get resposne from " + address + " with error: " + err.Message + "\r\n";
                return "";
            }
        }
    }
}
