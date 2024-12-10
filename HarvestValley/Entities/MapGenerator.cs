using HarvestValley.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestValley.Entities
{
    public class MapGenerator : IGameEntity
    {
        public const int WALL_SPRITE_POS_X = 438;
        public const int WALL_SPRITE_POS_Y = 504;
        public const int BORDER_SPRITE_POS_X = 306;
        public const int BORDER_SPRITE_POS_Y = 372;
        public const int GRASS_SPRITE_POS_X = 0;
        public const int GRASS_SPRITE_POS_Y = 0;
        public const int GRASSNDIRTR_SPRITE_POS_X = 116;
        public const int GRASSNDIRTR_SPRITE_POS_Y = 58;
        public const int GRASSNDIRTL_SPRITE_POS_X = 116;
        public const int GRASSNDIRTL_SPRITE_POS_Y = 58;
        public const int GRASSNDIRTU_SPRITE_POS_X = 232;
        public const int GRASSNDIRTU_SPRITE_POS_Y = 0;
        public const int GRASSNDIRTD_SPRITE_POS_X = 174;
        public const int GRASSNDIRTD_SPRITE_POS_Y = 0;
        public const int GRASSNDIRTM_SPRITE_POS_X = 116;
        public const int GRASSNDIRTM_SPRITE_POS_Y = 58;
        public const int DIRT_SPRITE_POS_X = 58;
        public const int DIRT_SPRITE_POS_Y = 58;
        public const int WETDIRT_SPRITE_POS_X = 232;
        public const int WETDIRT_SPRITE_POS_Y = 58;

        //Ces 2 variable sont pour que votre goal et votre character apparaissent au milieu de la Tile
        public const int GOAL_OFFSET = 9;
        public const int PLAYER_OFFSET = 18;

        public Vector2 GoalPosition { get; set; }
        public Vector2 PlayerPosition { get; set; }

        private readonly List<Wall> _walls = new List<Wall>();
        private readonly List<Floor> _floors = new List<Floor>();

        readonly string[,] _mapTiles;

        bool IsWatered = false;

        private readonly EntityManager _entityManager;
        private readonly Texture2D _texture;

        public int DrawOrder => 0;

        public MapGenerator(string[,] mapTiles, EntityManager entityManager, Texture2D spriteSheet)
        {
            _entityManager = entityManager;
            _texture = spriteSheet;
            _mapTiles = mapTiles;
        }

        public void WaterDirt(Vector2 xyPos)
        {
            Floor f = _floors.FirstOrDefault(x => x.XYPos.X == xyPos.X && x.XYPos.Y == xyPos.Y);
            f.Sprite = new Sprite(_texture, new Rectangle(WETDIRT_SPRITE_POS_X, WETDIRT_SPRITE_POS_Y, Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT));
            int a = 5;
        }
        public void GoodDirt(Vector2 xyPos)
        {
            Floor f = _floors.FirstOrDefault(x => x.XYPos.X == xyPos.X && x.XYPos.Y == xyPos.Y);
            f.Sprite = new Sprite(_texture, new Rectangle(DIRT_SPRITE_POS_X, DIRT_SPRITE_POS_Y, Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT));
            int a = 5;
        }



        public void InitializeGameMap()
        {
            for (int row = 0; row < _mapTiles.GetLength(0); row++)
            {
                for (int col = 0; col < _mapTiles.GetLength(1); col++)
                {
                    //B: Border, D: Dirt, G: Grass, S: Selling Box P: Player
                    //R: GrassNDirtR, L: GrassNDirtL, U: GrassNDirtU, O: GrassNDirtD, M: GrassNDirtM
                    if (_mapTiles[row, col] == "D")
                    {
                        if(IsWatered == true)
                        {
                            GenerateWetTile(row, col);
                        }
                        else
                        {
                            GenerateDirtTile(row, col);
                        }

                        IsWatered = false;
                    }
                    if (_mapTiles[row, col] == "B")
                    {
                        GenerateBorderTile(row, col);
                    }
                    if (_mapTiles[row, col] == "G")
                    {
                        GenerateGrassTile(row, col);
                    }
                    if (_mapTiles[row, col] == "R")
                    {
                        GenerateGrassNDirtRTile(row, col);
                    }
                    if (_mapTiles[row, col] == "L")
                    {
                        GenerateGrassNDirtLTile(row, col);
                    }
                    if (_mapTiles[row, col] == "U")
                    {
                        GenerateGrassNDirtUTile(row, col);
                    }
                    if (_mapTiles[row, col] == "O")
                    {
                        GenerateGrassNDirtDTile(row, col);
                    }
                    if (_mapTiles[row, col] == "M")
                    {
                        GenerateGrassNDirtMTile(row, col);
                    }
                    if (_mapTiles[row, col] == "S")
                    {
                        GenerateGrassTile(row, col);
                        GoalPosition = new Vector2(row * Floor.FLOOR_WIDTH + GOAL_OFFSET, col * Floor.FLOOR_HEIGHT + GOAL_OFFSET);
                    }
                    if (_mapTiles[row, col] == "P")
                    {
                        GenerateGrassTile(row, col);
                        PlayerPosition = new Vector2(row * Floor.FLOOR_WIDTH + PLAYER_OFFSET, col * Floor.FLOOR_HEIGHT + PLAYER_OFFSET);
                    }
                }
            }
        }

        private void GenerateWallTile(int row, int col)
        {
            Sprite wallSprite = new Sprite(_texture, new Rectangle(WALL_SPRITE_POS_X, WALL_SPRITE_POS_Y, Wall.WALL_WIDTH, Wall.WALL_HEIGHT));
            Wall wall = new Wall(wallSprite, new Vector2(row * Wall.WALL_WIDTH, Wall.WALL_HEIGHT + col * Wall.WALL_HEIGHT));
            _walls.Add(wall);
            _entityManager.AddEntity(wall);
        }

        private void GenerateBorderTile(int row, int col)
        {
            Sprite wallSprite = new Sprite(_texture, new Rectangle(BORDER_SPRITE_POS_X, BORDER_SPRITE_POS_Y, Wall.WALL_WIDTH, Wall.WALL_HEIGHT));
            Wall wall = new Wall(wallSprite, new Vector2(row * Wall.WALL_WIDTH, Wall.WALL_HEIGHT + col * Wall.WALL_HEIGHT));
            _walls.Add(wall);
            _entityManager.AddEntity(wall);
        }

        private void GenerateGrassTile(int row, int col)
        {
            Sprite floorSprite = new Sprite(_texture, new Rectangle(GRASS_SPRITE_POS_X, GRASS_SPRITE_POS_Y, Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT));
            Floor floor = new Floor(FloorType.Grass, floorSprite, new Vector2(row * Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT + col * Floor.FLOOR_HEIGHT));
            _floors.Add(floor);
            _entityManager.AddEntity(floor);
        }
        private void GenerateWetTile(int row, int col)
        {
            Sprite floorSprite = new Sprite(_texture, new Rectangle(WETDIRT_SPRITE_POS_X, WETDIRT_SPRITE_POS_Y, Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT));
            Floor floor = new Floor(FloorType.Dirt, floorSprite, new Vector2(row * Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT + col * Floor.FLOOR_HEIGHT));
            _floors.Add(floor);
            _entityManager.AddEntity(floor);
        }
        private void GenerateDirtTile(int row, int col)
        {
            Sprite floorSprite = new Sprite(_texture, new Rectangle(DIRT_SPRITE_POS_X, DIRT_SPRITE_POS_Y, Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT));
            Floor floor = new Floor(FloorType.Dirt, floorSprite, new Vector2(row * Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT + col * Floor.FLOOR_HEIGHT));
            _floors.Add(floor);
            _entityManager.AddEntity(floor);
        }
        private void GenerateGrassNDirtRTile(int row, int col)
        {
            Sprite floorSprite = new Sprite(_texture, new Rectangle(GRASSNDIRTR_SPRITE_POS_X, GRASSNDIRTR_SPRITE_POS_Y, Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT));
            Floor floor = new Floor(FloorType.Grass, floorSprite, new Vector2(row * Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT + col * Floor.FLOOR_HEIGHT));
            _floors.Add(floor);
            _entityManager.AddEntity(floor);
        }
        private void GenerateGrassNDirtLTile(int row, int col)
        {
            Sprite floorSprite = new Sprite(_texture, new Rectangle(GRASSNDIRTL_SPRITE_POS_X, GRASSNDIRTL_SPRITE_POS_Y, Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT));
            Floor floor = new Floor(FloorType.Grass, floorSprite, new Vector2(row * Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT + col * Floor.FLOOR_HEIGHT));
            _floors.Add(floor);
            _entityManager.AddEntity(floor);
        }
        private void GenerateGrassNDirtUTile(int row, int col)
        {
            Sprite floorSprite = new Sprite(_texture, new Rectangle(GRASSNDIRTU_SPRITE_POS_X, GRASSNDIRTU_SPRITE_POS_Y, Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT));
            Floor floor = new Floor(FloorType.Grass, floorSprite, new Vector2(row * Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT + col * Floor.FLOOR_HEIGHT));
            _floors.Add(floor);
            _entityManager.AddEntity(floor);
        }
        private void GenerateGrassNDirtDTile(int row, int col)
        {
            Sprite floorSprite = new Sprite(_texture, new Rectangle(GRASSNDIRTD_SPRITE_POS_X, GRASSNDIRTD_SPRITE_POS_Y, Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT));
            Floor floor = new Floor(FloorType.Grass, floorSprite, new Vector2(row * Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT + col * Floor.FLOOR_HEIGHT));
            _floors.Add(floor);
            _entityManager.AddEntity(floor);
        }
        private void GenerateGrassNDirtMTile(int row, int col)
        {
            Sprite floorSprite = new Sprite(_texture, new Rectangle(GRASSNDIRTM_SPRITE_POS_X, GRASSNDIRTM_SPRITE_POS_Y, Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT));
            Floor floor = new Floor(FloorType.Grass, floorSprite, new Vector2(row * Floor.FLOOR_WIDTH, Floor.FLOOR_HEIGHT + col * Floor.FLOOR_HEIGHT));
            _floors.Add(floor);
            _entityManager.AddEntity(floor);
        }

        public string GetTileTypeAt(Vector2 position)
        {
            return "dirt";
        }
        public void Draw(SpriteBatch spriteBatch)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void DirtIsWatered()
        {
            IsWatered = true;
        }
    }
}
