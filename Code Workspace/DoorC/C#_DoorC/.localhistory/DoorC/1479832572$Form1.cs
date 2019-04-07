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
    public partial class Form1 : Form
    {
        List<string> items;
        public Form1()
        {
            items = new List<string>();
            InitializeComponent();
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
            Invoke(del);
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

        BluetoothDeviceInfo deviceInfo;
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
        bool disconnect = false;
        String[] message = new String[] { "", "", "" };

        int i = 0;
        void BluetoothClientConnectCallback(IAsyncResult result)
        {
            BluetoothClient client = (BluetoothClient)result.AsyncState;
            if (result.IsCompleted)
            {
                updateUI("client is connected now :)" + System.Environment.NewLine);
                Console.WriteLine(client.Connected);
                stream = client.GetStream();
                while (!disconnect)
                {
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
                        i++;
                        if (i>=3)
                        {
                            message[2] = message[1];
                            message[1] = message[0];
                            message[0] = myCompleteMessage.ToString();
                        
                        if ((message[2] + message[1] + message[0]).Contains("Patron"))
                        {
                            System.Console.WriteLine(System.DateTime.Now.ToString());
                        }
                            i=0;
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

    }
}
