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
        public static string outputConsole;

        public WebMinner(string address)
        {
            StartAddress = address;
            Database = new WebDataBase(address);
        }

        // one thread action
        public void Mining(int dept)
        {
            LostSiteCount = 0;
            ThreadCount = 0;
            var iteration = 0;
            Mining(StartAddress, -1, iteration, dept);
        }
        // one thread action
        private void Mining(string address, int parentId, int iteration, int dept)
        {
            var data = GetDataFromSite(address);
            if (data == "")
                return;

            var title = GetSiteTitleFromData(data);
            var newAddresses = GetAllAddressesFromData(data, address);
            parentId = Database.AddNewRow(address, title, newAddresses, parentId);

            if (++iteration > dept)
                return;

            foreach (var newAddress in newAddresses)
            {
                if (!newAddress.Contains(StartAddress))
                    continue;
                Mining(newAddress, parentId, iteration, dept);
            }
        }

        public void MiningMultiThread(int dept)
        {
            LostSiteCount = 0;
            ThreadCount = 0;
            var iteration = 0;
            MiningMultiThread(StartAddress, -1, iteration, dept, Database);
        }

        private static void MiningMultiThread(string address, int parentId, int iteration, int dept, WebDataBase dataBase)
        {
            ThreadCount++;
            var data = GetDataFromSite(address);
            if (data == "")
                return;

            var title = GetSiteTitleFromData(data);
            var newAddresses = GetAllAddressesFromData(data, address);
            parentId = dataBase.AddNewRow(address, title, newAddresses, parentId);

            if (++iteration > dept)
                return;

            var threads = new List<Thread>();
            foreach (var newAddress in newAddresses)
            {
                if (!newAddress.Contains(StartAddress))
                    continue;
                var thread = new Thread(() => MiningMultiThread(newAddress, parentId, iteration, dept, dataBase));
                thread.Start();
                threads.Add(thread);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
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
            req.Timeout = 20000;
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
                outputConsole += "Couldn't get resposne from " + address + " with error: " + err.Message + "\r\n";
                return "";
            }
        }
    }
}
