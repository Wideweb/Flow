using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;

namespace DirectXGame
{
    public class ScreenManager
    {
        public static event EventHandler<EventArgs> TerminateEvent;
        private static ScreenManager instance;
        [XmlIgnore]
        public Vector2 Dimentions { get; set; }
        [XmlIgnore]
        public ContentManager Content { set; get; }

        [XmlIgnore]
        public Socket RemoteSocket;
       
        XmlManager<GameScreen> xmlGameScreenManager;

        GameScreen currentScreen, newScreen;
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;

        [XmlIgnore]
        public bool isHost;

        public Image Image;
        [XmlIgnore]
        public bool isTransitioning { get; private set; }
        private string WaitinScreen;
        
        public ScreenManager() 
        {
            Dimentions = new Vector2(640, 480);
            currentScreen = new InitialScreen();
            WaitinScreen = String.Empty;

            xmlGameScreenManager = new XmlManager<GameScreen>();
            xmlGameScreenManager.Type = currentScreen.Type;
            currentScreen = xmlGameScreenManager.Load("Load/InitialScreen.xml");
        }

        public static void Terminate()
        {
            if (instance != null && instance.RemoteSocket != null)
                instance.RemoteSocket.Close();
            if(TerminateEvent != null)
                TerminateEvent(ScreenManager.Instance, null);
        }

        public static ScreenManager Instance
        {
            get
            {
                if(instance == null)
                {
                    XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
                    instance = xml.Load("Load/ScreenManager.xml");
                }

                return instance;
            }
        }

        public void ChangeScreens(string screenName)
        {
            if (!isTransitioning)
            {
                newScreen = (GameScreen)Activator.CreateInstance(Type.GetType("DirectXGame." + screenName));
                Image.isActive = true;
                Image.FadeEffect.Increase = true;
                Image.Alpha = 0.0f;
                isTransitioning = true;
            }
            else if (currentScreen.Type.Name != screenName)
            {
                WaitinScreen = screenName;
            }
        }

        private void Transition(GameTime gameTime)
        {
            if(isTransitioning)
            {
                Image.Update(gameTime);
                if(Image.Alpha == 1.0f)
                {
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;
                    xmlGameScreenManager.Type = currentScreen.Type;
                    if (File.Exists(currentScreen.xmlPath))
                        currentScreen = xmlGameScreenManager.Load(currentScreen.xmlPath);
                    currentScreen.LoadContent();
                }
                else if(Image.Alpha == 0.0f)
                {
                    Image.isActive = false;
                    isTransitioning = false;
                }
            }
        }

        public void LoadContent(ContentManager content) 
        {
            this.Content = new ContentManager(content.ServiceProvider, "Content");
            currentScreen.LoadContent();
            Image.LoadContent();
        }

        public void UnloadContent()
        {
            currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime) 
        {
            if(!isTransitioning && WaitinScreen != String.Empty && WaitinScreen != currentScreen.Type.Name)
            {
                this.ChangeScreens(WaitinScreen);
                WaitinScreen = String.Empty;
            }

            currentScreen.Update(gameTime);
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            currentScreen.Draw(spriteBatch);
            if (isTransitioning)
                Image.Draw(spriteBatch);
        }
    }
}
