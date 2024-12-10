using HarvestValley.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestValley.Entities
{
    public class Background : IGameEntity
    {
        public int DrawOrder { get; set; }

        public const int DEFAULT_SPRITE_POS_X = 0;
        public const int DEFAULT_SPRITE_POS_Y = 0;
        public const int DEFAULT_WIDTH = 640;
        public const int DEFAULT_HEIGHT = 480;

        public Sprite _background;

        public Background(Texture2D spriteSheet)
        {
            _background = new Sprite(spriteSheet, new Rectangle(DEFAULT_SPRITE_POS_X, DEFAULT_SPRITE_POS_Y, DEFAULT_WIDTH, DEFAULT_HEIGHT));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _background.Draw(spriteBatch, new Vector2(DEFAULT_SPRITE_POS_X, DEFAULT_SPRITE_POS_Y));
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}