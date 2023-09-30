using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollisionExample.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Knights_vs_Aliens.Sprites
{
    public class AlienProjectile
    {
        private Texture2D texture;

        private double animationTimer;

        private short prevAnimationFrame;

        private short curAnimationFrame = 2;

        public Vector2 Position;

        public Vector2 VelocityDirection;

        private const float PROJECTILE_SPEED = 90;

        public BoundingCircle bounds;

        public bool hasCollided;

        private Texture2D debugCircle;

        public AlienProjectile(Vector2 position, Vector2 velocity)
        {
            Position = position;
            VelocityDirection = Vector2.Normalize(velocity);
            bounds.Center = new Vector2(position.X + 16, position.Y + 8);
            bounds.Radius = 8;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Alien Orb");
            debugCircle = content.Load<Texture2D>("ball");
        }

        public void Update(GameTime gameTime)
        {
            Position += new Vector2(VelocityDirection.X * PROJECTILE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds, VelocityDirection.Y * PROJECTILE_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds) ;
            bounds.Center = new Vector2(Position.X, Position.Y);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (hasCollided) return;
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
            float rotation = getRotationAngle();
            var source = new Rectangle((int)spriteOrigin.X, (int)spriteOrigin.Y, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color.White, rotation, new Vector2(16, 16), 1f, SpriteEffects.None, 0);

            //Draw DebugCircle
            /*
            var rect = new Rectangle((int)bounds.Center.X - (int)bounds.Radius,
                                    (int)bounds.Center.Y - (int)bounds.Radius,
                                    (int)2 * (int)bounds.Radius, (int)2 * (int)bounds.Radius);
            spriteBatch.Draw(debugCircle, rect, Color.Red);
            */
        }

        private float getRotationAngle()
        {
            if(VelocityDirection.Y >= 0 && VelocityDirection.X > 0)
            {
                return ((float)Math.PI / 2) + (float)Math.Cos(1 / (getVectorMagnitude(VelocityDirection)));
            }
            else if(VelocityDirection.Y >= 0 && VelocityDirection.X < 0)
            {
                return ((float)Math.PI) + (float)Math.Cos(1 / (getVectorMagnitude(VelocityDirection)));
            }
            else if(VelocityDirection.Y <= 0 && VelocityDirection.X > 0)
            {
                return (float)Math.Cos(1/ (getVectorMagnitude(VelocityDirection)));
            }
            else
            {
                return ((float)Math.PI * (3/2)) + (float)Math.Cos(1 / (getVectorMagnitude(VelocityDirection)));
            }

        }

        private double getVectorMagnitude(Vector2 vector)
        {
            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }
    }
}
