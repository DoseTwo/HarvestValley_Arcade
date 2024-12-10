using HarvestValley.Entities;
using HarvestValley.Entities.Inventory;
using HarvestValley.Graphics;
using HarvestValley.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HarvestValley.System
{
    public class InputController
    {
        private Character _player;

        public InputController(Character player)
        {
            _player = player;
        }

        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Vector2 playerDirection = new Vector2(0, 0);
            //up & down
            if (keyboardState.IsKeyDown(Keys.W))
            {
                playerDirection.Y--;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                playerDirection.Y++;
            }
            //left & right
            if (keyboardState.IsKeyDown(Keys.A))
            {
                playerDirection.X--;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                playerDirection.X++;
            }

            //Item logic
            //i'll add this eventually i just need to retool controls

            _player.Run(playerDirection);
        }
    }
}
