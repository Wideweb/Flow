using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectXGame
{
    public class LinerHead : PlayerPart
    {
        public bool isActive;

        public LinerHead()
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
            if (isActive)
            {
                ControlMovementSpeed(gameTime);
                DirectMovement(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void ControlMovementSpeed(GameTime gameTime)
        {
            if (InputManager.Instance.MousePressed())
            {
                if (currentMoveSpeed == MoveSpeed)
                {
                    currentMoveSpeed = MoveSpeed + Acceleration;
                }
            }

            if (currentMoveSpeed > MoveSpeed)
            {
                currentMoveSpeed -= DeccelerationRate * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                currentMoveSpeed = MoveSpeed;
            }
        }

        private void DirectMovement(GameTime gameTime)
        {
            Vector2 target = InputManager.Instance.MousePosition();

            float dx = target.X - Image.Position.X - Image.SourceRect.Width / 2;
            float dy = target.Y - Image.Position.Y - Image.SourceRect.Height / 2;

            if(Math.Abs(dy) < 10 && (Math.Abs(dx) < 10))
                return;

            newAngle = (float)Math.Atan2(dy, dx);

            Velocity.X = (float)Math.Cos(angle) * currentMoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity.Y = (float)Math.Sin(angle) * currentMoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
    }
}
