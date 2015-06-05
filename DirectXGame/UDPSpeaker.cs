using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using TransferingDataLib;

namespace DirectXGame
{
    class UDPSpeaker
    {
        Socket speaker;
        IPEndPoint ep;
        byte[] buffer;

        public bool Running { get; private set; }

        public UDPSpeaker(int port)
        {
            ep = new IPEndPoint(IPAddress.Broadcast, port);
        }

        public void StartBroadcasting(string text)
        {
            if (Running)
                return;

            speaker = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp);
            speaker.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            speaker.Ttl = 1;
            buffer = Encoding.ASCII.GetBytes(text);
            speaker.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, ep, SendToCallback, null);

            Running = true;
        }

        public void Stop()
        {
            if (!Running)
                return;

            speaker.Close();
            Running = false;
        }

        private void SendToCallback(IAsyncResult ar)
        {
            try
            {
                int sendedBytes = speaker.EndSendTo(ar);
            }
            catch { }

            if (Running)
            {
                try
                {
                    Thread.Sleep(500);
                    speaker.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, ep, SendToCallback, null);
                }
                catch { }
            }
        }

        private IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

    }
}
