/*################# Main_Window.cs ###############################
# Eklektik Design
# Micah Richards
# 1/24/2018
#           
# Purpose: Launches the SDF3K Program
#
###############################################################*/

namespace SDF3K
{
    /*################# Libraries #####################################*/
    using System;
    using System.Windows.Forms;
    using static SDF3K.Constants;

    public partial class Main_Window : Form
    {

        /*################# Variables #################################*/
        internal Excel __excelIn;               //## Reference to the excel document in use

        internal Excel __excelOut;              //## Reference to the excel document in use

        internal String __sourcePath;           //## Filepath to excel document


        /*################# Private Functions #########################*/

        /*################# ButtonBrowseClick ##########################
        # Purpose: Allows the user to browse for a valid excel file on
        #           press
        #
        ###############################################################*/
        private void ButtonBrowseClick(object sender, EventArgs e)
        {
            String[] tempstrarray;
            String displaypath;

            if (FileBrowser.ShowDialog() == DialogResult.OK)
            {
                __sourcePath = FileBrowser.FileName;
                __excelIn = new Excel(__sourcePath);
                tempstrarray = __excelIn.GetWorksheetNames();
                displaypath = __sourcePath;

                if (!(tempstrarray.Length == 1 && tempstrarray[0] == "error " + ERROR))
                {
                    Worksheetcombobox.DataSource = tempstrarray;
                    if (tempstrarray.Length > 0)
                    {
                        Buttonformat.Enabled = true;

                        if (__sourcePath.Length > 13)
                        {
                            displaypath = "..." + displaypath.Substring(displaypath.Length - 10, 10);
                        }
                        Labelfilename.Text = displaypath;
                    }
                    else
                    {
                        MessageBox.Show("That file says it has no data", "Error");
                    }
                }
            }
        }

        /*################# ButtonFormatClick ##########################
        # Purpose: Initates the formatting procedure
        #
        ###############################################################*/
        private void ButtonFormatClick(object sender, EventArgs e)
        {
            int monthid = Monthcombobox.SelectedIndex;
            int day = Daycombobox.SelectedIndex;
            String sheet = (String)Worksheetcombobox.SelectedItem;
            String month = ((MONTHS)monthid).ToString();
            Data_Processing dp = new Data_Processing();

            __excelIn.OpenWorksheet((String)Worksheetcombobox.SelectedItem);
            dp.SortData(__excelIn);
            __excelOut = new Excel(__sourcePath.Remove(__sourcePath.LastIndexOf('\\') + 1) + month + " Summary Report.xlsx", "Month Summary - " + sheet);
            dp.CreateReport(__excelOut, day, monthid);
        }


        /*################# Public Functions ##########################*/


        /*################# Constructors ##############################*/

        /*################# MainWindow ##################################
        # Purpose: Creates the window for SDF3K
        #
        ###############################################################*/
        public Main_Window()
        {
            InitializeComponent();
            FileBrowser.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            Monthcombobox.SelectedIndex = DateTime.Today.Month - 1;
            Daycombobox.SelectedIndex = 0;
        }
    }
}
