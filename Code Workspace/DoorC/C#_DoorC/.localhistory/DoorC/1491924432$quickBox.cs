using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorC
{
    class quickBox
    {
        BluetoothClient client;
        int num;
        private quickBox(BluetoothClient BC, int numb)
        {
            client = BC;
            num = numb;
        }
    }
}
