/*################# Bluetooth #######################################
# Eklektik Design
# Micah Richards
# 07/11/17
# Purpose: Handles Bluetooth interactions
#           uses reference InTheHand.net
#
###############################################################*/


/*################# Libraries #################################*/
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoorC
{
    class Bluetooth
    {
        /*################# Variables #################################*/
        private List<Thread> Connections = new List<Thread>();  //## list of available connections
        private BluetoothDeviceInfo deviceInfo;                 //## information on selected device
        Thread bluetoothScanThread;
        private BluetoothDeviceInfo[] devices;                  //## list of devices
        private NetworkStream stream = null;                    //## incoming data
        private Guid mUUID = new Guid("00001101-0000-1000-8000-00805F9B34FB");
        //## guid for bluetooth connection
        private int currentConnection = 0;                      //## tracks connection ID
        private string myPin = "1234";                          //## pin for HC-05 authetication
        private List<string> items = new List<string>();        //## parsed data
        bool exitflagset = false;


        /*################# Public Functions ##########################*/

        /*################# Bluetooth ###################################
        # Purpose: Constructor for Bluetooth class
        #          
        # Inputs:  None
        # 	   
        # Outputs: Bluetooth - object
        #          
        ###############################################################*/
        public Bluetooth()
        {
        }

        /*################# GetDeviceInfo ###############################
        # Purpose: Handles getting the deviceinfo property
        #          
        # Inputs:  None
        # 	   
        # Outputs: BluetoothDeviceInfo - object
        #          
        ###############################################################*/
        public BluetoothDeviceInfo GetDeviceInfo()
        {
            return deviceInfo;
        }

        /*################# CloseConnection #############################
        # Purpose: Closes connection with Bluetooth device
        #          
        # Inputs:  None
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void CloseConnection()
        {
            exitflagset = true;
        }

        /*################# StartScan ###################################
        # Purpose: Initiates Bluetooth Scan procedure
        #          
        # Inputs:  None
        # 	   
        # Outputs: Bluetooth thread
        #          
        ###############################################################*/
        public void StartScan()
        {
            bluetoothScanThread = new Thread(new ThreadStart(Scan));
            bluetoothScanThread.Start();
        }

        /*################# TryToPair ###################################
        # Purpose: Attempts to pair current device with selected
        #           Bluetooth device
        #
        # Inputs:  int - DeviceList ID of selected device
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        public void TryToPair(int selected)
        {
            deviceInfo = devices.ElementAt(selected);
            OutputStatus(deviceInfo.DeviceName + " was selected, attempting connect" + System.Environment.NewLine);

            if (PairDevice())
            {
                OutputStatus("device paired.." + System.Environment.NewLine);
                OutputDeviceName(deviceInfo.DeviceName);
                OutputStatus("starting connect thread" + System.Environment.NewLine);
                Connections.Add(new Thread(() => ClientConnectThread(GetID())));
                Connections[Connections.Count - 1].IsBackground = true;
                Connections[Connections.Count - 1].Start();
            }
            else
            {
                OutputStatus("Pair failed" + System.Environment.NewLine);
            }

        }


        /*################# Extendable Functions ######################*/

        /*################# GetID #######################################
      # Purpose: Returns incrementing ID for connections
      #          
      # Inputs:  None
      # 	 
      # Outputs: int - ID number
      #          
      ###############################################################*/
        public virtual int GetID()
        {
            currentConnection += 1;
            return currentConnection;
        }

        /*################# OutputDeviceName ############################
        # Purpose: Forwards detected device name to class extension
        #          
        # Inputs:  String - connected device name
        # 	 
        # Outputs: None
        #          
        ###############################################################*/
        public virtual void OutputDeviceName(String title)
        {
            //## Override this function in local class
        }

        /*################# OutputScan ##################################
        # Purpose: Forwards detected device list to class extension
        #          
        # Inputs:  List<String> - detected devices
        # 	 
        # Outputs: None
        #          
        ###############################################################*/
        public virtual void OutputScan(List<String> items)
        {
            //## Override this function in local class
        }

        /*################# OutputStatus ################################
        # Purpose: Forwards status information to class extension
        #          
        # Inputs:  String - message to be sent
        # 	 
        # Outputs: None
        #          
        ###############################################################*/
        public virtual void OutputStatus(String msg)
        {
            //## Override this function in local class
        }

        /*################# WorkWithData ################################
        # Purpose: Forwards data to class extension for processing
        #          
        # Inputs:  String - message to be sent
        # 	 
        # Outputs: None
        #          
        ###############################################################*/
        public virtual void WorkWithData(String msg)
        {
            //## Override this function in local class
        }


        /*################# Private Functions #########################*/

        /*################# BluetoothClientConnectCallback ##############
        # Purpose: Receives data from target Bluetooth device
        #          
        # Inputs:  IAsyncResult - status of asynchronous bluetooth
        # 	        operation
        #
        # Outputs: None
        #          
        ###############################################################*/
        private void BluetoothClientConnectCallback(IAsyncResult result)
        {
            QuickBox cct = (QuickBox)result.AsyncState;
            int thr = cct.GetNum() - 1;
            OutputStatus("Connected." + System.Environment.NewLine);

            Console.WriteLine(result.IsCompleted);
            BluetoothClient client = cct.GetClient();
            if (result.IsCompleted)
            {
                Console.WriteLine(client.Connected);

                while (client.Connected)
                {
                    stream = client.GetStream();
                    if (stream.CanRead)
                    {

                        byte[] myReadBuffer = new byte[1024];
                        StringBuilder myCompleteMessage = new StringBuilder();
                        int numberOfBytesRead = 0;

                        //###### Incoming message may be larger than the buffer size. ######//
                        do
                        {
                            numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);

                            myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
                        }


                        while (stream.DataAvailable);
                        WorkWithData(myCompleteMessage.ToString());
                        Thread.Sleep(100);
                    }
                    if (exitflagset)
                    {
                        stream.Close();
                        break;
                    }
                }
            }
            else
            {
                OutputStatus("Sorry.  You cannot read from this NetworkStream." + System.Environment.NewLine);
            }
            Console.ReadLine();
        }

        /*################# ClientConnectThread #########################
        # Purpose: Initiates connection handshake between current device
        #           and target Bluetooth device
        #
        # Inputs:  int - thread number
        # 	   
        # Outputs: None
        #          
        ###############################################################*/
        private void ClientConnectThread(int thr)
        {
            BluetoothClient client = new BluetoothClient();
            QuickBox cct = new QuickBox(client, thr);
            OutputStatus("attempting connect" + System.Environment.NewLine);
            client.BeginConnect(deviceInfo.DeviceAddress, mUUID, this.BluetoothClientConnectCallback, cct);
        }

        /*################# PairDevice ##################################
       # Purpose: Attempts authetification with target bluetooth device
       #          
       # Inputs:  None
       # 	 
       # Outputs: Boolean - is successful
       #          
       ###############################################################*/
        private bool PairDevice()
        {
            if (!deviceInfo.Authenticated)
            {
                if (!BluetoothSecurity.PairRequest(deviceInfo.DeviceAddress, myPin))
                {
                    return false;
                }
            }
            return true;
        }

        /*################# Scan ########################################
        # Purpose: Scans vicinity for available Bluetooth connections
        #          
        # Inputs:  None
        # 	   
        # Outputs: Updates DeviceList
        #          
        ###############################################################*/
        private void Scan()
        {
            items.Clear();
            OutputStatus("Starting Scan.." + System.Environment.NewLine);
            try
            {
                BluetoothClient client = new BluetoothClient();
                devices = client.DiscoverDevicesInRange();
                OutputStatus("Scan Complete" + System.Environment.NewLine);
                OutputStatus(devices.Length.ToString() + " devices discovered" + System.Environment.NewLine);
                foreach (BluetoothDeviceInfo d in devices)
                {
                    items.Add(d.DeviceName);
                }

                OutputScan(items);
            }
            catch (System.PlatformNotSupportedException e)
            {
                System.Windows.Forms.MessageBox.Show("I'm sorry, This computer doesn't support Bluetooth", "Error");
                OutputStatus("Please close this window");
            }
        }
    }
}