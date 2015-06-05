using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectXGame
{
    public class DefeatScreen : GameScreen
    {
        private Image Image;

        public DefeatScreen()
        {
            Image = new Image();
            Image.Text = "DEFEAT";
        }

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

            if (InputManager.Instance.keyPressed(Keys.Enter))
                ScreenManager.Instance.ChangeScreens("Menu.TitleScreen");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
