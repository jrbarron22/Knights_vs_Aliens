using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Knights_vs_Aliens
{
    public enum Direction
    {
        Down,
        Right,
        Up,
        Left
    }
    public class AlienOrbSprite
    {
        private Texture2D texture;

        private double directionTimer;

        private double animationTimer;

        private short prevAnimationFrame;

        private short curAnimationFrame = 2;

        public Direction Direction;

        public Vector2 Position;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Alien Orb");
        }

        public void Update(GameTime gameTime)
        {
            //Update Direction Timer
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Switch Timer every 2 seconds
            if (directionTimer > 2.0)
            {
                switch (Direction)
                {
                    case Direction.Up:
                        Direction = Direction.Down;
                        break;
                    case Direction.Down:
                        Direction = Direction.Up;
                        break;
                }
                directionTimer -= 2.0;
            }

            switch (Direction)
            {
                case Direction.Up:
                    Position += new Vector2(0, -1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Down:
                    Position += new Vector2(0, 1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frame
            if (animationTimer > 0.15)
            {
                switch (curAnimationFrame)
                {
                    case 1:
                        prevAnimationFrame = 1;
                        curAnimationFrame = 2;
                        break;
                    case 2:

                        if (prevAnimationFrame == 1)
                        {
                            curAnimationFrame = 3;
                        }
                        else
                        {
                            curAnimationFrame = 1;
                        }
                        prevAnimationFrame = 2;
                        break;
                    case 3:
                        prevAnimationFrame = 3;
                        curAnimationFrame = 2;
                        break;
                }
                animationTimer -= 0.15;
            }
            Vector2 spriteOrigin = new Vector2(0, 0);
            switch (curAnimationFrame)
            {
                case 1:
                    break;
                case 2:
                    spriteOrigin = new Vector2(32, 0);
                    break;
                case 3:
                    spriteOrigin = new Vector2(0, 32);
                    break;
            }

            //Draw the sprite
            float rotation;
            if (Direction == Direction.Down) rotation = (float)Math.PI;
            else rotation = 0;
            var source = new Rectangle((int)spriteOrigin.X, (int)spriteOrigin.Y, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color.White, rotation, new Vector2(16, 16), 1.5f, SpriteEffects.None, 0);
        }
    }
}
