/*################# Data_Processing.cs ###########################
# Eklektik Design
# Micah Richards
# 2/6/2018
#           
# Purpose: Organizes the data and creates the report for DoorC
#
###############################################################*/

namespace SDF3K
{

    /*################# Libraries #################################*/
    using OfficeOpenXml;
    using System;
    using System.Windows.Forms;
    using static SDF3K.Constants;
    internal class Data_Processing
    {

        /*################# Private Functions #########################*/

        internal int[] __allCount;                    //## [column] - int

        internal int[][] __monthCount;                //## [column][month] - int

        internal int[][][] __dayCount;                //## [column][month][day] - int

        internal int[][][][] __hourCount;             //## [column][month][day][hour] - int

        internal Object[][] __allData;                //## [column][data] - Date  Import of excel data

        internal Object[][][][][] __hourData;         //## [column][month][day][hour][data] - Date


        /*################# Private Functions #########################*/

        /*################# AddCalendar #################################
        # Purpose: Adds and populates the report's calendar
        #                  
        ###############################################################*/
        private void AddCalendar(Excel excelOut, int[][] data, int day, int monthId)
        {
            excelOut.Calendar(1, 0, day, monthId);
            excelOut.WriteFile(data.IntToObj(), 1, 2);
        }

        /*################# AddGraphs ###################################
        # Purpose: Adds a weekly and monthly summary graph to the report
        #                  
        ###############################################################*/
        private void AddGraphs(Excel excelOut, int monthId)
        {
            int msize;
            int hsize;
            ExcelRange mdata;
            ExcelRange mid;
            ExcelRange hdata;
            ExcelRange hid;
            String worksheet = excelOut.GetCurrentWorksheet();

            excelOut.CreateWorksheet("DataStorage" + worksheet.Substring(worksheet.LastIndexOf("-") - 1));
            excelOut.WriteRow(__dayCount[0][monthId].IntToObj(), 0, 0);

            msize = __dayCount[0][monthId].Length;
            for (int i = 0; i < msize; i++)
            {
                excelOut.WriteCell(i + 1, i, 1, false);
            }

            mdata = excelOut.CreateRange(0, 0, msize, 0);
            mid = excelOut.CreateRange(0, 1, msize, 1);


            hsize = 0;
            for (int i = 0; i < 31; i++)
            {
                for (int j = 0; j < 24; j++)
                {
                    excelOut.WriteCell(__hourCount[0][monthId][i][j], i * 24 + j, 2, false);
                    excelOut.WriteCell(i + ", " + j, i * 24 + j, 3, false);
                    hsize++;
                }
            }

            hdata = excelOut.CreateRange(0, 2, hsize, 2);
            hid = excelOut.CreateRange(0, 3, hsize, 3);


            excelOut.OpenWorksheet(worksheet);
            excelOut.AddGraph("Monthdata", 600, 0, 800, 300, mdata, mid);
            excelOut.AddGraph("Hourdata", 0, 350, 1400, 300, hdata, hid);
        }

        /*################# AddTotals ###################################
        # Purpose: Adds the weekly and monthly totals to the report
        #                  
        ###############################################################*/
        private void AddTotals(Excel excelOut, int[][] data)
        {
            int sum;

            for (int i = 0; i < data.Length; i++)
            {
                sum = 0;
                for (int j = 0; j < data[i].Length; j++)
                {
                    sum += data[i][j];
                }
                excelOut.WriteCell(sum, 8, i + 2, false);
            }

            excelOut.SetBorder(1, excelOut.CreateRange(8, 1, 8, 7));
            excelOut.WriteCell("Total", 8, 1, false);
            excelOut.WriteCell(__monthCount[0], 8, 7, true);
        }

        /*################# AddWeekMarks ################################
        # Purpose: Adds the start and end date label for each week
        #                  
        ###############################################################*/
        private void AddWeekMarks(Excel excelOut)
        {
            String[] weeks = new String[] { "1st - 7th", "8th - 14th", "15th-21st", "22nd-28th", "29th-End" };
            excelOut.SetBorder(1, excelOut.CreateRange(0, 2, 0, 6));
            excelOut.WriteColumn(weeks, 0, 2);
        }


        /*################# Public Functions ##########################*/

        /*################# CreateReport ################################
        # Purpose: Runs all necessary report functions
        #                  
        ###############################################################*/
        public void CreateReport(Excel excelOut, int day, int monthId)
        {
            int[][] data = __dayCount[0][monthId].OneToTwo(7);
            String month = ((MONTHS)monthId).ToString();

            AddWeekMarks(excelOut);
            AddCalendar(excelOut, data, day, monthId);
            AddTotals(excelOut, data);
            AddGraphs(excelOut, monthId);

            MessageBox.Show(month + " Formatting Complete!", "Done!");
            
        }

        /*################# SortData ####################################
        # Purpose: Parses the data into the proper lists
        #                  
        ###############################################################*/
        public void SortData(Excel excelIn)
        {
            int counthours;
            int countdays;
            int countmonths;

            __allData = excelIn.ReadFile();                                //## [column][data] - Date  Import of excel data
            __hourData = new Object[__allData.Length][][][][];           //## [column][month][day][hour][data] - Date
            __allCount = new int[__allData.Length];                      //## [column] - int
            __monthCount = new int[__allData.Length][];                  //## [column][month] - int
            __dayCount = new int[__allData.Length][][];                  //## [column][month][day] - int
            __hourCount = new int[__allData.Length][][][];               //## [column][month][day][hour] - int

            for (int i = 0; i < __allData.Length; i++)
            {
                Object[][] monthdata;                                   //## Excel data sorted by month
                __allCount[i] = __allData[i].Length;

                __monthCount[i] = new int[12];
                __dayCount[i] = new int[12][];
                __hourCount[i] = new int[12][][];
                __hourData[i] = new Object[12][][][];

                monthdata = Sorting.SortByMonth(__allData[i]);


                for (int j = 0; j < 12; j++)
                {
                    Object[][] daydata;                                 //## Excel data sorted by day

                    countmonths = 0;
                    try{ countmonths = monthdata[j].Length; } catch { }
                    __monthCount[i][j] = countmonths;

                    __dayCount[i][j] = new int[31];
                    __hourCount[i][j] = new int[31][];
                    __hourData[i][j] = new Object[31][][];

                    if (monthdata[j] != null)
                    {
                        daydata = Sorting.SortByDay(monthdata[j]);

                        for (int k = 0; k < 31; k++)
                        {
                            countdays = 0;
                            try{ countdays = daydata[k].Length; } catch{ }
                            __dayCount[i][j][k] = countdays;

                            __hourCount[i][j][k] = new int[24];

                            if (daydata[k] != null)
                            {
                                __hourData[i][j][k] = Sorting.SortByHour(daydata[k]);
                            }

                            for (int l = 0; l < 24; l++)
                            {
                                counthours = 0;
                                try{ counthours = __hourData[i][j][k][l].Length; } catch { }
                                __hourCount[i][j][k][l] = counthours;
                            }
                        }
                    }
                }
            }
        }


        /*################# Constructors ##############################*/

        /*################# Data_Processing ##############################
        # Purpose: Creates a handle to a unique DataProcessing object
        #                  
        ###############################################################*/
        public Data_Processing()
        {
        }
    }
}
