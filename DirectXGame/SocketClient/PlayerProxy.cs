using DirectXGame.SocketClient;
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
using System.Xml.Serialization;
using TransferingDataLib;

namespace DirectXGame
{
    public class PlayerProxy : IPlayer
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

        UDPListener listener;
        public int Port { get; private set; }

        public event DiedHandler Died;

        public PlayerProxy(int port)
        {
            Port = port;
            listener = new UDPListener();
            listener.Received += listener_Received;
            listener.Disconnect += listener_Disconnect;
        }

        void listener_Disconnect()
        {
            ScreenManager.Instance.ChangeScreens("Menu.TitleScreen");
        }

        public void LoadContent()
        {
            var playerLoader = new XmlManager<Liner>();
            Player = playerLoader.Load("GamePlay/RemotePlayer.xml");

            for (int i = Parts.Count - 1; i > 0; i--)
            {
                Parts[i].LoadContent();
                if (Parts[i] is Cell)
                {
                    (Parts[i] as Cell).Link = Parts[i - 1];
                }
                else if (Parts[i] is LinerTail)
                {
                    (Parts[i] as LinerTail).Link = Parts[i - 1];
                }
            }

            if (Parts.Count > 0)
                Parts[0].LoadContent();

            listener.Start(Port);
        }

        void listener_Received(IPEndPoint e, byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(data, 0, data.Length);
            TransferingData td = (TransferingData)bf.Deserialize(ms);

            
            while (td.PlayerParts.Count < Parts.Count)
            {
                Reduce();
                Enemy.Enlarge();
            }


            while (td.PlayerParts.Count > Parts.Count)
                Enlarge();

            for (var i = 0; i < td.PlayerParts.Count; i++)
            {
                Parts[i].newAngle = td.PlayerParts[i].angle;
                Parts[i].Image.Position = new Vector2(td.PlayerParts[i].X, td.PlayerParts[i].Y);
                Parts[i].Velocity = new Vector2(td.PlayerParts[i].VelosityX, td.PlayerParts[i].VelosityY);
                Parts[i].currentMoveSpeed = td.PlayerParts[i].CurrentSpeed;
            }

            if (Parts.Count <= 2 && Died != null)
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


        public void UnloadContent()
        {
            listener.Stop();
            Player.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Player.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);
        }
    }
}
