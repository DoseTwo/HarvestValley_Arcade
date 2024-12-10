using HarvestValley.Entities;
using HarvestValley.Entities.Inventory;
using HarvestValley.Graphics;
using HarvestValley.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace HarvestValley
{
    public class HarvestValley : Game
    {
        public const bool DEBUG = false; //self explanitory, enable for debugging features
        public const string GAME_VERSION = "1.0a";

        public const int MAX_POS_Y = 50;

        public const string PLAYER_TEXTURE = "greenhairgirl";
        public const string TILE_ASSET_SPRITESHEET = "map";
        public const string BEETROOTSEED_ASSET_SPRITESHEET = "beetroot_seeds";
        public const string CAROTSEED_ASSET_SPRITESHEET = "wheat_seeds";
        public const string BEETROOT_ASSET_SPRITESHEET = "beetroot";
        public const string CAROT_ASSET_SPRITESHEET = "carrot";
        public const string WATER_ASSET_SPRITESHEET = "water_bucket";
        public const string HOE_ASSET_SPRITESHEET = "iron_hoe";
        public const string BEETROOTSEED_TEXTURE = "redseed";
        private const string FONT_ASSET = "Cream_Beige";
        private const string FONT_ASSET_BIG = "Cream_Beige_BIG";
        public const string MONEY_ICO = "moneyico";
        public const string MONEYBIN_TEXTURE = "moneyBin";
        public const string WATERING_SOUNDEFFECT = "WatheringEffect";
        public const string BACKGROUNDNOISES = "BackgroundNoises";
        private bool endSave = false;


        public const int WINDOW_WIDTH = MapReader.LEVEL_COLUMN_SIZE * Wall.WALL_WIDTH;
        public const int WINDOW_HEIGHT = Wall.WALL_HEIGHT + MapReader.LEVEL_ROW_SIZE * Wall.WALL_HEIGHT;
        Vector2 topMiddlePosition = new Vector2(WINDOW_HEIGHT / 2, 0);


        Texture2D _moneyBin;
        Texture2D playertex;
        Character player;
        Texture2D _tileSet;
        Texture2D inventoryBackground;
        Texture2D moneyIcon;
        SpriteFont _font;
        SpriteFont _fontBig;
        Texture2D redseed;
        Plant newplant;
        MoneyManager _moneyManager;
        FileSystem _fileSystem = new FileSystem();


        public static Inventory inventory = new Inventory(20);
        private Song _backgroundNoises;
        private SoundEffect wateringSound;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private EntityManager _entityManager;
        private InputController _inputController;
        private CollisionManager _collisionManager;
        private MapGenerator _mapGenerator;
        private TimeSpan _timer;
        private bool _timerRunning = true;
        private int selectedItemIndex = 0;
        private int previousScrollValue = 0;
        private MoneyBin moneyBin;


        private Texture2D[] LoadPlantSprites(string plantName)
        {
            //All the plants texture
            switch (plantName)
            {
                case "Beetroot Seed":
                    return new[]
                    {
                        Content.Load<Texture2D>("redseed"),
                        Content.Load<Texture2D>("beetrootSprout"),
                        Content.Load<Texture2D>("beetrootYoung"),
                        Content.Load<Texture2D>("BeetrootReady"),
                        Content.Load<Texture2D>("oldplant")
    };
                case "Carrot Seed":
                    return new[]
                    {
                        Content.Load<Texture2D>("greenseed"),
                        Content.Load<Texture2D>("CarrotSprout"),
                        Content.Load<Texture2D>("CarrotYoung"),
                        Content.Load<Texture2D>("CarrotReady"),
                        Content.Load<Texture2D>("oldplant")
    };
                default:
                    throw new ArgumentException("Unknown plant name: " + plantName);
            }

        }

        public HarvestValley()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _entityManager = new EntityManager();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.SynchronizeWithVerticalRetrace = true;
            Window.Title = "Harvest Valley";
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _backgroundNoises = Content.Load<Song>(BACKGROUNDNOISES);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            playertex = Content.Load<Texture2D>(PLAYER_TEXTURE);
            redseed = Content.Load<Texture2D>(BEETROOTSEED_TEXTURE);
            _tileSet = Content.Load<Texture2D>(TILE_ASSET_SPRITESHEET);
            _moneyBin = Content.Load<Texture2D>(MONEYBIN_TEXTURE);


            //Inventory
            Texture2D beetrootSeedTexture = Content.Load<Texture2D>(BEETROOTSEED_ASSET_SPRITESHEET);
            Texture2D carrotSeedTexture = Content.Load<Texture2D>(CAROTSEED_ASSET_SPRITESHEET);
            Texture2D beetrootTexture = Content.Load<Texture2D>(BEETROOT_ASSET_SPRITESHEET);
            Texture2D carrotTexture = Content.Load<Texture2D>(CAROT_ASSET_SPRITESHEET);
            Texture2D waterTexture = Content.Load<Texture2D>(WATER_ASSET_SPRITESHEET);
            Texture2D hoeTexture = Content.Load<Texture2D>(HOE_ASSET_SPRITESHEET);
            moneyIcon = Content.Load<Texture2D>(MONEY_ICO);
            _font = Content.Load<SpriteFont>(FONT_ASSET);
            _fontBig = Content.Load<SpriteFont>(FONT_ASSET_BIG);
            inventoryBackground = new Texture2D(GraphicsDevice, 1, 1);
            inventoryBackground.SetData(new[] { Color.BlueViolet * 0.5f });


            //map and player
            MapReader levelReader = new MapReader();
            string[,] level = levelReader.LoadLevelData();
            _moneyManager = new MoneyManager(0);

            _mapGenerator = new MapGenerator(level, _entityManager, _tileSet);
            _mapGenerator.InitializeGameMap();
            moneyBin = new MoneyBin(topMiddlePosition, _moneyBin, _moneyManager, inventoryBackground);
            player = new Character(playertex, _mapGenerator.PlayerPosition, "Player", inventoryBackground);
            wateringSound = Content.Load<SoundEffect>("WatheringEffect");


            _inputController = new InputController(player);
            _entityManager.AddEntity(player);
            _entityManager.AddEntity(moneyBin);
            _entityManager.AddEntity(_mapGenerator);
            _fileSystem.CreateFile("saves");
            _collisionManager = new CollisionManager(_mapGenerator, _entityManager);
            _timer = TimeSpan.FromMinutes(5);

            inventory.AddItem(new SeedItem("Beetroot Seed", beetrootSeedTexture, 1));
            inventory.AddItem(new SeedItem("Carrot Seed", carrotSeedTexture, 1));
            inventory.AddItem(new VegetableItem("Beetroot", beetrootTexture, 0, 15));
            inventory.AddItem(new VegetableItem("Carrot", carrotTexture, 0, 30));
            inventory.AddItem(new ToolItem("Hoe", hoeTexture));
            inventory.AddItem(new ToolItem("Watering Can", waterTexture));

            MediaPlayer.Play(_backgroundNoises);
            MediaPlayer.IsRepeating = true;

        }

        protected override void Update(GameTime gameTime)
        {
            if (_timerRunning)
            {
                Texture2D beetrootSeedTexture = Content.Load<Texture2D>(BEETROOTSEED_ASSET_SPRITESHEET);
                Texture2D carrotSeedTexture = Content.Load<Texture2D>(CAROTSEED_ASSET_SPRITESHEET);
                Texture2D beetrootTexture = Content.Load<Texture2D>(BEETROOT_ASSET_SPRITESHEET);
                Texture2D carrotTexture = Content.Load<Texture2D>(CAROT_ASSET_SPRITESHEET);
                Texture2D waterTexture = Content.Load<Texture2D>(WATER_ASSET_SPRITESHEET);
                Texture2D hoeTexture = Content.Load<Texture2D>(HOE_ASSET_SPRITESHEET);

                MouseState mouseState = Mouse.GetState();
                KeyboardState keyboardState = Keyboard.GetState();

                //Inventory
                int currentScrollValue = mouseState.ScrollWheelValue;

                if (currentScrollValue > previousScrollValue)
                {
                    selectedItemIndex = (selectedItemIndex - 1 + inventory.GetItems().Count) % inventory.GetItems().Count;
                }
                else if (currentScrollValue < previousScrollValue)
                {
                    selectedItemIndex = (selectedItemIndex + 1) % inventory.GetItems().Count;
                }

                previousScrollValue = currentScrollValue;

                //Planting system
                if (player.canPlant == true)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && inventory.GetItems().Count > 0)
                    {
                        Item selectedItem = inventory.GetItems()[selectedItemIndex];

                        if (selectedItem is SeedItem seedItem)
                        {
                            PlantSeed(seedItem, player.tilepos);
                        }
                    }
                }

                //Watering System
                if (mouseState.LeftButton == ButtonState.Pressed && inventory.GetItems().Count > 0)
                {
                    Item selectedItem = inventory.GetItems()[selectedItemIndex];

                    if (selectedItem is ToolItem toolItem && toolItem.Name == "Watering Can")
                    {
                        foreach (Plant plant in _entityManager.GetEntitiesOfType<Plant>())
                        {
                            if (player.CollisionBox.Intersects(plant.CollisionBox))
                            {
                                // Water the plant
                                plant.Water();
                            }
                        }
                    }
                }

                //Havesting System
                if (mouseState.LeftButton == ButtonState.Pressed && inventory.GetItems().Count > 0)
                {
                    Item selectedItem = inventory.GetItems()[selectedItemIndex];

                    if (selectedItem is ToolItem toolItem && toolItem.Name == "Hoe")
                    {
                        foreach (Plant plant in _entityManager.GetEntitiesOfType<Plant>())
                        {
                            if ((plant.IsReadyForHarvest() || plant.IsTooOldReadyForHarvest()) && player.CollisionBox.Intersects(plant.CollisionBox))
                            {
                                foreach (Floor floor in _entityManager.GetEntitiesOfType<Floor>())
                                {
                                    //Positions
                                    if (floor.Position == plant.Position)
                                        floor.Plantable = true;
                                }
                                //Havestings
                                HarvestPlant(plant);
                                _entityManager.RemoveEntity(plant);
                            }
                            //Different tiles
                            _mapGenerator.DirtIsWatered();
                        }
                    }
                }


                //timer logic
                if (_timerRunning)
                {
                    _timer -= gameTime.ElapsedGameTime;

                    //if timer hits zero stop it + game (work on that)
                    if (_timer <= TimeSpan.Zero)
                    {
                        _timer = TimeSpan.Zero;
                        _timerRunning = false;
                    }
                }

                //debug controls
                if (keyboardState.IsKeyDown(Keys.F3) && DEBUG)
                {
                    _moneyManager.Add(12);
                }

                _inputController.ProcessControls(gameTime);
                _entityManager.Update(gameTime);
                _collisionManager.CheckCollisions();
            }
            //saves
            else if (!endSave)
            {
                if (_fileSystem.ReadFromIni("saves", "score") != null)
                {
                    if (_moneyManager.getCurrent() >= Int32.Parse(_fileSystem.ReadFromIni("saves", "score")))
                        _fileSystem.WriteToIni("saves", "score", _moneyManager.getCurrent().ToString());
                }
                endSave = true;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    RestartGame();
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            _entityManager.Draw(_spriteBatch);

            _mapGenerator.Draw(_spriteBatch);

            newplant?.Draw(_spriteBatch);
            moneyBin.Draw(_spriteBatch);
            player.Draw(_spriteBatch);

            //Inventory Visual
            int iconSize = 32;
            int spacing = 10;
            int inventoryWidth = inventory.GetItems().Count * (iconSize + spacing);
            int inventoryHeight = iconSize + 5;
            int xOffset = (WINDOW_WIDTH - inventoryWidth) / 2;
            int yOffset = WINDOW_HEIGHT - iconSize - 20;


            Rectangle inventoryBackgroundRect = new Rectangle(xOffset - 10, yOffset - 10, inventoryWidth + 20, inventoryHeight + 20);
            _spriteBatch.Draw(inventoryBackground, inventoryBackgroundRect, Color.White);

            for (int i = 0; i < inventory.GetItems().Count; i++)
            {
                Item item = inventory.GetItems()[i];
                Rectangle iconRect = new Rectangle(xOffset + (iconSize + spacing) * i, yOffset, iconSize, iconSize);
                if (i == selectedItemIndex)
                {
                    _spriteBatch.Draw(item.Texture, iconRect, Color.Pink);
                }
                else
                {
                    _spriteBatch.Draw(item.Texture, iconRect, Color.White);
                }

                if (item.Type == ItemType.Vegetable)
                {
                    _spriteBatch.DrawString(_font, (item.Quantity).ToString(), new Vector2(xOffset + (iconSize + spacing) * i, yOffset), Color.White);
                }
            }

            //money display AKA our score
            Rectangle moneyBgRect = new Rectangle(WINDOW_WIDTH - 160, 10, 150, 50);
            _spriteBatch.Draw(inventoryBackground, moneyBgRect, Color.White);
            _spriteBatch.Draw(moneyIcon, new Vector2(WINDOW_WIDTH - 45, 18), Color.White);
            Vector2 _base = new Vector2(WINDOW_WIDTH - 55, 25);
            float _textWidth = _font.MeasureString(_moneyManager.getCurrent().ToString()).X;
            _spriteBatch.DrawString(_font, _moneyManager.getCurrent().ToString(), new Vector2((_base.X + 10) - _textWidth, _base.Y), Color.White);
            //draw highscore
            string _hightext = _fileSystem.ReadFromIni("saves", "score");
            if (_hightext == null)
                _hightext = "0";
            _spriteBatch.DrawString(_font, "Highscore: " + _hightext, new Vector2(_base.X - 105, _base.Y + 35), Color.White);

            //timer display
            Rectangle timerBgRect = new Rectangle((WINDOW_WIDTH / 2) - 50, 10, 100, 50);
            _spriteBatch.Draw(inventoryBackground, timerBgRect, Color.White);
            string timeText = (_timer.Minutes < 9 ? $"0{_timer.Minutes}m" : $"{_timer.Minutes}m:") + (_timer.Seconds < 9 ? $"0{_timer.Seconds}s" : $"{_timer.Seconds}s");
            _spriteBatch.DrawString(_font, timeText, new Vector2((WINDOW_WIDTH / 2) - 30, 25), Color.White);

            //version!
            _spriteBatch.DrawString(_font, "v" + GAME_VERSION, new Vector2((WINDOW_WIDTH - _font.MeasureString("v" + GAME_VERSION).X), WINDOW_HEIGHT - _font.MeasureString("v" + GAME_VERSION).Y), Color.White);

            //debug junk
            if (DEBUG)
            {
                _spriteBatch.DrawString(_font, "DEBUG MODE", new Vector2(0, 0), Color.Red);
                _spriteBatch.DrawString(_font, "player canPlant: " + player.canPlant.ToString(), new Vector2(0, 25), Color.Red);
                _spriteBatch.DrawString(_font, "Tile Plantable?: " + player.curTile, new Vector2(0, 50), Color.Red);
                _spriteBatch.DrawString(_font, "Current Position (X, Y): " + player.tilepos.X + ", " + player.tilepos.Y, new Vector2(0, 75), Color.Red);
                _spriteBatch.DrawString(_font, "moneybin Collide?: " + moneyBin.isColliding, new Vector2(0, 100), Color.Red);
            }

            if (!_timerRunning)
            {
                float _offsetX = _fontBig.MeasureString("Finish!!").X;
                float _offsetY = _fontBig.MeasureString("Finish!!").Y;
                _spriteBatch.Draw(inventoryBackground, new Rectangle(0, (WINDOW_HEIGHT / 2) - 37, WINDOW_WIDTH, 75), Color.Black);
                _spriteBatch.DrawString(_fontBig, "Finish!!", new Vector2((WINDOW_WIDTH / 2) - _offsetX / 2, (WINDOW_HEIGHT / 2) - _offsetY / 2), Color.White);
                _spriteBatch.DrawString(_font, "Press R to Restart!", new Vector2((WINDOW_WIDTH / 2) - _font.MeasureString("Press R to Restart!").X / 2, (WINDOW_HEIGHT / 2) - (_font.MeasureString("Press R to Restart!").Y / 2) + 50), Color.White);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //Planting Logic
        private void PlantSeed(SeedItem seedItem, Vector2 position)
        {
            Texture2D[] plantSprites = LoadPlantSprites(seedItem.Name);

            string seedName = seedItem.Name; 
            Plant newPlant;

            switch (seedName)
            {
                case "Beetroot Seed":
                    newPlant = new Plant(_mapGenerator, position, LoadPlantSprites("Beetroot Seed"), wateringSound, "Beetroot");
                    break;
                case "Carrot Seed":
                    newPlant = new Plant(_mapGenerator, position, LoadPlantSprites("Carrot Seed"), wateringSound, "Carrot");
                    break;
                default:
                    throw new ArgumentException("Unknown seed type: " + seedName);
            }
            _entityManager.AddEntity(newPlant);

            foreach (Floor f in _entityManager.GetEntitiesOfType<Floor>())
            {
                if (position == f.Position)
                    f.Plantable = false;
            }
                //seedItem.Quantity -= 1;
                //if (seedItem.Quantity <= 0)
                //{
                //    inventory.RemoveItem(seedItem);
                //}
            }

        //Havesting Logic
        private void HarvestPlant(Plant plant)
        {
            Texture2D beetrootTexture = Content.Load<Texture2D>(BEETROOT_ASSET_SPRITESHEET);
            Texture2D carrotTexture = Content.Load<Texture2D>(CAROT_ASSET_SPRITESHEET);

            switch (plant.PlantType)
            {
                case "Carrot":
                    inventory.AddItem(new VegetableItem("Carrot", carrotTexture, 3, 20));
                    break;

                case "Beetroot":
                    inventory.AddItem(new VegetableItem("Beetroot", beetrootTexture, 2, 15));
                    break;

                default:
                    Console.WriteLine($"Unknown plant type: {plant.PlantType}");
                    break;
            }
        }

        private void RestartGame()
        {
            _entityManager.Clear();
            inventory.ResetInv();
            LoadContent();
            _timerRunning = true;
            endSave = false;
        }


    }
}
