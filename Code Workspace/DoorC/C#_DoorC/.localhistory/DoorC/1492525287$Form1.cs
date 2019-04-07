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
            addBox();
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
            //    button1.Enabled = false;
            updateUI(dtxtp[2] - 1, "Starting Scan.." + System.Environment.NewLine);
            BluetoothClient client = new BluetoothClient();
            devices = client.DiscoverDevicesInRange();
            updateUI(dtxtp[2] - 1, "Scan Complete" + System.Environment.NewLine);
            updateUI(dtxtp[2] - 1, devices.Length.ToString() + " devices discovered" + System.Environment.NewLine);
            foreach (BluetoothDeviceInfo d in devices)
            {
                items.Add(d.DeviceName);
            }
            updateDeviceList();
            //  button1.Enabled = true;
        }

        private void updateUI(int box, string message)
        {
            Func<int> del = delegate()
            {
                TextBox txb = (TextBox) this.Controls["txtD" + box];
                txb.AppendText(message);
                return 0;
            };
            if (!IsHandleCreated)
            {
                this.CreateControl();
            }
            try
            {
                Invoke(del);
            }
            catch (ObjectDisposedException e)
            {
                Console.Out.WriteLine("AppendText: " + e);
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

        public int[] dtxtp = { 20, 18, 0 };
        private void addBox()
        {
            Func<int> del = delegate()
            {
                this.Controls.Add(new TextBox() { Name = "txtD" + dtxtp[2], Location = new System.Drawing.Point(dtxtp[0], dtxtp[1]), Size = new System.Drawing.Size(200, 400), Multiline = true });
                this.Controls.Add(new TrackBar() { Name = "tbD" + dtxtp[2], Location = new System.Drawing.Point(dtxtp[0], dtxtp[1] + 420), Size = new System.Drawing.Size(190, 30), Maximum = 250, Value = 10 });
                TrackBar t = (TrackBar) this.Controls["tbD" + dtxtp[2]];
                t.Scroll += (s, e) => { var sender = s as TrackBar; Label l = (Label)this.Controls["lbtbD" + (sender as TrackBar).Name.ToString().Substring(3)]; l.Text = (sender as TrackBar).Value.ToString(); };
                this.Controls.Add(new Label() { Name = "lbtbD" + dtxtp[2], Location = new System.Drawing.Point(dtxtp[0] + 190, dtxtp[1] + 420), Size = new System.Drawing.Size(180, 30), Text = "10" });
                this.Controls.Add(new RadioButton() { Name = "rawD" + dtxtp[2], Location = new System.Drawing.Point(dtxtp[0]+20, dtxtp[1] + 440), Size = new System.Drawing.Size(50, 50), Text = "Raw" });
                this.Controls.Add(new RadioButton() { Name = "cleD" + dtxtp[2], Location = new System.Drawing.Point(dtxtp[0]+70, dtxtp[1] + 440), Size = new System.Drawing.Size(60, 50), Text = "Clean" });
                this.Controls.Add(new RadioButton() { Name = "jpaD" + dtxtp[2], Location = new System.Drawing.Point(dtxtp[0]+130, dtxtp[1] + 440), Size = new System.Drawing.Size(90, 50), Text = "Patron" });
               
                trigger.Add(10);
                pass.Add(0);
                stand.Add(0);

                dtxtp[2]++;
                dtxtp[0] += 200;
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

        List<Thread> Connections = new List<Thread>();
        BluetoothDeviceInfo deviceInfo;
        int cell = 1;
        int tcount = 0;
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            deviceInfo = devices.ElementAt(listBox1.SelectedIndex);
            updateUI(dtxtp[2] - 1, deviceInfo.DeviceName + " was selected, attempting connect" + System.Environment.NewLine);

            if (pairDevice())
            {
                updateUI(dtxtp[2] - 1, "device paired.." + System.Environment.NewLine);
                updateUI(dtxtp[2] - 1, "starting connect thread" + System.Environment.NewLine);
                Connections.Add(new Thread(() => ClientConnectThread(tcount)));
                tcount++;
                Connections[Connections.Count - 1].Start();

            }
            else
            {
                updateUI(dtxtp[2] - 1, "Pair failed" + System.Environment.NewLine);
            }

        }

        Guid mUUID = new Guid("00001101-0000-1000-8000-00805F9B34FB");
        private void ClientConnectThread(int thr)
        {
            BluetoothClient client = new BluetoothClient();
            quickBox cct = new quickBox(client, thr);
            updateUI(dtxtp[2] - 1, "attempting connect" + System.Environment.NewLine);
            client.BeginConnect(deviceInfo.DeviceAddress, mUUID, this.BluetoothClientConnectCallback, cct);
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
            quickBox cct = (quickBox)result.AsyncState;
            int thr = cct.getNum();
            int thisbox = dtxtp[2] - 1;
            addBox();
            var ws = pck.Workbook.Worksheets.SingleOrDefault(x => x.Name == "Content");
            if (ws == null)
            {
                ws = pck.Workbook.Worksheets.Add("Content");
            }
            Console.WriteLine(result.IsCompleted);
            BluetoothClient client = cct.getClient();
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
                            if (number < trigger[thr])
                            {
                                if (isPerson)
                                {
                                    Console.WriteLine(thr + ": " + 0);
                                }
                                stand[thr]++;
                            }
                            else
                            {
                                if (!isPerson)
                                {
                                    Console.WriteLine(thr + ": " + 150);
                                }
                                pass[thr]++;
                            }
                        }

                        if (!isPerson)
                        {
                            if (stand[thr] > 50)
                            {
                                isPerson = true;
                                pass[thr] = 0;
                                stand[thr] = 0;
                            }
                            if (pass[thr] > 200)
                            {
                                pass[thr] = 0;
                                stand[thr] = 0;
                            }
                        }
                        if (isPerson)
                        {
                            if (pass[thr] > 50)
                            {
                                isPerson = false;
                                pass[thr] = 0;
                                stand[thr] = 0;
                                ws.InsertRow(1, 1);
                                ws.Cells["A" + thr].Value = DateTime.Now.ToString("h:mm:ss tt");
                                Console.WriteLine(thr + ": " + "THIS IS THE PLACE WHERE THE IMPORTANT INFORMATION IS AND THIS LINE IS LONG");// + DateTime.Now.ToString("h:mm:ss tt"));
                                try
                                {
                                    pck.Save();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(thr + ": " + "pck.Save: " + e);
                                }
                            }
                            if (stand[thr] > 500)
                            {
                                pass[thr] = 0;
                                stand[thr] = 0;

                            }
                        }



                        // Print out the received message to the console.
                        updateUI(thisbox, "" + myCompleteMessage + System.Environment.NewLine);

                    }
                    else
                    {
                        updateUI(thisbox, "Sorry.  You cannot read from this NetworkStream." + System.Environment.NewLine);
                    }
                }
                Console.ReadLine();
            }

        }
        List<int> trigger = new List<int>();
        List<int> stand = new List<int>();
        List<int> pass = new List<int>();
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
