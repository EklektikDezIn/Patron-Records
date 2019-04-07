/*################# Excel #######################################
# Eklektik Design
# Micah Richards
# 07/11/17
# Purpose: Handles work with Excel files for general purpose
#           uses reference EPPlus
#
###############################################################*/

namespace SDF3K
{
    using OfficeOpenXml;
    using OfficeOpenXml.Drawing.Chart;
    using OfficeOpenXml.Style;
    using System;
    using System.IO;
    using System.Windows.Forms;
    using static SDF3K.Constants;

    internal class Excel
    {

        /*################# Variables #################################*/

        private Boolean[] __readyToGo = { false, false, false };      //## Set to True when all initial values are set (File, Package, Worksheet)

        private FileInfo __file;                                      //## File to be modified

        private ExcelPackage __package;                               //## Selected package in file

        private ExcelWorksheet __workSheet;                           //## Selected worksheet in package


        /*################# Public Functions ##########################*/

        /*################# AddGraph ####################################
        # Purpose: Adds a chart of a given size to a given position in
        #           the excel document
        #
        ###############################################################*/
        public void AddGraph(String chartName, int posX, int posY, int sizeX, int sizeY, ExcelRange Data1, ExcelRange Data2)
        {
            if (__readyToGo[2])
            {
                var chart = __workSheet.Drawings.AddChart(chartName, eChartType.AreaStacked);

                //## Set position and size
                chart.Title.Text = chartName;
                chart.SetPosition(posY, posX);
                chart.SetSize(sizeX, sizeY);

                //## Add one serie. 
                var serie = chart.Series.Add(Data1, Data2);
                SavePackage();
            }
        }

        /*################# CountColumns ################################
        # Purpose: Returns the number of columns used in the file
        #
        ###############################################################*/
        public int CountColumns()
        {
            if (__readyToGo[2])
            {
                return __workSheet.Dimension.End.Column - (__workSheet.Dimension.Start.Column - 1);
            }
            return ERROR;
        }

        /*################# CountColumns ################################
        # Purpose: Returns the number of columns in a given row
        #
        ###############################################################*/
        public int CountColumns(int row)
        {
            int max;
            int final;

            if (__readyToGo[2])
            {
                max = CountColumns();
                final = max;
                for (int i = max; i > 0; i--)
                {
                    if (ReadCell(i - 1, row - 1) == null)
                    {
                        final--;
                    }
                    else
                    {
                        return final;
                    }
                }

            }

            return ERROR;
        }

        /*################# CountRows ###################################
        # Purpose: Returns the number of rows used in the file
        #
        ###############################################################*/
        public int CountRows()
        {
            if (__readyToGo[2])
            {
                return __workSheet.Dimension.End.Row - (__workSheet.Dimension.Start.Row - 1);
            }
            return ERROR;
        }

        /*################# CountRows ###################################
        # Purpose: Returns the number of rows used in a given column
        #
        ###############################################################*/
        public int CountRows(int column)
        {
            int final;
            int max;
            if (__readyToGo[2])
            {
                max = CountRows();
                final = max;
                for (int i = max; i > 0; i--)
                {
                    if (ReadCell(column - 1, i - 1) == null)
                    {
                        final--;
                    }
                    else
                    {
                        return final;
                    }
                }

            }
            return ERROR;
        }

        /*################# Private Functions #########################*/

        /*################# CreateFile ##################################
        # Purpose: Creates a file object for use by other functions
        #          
        # Inputs:  String - filename
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        private void CreateFile(String fileName)
        {
            try
            {
                __file = new FileInfo(fileName);
                __readyToGo[0] = true;
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Please make sure \"" + __file + "\" is a valid file.  Aborting current procedure.", "Error");
                __readyToGo[0] = false;
                __readyToGo[1] = false;
                __readyToGo[2] = false;
            }
        }

        /*################# CreatePackage ###############################
        # Purpose: Creates a package in the current file
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        private void CreatePackage()
        {
            if (__readyToGo[0])
            {
                try
                {
                    __package = new ExcelPackage(__file);
                    __readyToGo[1] = true;
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Please make sure \"" + __file + "\" is not currently open.  Aborting current procedure.", "Error");
                    __readyToGo[1] = false;
                    __readyToGo[2] = false;
                }
            }
        }

        /*################# CreateRange #################################
        # Purpose: Creates a range for use in other Excel.cs functions
        #
        ###############################################################*/
        public ExcelRange CreateRange(int startCol, int startRow, int endCol, int endRow)
        {
            if (__readyToGo[2])
            {
                /*## [FromRow, FromCol, ToRow, ToCol]## */
                return __workSheet.Cells[startRow + 1, startCol + 1, endRow + 1, endCol + 1];
            }
            return null;
        }

        /*################# CreateWorksheet #############################
        # Purpose: Creates a worksheet in the current package
        #          
        # Inputs:  String - name of worksheet
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void CreateWorksheet(String worksheetName)
        {
            if (__readyToGo[1])
            {
                try
                {
                    __workSheet = __package.Workbook.Worksheets.Add(__package.Workbook.Worksheets.Count + ". " + worksheetName);
                    __readyToGo[2] = true;
                    SavePackage();
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Please make sure \"" + __file + "\" is a valid worksheet.  Aborting current procedure.", "Error");
                    __readyToGo[2] = false;
                }
            }
        }

        /*################# GetCurrentWorksheet #########################
        # Purpose: Returns the name of the current worksheet
        #
        ###############################################################*/
        public String GetCurrentWorksheet()
        {
            if (__readyToGo[2])
            {
                return __workSheet.Name;
            }
            else
            {
                return BLANK;
            }
        }

        /*################# GetWorksheetNames ###########################
        # Purpose: Returns the list of worksheets in the given excel file
        #
        ###############################################################*/
        public String[] GetWorksheetNames()
        {
            String[] names = new String[1];

            if (__readyToGo[1])
            {
                names = new String[__package.Workbook.Worksheets.Count];
                foreach (ExcelWorksheet w in __package.Workbook.Worksheets)
                {
                    names[w.Index - 1] = w.Name;
                }
            }
            else
            {
                names[0] = "error " + ERROR;
            }
            return names;
        }

        /*################# MergeCells ##################################
        # Purpose: Merges a given range of cells
        #          
        ###############################################################*/
        public void MergeCells(ExcelRange data)
        {
            if (__readyToGo[2])
            {
                data.Merge = true;
                SavePackage();
            }
        }

        /*################# NameCol #####################################
        # Purpose: Places a title at the top of a given column
        #
        # Inputs:  String - text to be written
        # 	       int - column number at which to write
        #
        # Outputs: None
        #          
        ###############################################################*/
        public void NameCol(String msg, int columnNumber)
        {
            if (__readyToGo[2])
            {
                __workSheet.InsertRow(1, 1);
                WriteCell(msg, 1, columnNumber, true);
            }
        }

        /*################# OpenWorksheet ###############################
        # Purpose: Selects the worksheet for the excel document
        #
        ###############################################################*/
        public void OpenWorksheet(String worksheetName)
        {
            if (__readyToGo[1])
            {
                __workSheet = __package.Workbook.Worksheets[worksheetName];
                __readyToGo[2] = true;
            }
        }

        /*################# ReadCell ####################################
        # Purpose: Returns the value of a given cell
        #
        ###############################################################*/
        public Object ReadCell(int column, int row)
        {
            if (__readyToGo[2])
            {
                return __workSheet.Cells[row + 1, column + 1].Value;
            }
            return ERROR;
        }

        /*################# ReadColumn ##################################
        # Purpose: Returns a list of values in a given column
        #
        ###############################################################*/
        public Object[] ReadColumn(int column)
        {
            Object[] data = new Object[CountRows()];

            if (__readyToGo[2])
            {
                for (int i = __workSheet.Dimension.Start.Row;
                         i <= __workSheet.Dimension.End.Row;
                         i++)
                {
                    data[i - 1] = ReadCell(column - 1, i - 1);
                }

            }
            return data;
        }

        /*################# ReadFile ####################################
        # Purpose: Returns a matrix of all cells in a worksheet
        #
        ###############################################################*/
        public Object[][] ReadFile()
        {
            Object[][] data = new Object[CountColumns()][];

            if (__readyToGo[2])
            {
                for (int j = __workSheet.Dimension.Start.Column; j <= __workSheet.Dimension.End.Column; j++)
                {
                    data[j - 1] = new Object[CountRows(j)];
                    for (int i = __workSheet.Dimension.Start.Row; i <= __workSheet.Dimension.End.Row; i++)
                    {
                        data[j - 1][i - 1] = ReadCell(j - 1, i - 1);
                    }
                }
            }
            return data;
        }

        /*################# ReadRow #####################################
        # Purpose: Returns a list of values in a given row
        #
        ###############################################################*/
        public Object[] ReadRow(int row)
        {
            Object[] data = new Object[CountColumns()];

            if (__readyToGo[2])
            {

                for (int j = __workSheet.Dimension.Start.Column; j <= __workSheet.Dimension.End.Column; j++)
                {
                    data[j - 1] = ReadCell(j - 1, row - 1);
                }
            }
            return data;
        }

        /*################# SavePackage #################################
        # Purpose: Saves changes to the Excel file
        #          
        # Inputs:  int - last line written to
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        private void SavePackage()
        {
            if (__readyToGo[1])
            {
                try
                {
                    __package.Save();
                }
                catch (Exception e)
                {
                    Console.WriteLine("SavePackage: " + e);
                }
            }
        }

        /*################# SetBorder ###################################
        # Purpose: Sets a border of given thickness over a range of cells
        #
        ###############################################################*/
        public void SetBorder(int thickness, ExcelRange data)
        {
            if (__readyToGo[2])
            {
                ExcelBorderStyle thicks;
                switch (thickness)
                {
                    case 0: thicks = ExcelBorderStyle.None; break;
                    case 1: thicks = ExcelBorderStyle.Thin; break;
                    case 2: thicks = ExcelBorderStyle.Medium; break;
                    case 3: thicks = ExcelBorderStyle.Thick; break;
                    default: thicks = (ExcelBorderStyle)(ERROR); break;  //Invalid option
                }
                data.Style.Border.Top.Style = thicks;
                data.Style.Border.Left.Style = thicks;
                data.Style.Border.Right.Style = thicks;
                data.Style.Border.Bottom.Style = thicks;
            }
        }

        /*################# WriteCell ###################################
        # Purpose: Writes a value to a given cell
        #
        ###############################################################*/
        public void WriteCell(Object data, int column, int row, bool save)
        {
            if (__readyToGo[2])
            {
                __workSheet.Cells[row + 1, column + 1].Value = data;
            }
            if (save)
            {
                SavePackage();
            }
        }

        /*################# WriteColumn #################################
        # Purpose: Writes a list of data to a given column
        #
        ###############################################################*/
        public void WriteColumn(Object[] data, int column, int row)
        {
            if (__readyToGo[2])
            {
                for (int i = 0; i < data.Length; i++)
                {
                    WriteCell(data[i], column, i + row, false);
                }
                SavePackage();
            }
        }

        /*################# WriteFile ###################################
        # Purpose: Writes a matrix of data to a given worksheet
        #
        ###############################################################*/
        public void WriteFile(Object[][] data, int column, int row)
        {
            if (__readyToGo[2])
            {
                for (int i = 0; i < data.Length; i++)
                {
                    for (int j = 0; j < data[i].Length; j++)
                    {
                        WriteCell(data[i][j], j + column, i + row, false);
                    }
                }
                SavePackage();
            }
        }

        /*################# WriteRow ####################################
        # Purpose: Writes a list of data to a given row
        #
        ###############################################################*/
        public void WriteRow(Object[] data, int column, int row)
        {
            if (__readyToGo[2])
            {
                for (int j = 0; j < data.Length; j++)
                {
                    WriteCell(data[j], j + column, row, false);
                }
                SavePackage();
            }
        }

        /*################# WriteToTopOfCol ##################################
        # Purpose: Writes a given message to the Excel file at a given 
        #           column
        #
        # Inputs:  String - text to be written
        # 	       int - column number at which to write
        #
        # Outputs: None
        #          
        ###############################################################*/
        public void WriteToTopOfCol(Object val, int columnNumber)
        {
            if (__readyToGo[2])
            {
                __workSheet.InsertRow(2, 1);
                WriteCell(val, 2, columnNumber, true);
            }
        }


        /*################# Constructors ##############################*/

        /*################# Excel #######################################
        # Purpose: Contructor for Excel class
        #          
        # Inputs:  String - file in which to work
        # 	   
        # Outputs: Excel - object
        #          
        ###############################################################*/
        public Excel(String fileName, String worksheetName)
        {
            CreateFile(fileName);
            CreatePackage();
            CreateWorksheet(worksheetName);
        }

        /*################# Excel #####################################
        # Purpose: Contructor for Excel class when worksheet is unknown
        #
        ###############################################################*/
        public Excel(String fileName)
        {
            CreateFile(fileName);
            CreatePackage();
            SavePackage();
        }
    }
}
