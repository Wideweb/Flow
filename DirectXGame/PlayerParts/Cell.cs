using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TransferingDataLib;

namespace DirectXGame
{
    public class Cell : PlayerPart
    {
        [XmlIgnore]
        public PlayerPart Link;

        public Cell()
        {
            Velocity = Vector2.Zero;
            angle = 0.0f;
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
            ControlMovementSpeed(gameTime);
            DirectMovement(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void ControlMovementSpeed(GameTime gameTime)
        {
            if (DistanceSquare(Link.Image.Position + Link.Image.Origin, Image.Position + Image.Origin)
                < Image.DiagonalSquare)
            {
                currentMoveSpeed = 0;
            }
            else
            {
                currentMoveSpeed = Link.currentMoveSpeed;
            }
        }

        private void DirectMovement(GameTime gameTime)
        {
            Vector2 target = Link.Image.Position;

            if(Link is LinerHead)
            {
                target.X -= (float)(Math.Cos(Link.angle) * Link.Image.SourceRect.Width * 0.2);
                target.Y -= (float)(Math.Sin(Link.angle) * Link.Image.SourceRect.Width * 0.2);
            }

            float dx = target.X + Link.Image.SourceRect.Width / 2 - Image.Position.X - Image.SourceRect.Width / 2;
            float dy = target.Y + Link.Image.SourceRect.Height / 2 - Image.Position.Y - Image.SourceRect.Height / 2;

            angle = (float)Math.Atan2(dy, dx);

            Velocity.X = (float)Math.Cos(angle) * currentMoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity.Y = (float)Math.Sin(angle) * currentMoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            newAngle = angle;
        }
    }
}
