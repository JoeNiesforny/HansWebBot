using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using HansWebCrawler;

namespace HansWebCrawlerTests
{
    [TestClass]
    public class ParseRobotsTest
    {
        [TestMethod]
        public void ParsingRobotsFile_ResultRobotsAllowGoEveryWhere()
        {
            Assert.AreEqual(0, ParseRobots.GetRobotsFile("http://www.meteo.pl/").Count);
        }

        [TestMethod]
        public void ParsingRobotsFile_ResultRobotsCantLookIntoAtLeastOne()
        {
            Assert.AreNotEqual(0, ParseRobots.GetRobotsFile("https://apps.google.com/").Count);
        }

        [TestMethod]
        public void ParsingRobotsFileWikipedia_CheckIfListWithDisallowIsProper()
        {
            List<string> disallowList = new List<string>()
            {
                "https://pl.wikipedia.org/w/",
                "https://pl.wikipedia.org/api/",
                "https://pl.wikipedia.org/trap/"
            };
            CollectionAssert.AreEqual(disallowList, ParseRobots.GetRobotsFile("https://pl.wikipedia.org"));
        }

        [TestMethod]
        public void ParsingRobotsMetaData_ResultTrue()
        {
            Assert.IsTrue(ParseRobots.GetRobotsMetaData("https://www.wp.pl/"));
        }

        // Couldn't find any address for negative result
        //[TestMethod]
        //public void ParsingRobotsMetaData_ResultFalse()
        //{
        //    Assert.IsFalse(ParseRobots.GetRobotsMetaData("https://www.wp.pl/"));
        //}
    }
}
