using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TransferingDataLib;

namespace DirectXGame
{
    class TCPListener
    {
        const int BUFFER_SIZE = 1024;

        Socket listener;
        byte[] buffer;

        public delegate void EndPointReceivedHandler(EndPoint e);
        public event EndPointReceivedHandler Received;

        public bool Running
        {
            get;
            private set;
        }

        public void Start(int port)
        {
            if (Running)
                return;

            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, port));
            listener.Listen(0);

            buffer = new byte[BUFFER_SIZE];
            listener.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceivedCallback, null);
            Running = true;
        }

        public void Stop()
        {
            if (!Running)
                return;

            listener.Close();
            Running = false;
        }

        private void ReceivedCallback(IAsyncResult ar)
        {
            try
            {
                EndPoint remote_ep = new IPEndPoint(IPAddress.Any, 0);
                int rec = listener.EndReceiveFrom(ar, ref remote_ep);
                
                if (Received != null)
                    Received(remote_ep);
            }
            catch { }

            if (Running)
            {
                try
                {
                    listener.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceivedCallback, null);
                }
                catch { }
            }
        }
    }
}
