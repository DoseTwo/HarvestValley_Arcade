﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace HarvestValley.Graphics
{
    public class SpriteAnimation
    {
        private List<SpriteAnimationFrame> _frames = new List<SpriteAnimationFrame>();

        public SpriteAnimationFrame this[int index]
        {
            get { return GetFrame(index); }
        }

        public SpriteAnimationFrame CurrentFrame
        {
            get
            {
                return _frames
                    .Where(f => f.TimeStamp <= PlaybackProgress)
                    .OrderBy(f => f.TimeStamp)
                    .LastOrDefault();
            }
        }

        public float PlaybackProgress { get; private set; }
        public bool IsPlaying { get; private set; }
        public bool ShouldLoop { get; set; } = true;

        public float Duration
        {
            get
            {
                if (!_frames.Any())
                    return 0;
                return _frames.Max(f => f.TimeStamp);
            }
        }
        private SpriteAnimationFrame GetFrame(int index)
        {
            if (index < 0 || index >= _frames.Count)
            {
                throw new ArgumentOutOfRangeException("index", "A frame with index " + index + " does not exist in this animation.");
            }
            return _frames[index];
        }

        public void AddFrame(Sprite sprite, float timeStamp)
        {
            SpriteAnimationFrame frame = new SpriteAnimationFrame(sprite, timeStamp);
            _frames.Add(frame);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
         //N'oubliez pas que DRAW ne bouge rien il sert simplement à dessiné.
         //Les seules vérification valide sont pour valider si quelque chose doit être affiché ou pas.
         SpriteAnimationFrame frame = CurrentFrame;

            if (frame != null)
            {
                frame.Sprite.Draw(spriteBatch, position);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                PlaybackProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (PlaybackProgress > Duration)
                {
                    if (ShouldLoop)
                        PlaybackProgress -= Duration;
                    else
                        Stop();
                }
            }
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            PlaybackProgress = 0;
        }

        public void Clear()
        {
            Stop();
            _frames.Clear();
        }
    }
}
