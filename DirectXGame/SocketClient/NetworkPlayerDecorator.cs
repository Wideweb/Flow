using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class NetworkPlayerDecorator : IPlayer
    {
        public IPlayer Player { get; private set; }
        public List<PlayerPart> Parts
        {
            get { return Player.Parts; }
            set { Player.Parts = value; }
        }
        public IPlayer Enemy
        {
            get { return Player.Enemy; }
            set { Player.Enemy = value; }
        }

        Socket client;
        IPEndPoint ep;
        byte[] buffer;
        int timeFromLastSend;
        int sendDelay;

        public event DiedHandler Died;

        public NetworkPlayerDecorator(int port, IPlayer player)
        {
            Player = player;
            Player.Died += Player_Died;

            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            ep = (IPEndPoint)ScreenManager.Instance.RemoteSocket.RemoteEndPoint;
            ep.Port = port;

            timeFromLastSend = 0;
            sendDelay = 50;
        }

        void Player_Died()
        {
            if (Died != null)
                Died();
        }

        public void Reduce()
        {
            Player.Reduce();
        }

        public void Enlarge()
        {
            Player.Enlarge();
        }

        public void LoadContent()
        {
            Player.LoadContent();

            SetBuffer();
            client.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, ep, SendedToCallback, null);
        }

        public void UnloadContent()
        {
            Player.UnloadContent();
            client.Close();
        }

        public void Update(GameTime gameTime)
        {
            Player.Update(gameTime);

            timeFromLastSend += gameTime.ElapsedGameTime.Milliseconds;
            if (timeFromLastSend > sendDelay)
            {
                timeFromLastSend = 0;
                BeginSend();
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);
        }


        private void SendedToCallback(IAsyncResult ar)
        {
            try
            {
                client.EndSend(ar);
            }
            catch { }

        }

        private void BeginSend()
        {
            SetBuffer();
            client.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, ep, SendedToCallback, null);
        }

        private void SetBuffer()
        {
            TransferingData td = new TransferingData();
            GameObject go;
            for (var i = 0; i < Player.Parts.Count; i++)
            {
                go = new GameObject
                {
                    angle = Player.Parts[i].angle,
                    CurrentSpeed = Player.Parts[i].currentMoveSpeed,
                    X = Player.Parts[i].Image.Position.X,
                    Y = Player.Parts[i].Image.Position.Y,
                    VelosityX = Player.Parts[i].Velocity.X,
                    VelosityY = Player.Parts[i].Velocity.Y
                };

                td.PlayerParts.Add(go);
            }

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, td);

            buffer = ms.ToArray();
        }
    }
}
