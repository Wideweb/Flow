using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectXGame
{
    public class Sprite
    {
        public int millisecondsPerFrame { get; set; }
        public bool Looped { get; set; }
        public SpriteEffects spriteEffect { get; set; }
        public float Scale { get; set; }

        Texture2D texture;
        Point firstFrame;
        public Point sheetSize;
        public Point frameSize;
        public int framesNumber;

        public Point currentFrame;
        int timeSinceLastFrame;
        bool play = false;
        


        public Sprite(Texture2D texture, Point frameSize, Point sheetSize, int framesNumber)
        {
            this.texture = texture;
            this.frameSize = frameSize;
            this.sheetSize = sheetSize;
            this.framesNumber = framesNumber;

            firstFrame = new Point(0, 0);
            currentFrame = firstFrame;
            Scale = 1;
            spriteEffect = SpriteEffects.None;

            millisecondsPerFrame = 50;
            timeSinceLastFrame = 0;
        }
        

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                Animate();
            }
        }

        private void Animate()
        {
            StraightAnimation();
        }

        private void StraightAnimation()
        {
            if (play && isTheLastFrame() && Looped == false)
            {
                play = false;
            }

            if (isTheLastFrame() && Looped == true)
                currentFrame = firstFrame;
            else if (play)
                NextFrame();
        }

        private void NextFrame()
        {
            currentFrame.X++;
            if (currentFrame.X >= sheetSize.X)
            {
                currentFrame.X = 0;
                currentFrame.Y++;
            }
        }

        private bool isTheLastFrame()
        {
            if (currentFrame.X - firstFrame.X + 1 + (currentFrame.Y - firstFrame.Y) * sheetSize.X >= framesNumber)
                return true;
            else
                return false;
        }

        public void ResetAnimation()
        {
            currentFrame = firstFrame;
        }

        public void Play()
        {
            play = true;
        }

        public void Stop()
        {
            play = false;
        }

        public Rectangle SourceRect()
        {
            return new Rectangle((int)currentFrame.X * frameSize.X,
                (int)currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y);
        }

        public Vector2 OriginVect()
        {
            return new Vector2(frameSize.X / 2, frameSize.Y / 2);
        }


        /*
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, position,
                new Rectangle(
                    currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y
                    ),
                Color.White * 0.8f, 0, 
                new Vector2(
                    frameSize.X / 2,
                    frameSize.Y / 2
                    ),
                Scale, spriteEffect, 0);
        }*/
        
    }
}
