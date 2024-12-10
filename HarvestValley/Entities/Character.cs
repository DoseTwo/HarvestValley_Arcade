using HarvestValley.Extensions;
using HarvestValley.Extenstions;
using HarvestValley.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace HarvestValley.Entities
{
    public partial class Character : IGameEntity
    {
        //public const int DEFAULT_SPRITE_POS_X = 0;
        //public const int DEFAULT_SPRITE_POS_Y = 0;
        //public const int DEFAULT_WIDTH = 49;
        //public const int DEFAULT_HEIGHT = 64;

        //public const int NUMBER_OF_FRAMES = 5;

        public const int DEFAULT_SPRITE_POS_X = 0;
        public const int DEFAULT_SPRITE_POS_Y = 0;
        public const int DEFAULT_WIDTH = 100;
        public const int DEFAULT_HEIGHT = 120;

        public const int NUMBER_OF_FRAMES = 4;

        public const float ANIMATION_FRAME_LENGHT = 0.1f;
        public const float SPEED = 150f;

        public bool IsMoving { get; set; }  = false;
        public bool canPlant { get; set; } = false;
        public bool curTile { get; set; } = false;
        public Vector2 tilepos { get; set; } = new Vector2 (0, 0);

        private Dictionary<State, Rectangle> _spriteRecs = new Dictionary<State, Rectangle>
        {
            
            { State.Left, new Rectangle(DEFAULT_SPRITE_POS_X, DEFAULT_SPRITE_POS_Y + DEFAULT_HEIGHT * 3, DEFAULT_WIDTH, DEFAULT_HEIGHT) },
            { State.Down, new Rectangle(DEFAULT_SPRITE_POS_X, DEFAULT_SPRITE_POS_Y, DEFAULT_WIDTH, DEFAULT_HEIGHT)},
            { State.Up, new Rectangle(DEFAULT_SPRITE_POS_X, DEFAULT_SPRITE_POS_Y + DEFAULT_HEIGHT * 1, DEFAULT_WIDTH, DEFAULT_HEIGHT) },
            { State.Right, new Rectangle(DEFAULT_SPRITE_POS_X, DEFAULT_SPRITE_POS_Y + DEFAULT_HEIGHT * 2, DEFAULT_WIDTH, DEFAULT_HEIGHT) }

        };

        //Pour chaque valeur du enum, je déclare un instance de mon objet.
        private Dictionary<State, SpriteAnimation> _animations = new Dictionary<State, SpriteAnimation>();
        private Dictionary<State, Sprite> _idleSprites = new Dictionary<State, Sprite>();
        public SpriteAnimation _currentAnimation;

        public Texture2D _spriteSheet;
        public Texture2D debug;
        public Vector2 OldPosition { get; set; }
        public Vector2 Position { get; set; } 
        public Vector2 Velocity { get; set; }
        public int DrawOrder {get;set;}

        public string CharacterName { get; private set; }

        private State _state = State.Down;

        public Rectangle CollisionBox
        {
            get
            {
                Rectangle box = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), DEFAULT_WIDTH, DEFAULT_HEIGHT);
                box.Inflate(-1, -1);
                return box;
            }
        }
        public Character(Texture2D spriteSheet, Vector2 position, string characterName, Texture2D _debug)
        {
            _spriteSheet = spriteSheet;
            Position = position;
            Velocity = Vector2.Zero;
            CreateAnimations();
            CreateIdleSprites();
            CharacterName = characterName;
            debug = _debug;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //Si j'appuie sur des flèches ou clés pour le character, il va jouer l'animation correspondante,
            //sinon il va seulement montrer le sprite pour le state.
            if (IsMoving)
            {
                _currentAnimation.Draw(spriteBatch, Position);
            }
            else
            {
                _idleSprites[_state].Draw(spriteBatch, Position);
            }
            if (HarvestValley.DEBUG)
                spriteBatch.Draw(debug, CollisionBox, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            OldPosition = Position;
            ValidatedColision(gameTime);
            _currentAnimation.Update(gameTime);
        }
        private void CreateAnimations()
        {
            //Créé une animation pour chacun des states défini
            foreach (State s in Enum.GetValues(typeof(State)))
            {
                _animations.Add(s, new SpriteAnimation());

                for (int i = 0; i < NUMBER_OF_FRAMES; i++)
                {
                    //je vais chercher le rectangle pour le state défini
                    Rectangle rec = _spriteRecs[s];
                    rec.X += DEFAULT_WIDTH * i;
                    _animations[s].AddFrame(new Sprite(_spriteSheet, rec), ANIMATION_FRAME_LENGHT * i);
                }
            }
            _currentAnimation = _animations[_state];
        }

        private void CreateIdleSprites()
        {
            //Créé un sprite pour quand le character ne bouge pas pour chacun des states
            foreach (State s in Enum.GetValues(typeof(State)))
            {
                Rectangle baseRectangle = _spriteRecs[s];
                baseRectangle.X += DEFAULT_WIDTH; 
                _idleSprites.Add(s, new Sprite(_spriteSheet, baseRectangle));
            }
        }
        private void ValidatedColision(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Position.X < 0 || Position.X > (HarvestValley.WINDOW_WIDTH - DEFAULT_WIDTH) || 
                Position.Y < HarvestValley.MAX_POS_Y - DEFAULT_HEIGHT || Position.Y > (HarvestValley.WINDOW_HEIGHT - DEFAULT_HEIGHT))
            {
                Position = OldPosition ;
            }
        }

        public void Run (Vector2 direction)
        {
            Velocity = direction * SPEED;
            UpdateAnimation();
        }


        private void UpdateAnimation()
        {
            //Valide dans quel direction que je character va et je change son state en conséquence.
            if (Velocity.Y < 0) {
                _state = State.Up;
            } 
            else if (Velocity.Y > 0)
            {
                _state = State.Down;
            }
            else if (Velocity.X < 0)
            {
                _state = State.Left;
            }
            else if (Velocity.X > 0)
            {
                _state = State.Right;
            }

            _currentAnimation = _animations[_state];


            if (Velocity.X != 0 || Velocity.Y != 0){
                _currentAnimation.Play();
                IsMoving = true;
            }
            else
            {
                _currentAnimation.Stop();
                IsMoving= false;
            }
        }
    }
}
