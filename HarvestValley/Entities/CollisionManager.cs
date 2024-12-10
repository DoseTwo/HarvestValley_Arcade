using HarvestValley.Entities.Inventory;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestValley.Entities
{
    public class CollisionManager
    {

        private readonly EntityManager _entityManager;

        private readonly MapGenerator _mapGenerator;

        public CollisionManager(MapGenerator mapGenerator, EntityManager entityManager)
        {
            _mapGenerator = mapGenerator;
            _entityManager = entityManager;
        }

        public void CheckCollisions()
        {

            foreach (Character character in _entityManager.GetEntitiesOfType<Character>())
            {
                foreach (Wall w in _entityManager.GetEntitiesOfType<Wall>())
                {
                    if (character.CollisionBox.Intersects(w.CollisionBox))
                    {
                        //we never ended up using this
                        // to add depending of what we want to do
                    }
                }

                foreach (Floor f in _entityManager.GetEntitiesOfType<Floor>())
                {
                    if (character.CollisionBox.Intersects(f.CollisionBox))
                    {
                        character.curTile = f.Plantable;
                        character.tilepos = f.Position;
                        if (f.Type == FloorType.Dirt && f.Plantable == true)
                        {
                            character.canPlant = true;
                        }
                        else
                            character.canPlant = false;
                    }
                }
            }

            foreach (Plant p in _entityManager.GetEntitiesOfType<Plant>())
            {
                foreach (Floor f in _entityManager.GetEntitiesOfType<Floor>())
                {
                    if (p.CollisionBox.Intersects(f.CollisionBox))
                    {
                        if (f.Type == FloorType.Dirt)
                        {
                            f.Plantable = false;
                        }
                        else
                            f.Plantable = true;

                        //maybe i can make this Work
                    }
                }
            }

            foreach (MoneyBin m in  _entityManager.GetEntitiesOfType<MoneyBin>())
            {
                foreach (Character c in _entityManager.GetEntitiesOfType<Character>())
                {
                    if (c.CollisionBox.Intersects(m.CollisionBox))
                    {
                        m.isColliding = true;
                        for (int i = 0; i < HarvestValley.inventory.GetItems().Count; i++)
                        {
                            if (HarvestValley.inventory.GetItems()[i].Type == ItemType.Vegetable)
                            {
                                if (Keyboard.GetState().IsKeyDown(Keys.E))
                                {
                                    if (HarvestValley.inventory.GetItems()[i].Quantity > 0)
                                    {
                                        m.Sell(HarvestValley.inventory.GetItems()[i]);
                                        HarvestValley.inventory.RemoveItem(HarvestValley.inventory.GetItems()[i].Name);
                                    }
                                }
                            }
                        }
                    }
                    else
                        m.isColliding = false;
                }
            }
        }
    }
}
