using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestValley.Graphics
{
    public class SpriteAnimationFrame
    {
        private Sprite _sprite;
        public float TimeStamp { get; }

        public Sprite Sprite
        {
            get => _sprite;
            set { 
                if (value == null)
                {
                    throw new ArgumentNullException("value", "The sprite cannot be null.");
                }
                _sprite = value;
            }
        }

        public SpriteAnimationFrame (Sprite sprite, float timeStamp)
        {
            _sprite = sprite;
            TimeStamp = timeStamp;

        }
    }
}
