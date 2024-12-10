using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarvestValley.Graphics;
using HarvestValley.Entities.Inventory;
using HarvestValley.System;

namespace HarvestValley.Entities
{
    public class MoneyBin : IGameEntity, ICollidable
    {
        public Vector2 Position { get; set; }
        public Texture2D Sprite { get; set; }
        public Texture2D debug { get; set; }
        public MoneyManager Money { get; set; }
        public bool isColliding { get; set; }
        public int DrawOrder => 0;
        private int SpriteWidth = 100;
        private int SpriteHeight = 70;
        public Rectangle CollisionBox
        {
            get
            {
                Rectangle r = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), SpriteWidth, SpriteHeight);
                r.Inflate(-1, -1);
                return r;
            }
        }

        public MoneyBin(Vector2 position, Texture2D sprite, MoneyManager _money, Texture2D _debug = null, bool _isColliding = false)
        {
            Position = position;
            Sprite = sprite;
            Money = _money;
            debug = _debug;
            isColliding = _isColliding;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite != null)
            {
                spriteBatch.Draw(Sprite, Position, Color.White); 
            }
            if (HarvestValley.DEBUG)
                spriteBatch.Draw(debug, CollisionBox, Color.White);
        }

        public void Sell(Item _item)
        {
            if (_item.Type == ItemType.Vegetable)
            {
                Money.Add(_item.Pricing);
            }
        }
    }
}
