using HarvestValley.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace HarvestValley.Entities
{
    public enum FloorType
    {
        Grass,
        Dirt
    }
    public class Floor : IGameEntity, ICollidable
    {
        public const int FLOOR_WIDTH = 58;
        public const int FLOOR_HEIGHT = 58;
        private const int COLLISION_BOX_INSET = 1;
        public FloorType Type { get; set; }

        public int DrawOrder => 0;
        public Vector2 Position { get; set; }
        public Vector2 XYPos { get; set; }

        public Sprite Sprite { get; set; }

        public bool Plantable { get; set; }

        public Rectangle CollisionBox
        {
            get
            {
                Rectangle r = new Rectangle((int)Math.Round(Position.X) + 58, (int)Math.Round(Position.Y), FLOOR_WIDTH - 58, FLOOR_HEIGHT);
                r.Inflate(-COLLISION_BOX_INSET, -COLLISION_BOX_INSET);
                return r;
            }
        }

        public Floor(FloorType type, Sprite sprite, Vector2 position, bool platnable = true)
        {
            Type = type;
            Sprite = sprite;
            Position = position;
            Plantable = platnable;
            this.XYPos = new Vector2(
              (int)position.X / 58,
              (int)position.Y / 58
              );
        }

        public bool IsFarmable()
        {
            return Type == FloorType.Dirt; 
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
