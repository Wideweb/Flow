using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectXGame
{
    public delegate void DiedHandler();

    public interface IPlayer
    {
        [XmlElement("LinerHead", typeof(LinerHead))]
        [XmlElement("Cell", typeof(Cell))]
        [XmlElement("LinerTail", typeof(LinerTail))]
        List<PlayerPart> Parts { get; set; }

        [XmlIgnore]
        IPlayer Enemy { get; set; }

        event DiedHandler Died;

        void Enlarge();
        void Reduce();
        void LoadContent();
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
