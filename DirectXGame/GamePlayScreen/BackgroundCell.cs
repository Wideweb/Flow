using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectXGame
{
    public class BackgroundCell : PlayerPart
    {
        static Random rand = new Random(DateTime.Today.TimeOfDay.Milliseconds);
        int timeFromLastRedirect;
        int delay;

        public BackgroundCell()
        {
            Velocity = Vector2.Zero;
            
            Image = new Image();
            Image.Path = "Gameplay/part";
            int scale = rand.Next(3, 6);
            Image.Scale = new Vector2(scale, scale);
            Image.Alpha = (float)(rand.Next(1, 3) / 20.0);
            
            int x = rand.Next(0, 640);
            int y = rand.Next(0, 480);
            Image.Position = new Vector2(x, y);

            delay = 0;
            timeFromLastRedirect = 0;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            timeFromLastRedirect += gameTime.ElapsedGameTime.Milliseconds;
            if (timeFromLastRedirect > delay)
            {
                timeFromLastRedirect = 0;
                ConsiderBounds();
                currentMoveSpeed = rand.Next(10, 50);
                angle = (float)rand.NextDouble() * 6;
                DirectMovement(gameTime);
                
                delay = rand.Next(3, 8) * 1000;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void DirectMovement(GameTime gameTime)
        {
            Velocity.X = (float)Math.Cos(angle) * currentMoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity.Y = (float)Math.Sin(angle) * currentMoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Image.Rotation = angle;
        }

        private void ConsiderBounds()
        {
            if(Image.Position.X > ScreenManager.Instance.Dimentions.X + Image.SourceRect.Width * Image.Scale.X)
            {
                Image.Position.X = -Image.SourceRect.Width * Image.Scale.X;
            }
            else if(Image.Position.X < -Image.SourceRect.Width * Image.Scale.X)
            {
                Image.Position.X = ScreenManager.Instance.Dimentions.X + Image.SourceRect.Width * Image.Scale.X;
            }

            if(Image.Position.Y > ScreenManager.Instance.Dimentions.Y + Image.SourceRect.Height * Image.Scale.Y)
            {
                Image.Position.Y = -Image.SourceRect.Height * Image.Scale.Y;
            }
            else if(Image.Position.Y < -Image.SourceRect.Height * Image.Scale.Y)
            {
                Image.Position.Y = ScreenManager.Instance.Dimentions.Y + Image.SourceRect.Height * Image.Scale.Y;
            }
        }
    }
}
