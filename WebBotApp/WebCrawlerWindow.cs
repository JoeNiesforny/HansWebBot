using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HansWebCrawler;
using System.Diagnostics;

namespace WebCrawlerApp
{
    public partial class WebCrawlerWindow : Form
    {
        const string _Address = "http://bg.pg.edu.pl";
        const int _Depth = 1;
        WebMinner _Minner;

        public WebCrawlerWindow()
        {
            InitializeComponent();
            _Minner = new WebMinner(_Address);
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            Console.Text = "";
            _Minner.Database.ClearDatabase();
            Stopwatch stopWatch = Stopwatch.StartNew();
            _Minner.MiningMultiThread(_Depth);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.Text = WebMinner.outputConsole;
            Console.Text += "Finished using " + WebMinner.ThreadCount + " threads in time " + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds +
                            ":" + ts.Milliseconds + " (total " + ts.TotalMilliseconds + " ms)\r\n";
            Console.Text += "Found " + _Minner.Database.DataSet.Tables[Relation.Name].Rows.Count + " sites starting from " + _Address + "\r\n";
            Console.Text += "Looked through to " + _Minner.Database.DataSet.Tables[Content.Name].Rows.Count + " sites\r\n";
            Console.Text += "Didn't get reposne from " + WebMinner.LostSiteCount + " sites\r\n";
            WebMinner.outputConsole = "";

            Console2.Text = Console.Text;
            ContentDataGrid.DataSource = _Minner.Database.DataSet.Tables[Content.Name];
            RelationDataGrid.DataSource = _Minner.Database.DataSet.Tables[Relation.Name];
            ContentDataGrid2.DataSource = _Minner.Database.DataSet.Tables[Content.Name];
            RelationDataGrid2.DataSource = _Minner.Database.DataSet.Tables[Relation.Name];
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _Minner.Database.ClearDatabase();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _Minner.Database.SaveToXml();
        }

        private void countInOutButton_Click(object sender, EventArgs e)
        {
            _Minner.Database.CountInOutFromAcquireSites();
        }

        private void loadDatabaseFromXmlButton_Click(object sender, EventArgs e)
        {
            _Minner.Database.LoadFromXml();
            ContentDataGrid.DataSource = _Minner.Database.DataSet.Tables[Content.Name];
            RelationDataGrid.DataSource = _Minner.Database.DataSet.Tables[Relation.Name];
            ContentDataGrid2.DataSource = _Minner.Database.DataSet.Tables[Content.Name];
            RelationDataGrid2.DataSource = _Minner.Database.DataSet.Tables[Relation.Name];
        }
    }
}
