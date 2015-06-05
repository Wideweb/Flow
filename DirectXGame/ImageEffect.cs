using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectXGame
{
    public class ImageEffect
    {
        protected Image Image;
        public bool isActive;

        public ImageEffect()
        {
            isActive = false;
        }

        public virtual void LoadContent(ref Image Image)
        {
            this.Image = Image;
        }

        public virtual void UnloadContent()
        {

        }
        public virtual void Update(GameTime gameTime)
        {
             
        }
    }
}
