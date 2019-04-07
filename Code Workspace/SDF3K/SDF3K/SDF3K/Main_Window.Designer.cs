namespace SDF3K
{
    partial class Main_Window
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Window));
            this.Buttonformat = new System.Windows.Forms.Button();
            this.Monthcombobox = new System.Windows.Forms.ComboBox();
            this.Labelmonth = new System.Windows.Forms.Label();
            this.Labelday = new System.Windows.Forms.Label();
            this.Daycombobox = new System.Windows.Forms.ComboBox();
            this.Labelfile = new System.Windows.Forms.Label();
            this.Buttonbrowse = new System.Windows.Forms.Button();
            this.Labelfilename = new System.Windows.Forms.Label();
            this.FileBrowser = new System.Windows.Forms.OpenFileDialog();
            this.Labelworksheet = new System.Windows.Forms.Label();
            this.Worksheetcombobox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Buttonformat
            // 
            this.Buttonformat.Enabled = false;
            this.Buttonformat.Location = new System.Drawing.Point(15, 134);
            this.Buttonformat.Name = "Buttonformat";
            this.Buttonformat.Size = new System.Drawing.Size(257, 23);
            this.Buttonformat.TabIndex = 5;
            this.Buttonformat.Text = "Format";
            this.Buttonformat.UseVisualStyleBackColor = true;
            this.Buttonformat.Click += new System.EventHandler(this.ButtonFormatClick);
            // 
            // Monthcombobox
            // 
            this.Monthcombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Monthcombobox.FormattingEnabled = true;
            this.Monthcombobox.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.Monthcombobox.Location = new System.Drawing.Point(122, 13);
            this.Monthcombobox.Name = "Monthcombobox";
            this.Monthcombobox.Size = new System.Drawing.Size(150, 21);
            this.Monthcombobox.TabIndex = 1;
            // 
            // Labelmonth
            // 
            this.Labelmonth.AutoSize = true;
            this.Labelmonth.Location = new System.Drawing.Point(12, 16);
            this.Labelmonth.Name = "Labelmonth";
            this.Labelmonth.Size = new System.Drawing.Size(74, 13);
            this.Labelmonth.TabIndex = 0;
            this.Labelmonth.Text = "Current Month";
            // 
            // Labelday
            // 
            this.Labelday.AutoSize = true;
            this.Labelday.Location = new System.Drawing.Point(12, 43);
            this.Labelday.Name = "Labelday";
            this.Labelday.Size = new System.Drawing.Size(95, 13);
            this.Labelday.TabIndex = 0;
            this.Labelday.Text = "First Day Of Month";
            // 
            // Daycombobox
            // 
            this.Daycombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Daycombobox.FormattingEnabled = true;
            this.Daycombobox.Items.AddRange(new object[] {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"});
            this.Daycombobox.Location = new System.Drawing.Point(122, 40);
            this.Daycombobox.Name = "Daycombobox";
            this.Daycombobox.Size = new System.Drawing.Size(150, 21);
            this.Daycombobox.TabIndex = 2;
            // 
            // Labelfile
            // 
            this.Labelfile.AutoSize = true;
            this.Labelfile.Location = new System.Drawing.Point(12, 72);
            this.Labelfile.Name = "Labelfile";
            this.Labelfile.Size = new System.Drawing.Size(74, 13);
            this.Labelfile.TabIndex = 0;
            this.Labelfile.Text = "File To Format";
            // 
            // Buttonbrowse
            // 
            this.Buttonbrowse.Location = new System.Drawing.Point(197, 67);
            this.Buttonbrowse.Name = "Buttonbrowse";
            this.Buttonbrowse.Size = new System.Drawing.Size(75, 23);
            this.Buttonbrowse.TabIndex = 3;
            this.Buttonbrowse.Text = "Browse";
            this.Buttonbrowse.UseVisualStyleBackColor = true;
            this.Buttonbrowse.Click += new System.EventHandler(this.ButtonBrowseClick);
            // 
            // Labelfilename
            // 
            this.Labelfilename.AutoSize = true;
            this.Labelfilename.Location = new System.Drawing.Point(123, 72);
            this.Labelfilename.Name = "Labelfilename";
            this.Labelfilename.Size = new System.Drawing.Size(68, 13);
            this.Labelfilename.TabIndex = 0;
            this.Labelfilename.Text = "Select File ->";
            // 
            // FileBrowser
            // 
            this.FileBrowser.DefaultExt = "csv";
            this.FileBrowser.Filter = "Excel files (*.xls or .xlsx)|.xls;*.xlsx";
            this.FileBrowser.Title = "Select The Data Source";
            // 
            // Labelworksheet
            // 
            this.Labelworksheet.AutoSize = true;
            this.Labelworksheet.Location = new System.Drawing.Point(12, 99);
            this.Labelworksheet.Name = "Labelworksheet";
            this.Labelworksheet.Size = new System.Drawing.Size(104, 13);
            this.Labelworksheet.TabIndex = 0;
            this.Labelworksheet.Text = "Name Of Worksheet";
            // 
            // Worksheetcombobox
            // 
            this.Worksheetcombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Worksheetcombobox.FormattingEnabled = true;
            this.Worksheetcombobox.Location = new System.Drawing.Point(122, 96);
            this.Worksheetcombobox.Name = "Worksheetcombobox";
            this.Worksheetcombobox.Size = new System.Drawing.Size(150, 21);
            this.Worksheetcombobox.TabIndex = 4;
            // 
            // Main_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 167);
            this.Controls.Add(this.Labelworksheet);
            this.Controls.Add(this.Worksheetcombobox);
            this.Controls.Add(this.Labelfilename);
            this.Controls.Add(this.Buttonbrowse);
            this.Controls.Add(this.Labelfile);
            this.Controls.Add(this.Labelday);
            this.Controls.Add(this.Daycombobox);
            this.Controls.Add(this.Labelmonth);
            this.Controls.Add(this.Monthcombobox);
            this.Controls.Add(this.Buttonformat);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main_Window";
            this.Text = "SDF3K";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Buttonformat;
        private System.Windows.Forms.ComboBox Monthcombobox;
        private System.Windows.Forms.Label Labelmonth;
        private System.Windows.Forms.Label Labelday;
        private System.Windows.Forms.ComboBox Daycombobox;
        private System.Windows.Forms.Label Labelfile;
        private System.Windows.Forms.Button Buttonbrowse;
        private System.Windows.Forms.Label Labelfilename;
        private System.Windows.Forms.OpenFileDialog FileBrowser;
        private System.Windows.Forms.Label Labelworksheet;
        private System.Windows.Forms.ComboBox Worksheetcombobox;
    }
}

