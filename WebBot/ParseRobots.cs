using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace HansWebCrawler
{
    public static class ParseRobots
    {

        static public bool GetRobotsMetaData(string address)
        {
            var req = WebRequest.Create(address);
            var webResposne = req.GetResponse();
            var webStream = webResposne.GetResponseStream();
            StreamReader webReader = new StreamReader(webStream);

            while (!webReader.EndOfStream)
            {
                var data = webReader.ReadLine().ToLower();
                var metaRobots = Regex.Match(data, "<meta\\sname=\\\"robots\\\"\\scontent=\\\"([a-z,]*)\\\">");
                if (metaRobots.Success)
                {
                    var metaRobotsContent = metaRobots.Groups[1].Value;
                    if (metaRobotsContent.Contains("nofollow") || metaRobotsContent.Contains("noindex"))
                        return false;
                }
            }
            return true;
        }

        static public List<string> GetRobotsFile(string address)
        {
            try
            {
                List<string> disallowPlaces = new List<string>();
                var userFilter = "user-agent:";
                var allowFilter = "allow:";
                var disallowFilter = "disallow:";

                var req = WebRequest.Create(address + "/robots.txt");
                var webStream = req.GetResponse().GetResponseStream();
                StreamReader webReader = new StreamReader(webStream);
                while (!webReader.EndOfStream)
                {
                    var data = webReader.ReadLine().ToLower();
                    if (data.StartsWith(userFilter))
                    {
                        var user = data.Substring(userFilter.Length).Trim();
                        if (user == "*")
                            while (!string.IsNullOrEmpty(data))
                            {
                                data = webReader.ReadLine().ToLower();
                                if (data.StartsWith(disallowFilter))
                                {
                                    var newAddress = data.Substring(disallowFilter.Length).Trim();
                                    if (!newAddress.StartsWith(address))
                                        newAddress = address + newAddress;
                                    disallowPlaces.Add(newAddress);
                                }
                                else
                                {
                                    if (data.StartsWith(allowFilter))
                                        continue;
                                    else
                                        break;
                                }
                            }
                    }
                }
                return disallowPlaces;
            }
            catch (UriFormatException err)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Provided bad address");
                Console.ResetColor();
                throw new Exception(err.Message, err);
            }
        }
    }
}
