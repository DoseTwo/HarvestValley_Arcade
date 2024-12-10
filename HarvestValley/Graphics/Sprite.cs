using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HarvestValley.Graphics
{
    public class Sprite
    {
        //Pour storer la texture
        public Texture2D Texture { get; private set; }
        //Pour contenir les informations du rectangle de notre sprite. (height, width)
        private Rectangle _rec;
        public Color tintColor { get; set; } = Color.White;

        public Sprite(Texture2D texture, Rectangle rec)
        {
            Texture = texture;
            _rec = rec;
        }

        //On défini la propre méthode Draw que l'on peut appeler dans la methode Draw de notre application principale
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, position, _rec, tintColor);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects se)
        {
            spriteBatch.Draw(Texture, position, _rec, tintColor, 0f, new Vector2(
            Texture.Width, Texture.Height), 1f, se, 1f);
        }
    }
}
