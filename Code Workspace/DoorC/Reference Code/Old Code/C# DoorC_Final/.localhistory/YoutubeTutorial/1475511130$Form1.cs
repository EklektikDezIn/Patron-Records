using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (!mySerialPort.IsOpen)
            {
                mySerialPort.Open();
                tbRX.Text = "port opened :) ";
            }
            else
                tbRX.Text = "port busy :( ";


        }

        private string rxString;
        private void mySerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            rxString = mySerialPort.ReadExisting();
            this.Invoke(new EventHandler(displayText));

        }

        private void displayText(object o, EventArgs e)
        {
            tbRX.AppendText(rxString);
        }
        private void bSend_Click(object sender, EventArgs e)
        {
            mySerialPort.Write(tbTX.Text);
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            tbTX.Clear();
            tbRX.Clear();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mySerialPort.Close();
        }

        private void tbTX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (mySerialPort.IsOpen && checkBox1.Checked)
            {
                char[] ch = new char[1];
                ch[0] = e.KeyChar;
                mySerialPort.Write(ch, 0, 1);
            }
        }
    }
}
