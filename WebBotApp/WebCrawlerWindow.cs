﻿using System;
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
using System.IO;
using System.Threading;

namespace WebCrawlerApp
{
    public partial class WebCrawlerWindow : Form
    {
        const string _Address = "http://pg.edu.pl/";
        const int _Depth = 30;
        WebMinner _Minner;
        const int _TimeoutRequest = 5000;
        const int _WorkingThreadLimit = 500; // thread limit is to support 32-bit version

        public WebCrawlerWindow()
        {
            InitializeComponent();
            _Minner = new WebMinner(_Address, _TimeoutRequest);
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            Console.Text = "";
            _Minner.Database.ClearDatabase();
            Stopwatch stopWatch = Stopwatch.StartNew();
            _Minner.Mining(_Depth, _WorkingThreadLimit);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.Text = WebMinner.OutputConsole;
            Console.Text += "Finished using " + WebMinner.ThreadCount + " threads in time " + ts.ToString(@"hh\:mm\:ss\:fffffff") + " (total " + ts.TotalMilliseconds + " ms)\r\n";
            Console.Text += "Found " + _Minner.Database.DataSet.Tables[Relation.Name].Rows.Count + " sites starting from " + _Address + "\r\n";
            Console.Text += "Looked through to " + _Minner.Database.DataSet.Tables[Content.Name].Rows.Count + " sites. One page took " + ts.TotalMilliseconds / _Minner.Database.DataSet.Tables[Content.Name].Rows.Count + " ms\r\n";
            Console.Text += "Didn't get reposne from " + WebMinner.LostSiteCount + " sites\r\n";
            Console2.Text = Console.Text;
            ContentDataGrid.DataSource = _Minner.Database.DataSet.Tables[Content.Name];
            RelationDataGrid.DataSource = _Minner.Database.DataSet.Tables[Relation.Name];
            ContentDataGrid2.DataSource = _Minner.Database.DataSet.Tables[Content.Name];
            RelationDataGrid2.DataSource = _Minner.Database.DataSet.Tables[Relation.Name];
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _Minner.Database.ClearDatabase();
            _Minner = new WebMinner(_Address, _TimeoutRequest);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _Minner.Database.SaveToXml();
            File.WriteAllText("ConsoleOutput.txt", Console.Text);
        }

        private void countInOutButton_Click(object sender, EventArgs e)
        {
            _Minner.Database.CountInOutFromAcquireSites();
            var series = chart1.Series.Add("Rozkład Wejść");
            List<int> list = new List<int>();
            foreach (DataRow row in _Minner.Database.DataSet.Tables[Content.Name].Rows)
                list.Add(int.Parse(row[Content.In].ToString()));
            list.Sort();
            foreach (var element in list)
                series.Points.Add(element);
            chart1.ChartAreas[0].AxisY.Title = "Ilość wejść";
            var series2 = chart2.Series.Add("Rozkład Wyjść");
            list = new List<int>();
            foreach (DataRow row in _Minner.Database.DataSet.Tables[Content.Name].Rows)
                list.Add(int.Parse(row[Content.Out].ToString()));
            list.Sort();
            foreach (var element in list)
                series2.Points.Add(element);
            series2.Color = Color.Red;
            chart2.ChartAreas[0].AxisY.Title = "Ilość wyjść";
        }

        private void loadDatabaseFromXmlButton_Click(object sender, EventArgs e)
        {
            _Minner.Database.LoadFromXml();
            ContentDataGrid.DataSource = _Minner.Database.DataSet.Tables[Content.Name];
            RelationDataGrid.DataSource = _Minner.Database.DataSet.Tables[Relation.Name];
            ContentDataGrid2.DataSource = _Minner.Database.DataSet.Tables[Content.Name];
            RelationDataGrid2.DataSource = _Minner.Database.DataSet.Tables[Relation.Name];
            Console.Text = File.ReadAllText("ConsoleOutput.txt");
            Console2.Text = File.ReadAllText("ConsoleOutput.txt");
        }
    }
}
