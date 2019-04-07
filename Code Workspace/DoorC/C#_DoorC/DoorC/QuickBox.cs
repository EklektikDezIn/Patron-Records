/*################# QuickBox ####################################
# Eklektik Design
# Micah Richards
# 11/10/16
# Purpose: Stores information on a given Bluetooth connection
#           for general purpose
#
###############################################################*/


/*################# Libraries #################################*/
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorC
{
    class QuickBox
    {
        /*################# Variables #################################*/
        private BluetoothClient client;                     //## Handle to a specified Bluetooth Connection
        private int num;                                    //## stores an ID number for the QuickBox


        /*################# Public Functions ##########################*/

        /*################# QuickBox ####################################
        # Purpose: Constructor for QuickBox class
        #          
        # Inputs:  BluetoothClient - device for connection
                   int - ID Number
        # 	   
        # Outputs: QuickBox - object
        #          
        ###############################################################*/
        public QuickBox(BluetoothClient BC, int numb)
        {
            client = BC;
            num = numb;
        }

        /*################# GetClient ###################################
        # Purpose: Handles getting the client value
        #          
        # Inputs:  None
        # 	   
        # Outputs: BluetoothClient - current client value
        #          
        ###############################################################*/
        public BluetoothClient GetClient()
        {
            return client;
        }

        /*################# GetNum ######################################
        # Purpose: Handles getting the num value
        #          
        # Inputs:  None
        # 	   
        # Outputs: int - current num value
        #          
        ###############################################################*/
        public int GetNum()
        {
            return num;
        }
    }
}
