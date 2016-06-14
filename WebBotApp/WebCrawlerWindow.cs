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
        const string Address = "http://bg.pg.edu.pl/";
        const int Depth = 1;
        WebMinner _minner;

        public WebCrawlerWindow()
        {
            InitializeComponent();
            _minner = new WebMinner(Address);
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            Console.Text = "";
            _minner.Database.ClearDatabase();
            Stopwatch stopWatch = Stopwatch.StartNew();
            _minner.MiningMultiThread(Depth);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.Text = WebMinner.outputConsole;
            Console.Text += "Finished using " + WebMinner.ThreadCount + " threads in time " + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds +
                            ":" + ts.Milliseconds + " (total " + ts.TotalMilliseconds + " ms)\r\n";
            Console.Text += "Found " + _minner.Database.DataSet.Tables["Relation"].Rows.Count + " sites starting from " + Address + "\r\n";
            Console.Text += "Looked through to " + _minner.Database.DataSet.Tables["Content"].Rows.Count + " sites\r\n";
            Console.Text += "Didn't get reposne from " + WebMinner.LostSiteCount + " sites\r\n";
            WebMinner.outputConsole = "";

            Console2.Text = Console.Text;
            ContentDataGrid.DataSource = _minner.Database.DataSet.Tables["Content"];
            RelationDataGrid.DataSource = _minner.Database.DataSet.Tables["Relation"];
            ContentDataGrid2.DataSource = _minner.Database.DataSet.Tables["Content"];
            RelationDataGrid2.DataSource = _minner.Database.DataSet.Tables["Relation"];
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _minner.Database.ClearDatabase();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _minner.Database.SaveToXml();
        }
    }
}
