using Microsoft.Xna.Framework;
using System;

namespace HarvestValley.Extenstions
{
    public class ScoreAttribute : Attribute
    {
        public int Score { get; protected set; } 

        public ScoreAttribute(int value)
        {
            this.Score = value;
        }
    }

    public class SpriteRectangleAttribute : Attribute
    {
        public Rectangle SpriteRectangle { get; protected set; }

        public SpriteRectangleAttribute(int x, int y, int width, int height) {
            this.SpriteRectangle = new Rectangle(x, y, width, height);
        }
    }
}
