namespace WebCrawlerApp
{
    partial class WebCrawlerWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ContentDataGrid = new System.Windows.Forms.DataGridView();
            this.Console = new System.Windows.Forms.TextBox();
            this.RelationDataGrid = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.runButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.saveDatabaseToXMLButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.ContentDataGrid2 = new System.Windows.Forms.DataGridView();
            this.RelationDataGrid2 = new System.Windows.Forms.DataGridView();
            this.Console2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ContentDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RelationDataGrid)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContentDataGrid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RelationDataGrid2)).BeginInit();
            this.SuspendLayout();
            // 
            // ContentDataGrid
            // 
            this.ContentDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ContentDataGrid.Location = new System.Drawing.Point(0, 0);
            this.ContentDataGrid.Name = "ContentDataGrid";
            this.ContentDataGrid.Size = new System.Drawing.Size(603, 644);
            this.ContentDataGrid.TabIndex = 0;
            // 
            // Console
            // 
            this.Console.Location = new System.Drawing.Point(0, 0);
            this.Console.Multiline = true;
            this.Console.Name = "Console";
            this.Console.ReadOnly = true;
            this.Console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Console.Size = new System.Drawing.Size(600, 639);
            this.Console.TabIndex = 1;
            // 
            // RelationDataGrid
            // 
            this.RelationDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RelationDataGrid.Location = new System.Drawing.Point(0, 0);
            this.RelationDataGrid.Name = "RelationDataGrid";
            this.RelationDataGrid.Size = new System.Drawing.Size(603, 644);
            this.RelationDataGrid.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(611, 676);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ContentDataGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(603, 650);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main database";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.RelationDataGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(603, 650);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Relation database";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.Console);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(603, 650);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Console output";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.tabControl1);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel2);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1332, 685);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(3, 3);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(236, 23);
            this.runButton.TabIndex = 8;
            this.runButton.Text = "Run Hans";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(245, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(103, 23);
            this.clearButton.TabIndex = 9;
            this.clearButton.Text = "Clear data";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // saveDatabaseToXMLButton
            // 
            this.saveDatabaseToXMLButton.Location = new System.Drawing.Point(354, 3);
            this.saveDatabaseToXMLButton.Name = "saveDatabaseToXMLButton";
            this.saveDatabaseToXMLButton.Size = new System.Drawing.Size(136, 23);
            this.saveDatabaseToXMLButton.TabIndex = 10;
            this.saveDatabaseToXMLButton.Text = "Save database to XML";
            this.saveDatabaseToXMLButton.UseVisualStyleBackColor = true;
            this.saveDatabaseToXMLButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.tabControl2);
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(620, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(688, 672);
            this.flowLayoutPanel2.TabIndex = 11;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.runButton);
            this.flowLayoutPanel3.Controls.Add(this.clearButton);
            this.flowLayoutPanel3.Controls.Add(this.saveDatabaseToXMLButton);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 631);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(596, 34);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(685, 622);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.ContentDataGrid2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(677, 596);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Main database";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.RelationDataGrid2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(677, 596);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Relation database";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.Console2);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(677, 596);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Console output";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // ContentDataGrid2
            // 
            this.ContentDataGrid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ContentDataGrid2.Location = new System.Drawing.Point(-1, 0);
            this.ContentDataGrid2.Name = "ContentDataGrid2";
            this.ContentDataGrid2.Size = new System.Drawing.Size(672, 593);
            this.ContentDataGrid2.TabIndex = 1;
            // 
            // RelationDataGrid2
            // 
            this.RelationDataGrid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RelationDataGrid2.Location = new System.Drawing.Point(-1, 0);
            this.RelationDataGrid2.Name = "RelationDataGrid2";
            this.RelationDataGrid2.Size = new System.Drawing.Size(675, 593);
            this.RelationDataGrid2.TabIndex = 1;
            // 
            // Console2
            // 
            this.Console2.Location = new System.Drawing.Point(0, 0);
            this.Console2.Multiline = true;
            this.Console2.Name = "Console2";
            this.Console2.ReadOnly = true;
            this.Console2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Console2.Size = new System.Drawing.Size(668, 590);
            this.Console2.TabIndex = 2;
            // 
            // WebCrawlerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1345, 700);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "WebCrawlerWindow";
            this.Text = "Hans Web Crawler";
            ((System.ComponentModel.ISupportInitialize)(this.ContentDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RelationDataGrid)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ContentDataGrid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RelationDataGrid2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ContentDataGrid;
        private System.Windows.Forms.TextBox Console;
        private System.Windows.Forms.DataGridView RelationDataGrid;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView ContentDataGrid2;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button saveDatabaseToXMLButton;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.DataGridView RelationDataGrid2;
        private System.Windows.Forms.TextBox Console2;
    }
}

