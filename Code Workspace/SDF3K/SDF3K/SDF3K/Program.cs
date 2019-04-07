/*################# Program.cs #######################################
# Eklektik Design
# Micah Richards
# 2/6/2018
#           
# Purpose: Launches SDF3K
#
###############################################################*/

namespace SDF3K
{

    /*################# Libraries #####################################*/
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        [STAThread]

        /*################# IntToObj #######################################
        # Purpose: Launches the SDF3K main window
        #                  
        ###############################################################*/
        internal static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main_Window());
        }
    }
}
