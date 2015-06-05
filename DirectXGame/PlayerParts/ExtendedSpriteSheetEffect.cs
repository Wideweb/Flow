using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectXGame
{
    public class ExtendedSpriteSheetEffect : ImageEffect
    {
        [XmlIgnore]
        public Sprite sprite; 

        public ExtendedSpriteSheetEffect()
        {
        }

        public override void LoadContent(ref Image Image)
        {
            base.LoadContent(ref Image);
            sprite = new Sprite(Image.Texture, new Point(128, 118), new Point(16, 1), 16);
            sprite.Looped = true;
            sprite.Play();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            Image.SourceRect = sprite.SourceRect();
        }
    }
}
