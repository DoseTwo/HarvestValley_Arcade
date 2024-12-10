using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestValley.Entities.Inventory
{
    public class Inventory
    {
        private List<Item> items;
        private int capacity;

        public Inventory(int capacity = 6)
        {
            this.capacity = capacity;
            items = new List<Item>();
        }

        public bool AddItem(Item item)
        {
            var existingItem = items.Find(i => i.Name == item.Name);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                return true;
            }

            if (items.Count >= capacity)
            {
                return false;
            }

            items.Add(item);
            return true;
        }

        public bool RemoveItem(string itemName)
        {
            var item = items.Find(i => i.Name == itemName);
            if (item != null)
            {
                if (item.Quantity > 0)
                {
                    item.Quantity--;
                }
                else
                {
                    items.Remove(item);
                }
                return true;
            }
            return false;
        }

        public bool HarvestVegetable(string vegetableName)
        {
            var vegetable = items.Find(i => i.Name == vegetableName && i.Type == ItemType.Vegetable);
            if (vegetable != null)
            {
                vegetable.Quantity++;
                return true;
            }

            return false; 
        }


        public List<Item> GetItems()
        {
            return items;
        }

        public bool IsInfiniteSeed(string itemName)
        {
         
            return itemName == "Beetroot Seed" || itemName == "Carrot Seed";
        }

        public void ResetInv()
        {
            for (int i = 0; i < items.Count; i++)
                items[i].Quantity = 0;
        }
    }
}
