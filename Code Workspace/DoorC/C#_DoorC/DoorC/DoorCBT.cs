/*################# DoorCBT #######################################
# Eklektik Design
# Micah Richards
# 07/11/17
# Purpose: Extends Bluetooth class for DoorC
#           
###############################################################*/


/*################# Libraries #################################*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorC
{
    class DoorCBT : Bluetooth
    {
        /*################# Variables #################################*/
        private Window window;                                  //## link to Window parent


        /*################# Public Functions ##########################*/

        /*################# DoorCBT #####################################
        # Purpose: Constructor for DoorCBT class
        #          
        # Inputs:  Window - parent class
        # 	   
        # Outputs: DoorCBT - object
        #          
        ###############################################################*/
        public DoorCBT(Window win)
        {
            window = win;
        }

        /*################# GetID #######################################
        # Purpose: Returns Window ID as Bluetooth connection ID
        #          
        # Inputs:  None
        # 	 
        # Outputs: int - ID number
        #          
        ###############################################################*/
        public override int GetID()
        {
            return window.GetIDnumber()[1];
        }

        /*################# OutputDeviceName ############################
       # Purpose: Forwards detected device name to the Window's Excel
       #           handler
       #          
       # Inputs:  String - connected device name
       # 	 
       # Outputs: None
       #          
       ###############################################################*/
        public override void OutputDeviceName(String title)
        {
            window.WriteToExcelTitle(title);
        }

        /*################# OutputScan ##################################
        # Purpose: Forwards detected device list to the Window
        #          
        # Inputs:  List<String> - detected devices
        # 	 
        # Outputs: None
        #          
        ###############################################################*/
        public override void OutputScan(List<String> items)
        {
            window.UpdateDeviceList(items);
        }

        /*################# OutputStatus ##################################
        # Purpose: Forwards status information to the Window
        #          
        # Inputs:  String - message to be sent
        # 	 
        # Outputs: None
        #          
        ###############################################################*/
        public override void OutputStatus(String msg)
        {
            window.UpdateOutput(msg);
        }

        /*################# WorkWithData ##################################
        # Purpose: Forwards data to class extension for processing
        #          
        # Inputs:  String - message to be sent
        # 	 
        # Outputs: None
        #          
        ###############################################################*/
        public override void WorkWithData(String str)
        {
            DataProcessing.DataToPatron(str, window);
        }



    }
}
