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
        DataTable _WebContent;
        DataTable _WebRelation;
        Mutex _AccessMutex;

        public WebDataBase(string address) // ToDo - add what type of context should be looking for
        {
            _AccessMutex = new Mutex();
            var columnId = new DataColumn(Content.Id, typeof(int)) { Unique = true, AutoIncrement = true, ReadOnly = true };
            var columnParentId = new DataColumn(Relation.ParentId, typeof(int));

            _WebContent = new DataTable(Content.Name);
            _WebContent.Columns.Add(columnId);
            _WebContent.PrimaryKey = new DataColumn[] { columnId };
            _WebContent.Columns.Add(Content.ParentId, typeof(int));
            _WebContent.Columns.Add(Content.Address, typeof(string));
            _WebContent.Columns.Add(Content.Title, typeof(string));

            _WebRelation = new DataTable(Relation.Name);
            _WebRelation.Columns.Add(columnParentId);
            _WebRelation.Columns.Add(Relation.Address, typeof(string));
            _WebRelation.Columns.Add(Relation.Visited, typeof(bool)).DefaultValue = false;

            DataSet = new DataSet(address + "_" + DateTime.Now.Date.ToString());
            DataSet.Tables.Add(_WebRelation);
            DataSet.Tables.Add(_WebContent);

            var relationIdParent = new DataRelation("ParentToChild", columnId, columnParentId);
            DataSet.Relations.Add(relationIdParent);
        }

        public int AddNewRow(string address, string title, int parentId = -1)
        {
            _AccessMutex.WaitOne(); // to prevent before multiple access in same time
            var newRow = _WebContent.NewRow();
            newRow[Content.Address] = address;
            newRow[Content.Title] = title;
            newRow[Content.ParentId] = parentId;
            _WebContent.Rows.Add(newRow);
            var id = (int)_WebContent.Rows[_WebContent.Rows.Count - 1][Content.Id];
            _AccessMutex.ReleaseMutex();
            if (parentId > -1)
            {
                var relationNewRow = _WebRelation.NewRow();
                relationNewRow[Relation.ParentId] = parentId;
                relationNewRow[Relation.Address] = address;
                _WebRelation.Rows.Add(relationNewRow);
            }
            return id;
        }

        // purpose to use parameter is to avoid looking for parent address of new address == speed up
        public int AddNewRow(string address, string title, List<string> addresses, int parentId = -1)
        {
            _AccessMutex.WaitOne();
            var newRow = _WebContent.NewRow();
            newRow[Content.Address] = address;
            newRow[Content.Title] = title;
            newRow[Content.ParentId] = parentId;
            _WebContent.Rows.Add(newRow);

            if (parentId > -1)
            {
                var relationNewRow = _WebRelation.NewRow();
                relationNewRow[Relation.ParentId] = parentId;
                relationNewRow[Relation.Address] = address;
                _WebRelation.Rows.Add(relationNewRow);
            }
            var id = (int)_WebContent.Rows[_WebContent.Rows.Count - 1][Content.Id];

            foreach (var childAddress in addresses)
            {
                var relationNewRow = _WebRelation.NewRow();
                relationNewRow[Relation.ParentId] = id;
                relationNewRow[Relation.Address] = childAddress;
                _WebRelation.Rows.Add(relationNewRow);
            }
            _AccessMutex.ReleaseMutex();
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
            var rows = _WebContent.Select(Content.Address + " = '" + address + "'");
            if (rows.Length == 0)
                return "";
            return (string)rows[0][Content.Title];
        }

        public string GetParentAddress(string address)
        {
            var rows = _WebContent.Select(Content.Address + " = '" + address + "'");
            if (rows.Length == 0)
                return "";
            var parentId = (int)rows[0][Content.ParentId];
            if (parentId >= 0)
            {
                rows = _WebContent.Select("Id = " + parentId + "");
                return (string)rows[0][Content.Address];
            }
            else
                return "";
        }

        public List<string> GetChildrenAddresses(string address)
        {
            var listOfChildAddresses = new List<string>();
            var rows = _WebContent.Select(Content.Address + " = '" + address + "'");
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
            _WebContent.Columns.Add(Content.In, typeof(int));
            _WebContent.Columns.Add(Content.Out, typeof(int));
            var rows = _WebContent.Rows;

            foreach (DataRow row in rows)
            {
                var id = (int)row[Content.Id];
                var countOut = _WebRelation.Select(Relation.ParentId + " = " + id).Length;
                row[Content.Out] = countOut;
                var address = (string)row[Content.Address];
                var countIn = _WebRelation.Select(Relation.Address + " = '" + address + "'").Length;
                row[Content.In] = countIn;
            }
        }

        public void MarkAddressAsVisited(string address)
        {
            _AccessMutex.WaitOne();
            var rows = _WebRelation.Select(Relation.Address + " = '" + address + "'");
            foreach (DataRow row in rows)
            {
                row[Relation.Visited] = true;
            }
            _AccessMutex.ReleaseMutex();
        }

        public bool CheckIfAddressWasVisited(string address)
        {
            _AccessMutex.WaitOne();
            var rows = _WebRelation.Select(Relation.Address + " = '" + address + "'");
            var result = (bool)rows[0][Relation.Visited];
            _AccessMutex.ReleaseMutex();
            return result;
        }
    }
}
