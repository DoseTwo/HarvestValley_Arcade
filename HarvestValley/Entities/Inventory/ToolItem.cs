using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestValley.Entities.Inventory
{
    public class ToolItem : Item
    {
        public ToolItem(string name, Texture2D texture)
            : base(name, texture, ItemType.Tool)
        {

        }

        public override void Use()
        {
            // Hoe: Till soil
            // Watering Can: Water crops
        }
    }
}
