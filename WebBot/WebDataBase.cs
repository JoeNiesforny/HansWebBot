using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace HansWebCrawler
{
    public class WebDataBase
    {
        public DataSet DataSet;
        DataTable _webContent;
        DataTable _webRelation;
        Mutex addingNewRow;

        public WebDataBase(string address) // ToDo - add what type of context should be looking for
        {
            addingNewRow = new Mutex();
            var columnId = new DataColumn("Id", typeof(int)) { Unique = true, AutoIncrement = true, ReadOnly = true };
            var columnParentId = new DataColumn("ParentId", typeof(int));

            _webContent = new DataTable("Content");
            _webContent.Columns.Add(columnId);
            _webContent.PrimaryKey = new DataColumn[] { columnId };
            _webContent.Columns.Add("ParentId", typeof(int));
            _webContent.Columns.Add("Address", typeof(string));
            _webContent.Columns.Add("Title", typeof(string));

            _webRelation = new DataTable("Relation");
            _webRelation.Columns.Add(columnParentId);
            _webRelation.Columns.Add("Address", typeof(string));

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
            newRow["Address"] = address;
            newRow["Title"] = title;
            newRow["ParentId"] = parentId;
            _webContent.Rows.Add(newRow);
            var id = (int)_webContent.Rows[_webContent.Rows.Count - 1]["Id"];
            addingNewRow.ReleaseMutex();
            if (parentId > -1)
            {
                var relationNewRow = _webRelation.NewRow();
                relationNewRow["ParentId"] = parentId;
                relationNewRow["Address"] = address;
                _webRelation.Rows.Add(relationNewRow);
            }
            return id;
        }

        // purpose to use parameter is to avoid looking for parent address of new address == speed up
        public int AddNewRow(string address, string title, List<string> addresses, int parentId = -1)
        {
            addingNewRow.WaitOne();
            var newRow = _webContent.NewRow();
            newRow["Address"] = address;
            newRow["Title"] = title;
            newRow["ParentId"] = parentId;
            _webContent.Rows.Add(newRow);

            if (parentId > -1)
            {
                var relationNewRow = _webRelation.NewRow();
                relationNewRow["ParentId"] = parentId;
                relationNewRow["Address"] = address;
                _webRelation.Rows.Add(relationNewRow);
            }
            var id = (int)_webContent.Rows[_webContent.Rows.Count - 1]["Id"];

            foreach (var childAddress in addresses)
            {
                var relationNewRow = _webRelation.NewRow();
                relationNewRow["ParentId"] = id;
                relationNewRow["Address"] = childAddress;
                _webRelation.Rows.Add(relationNewRow);
            }
            addingNewRow.ReleaseMutex();
            return id;
        }

        public void SaveToXml()
        {
            DataSet.WriteXml("WebDataBase.xml");
        }

        public string GetTitleFromAddress(string address)
        {
            var rows = _webContent.Select("Address = '" + address + "'");
            if (rows.Length == 0)
                return "";
            return (string)rows[0]["Title"];
        }

        public string GetParentAddress(string address)
        {
            var rows = _webContent.Select("Address = '" + address + "'");
            if (rows.Length == 0)
                return "";
            var parentId = (int)rows[0]["ParentId"];
            if (parentId >= 0)
            {
                rows = _webContent.Select("Id = " + parentId + "");
                return (string)rows[0]["Address"];
            }
            else
                return "";
        }

        public List<string> GetChildrenAddresses(string address)
        {
            var listOfChildAddresses = new List<string>();
            var rows = _webContent.Select("Address = '" + address + "'");
            if (rows.Length == 0)
                return listOfChildAddresses;
            var childsRow = rows[0].GetChildRows("ParentToChild");
            foreach (var child in childsRow)
            {
                listOfChildAddresses.Add((string)child["Address"]);
            }
            return listOfChildAddresses;
        }

        public void ClearDatabase()
        {
            foreach (DataTable table in DataSet.Tables)
                 table.Clear();
        }
    }
}
