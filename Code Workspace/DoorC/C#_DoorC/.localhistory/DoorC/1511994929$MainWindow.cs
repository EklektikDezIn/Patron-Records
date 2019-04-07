/*################# MainWindow ##################################
# Eklektik Design
# Micah Richards
# 11/10/16
# Purpose: Handles the Main Window for DoorC
#           
###############################################################*/


/*################# Libraries #################################*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InTheHand;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using System.IO;
using System.Net.Sockets;

namespace DoorC
{
    public partial class MainWindow : Form
    {
        /*################# Variables #################################*/
        private Excel excel;                                //## handles interfacing with excel document
        private MainWindow form = null;                     //## self reference
        private List<Window> scanners = new List<Window>(); //## list of Window objects in use
        private int totalconnections = 0;

        /*################# Public Functions ##########################*/

        /*################# MainWindow ##################################
        # Purpose: Constructor for MainWindow class
        #          
        # Inputs:  None
        # 	   
        # Outputs: MainWindow - object
        #          
        ###############################################################*/
        public MainWindow()
        {
            InitializeComponent();
            excel = new Excel("Data");
            Console.Out.WriteLine(DateTime.Now.ToString(@"MM\/dd\/yyyy hh:mm:ss tt"));
            form = this;
        }

        /*################# AddStaticGroupBox ###########################
        # Purpose: Handler for adding a groupbox to the window from
        #           outside MainWindow class
        #
        # Inputs:  Control - the control to be added
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void AddStaticGroupBox(GroupBox groups)
        {
            form.Controls.Add(groups);
        }

        /*################# ClearDeviceList #############################
        # Purpose: Removes all items from DeviceList
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void ClearDeviceList()
        {
            DeviceList.DataSource = null;
            DeviceList.Items.Clear();
        }

        /*################# DeleteWindow ################################
        # Purpose: Removes a window and updates the remaining
        #          
        # Inputs:  Window - window to delete
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void DeleteWindow(int id, GroupBox groups)
        {
            form.Controls.Remove(groups);
            scanners.RemoveAt(id);
            for (int i = 0;i<scanners.Count;i++){
                scanners[i].SetIDnumber(i);
            }
            form.Size = new Size(form.Size.Width - 220, 720);
        }
        
        /*################# UpdateDeviceList ############################
        # Purpose: Updates DeviceList with new items
        #          
        # Inputs:  List<String> - new items
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void UpdateDeviceList(List<String> items)
        {
            Func<int> del = delegate ()
            {
                DeviceList.DataSource = items;
                return 0;
            };
            try
            {
                Invoke(del);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("updateDeviceList: " + e);
            }
        }

        /*################# UpdateTextBox ###############################
        # Purpose: Appends a given message to a given textbox
        #          
        # Inputs:  Textbox - target for displaying text
        # 	       String - text to be displayed
        #
        # Outputs: None
        #          
        ###############################################################*/
        public void UpdateTextBox(TextBox textbox, String message)
        {
            Func<int> del = delegate ()
            {
                try
                {
                    textbox.AppendText(message);
                }
                catch
                {
                }

                return 0;
            };
            if (InvokeRequired)
            {
                Invoke(del);
            }
            else
            {
                del();
            }
        }

        /*################# UpdateLabel ###############################
       # Purpose: Appends a given message to a given Label
       #          
       # Inputs:  Label - target for displaying text
       # 	       String - text to be displayed
       #
       # Outputs: None
       #          
       ###############################################################*/
        public void UpdateLabel(Label label, String message)
        {
            Func<int> del = delegate ()
            {
                label.Text = message;
                return 0;
            };
            try
            {
                Invoke(del);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("updateDeviceList: " + e);
            }
        }

        /*################# WriteToExcelData ############################
        # Purpose: Handles passing data to local Excel object for
        #           writing
        #
        # Inputs:  String - text to write
        # 	       int - line number to write to
        #
        # Outputs: None
        #          
        ###############################################################*/
        public void WriteToExcelData(String msg, int linenumber)
        {
            excel.WriteToCol(msg, linenumber);
        }

        /*################# WriteToExcelTitle ###########################
        # Purpose: Handles passing data to local Excel object for
        #           writing
        #
        # Inputs:  String - text to write
        # 	       int - line number to write to
        #
        # Outputs: None
        #          
        ###############################################################*/
        public void WriteToExcelTitle(String title, int linenumber)
        {
            excel.NameCol(title, linenumber);
        }


        /*################# Private Functions #########################*/

        /*################# ScanButton_Click ############################
        # Purpose: Creates new Window object when the Scan button is
        #           clicked
        #
        # Inputs:  object - sender
        # 	       EventArgs - e
        #
        # Outputs: None
        #          
        ###############################################################*/
        private void ScanButton_Click(object sender, EventArgs e)
        {
            CreateWindow();
        }

        /*################# CreateWindow ################################
        # Purpose: Creates new Window object and starts its Bluetooth's
        #           scan method
        #
        # Inputs:  None
        # 	   
        # Outputs: Creates Window object
        #          
        ###############################################################*/
        private void CreateWindow()
        {
            scanners.Add(new Window(form, scanners.Count, totalconnections - 1));
            form.Size = new Size(form.Size.Width+220, 720);
        }

        /*################# MainWindow_Load #############################
        # Purpose: Creates initial Window object on MainWindow load
        #          
        # Inputs:  object - sender
        # 	       EventArgs - e
        #
        # Outputs: None
        #          
        ###############################################################*/
        private void MainWindow_Load(object sender, EventArgs e)
        {
            totalconnections++;
            CreateWindow();
        }

        /*################# DeviceList_DoubleClick ######################
        # Purpose: Begins scanning for Bluetooth devices with newest
        #           Window's Bluetooth object when DeviceList is double
        #           clicked
        #
        # Inputs:  object - sender
        # 	       EventArgs - e
        #
        # Outputs: None
        #          
        ###############################################################*/
        private void DeviceList_DoubleClick(object sender, EventArgs e)
        {
            if (scanners.Count == 0)
            {
                CreateWindow();
            }
            scanners[scanners.Count - 1].TryToPairBluetooth(DeviceList.SelectedIndex);
            ClearDeviceList();
        }
    }
}
