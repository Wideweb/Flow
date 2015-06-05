using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectXGame
{
    public class InitialScreen : GameScreen
    {
        public Image Image;
        public int Delay;

        public InitialScreen(){ }

        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
        }

        public override void Update(GameTime gameTime) 
        {
            base.Update(gameTime);
            Image.Update(gameTime);

            if (gameTime.TotalGameTime.TotalSeconds > Delay && !ScreenManager.Instance.isTransitioning)
            {
                ScreenManager.Instance.ChangeScreens("Menu.TitleScreen");
            }

            if (InputManager.Instance.keyPressed(Keys.Enter, Keys.Z))
                ScreenManager.Instance.ChangeScreens("Menu.TitleScreen");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
