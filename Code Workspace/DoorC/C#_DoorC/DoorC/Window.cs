/*################# Window ######################################
# Eklektik Design
# Micah Richards
# 07/11/17
# Purpose: Class constructor to handle all connections and
#           interactions with users and peripheral devices
#           for DoorC
#
###############################################################*/


/*################# Libraries #################################*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoorC
{
    public class Window
    {
        /*################# Variables #################################*/
        private int[] IDnumber = new int[2];        //## identifies window in group
        private int Pass;                           //## tracks number of 'no object' readings
        private int RCP;                            //## tracks selected radio button (Raw Clean Patron)
        private int Stand;                          //## tracks number of 'object' readings
        private int TriggerVal;                     //## stores tipping point between 'object' and 'no object' readings
        private int TriggerCountValid;              //## stores number of Stand readings required to register patron
        private int TriggerCountReset;              //## stores number of Pass readings required to return to waiting state
        private int HorizSpace;                     //## stores list of spacing metrics for better consistency
        AverageNum Laser = new AverageNum(1000);    //## maintains an average value of the Laser light readings
        AverageNum Ambient = new AverageNum(10);    //## maintains an average value of the Ambient light readings

        private MainWindow Home;                    //## link to MainWindow parent

        private DoorCBT BlueTooth;                  //## handles bluetooth connections

        private GroupBox Group;                     //## holds the controls

        private TextBox Output;                     //## displays data

        private Label TriggerPointName;             //## labels trigger point slider
        private Label TriggerPointValue;            //## displays current trigger point value
        private TrackBar TriggerPointScroll;        //## scrolls to set trigger point value

        private Label TriggerCountValidName;        //## labels duration of validity slider
        private Label TriggerCountValidValue;       //## displays duration of validity value
        private TrackBar TriggerCountValidScroll;   //## scrolls to set duration of validity value

        private Label TriggerCountResetName;        //## labels duration of reset slider
        private Label TriggerCountResetValue;       //## displays current duration of reset value
        private TrackBar TriggerCountResetScroll;   //## scrolls to set duration of reset value

        private Label AmbientLightName;             //## labels ambient light reading
        private Label AmbientLightValue;            //## displays ambient light reading

        private Label LaserLightName;               //## labels laser light reading
        private Label LaserLightValue;              //## displays laser light reading

        private RadioButton RadioRaw;               //## set to display raw sensor data
        private RadioButton RadioClean;             //## set to display refined sensor data
        private RadioButton RadioPatron;            //## set to only display registered patron

        private Button Disconnect;                  //## deletes this Window


        /*################# Public Functions ##########################*/

        /*################# Window ######################################
        # Purpose: Constructor for Window Class
        #          
        # Inputs:  Form1 - connect to root window
        # 	       int   - distinguish window from others
        #
        # Outputs: Window - object
        #          
        ###############################################################*/
        public Window(MainWindow home, int posID, int ID)
        {
            Home = home;
            IDnumber[0] = posID;
            IDnumber[1] = ID;

            HorizSpace = 5 + IDnumber[0] * 220;

            TriggerVal = 350;
            TriggerCountValid = 100;
            TriggerCountReset = 500;
            Stand = 50;
            Pass = 200;

            InitGUIItems();
            AddGUIItemsToForm();

            BlueTooth = new DoorCBT(this);
            BlueTooth.StartScan();
        }

        /*################# WriteToExcelData ############################
        # Purpose: Passes Excel cell data to the root Excel handler
        #          
        # Inputs:  String - data to be written
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void WriteToExcelData(String msg)
        {
            Home.WriteToExcelData(msg, IDnumber[1]);
        }

        /*################# WriteToExcelTitle ############################
        # Purpose: Passes Excel Title to the root Excel handler
        #          
        # Inputs:  String - title to be written
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void WriteToExcelTitle(String msg)
        {
            Home.WriteToExcelTitle(msg, IDnumber[1]);
        }


        /*################# Get/set Functions #########################*/

        /*################# GetTriggerVal ###############################
        # Purpose: Handles getting the TriggerVal value
        #          
        # Inputs:  None
        # 	   
        # Outputs: int - current TriggerVal value
        #          
        ###############################################################*/
        public int GetTriggerVal()
        {
            return TriggerVal;
        }

        /*################# SetTriggerVal ###############################
        # Purpose: Handles setting the TriggerVal value
        #          
        # Inputs:  int - new TriggerVal value
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void SetTriggerVal(int value)
        {
            TriggerVal = value;
        }

        /*################# GetIDnumber #################################
        # Purpose: Handles getting the IDnumber value
        #          
        # Inputs:  None
        # 	   
        # Outputs: int - current IDnumber value
        #          
        ###############################################################*/
        public int[] GetIDnumber()
        {
            return IDnumber;
        }

        /*################# SetIDnumber #################################
        # Purpose: Handles setting the IDnumber value
        #          
        # Inputs:  int - new IDnumber value
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void SetIDnumber(int value)
        {
            IDnumber[0] = value;
            UpdateHorizSpace();
        }

        /*################# GetTriggerCountValid ########################
        # Purpose: Handles getting the TriggerCountValid value
        #          
        # Inputs:  None
        # 	   
        # Outputs: int - current TriggerCountValid value
        #          
        ###############################################################*/
        public int GetTriggerCountValid()
        {
            return TriggerCountValid;
        }

        /*################# SetTriggerCountValid ########################
        # Purpose: Handles setting the TriggerCountValid value
        #          
        # Inputs:  int - new TriggerCountValid value
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void SetTriggerCountValid(int value)
        {
            TriggerCountValid = value;
        }

        /*################# GetTriggerCountReset ########################
        # Purpose: Handles getting the TriggerCountReset value
        #          
        # Inputs:  None
        # 	   
        # Outputs: int - current TriggerCountReset value
        #          
        ###############################################################*/
        public int GetTriggerCountReset()
        {
            return TriggerCountReset;
        }

        /*################# SetTriggerCountReset ########################
        # Purpose: Handles setting the TriggerCountReset value
        #          
        # Inputs:  int - new TriggerCountReset value
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void SetTriggerCountReset(int value)
        {
            TriggerCountReset = value;
        }

        /*################# SetLaserLightValue ##########################
        # Purpose: Handles setting the LaserLightValue value
        #          
        # Inputs:  int - new LaserLightValue value
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void SetLaserLightValue(double value)
        {
            Laser.AddValue(value);
            int Output = (int)Laser.GetValue();
            Home.UpdateLabel(LaserLightValue, Output.ToString());
        }

        /*################# SetAmbientLightValue ########################
        # Purpose: Handles setting the AmbientLightValue value
        #          
        # Inputs:  int - new AmbientLightValue value
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void SetAmbientLightValue(double value)
        {
            Ambient.AddValue(value);
            int Output = (int)Ambient.GetValue();
            Home.UpdateLabel(AmbientLightValue, Output.ToString());
        }

        /*################# GetStand ####################################
        # Purpose: Handles getting the Stand value
        #          
        # Inputs:  None
        # 	   
        # Outputs: int - current Stand value
        #          
        ###############################################################*/
        public int GetStand()
        {
            return Stand;
        }

        /*################# SetStand ####################################
        # Purpose: Handles the setting of the Stand value
        #          
        # Inputs:  int - new Stand value
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void SetStand(int value)
        {
            Stand = value;
        }

        /*################# IncStand ####################################
        # Purpose: Handles incrementing the Stand value
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void IncStand()
        {
            Stand++;
        }

        /*################# GetPass #####################################
        # Purpose: Handles getting of Pass value
        #          
        # Inputs:  None
        # 	   
        # Outputs: int - current Pass value
        #          
        ###############################################################*/
        public int GetPass()
        {
            return Pass;
        }

        /*################# SetPass #####################################
        # Purpose: Handles setting of Pass value
        #          
        # Inputs:  int - new Pass value
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void SetPass(int value)
        {
            Pass = value;
        }

        /*################# IncPass #####################################
        # Purpose: Handles incrementing the Pass value
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void IncPass()
        {
            Pass++;
        }

        /*################# GetRCP ######################################
        # Purpose: Handles getting the Raw-Clean-Patron (RCP) value
        #          
        # Inputs:  None
        # 	   
        # Outputs: int - current RCP value
        #          
        ###############################################################*/
        public int GetRCP()
        {
            return RCP;
        }

        /*################# SetRCP ######################################
        # Purpose: Handles setting the RCP value
        #          
        # Inputs:  int - new RCP value
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void SetRCP(int value)
        {
            RCP = value;
        }


        /*################# Forwarding Functions ######################*/

        /*################# StartBluetoothScan ##########################
        # Purpose: Passes 'start scan' trigger to the Bluetooth handler
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void StartBluetoothScan()
        {
            BlueTooth.StartScan();
        }

        /*################# TryToPairBluetooth ##########################
        # Purpose: Passes 'start pairing' trigger to the Bluetooth 
        #           handler
        #
        # Inputs:  int - listbox ID of device to connect to
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void TryToPairBluetooth(int device)
        {
            BlueTooth.TryToPair(device);
        }

        /*################# UpdateOutput ################################
        # Purpose: Forwards message for Output to MainWindow
        #          
        # Inputs:  String - message to append
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void UpdateOutput(String msg)
        {
            Home.UpdateTextBox(Output, msg);
        }

        /*################# UpdateDeviceList ############################
        # Purpose: Refreshes the DeviceList with a new list of items
        #          
        # Inputs:  List<String> - collection of items to display
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void UpdateDeviceList(List<String> items)
        {
            Home.UpdateDeviceList(items);
        }


        /*################# Private Functions #########################*/

        /*################# AddGUIItemsToForm ###########################
        # Purpose: Passes all local controls to MainWindow
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        private void AddGUIItemsToForm()
        {
            Group.Controls.Add(Output);

            Group.Controls.Add(Disconnect);

            Group.Controls.Add(TriggerPointName);
            Group.Controls.Add(TriggerPointValue);
            Group.Controls.Add(TriggerPointScroll);

            Group.Controls.Add(TriggerCountValidName);
            Group.Controls.Add(TriggerCountValidValue);
            Group.Controls.Add(TriggerCountValidScroll);

            Group.Controls.Add(TriggerCountResetName);
            Group.Controls.Add(TriggerCountResetValue);
            Group.Controls.Add(TriggerCountResetScroll);

            Group.Controls.Add(AmbientLightName);
            Group.Controls.Add(AmbientLightValue);

            Group.Controls.Add(LaserLightName);
            Group.Controls.Add(LaserLightValue);

            Group.Controls.Add(RadioRaw);
            Group.Controls.Add(RadioClean);
            Group.Controls.Add(RadioPatron);

            Home.AddStaticGroupBox(Group);
        }

        /*################# DeleteThis ##################################
        # Purpose: Deletes this window object
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        private void DeleteThis()
        {
            BlueTooth.CloseConnection();
            Home.DeleteWindow(GetIDnumber()[0], Group);
        }

        /*################# InitGUIItems ################################
        # Purpose: Creates local controls
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        private void InitGUIItems()
        {
            List<System.Drawing.Size> Sizes = new List<System.Drawing.Size>
            {
                new System.Drawing.Size(200, 395),  //## 0 - Textbox
                new System.Drawing.Size(75, 25),    //## 1 - Button
                new System.Drawing.Size(100, 13),   //## 2 - Label Name
                new System.Drawing.Size(25, 13),    //## 3 - Label Value
                new System.Drawing.Size(185, 45),   //## 4 - TrackBar
                new System.Drawing.Size(56, 17),    //## 5 - RadioButton
                new System.Drawing.Size(210, 680)   //## 6 - GroupBox
            };

            Group = new GroupBox()
            {
                Size = Sizes[6],
                Location = new System.Drawing.Point(HorizSpace, 0)
            };

            Output = new TextBox()
            {
                Name = "Output" + IDnumber,
                Location = new System.Drawing.Point(5, 10),
                Size = Sizes[0],
                Multiline = true
            };


            TriggerPointName = new Label()
            {
                Name = "TriggerPointName" + IDnumber,
                Location = new System.Drawing.Point(5, 410),
                Size = Sizes[2],
                Text = "Trigger Point"
            };

            TriggerPointValue = new Label()
            {
                Name = "TriggerPointValue" + IDnumber,
                Location = new System.Drawing.Point(175, 422),
                Size = Sizes[3],
                Text = "10"
            };

            TriggerPointScroll = new TrackBar()
            {
                Name = "TriggerPointScroll" + IDnumber,
                Location = new System.Drawing.Point(5, 422),
                Size = Sizes[4],
                Maximum = 1000,
                Value = 10
            };

            TriggerPointScroll.Scroll += (s, e) =>
            {
                TriggerVal = TriggerPointScroll.Value;
                TriggerPointValue.Text = TriggerVal.ToString();
            };


            TriggerCountValidName = new Label()
            {
                Name = "TriggerCountValidName" + IDnumber,
                Location = new System.Drawing.Point(5, 465),
                Size = Sizes[2],
                Text = "Duration of Validity"
            };

            TriggerCountValidValue = new Label()
            {
                Name = "TriggerCountValidValue" + IDnumber,
                Location = new System.Drawing.Point(175, 477),
                Size = Sizes[3],
                Text = "10"
            };

            TriggerCountValidScroll = new TrackBar()
            {
                Name = "TriggerCountValidScroll" + IDnumber,
                Location = new System.Drawing.Point(5, 477),
                Size = Sizes[4],
                Maximum = 1000,
                Value = 10
            };

            TriggerCountValidScroll.Scroll += (s, e) =>
            {
                TriggerCountValid = TriggerCountValidScroll.Value;
                TriggerCountValidValue.Text = TriggerCountValid.ToString();
            };


            TriggerCountResetName = new Label()
            {
                Name = "TriggerCountPassName" + IDnumber,
                Location = new System.Drawing.Point(5, 520),
                Size = Sizes[2],
                Text = "Duration of Reset"
            };

            TriggerCountResetValue = new Label()
            {
                Name = "TriggerCountPassValue" + IDnumber,
                Location = new System.Drawing.Point(175, 532),
                Size = Sizes[3],
                Text = "10"
            };

            TriggerCountResetScroll = new TrackBar()
            {
                Name = "TriggerCountPassScroll" + IDnumber,
                Location = new System.Drawing.Point(5, 532),
                Size = Sizes[4],
                Maximum = 1000,
                Value = 10
            };

            TriggerCountResetScroll.Scroll += (s, e) =>
            {
                TriggerCountReset = TriggerCountResetScroll.Value;
                TriggerCountResetValue.Text = TriggerCountReset.ToString();
            };

            AmbientLightName = new Label()
            {
                Name = "AmbientLightName" + IDnumber,
                Location = new System.Drawing.Point(25, 580),
                Size = Sizes[2],
                Text = "Ambient Light"
            };

            AmbientLightValue = new Label()
            {
                Name = "AmbientLightValue" + IDnumber,
                Location = new System.Drawing.Point(25 + Sizes[2].Width + 10, 580),
                Size = Sizes[3],
                Text = "Null"
            };

            LaserLightName = new Label()
            {
                Name = "LaserLightName" + IDnumber,
                Location = new System.Drawing.Point(25, 595),
                Size = Sizes[2],
                Text = "Laser Light"
            };

            LaserLightValue = new Label()
            {
                Name = "LaserLightValue" + IDnumber,
                Location = new System.Drawing.Point(25 + Sizes[2].Width + 10, 595),
                Size = Sizes[3],
                Text = "Null"
            };

            RadioRaw = new RadioButton()
            {
                Name = "RadioRaw" + IDnumber,
                Location = new System.Drawing.Point(5 + 10, 620),
                Size = Sizes[5],
                Text = "Raw",
                Checked = true
            };

            RadioRaw.CheckedChanged += (s, e) =>
            {
                RCP = 0;
            };


            RadioClean = new RadioButton()
            {
                Name = "RadioClean" + IDnumber,
                Location = new System.Drawing.Point(5 + 70, 620),
                Size = Sizes[5],
                Text = "Clean"
            };

            RadioClean.CheckedChanged += (s, e) =>
            {
                RCP = 1;
            };


            RadioPatron = new RadioButton()
            {
                Name = "RadioPatron" + IDnumber,
                Location = new System.Drawing.Point(5 + 130, 620),
                Size = Sizes[5],
                Text = "Patron"
            };

            RadioPatron.CheckedChanged += (s, e) =>
            {
                RCP = 2;
            };


            Disconnect = new Button()
            {
                Name = "Disconnect" + IDnumber,
                Location = new System.Drawing.Point(5, 650),
                Size = Sizes[1],
                Text = "Disconnect"
            };

            Disconnect.Click += (s, e) =>
            {
                DeleteThis();
            };
        }

        /*################# UpdateHorizSpace ############################
        # Purpose: Updates HorizSpace after IDnumber is changed
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        private void UpdateHorizSpace()
        {
            HorizSpace = 5 + IDnumber[0] * 220;
            Group.Location = new System.Drawing.Point(HorizSpace, 0);
        }
    }
}