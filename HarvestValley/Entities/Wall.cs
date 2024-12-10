using HarvestValley.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace HarvestValley.Entities
{
    public class Wall : IGameEntity, ICollidable
    {
        public const int WALL_WIDTH = 58;
        public const int WALL_HEIGHT = 58;
        private const int COLLISION_BOX_INSET = 1;
        public int DrawOrder => 0;
        public Vector2 Position { get; set; }
        public Sprite Sprite { get; }

        public Rectangle CollisionBox
        {
            get
            {
                Rectangle r = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), WALL_WIDTH, WALL_HEIGHT);
                r.Inflate(-COLLISION_BOX_INSET, -COLLISION_BOX_INSET);
                return r;
            }
        }

        public Wall(Sprite sprite, Vector2 position)
        {
            Sprite = sprite;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Position);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
