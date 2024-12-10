using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestValley.Entities.Inventory
{
    public class SeedItem : Item
    {

        public SeedItem(string name, Texture2D texture, int quantity)
            : base(name, texture, ItemType.Seed, quantity)
        {

        }


    }
}
