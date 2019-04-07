/*################# Program #####################################
# Eklektik Design
# Micah Richards
# 11/10/16
# Purpose: Entry point for DoorC
#           
###############################################################*/


/*################# Libraries #################################*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoorC
{
    static class Program
    {
        [STAThread]


        /*################# Public Functions ##########################*/

        /*################# Main ########################################
        # Purpose: Entry point for DoorC code
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
            catch (ObjectDisposedException e)
            {
                Console.Out.WriteLine("Main: " + e);
            }
        }
    }
}
