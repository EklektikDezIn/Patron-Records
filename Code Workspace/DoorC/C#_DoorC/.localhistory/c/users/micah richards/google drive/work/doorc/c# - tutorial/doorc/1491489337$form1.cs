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
using OfficeOpenXml;

namespace DoorC
{  
    public partial class Form1 : Form
    {
        
        List<string> items;
        public Form1()
        {
            items = new List<string>();
            InitializeComponent();
            createFile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            startScan();
        }

        private void startScan()
        {
            items.Clear();
            listBox1.DataSource = null;
            listBox1.Items.Clear();
            Thread bluetoothScanThread = new Thread(new ThreadStart(scan));
            bluetoothScanThread.Start();
        }

        BluetoothDeviceInfo[] devices;

        private void scan()
        {
            updateUI("Starting Scan.." + System.Environment.NewLine);
            BluetoothClient client = new BluetoothClient();
            devices = client.DiscoverDevicesInRange();
            updateUI("Scan Complete" + System.Environment.NewLine);
            updateUI(devices.Length.ToString() + " devices discovered" + System.Environment.NewLine);
            foreach (BluetoothDeviceInfo d in devices)
            {
                items.Add(d.DeviceName);
            }
            updateDeviceList();
        }

        private void updateUI(string message)
        {
            Func<int> del = delegate()
            {
                textBox1.AppendText(message);
                return 0;
            };
            if (!IsHandleCreated) { 
                this.CreateControl();
            }
           try
            {
                Invoke(del);
            }
           catch (ObjectDisposedException e)
           {
               Console.Out.WriteLine("AppendText: " +e);
           }
        }

        private void updateDeviceList()
        {
            Func<int> del = delegate()
            {
                listBox1.DataSource = items;
                return 0;
            };
            Invoke(del);
        }
         int[] dtxtp = {20,18};
        private void addBox(String name)
        {
            Func<int> del = delegate()
            {
                TextBox txtRun = new TextBox();
                txtRun.Name = "txtDynamic";
                txtRun.Location = new System.Drawing.Point(dtxtp[0], dtxtp[1]);
                txtRun.Size = new System.Drawing.Size(200, 25);
                // Add the textbox control to the form's control collection         
                this.Controls.Add(txtRun);
                dtxtp[0] += 200;
                return 0;
            };
            Invoke(del);
        }

        BluetoothDeviceInfo deviceInfo;
        int cell = 1;
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            deviceInfo = devices.ElementAt(listBox1.SelectedIndex);
            updateUI(deviceInfo.DeviceName + " was selected, attempting connect" + System.Environment.NewLine);

            if (pairDevice())
            {
                updateUI("device paired.." + System.Environment.NewLine);
                updateUI("starting connect thread" + System.Environment.NewLine);
                Thread bluetoothClientThread = new Thread(new ThreadStart(ClientConnectThread));
                bluetoothClientThread.Start();

            }
            else
            {
                updateUI("Pair failed" + System.Environment.NewLine);
            }

        }

        Guid mUUID = new Guid("00001101-0000-1000-8000-00805F9B34FB");
        private void ClientConnectThread()
        {
            BluetoothClient client = new BluetoothClient();
            updateUI("attempting connect" + System.Environment.NewLine);
            client.BeginConnect(deviceInfo.DeviceAddress, mUUID, this.BluetoothClientConnectCallback, client);
        }
        private static NetworkStream stream = null;
      //  String[] message = new String[] { "", "", "" };
//
      //  int i = 3;
        static FileInfo newFile = new FileInfo(@"C:\Users\Micah Richards\Google Drive\Work\DoorC\sample.xlsx");
        ExcelPackage pck;

        private void createFile()
        {
            try
            {
                pck = new ExcelPackage(newFile);
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Please make sure \"" + newFile + "\" is not currently open", "Error");
                Application.Exit();
                this.Close();
            }
        }
      
        void BluetoothClientConnectCallback(IAsyncResult result)
        {
          
           var ws = pck.Workbook.Worksheets.SingleOrDefault(x => x.Name == "Content");
           if (ws == null)
           {
               ws = pck.Workbook.Worksheets.Add("Content");
           }
            BluetoothClient client = (BluetoothClient)result.AsyncState;
            if (result.IsCompleted)
            {
                updateUI("client is connected now :)" + System.Environment.NewLine);
                Console.WriteLine(client.Connected);
                
                while (client.Connected)
                {
                    stream = client.GetStream();
                    if (stream.CanRead)
                    {

                        byte[] myReadBuffer = new byte[1024];
                        StringBuilder myCompleteMessage = new StringBuilder();
                        int numberOfBytesRead = 0;

                        // Incoming message may be larger than the buffer size. 
                        do
                        {
                            numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);

                            myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
                        }
                        
                        
                        while (stream.DataAvailable);
                        int number;
                  //      message[2] = message[1];
                 //       message[1] = message[0];
                  //      message[0] = myCompleteMessage.ToString();
                  //      if (i>=3 && (message[2] + message[1] + message[0]).Contains("Patron"))
                 //           {
                 //               System.Console.WriteLine(System.DateTime.Now.ToString());
                        List<string> msg = myCompleteMessage.ToString().Split("\r\n".ToCharArray()).ToList();
                        msg.RemoveAll(isBlank);

                        foreach (string dist in msg)
                        {
                            int.TryParse(dist, out number);
                            if (number < trigger)
                            {
                                if (isPerson) { 
                                    Console.WriteLine(0);
                                }
                                    stand++;
                            }
                            else
                            {
                                if (!isPerson)
                                {
                                    Console.WriteLine(150);
                                }
                                    pass++;
                            }
                        }

                        if (!isPerson)
                        {
                            if (stand > 50)
                            {
                                isPerson = true;
                                pass = 0;
                                stand = 0;
                            }
                            if (pass > 200)
                            {
                                pass = 0;
                                stand = 0;
                            }
                        }
                        if (isPerson)
                        {
                            if (pass > 50)
                            {
                                isPerson = false;
                                pass = 0;
                                stand = 0;
                                ws.InsertRow(1, 1);
                                ws.Cells["A1"].Value = DateTime.Now.ToString("h:mm:ss tt");
                                Console.WriteLine("THIS IS THE PLACE WHERE THE IMPORTANT INFORMATION IS AND THIS LINE IS LONG");// + DateTime.Now.ToString("h:mm:ss tt"));
                                pck.Save();
                            }
                            if (stand > 500)
                            {
                                pass = 0;
                                stand = 0;
                                
                            }
                        }
                        
                            
                        
                        // Print out the received message to the console.
                        updateUI("" + myCompleteMessage + System.Environment.NewLine);
                        
                    }
                    else
                    {
                        updateUI("Sorry.  You cannot read from this NetworkStream." + System.Environment.NewLine);
                    }
                }
                Console.ReadLine();
            }

        }
        int trigger = 10;
        int stand = 0;
        int pass = 0;
        bool isPerson = false;

        private static bool isBlank(String s)
        {
            return s.ToLower().Equals("");
        }

        string myPin = "1234";
        private bool pairDevice()
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

        private void Form1_Load(object sender, EventArgs e)
        {
            startScan();
        }

    }
}
