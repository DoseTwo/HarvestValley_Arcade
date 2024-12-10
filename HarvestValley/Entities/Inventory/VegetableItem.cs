using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestValley.Entities.Inventory
{
    public class VegetableItem : Item
    {

        public VegetableItem(string name, Texture2D texture, int quantity, int pricing)
            : base(name, texture, ItemType.Vegetable, quantity, pricing) 
        { 

        }

        public override void Use()
        {
            // Selling logic $$$$$$
        }
    }
}
