using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DirectXGame.Menu
{
    public class Menu
    {
        public event EventHandler OnMenuChange;

        public string Axis;
        public string Effects;
        [XmlElement("Item")]
        public List<MenuItem> Items;
        int itemNumber;
        string id;

        public int ItemNumber
        {
            get { return itemNumber; }
        }

        public void Transition(float alpha)
        {
            foreach(MenuItem item in Items)
            {
                item.Image.isActive = true;
                item.Image.Alpha = alpha;
                if (alpha == 0.0f)
                    item.Image.FadeEffect.Increase = true;
                else
                    item.Image.FadeEffect.Increase = false;
            }
        }

        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                OnMenuChange(this, null);
            }
        }

        void AlignMenuItems()
        {
            Vector2 dimentions = Vector2.Zero;
            foreach(MenuItem item in Items)
            {
                dimentions += new Vector2(item.Image.SourceRect.Width,
                    item.Image.SourceRect.Height);
            }

            dimentions = new Vector2((ScreenManager.Instance.Dimentions.X - dimentions.X) / 2,
                (ScreenManager.Instance.Dimentions.Y - dimentions.Y) / 2);

            foreach(MenuItem item in Items)
            {
                if (Axis == "X")
                    item.Image.Position = new Vector2(dimentions.X,
                        (ScreenManager.Instance.Dimentions.Y - item.Image.SourceRect.Height) / 2);
                else if (Axis == "Y")
                    item.Image.Position = new Vector2(
                        (ScreenManager.Instance.Dimentions.X - item.Image.SourceRect.Width) / 2, dimentions.Y);

                dimentions += new Vector2(item.Image.SourceRect.Width,
                    item.Image.SourceRect.Height);
            }
        }

        public Menu()
        {
            id = String.Empty;
            itemNumber = 0;
            Effects = String.Empty;
            Axis = "Y";
            Items = new List<MenuItem>();
        }

        public void LoadContent()
        {
            string[] split = Effects.Split(':');
            foreach(MenuItem item in Items)
            {
                item.Image.LoadContent();
                foreach(string s in split)
                    item.Image.ActivateEffect(s);
            }
            AlignMenuItems();
        }

        public void UnloadContent()
        {
            foreach (MenuItem item in Items)
                item.Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if(Axis == "X")
            {
                if (InputManager.Instance.keyPressed(Keys.Right))
                    itemNumber++;
                else if (InputManager.Instance.keyPressed(Keys.Left))
                    itemNumber--;
            }
            else if (Axis == "Y")
            {
                if (InputManager.Instance.keyPressed(Keys.Down))
                    itemNumber++;
                else if (InputManager.Instance.keyPressed(Keys.Up))
                    itemNumber--;
            }

            if (itemNumber < 0)
                itemNumber = 0;
            else if (itemNumber > Items.Count - 1)
                itemNumber = Items.Count - 1;

            for (int i = 0; i < Items.Count; i++ )
            {
                if (i == itemNumber)
                    Items[i].Image.isActive = true;
                else
                    Items[i].Image.isActive = false;

                Items[i].Image.Update(gameTime);
            }       
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MenuItem item in Items)
                item.Image.Draw(spriteBatch);
        }

    }
}
