using HansWebCrawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace HansWebCrawlerTests
{
    [TestClass]
    public class WebDatabaseTests
    {
        [TestMethod]
        public void AddingNewRowToDatabase_CheckIfContentIsUnderDesireAddress()
        {
            var newContent = "Mój Contekst";
            var dataBase = new WebDataBase("joe.pl");
            dataBase.AddNewRow("joe.pl/index", newContent);
            var getContent = dataBase.GetTitleFromAddress("joe.pl/index");
            Assert.AreEqual(newContent, getContent);
        }

        [TestMethod]
        public void AddOneParentOneChildToDatabase_CheckIfChildKnowParent()
        {
            var newContent = "Mój Contekst";
            var dataBase = new WebDataBase("joe.pl");
            var id = dataBase.AddNewRow("joe.pl/index", newContent);
            dataBase.AddNewRow("joe.pl/index/nowy", newContent, id);
            var getParentAddress = dataBase.GetParentAddress("joe.pl/index/nowy");
            Assert.AreEqual("joe.pl/index", getParentAddress);
        }

        [TestMethod]
        public void AddOneParentThreChildrenToDatabase_CheckIfChildrenKnowsParent()
        {
            var newContent = "Mój Contekst";
            var dataBase = new WebDataBase("joe.pl");
            var id = dataBase.AddNewRow("joe.pl/index", newContent);
            dataBase.AddNewRow("joe.pl/index/nowy1", newContent, id);
            dataBase.AddNewRow("joe.pl/index/nowy2", newContent, id);
            dataBase.AddNewRow("joe.pl/index/nowy3", newContent, id);
            var getParentAddress = dataBase.GetParentAddress("joe.pl/index/nowy1");
            Assert.AreEqual("joe.pl/index", getParentAddress);
            getParentAddress = dataBase.GetParentAddress("joe.pl/index/nowy2");
            Assert.AreEqual("joe.pl/index", getParentAddress);
            getParentAddress = dataBase.GetParentAddress("joe.pl/index/nowy3");
            Assert.AreEqual("joe.pl/index", getParentAddress);
        }

        [TestMethod]
        public void AddOneRowToDatabase_CheckIfThereIsNoChild()
        {
            var newContent = "Mój Contekst";
            var dataBase = new WebDataBase("joe.pl");
            var id = dataBase.AddNewRow("joe.pl/index", newContent);
            var getParentAddress = dataBase.GetParentAddress("joe.pl/index");
            Assert.AreEqual("", getParentAddress);
        }

        [TestMethod]
        public void AddOneParentAndTwoChildrenToDatabase_CheckIfChildConnectedToParentAreProper()
        {
            var childrenList = new List<string> { "joe.pl/index/a","joe.pl/index/b" };
            var newContent = "Mój Contekst";
            var dataBase = new WebDataBase("joe.pl");
            var id = dataBase.AddNewRow("joe.pl/index", newContent);
            foreach (var child in childrenList)
                dataBase.AddNewRow(child, newContent, id);
            var getChildrenAddressesResult = dataBase.GetChildrenAddresses("joe.pl/index");
            CollectionAssert.AreEqual(childrenList, getChildrenAddressesResult);
        }

        [TestMethod]
        public void AddOneParentWithListOfChildrenAddressesToDatabase_CheckIfAddressesAreProperlyAdded()
        {
            var childrenList = new List<string> { "joe.pl/index/a", "joe.pl/index/b", "joe.pl/index/c" };
            var title = "Mój Contekst";
            var dataBase = new WebDataBase("joe.pl");
            var id = dataBase.AddNewRow("joe.pl/index", title, childrenList);
            var getChildrenAddressesResult = dataBase.GetChildrenAddresses("joe.pl/index");
            CollectionAssert.AreEqual(childrenList, getChildrenAddressesResult);
        }

        [TestMethod]
        public void AddOneParentWithListOfChildrenAddressesToDatabase_ClearTables_CheckIfTablesAreEmpty()
        {
            var childrenList = new List<string> { "joe.pl/index/a", "joe.pl/index/b", "joe.pl/index/c" };
            var title = "Mój Contekst";
            var dataBase = new WebDataBase("joe.pl");
            var id = dataBase.AddNewRow("joe.pl/index", title, childrenList);
            dataBase.ClearDatabase();
            var getChildrenAddressesResult = dataBase.GetChildrenAddresses("joe.pl/index");
            CollectionAssert.AreEqual(new List<string>(), getChildrenAddressesResult);
        }
    }
}
