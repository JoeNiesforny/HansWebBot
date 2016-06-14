using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace HansWebCrawler
{
    // Content is describing content of content table in dataset
    public static class Content
    {
        public static string Name = "Content";
        public static string Id = "Id";
        public static string ParentId = "ParentId";
        public static string Address = "Address";
        public static string Title = "Title";
        public static string In = "In";
        public static string Out = "Out";
    }

    // Relation is describing content of relation table in dataset
    public static class Relation
    {
        public static string Name = "Relation";
        public static string ParentId = "ParentId";
        public static string Address = "Address";
        public static string Visited = "Visited";
    }

    public class WebDataBase
    {
        public DataSet DataSet;
        DataTable _webContent;
        DataTable _webRelation;
        Mutex addingNewRow;

        public WebDataBase(string address) // ToDo - add what type of context should be looking for
        {
            addingNewRow = new Mutex();
            var columnId = new DataColumn(Content.Id, typeof(int)) { Unique = true, AutoIncrement = true, ReadOnly = true };
            var columnParentId = new DataColumn(Relation.ParentId, typeof(int));

            _webContent = new DataTable(Content.Name);
            _webContent.Columns.Add(columnId);
            _webContent.PrimaryKey = new DataColumn[] { columnId };
            _webContent.Columns.Add(Content.ParentId, typeof(int));
            _webContent.Columns.Add(Content.Address, typeof(string));
            _webContent.Columns.Add(Content.Title, typeof(string));

            _webRelation = new DataTable(Relation.Name);
            _webRelation.Columns.Add(columnParentId);
            _webRelation.Columns.Add(Relation.Address, typeof(string));
            _webRelation.Columns.Add(Relation.Visited, typeof(bool));

            DataSet = new DataSet(address + "_" + DateTime.Now.Date.ToString());
            DataSet.Tables.Add(_webRelation);
            DataSet.Tables.Add(_webContent);

            var relationIdParent = new DataRelation("ParentToChild", columnId, columnParentId);
            DataSet.Relations.Add(relationIdParent);
        }

        public int AddNewRow(string address, string title, int parentId = -1)
        {
            addingNewRow.WaitOne(); // to prevent before multiple access in same time
            var newRow = _webContent.NewRow();
            newRow[Content.Address] = address;
            newRow[Content.Title] = title;
            newRow[Content.ParentId] = parentId;
            _webContent.Rows.Add(newRow);
            var id = (int)_webContent.Rows[_webContent.Rows.Count - 1][Content.Id];
            addingNewRow.ReleaseMutex();
            if (parentId > -1)
            {
                var relationNewRow = _webRelation.NewRow();
                relationNewRow[Relation.ParentId] = parentId;
                relationNewRow[Relation.Address] = address;
                _webRelation.Rows.Add(relationNewRow);
            }
            return id;
        }

        // purpose to use parameter is to avoid looking for parent address of new address == speed up
        public int AddNewRow(string address, string title, List<string> addresses, int parentId = -1)
        {
            addingNewRow.WaitOne();
            var newRow = _webContent.NewRow();
            newRow[Content.Address] = address;
            newRow[Content.Title] = title;
            newRow[Content.ParentId] = parentId;
            _webContent.Rows.Add(newRow);

            if (parentId > -1)
            {
                var relationNewRow = _webRelation.NewRow();
                relationNewRow[Relation.ParentId] = parentId;
                relationNewRow[Relation.Address] = address;
                _webRelation.Rows.Add(relationNewRow);
            }
            var id = (int)_webContent.Rows[_webContent.Rows.Count - 1][Content.Id];

            foreach (var childAddress in addresses)
            {
                var relationNewRow = _webRelation.NewRow();
                relationNewRow[Relation.ParentId] = id;
                relationNewRow[Relation.Address] = childAddress;
                _webRelation.Rows.Add(relationNewRow);
            }
            addingNewRow.ReleaseMutex();
            return id;
        }

        public void SaveToXml()
        {
            DataSet.WriteXml("WebDataBase.xml");
        }

        public void LoadFromXml()
        {
            DataSet.Clear();
            DataSet.ReadXml("WebDataBase.xml");
        }

        public string GetTitleFromAddress(string address)
        {
            var rows = _webContent.Select(Content.Address + " = '" + address + "'");
            if (rows.Length == 0)
                return "";
            return (string)rows[0][Content.Title];
        }

        public string GetParentAddress(string address)
        {
            var rows = _webContent.Select(Content.Address + " = '" + address + "'");
            if (rows.Length == 0)
                return "";
            var parentId = (int)rows[0][Content.ParentId];
            if (parentId >= 0)
            {
                rows = _webContent.Select("Id = " + parentId + "");
                return (string)rows[0][Content.Address];
            }
            else
                return "";
        }

        public List<string> GetChildrenAddresses(string address)
        {
            var listOfChildAddresses = new List<string>();
            var rows = _webContent.Select(Content.Address + " = '" + address + "'");
            if (rows.Length == 0)
                return listOfChildAddresses;
            var childsRow = rows[0].GetChildRows("ParentToChild");
            foreach (var child in childsRow)
            {
                listOfChildAddresses.Add((string)child[Content.Address]);
            }
            return listOfChildAddresses;
        }

        public void ClearDatabase()
        {
            foreach (DataTable table in DataSet.Tables)
                 table.Clear();
        }

        public void CountInOutFromAcquireSites()
        {
            _webContent.Columns.Add(Content.In, typeof(int));
            _webContent.Columns.Add(Content.Out, typeof(int));
            var rows = _webContent.Rows;

            foreach (DataRow row in rows)
            {
                var id = (int)row[Content.Id];
                var countOut = _webRelation.Select(Relation.ParentId + " = " + id).Length;
                row[Content.Out] = countOut;
                var address = (string)row[Content.Address];
                var countIn = _webRelation.Select(Relation.Address + " = '" + address + "'").Length;
                row[Content.In] = countIn;
            }
        }

        public void CheckAddressAsVisited(string address)
        {
            addingNewRow.WaitOne();
            var rows = _webRelation.Select(Relation.Address + " = '" + address + "'");
            foreach (DataRow row in rows)
            {
                row[Relation.Visited] = true;
            }
            addingNewRow.ReleaseMutex();
        }
    }
}
