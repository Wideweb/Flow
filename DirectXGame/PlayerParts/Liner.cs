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
    public class Liner : IPlayer
    {
        [XmlElement("LinerHead", typeof(LinerHead))]
        [XmlElement("Cell", typeof(Cell))]
        [XmlElement("LinerTail", typeof(LinerTail))]
        public List<PlayerPart> Parts;

        public bool isControled;

        List<PlayerPart> IPlayer.Parts
        {
            get
            {
                return Parts;
            }
            set
            {
                Parts = value;
            }
        }

        [XmlIgnore]
        public IPlayer Enemy { get; set; }
        public int HittingDelay;
        private int elapsedTimeFromLastHitting;

        public event DiedHandler Died;

        public Liner()
        {
            HittingDelay = 0;
            elapsedTimeFromLastHitting = 0;
        }

        public virtual void LoadContent()
        {
            for (int i = Parts.Count - 1; i > 0; i--)
            {
                Parts[i].LoadContent();
                if (Parts[i] is Cell)
                {
                    (Parts[i] as Cell).Link = Parts[i - 1];
                }
                else if (Parts[i] is LinerTail)
                {
                    (Parts[i] as LinerTail).Link = Parts[i - 1];
                }
            }

            if (Parts.Count > 0)
                Parts[0].LoadContent();
        }

        public virtual void UnloadContent()
        {
            try
            {
                foreach (PlayerPart part in Parts)
                {
                    part.UnloadContent();
                }
            }
            catch { }
        }

        public virtual void Update(GameTime gameTime)
        {
            try
            {
                foreach (PlayerPart part in Parts)
                {
                    part.Update(gameTime);
                }
            }
            catch { }


            if (isControled)
            {
                elapsedTimeFromLastHitting += gameTime.ElapsedGameTime.Milliseconds;
                if (HittingDelay < elapsedTimeFromLastHitting)
                {
                    elapsedTimeFromLastHitting = 0;
                    Hitting();
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                foreach (PlayerPart part in Parts)
                {
                    part.Draw(spriteBatch);
                }
            }
            catch { }
        }

        public void Reduce()
        {
            if (Parts.Count <= 2)
                return;

            Parts[Parts.Count - 2].UnloadContent();
            Parts.RemoveAt(Parts.Count - 2);

            (Parts[Parts.Count - 1] as LinerTail).Link = Parts[Parts.Count - 2];

            if (Parts.Count <= 2 && Died != null)
                Died();
        }

        public void Enlarge()
        {
            var cellLoader = new XmlManager<Cell>();
            var cell = cellLoader.Load("GamePlay/Cell.xml");
            cell.LoadContent();
            cell.Image.Position = Parts[Parts.Count - 2].Image.Position;
            cell.Link = Parts[Parts.Count - 2];

            (Parts[Parts.Count - 1] as LinerTail).Link = cell;

            Parts.Insert(Parts.Count - 1, cell);
        }

        private void Hitting()
        {
            if (Enemy == null || Enemy.Parts == null || Parts.Count <= 2)
                return;

            bool hit = false;
            PlayerPart cell;
            Vector2 headPos = Enemy.Parts[0].Image.Position + Enemy.Parts[0].Image.Origin;
            for (var i = 1; i < Parts.Count - 1; i++)
            {
                cell = Parts[i];
                if (DistanceSquare(headPos, cell.Image.Position + cell.Image.Origin) < cell.DiagonalSquare())
                {
                    hit = true;
                    break;
                }
            }

            if (hit && Parts.Count > 2)
            {
                Reduce();
            }
            else
            {
                elapsedTimeFromLastHitting = HittingDelay;
            }
        }
    
        private int DistanceSquare(Vector2 v1, Vector2 v2)
        {
            int distance = (int)((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y));
            return distance;
        }
    }
}
