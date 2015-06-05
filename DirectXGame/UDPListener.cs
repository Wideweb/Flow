using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TransferingDataLib;

namespace DirectXGame
{
    class UDPListener
    {
        const int BUFFER_SIZE = 1024;

        UdpClient client;
     
        public delegate void ReceivedHandler(IPEndPoint e, byte[] data);
        public event ReceivedHandler Received;
        public delegate void DisconnectHandler();
        public event DisconnectHandler Disconnect;

        public bool Running
        {
            get;
            private set;
        }

        public void Start(int port)
        {
            if (Running)
                return;
            
            client = new UdpClient(port);
            client.BeginReceive(ReceivedCallback, null);
            
            Running = true;
        }

        public void Stop()
        {
            if (!Running)
                return;

            client.Close();
            Running = false;
        }

        private void ReceivedCallback(IAsyncResult ar)
        {
            try
            {
                IPEndPoint remote_ep = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.EndReceive(ar, ref remote_ep);

                if (Received != null)
                    Received(remote_ep, data);

                if (data.Length == 0)
                    onDisconnect();
            }
            catch { }

            if (Running)
            {
                try
                {
                    client.BeginReceive(ReceivedCallback, null);
                }
                catch { }
            }
        }

        private void onDisconnect()
        {
            this.Stop();
            if (Disconnect != null)
                Disconnect();
        }
    }
}
