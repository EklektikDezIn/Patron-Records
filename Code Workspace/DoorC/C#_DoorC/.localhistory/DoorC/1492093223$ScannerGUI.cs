using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorC
{
    
         
    class ScannerGUI
    {
        int[] dtxtp = { 20, 18, 0 };
        public ScannerGUI(){
        txtRun.Add(new TextBox());
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
                return 0;
    }
    }
}
