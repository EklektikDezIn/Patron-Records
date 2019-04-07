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
        public ScannerGUI(int[] dtxtp){
            textBox tb = new textBox();
            tableLayoutPanel tlp = new tableLayoutPanel();
            txtRun[dtxtp[2]].Name = "txtDynamic" + dtxtp[2];
            txtRun[dtxtp[2]].Location = new System.Drawing.Point(dtxtp[0], dtxtp[1]);
            txtRun[dtxtp[2]].Size = new System.Drawing.Size(200, 600);
            txtRun[dtxtp[2]].Multiline = true;
            // Add the textbox control to the form's control collection         
            this.Controls.Add(txtRun[dtxtp[2]]);
            
            txtRun.Add(new TrackBar());
            txtRun[d]
            dtxtp[0] += 200;
            dtxtp[2]++;
        }
    }
}
