/*################# Excel #######################################
# Eklektik Design
# Micah Richards
# 07/11/17
# Purpose: Handles work with Excel files for general purpose
#           uses reference EPPlus
#
###############################################################*/


/*################# Libraries #################################*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Windows.Forms;
using System.IO;

namespace DoorC

{
    class Excel
    {
        /*################# Variables #################################*/
        private FileInfo File;                      //## file to be modified
        private ExcelPackage Package;               //## selected package in file
        private ExcelWorksheet WorkSheet;           //## selected worksheet in package


        /*################# Public Functions ##########################*/

        /*################# Excel #######################################
        # Purpose: Contructor for Excel class
        #          
        # Inputs:  String - file in which to work
        # 	   
        # Outputs: Excel - object
        #          
        ###############################################################*/
        public Excel(String filename, String worksheetname)
        {
            CreateFile(filename);
            CreatePackage();
            CreateWorksheet(worksheetname);
            SavePackage();
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
        public void NameCol(String msg, int columnnumber)
        {
            WorkSheet.InsertRow(1, 1);
            WorkSheet.Cells[(char)('A' + columnnumber) + "1"].Value = msg;
            SavePackage();
        }

        /*################# WriteToCol ##################################
        # Purpose: Writes a given message to the Excel file at a given 
        #           column
        #
        # Inputs:  String - text to be written
        # 	       int - column number at which to write
        #
        # Outputs: None
        #          
        ###############################################################*/
        public void WriteToCol(String msg, int columnnumber)
        {
            WorkSheet.InsertRow(2, 1);
            WorkSheet.Cells[(char)('A' + columnnumber) + "2"].Value = msg;
            SavePackage();
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
        private void CreateFile()
        {
            File = new FileInfo(filename);
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
            try
            {
                Package = new ExcelPackage(File);
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Please make sure \"" + File + "\" is not currently open", "Error");
                Application.Exit();
            }
        }

        /*################# CreateWorksheet #############################
        # Purpose: Creates a worksheet in the current package
        #          
        # Inputs:  String - name of worksheet
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        private void CreateWorksheet(String worksheetname)
        {
            if (WorkSheet == null)
            {
                WorkSheet = Package.Workbook.Worksheets.Add(worksheetname + Package.Workbook.Worksheets.Count);
            }
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
            try
            {
                Package.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine("SavePackage: " + e);
            }
        }
    }
}
