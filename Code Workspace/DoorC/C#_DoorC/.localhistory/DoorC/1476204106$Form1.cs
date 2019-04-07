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
            listBox1.Items.Clear();
            Thread bluetoothScanThread = new Thread(new ThreadStart(scan));
            bluetoothScanThread.Start();
        }
        BluetoothDeviceInfo[] devices;
        private void scan()
        {
            updateUI("Starting Scan..");
            BluetoothClient client = new BluetoothClient();
            DiscoverDevicesEventArgs = client.DiscoverDevices();
            devices = client.DiscoverDevicesInRange();
            updateUI("Scan Complete");
            updateUI(devices.Length.ToString() + " devices discovered");
            foreach (BluetoothDeviceInfo d in devices)
            {
                items.Add(d.DeviceName);
            }
        }
        private void updateUI(string message)
        {
            textBox1.AppendText(message + System.Environment.NewLine);
        }
    }
}
