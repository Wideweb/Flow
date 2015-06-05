using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace DirectXGame.SocketClient
{
    public class Listener
    {
        public delegate void SocketAcceptedHandler(Socket e);
        public event SocketAcceptedHandler Accepted;

        Socket listener;
        public int Port;

        public bool Running
        {
            get;
            private set;
        }

        public Listener()
        {
            Port = 0;
        }

        public void Start(int port)
        {
            if (Running)
                return;

            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
            listener.Bind(new IPEndPoint(0, port));
            listener.Listen(0);

            listener.BeginAccept(AcceptedCallback, null);
            Running = true;
        }

        public void Stop()
        {
            if (!Running)
                return;

            listener.Close();
            Running = false;
        }

        private void AcceptedCallback(IAsyncResult ar)
        {
            try
            {
                Socket s = listener.EndAccept(ar);

                if (Accepted != null)
                    Accepted(s);
            }
            catch
            {
            }

            if(Running)
            {
                try
                {
                    listener.BeginAccept(AcceptedCallback, null);
                }
                catch { }
            }
        }
    }
}
