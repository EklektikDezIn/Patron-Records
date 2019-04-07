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
    class ScannerGUI
    {
        public int[] dtxtp = { 20, 18, 0 };

        public void ScannerGUI(){

        }

        public TableLayoutPanel create(){
            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.Controls.Add(new TextBox() { Name = "txtD" + dtxtp[2], Location = new System.Drawing.Point(dtxtp[0], dtxtp[1]), Size = new System.Drawing.Size(200, 600), Multiline = true });


            dtxtp[0] += 200;
            dtxtp[2]++;

            return tlp;
        }
    }
}
