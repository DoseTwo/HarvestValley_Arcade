using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;


namespace HarvestValley.Entities
{
    enum GrowthStage
    {
        Seed,
        Sprout,
        Young,
        Grown,
        Old
    };

    public class Plant : IGameEntity, ICollidable
    {
        private MapGenerator generator;
        public Vector2 Position { get; set; }
        private Texture2D[] Sprites;
        private GrowthStage CurrentStage;
        private float GrowthTimer;
        private readonly float[] StageDurations = { 3f, 6f, 10f, 15f };
        private int CurrentStageIndex;
        public bool IsWatered { get; private set; }
        public int DrawOrder => 0;
        private SoundEffect WateringSound;

        private Vector2 XYPos { get; set; }

        public string PlantType { get; private set; }

        public const int SpriteWidth = 32;
        public const int SpriteHeight = 32;
        public Rectangle CollisionBox
        {
            get
            {
                Rectangle r = new Rectangle((int)Math.Round(Position.X) + 32, (int)Math.Round(Position.Y), SpriteWidth - 32, SpriteHeight);
                r.Inflate(-1, -1);
                return r;
            }
        }
        public Plant(MapGenerator generator, Vector2 position, Texture2D[] sprites, SoundEffect wateringSound, string plantType)
        {
            if (generator == null)
            {
                int a = 5;
            }
            this.generator = generator;
            Position = position;
            this.XYPos = new Vector2(
                (int)position.X / 58,
                (int)position.Y / 58
                );
            Sprites = sprites;
            CurrentStage = GrowthStage.Seed;
            WateringSound = wateringSound;
            GrowthTimer = 0f;
            CurrentStageIndex = 0;
            IsWatered = false;
            PlantType = plantType;
        }

        public void Water()
        {
            if (!IsWatered && CurrentStage != GrowthStage.Grown && CurrentStage != GrowthStage.Old)
            {
                IsWatered = true;
                WateringSound?.Play(0.5f, 0f, 0f);
                this.generator.WaterDirt(this.XYPos);
                Console.WriteLine("Plant is now watered!");
            }

        }

        private void SetPlantType(string newType)
        {
            if (PlantType != newType)
            {
                PlantType = newType;

            }
        }
        public void Update(GameTime gameTime)
        {
            if (CurrentStage == GrowthStage.Old)
            {
                IsWatered = false;
                return;
            }

            if (!IsWatered && CurrentStage != GrowthStage.Grown)
                return;

            GrowthTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (CurrentStageIndex < StageDurations.Length &&
                GrowthTimer >= StageDurations[CurrentStageIndex])
            {
                GrowthTimer = 0f;
                CurrentStageIndex++;

                if (CurrentStageIndex < StageDurations.Length)
                {
                    CurrentStage = (GrowthStage)CurrentStageIndex;
                }
                else
                {
                    CurrentStage = GrowthStage.Old;
                    HandlePlantTypeChange();
                }

                IsWatered = false;
                this.generator.GoodDirt(this.XYPos);
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, SpriteWidth, SpriteHeight);
            Color drawColor = (IsWatered && CurrentStage != GrowthStage.Grown)
            ? Color.DarkBlue * 0.5f
            : Color.White;
            spriteBatch.Draw(Sprites[CurrentStageIndex], Position, drawColor);
        }

        public bool IsReadyForHarvest()
        {
            return CurrentStage == GrowthStage.Grown;
        }
        public bool IsTooOldReadyForHarvest()
        {
            return CurrentStage == GrowthStage.Old;
        }
        private bool hasTriggeredTypeChange = false;
        private void HandlePlantTypeChange()
        {
            if (CurrentStage == GrowthStage.Old && !hasTriggeredTypeChange)
            {
                SetPlantType("OLDPlantType");
                hasTriggeredTypeChange = true;
            }
        }

    }
}