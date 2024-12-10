using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HarvestValley.Entities.Inventory
{
    public enum ItemType
    {
        Seed,
        Tool,
        Vegetable
    }

    public class Item
    {
        public string Name { get; private set; }
        public ItemType Type { get; private set; }
        public Texture2D Texture { get; set; }
        public int Quantity { get; set; }
        public int Pricing { get; set; }

        public Item(string name, Texture2D texture, ItemType type, int quantity = 1, int pricing = 0)
        {
            Name = name;
            Texture = texture;
            Type = type;
            Quantity = quantity;
            Pricing = pricing;
        }

        public virtual void Use()
        {
            //item logic 
        }
    }
}
