using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollisionExample.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;

namespace Knights_vs_Aliens.Sprites
{
    public class KnightSprite
    {
        private KeyboardState curKeyboardState;
        private KeyboardState prevKeyboardState;

        private const float SPEED = 130;

        public Vector2 Position = new Vector2(200, 200);

        private Texture2D knight;

        private double animationTimer;

        private short prevAnimationFrame;

        private short curAnimationFrame = 0;

        public Color color = Color.White;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - 32, 200 - 32), 44, 60);

        public BoundingRectangle Bounds => bounds;

        public void LoadContent(ContentManager content)
        {
            knight = content.Load<Texture2D>("Knight");
        }
        public void Update(GameTime gameTime)
        {
            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();

            float diagonalVelocity = SPEED / (float)Math.Sqrt(2);

            if (curKeyboardState.IsKeyDown(Keys.W) && curKeyboardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-diagonalVelocity, -diagonalVelocity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.W) && curKeyboardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(diagonalVelocity, -diagonalVelocity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.S) && curKeyboardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(diagonalVelocity, diagonalVelocity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.S) && curKeyboardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-diagonalVelocity, diagonalVelocity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.W))
            {
                Position += new Vector2(0, -SPEED) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.S))
            {
                Position += new Vector2(0, SPEED) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-SPEED, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(SPEED, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else
            {
                curAnimationFrame = 0;
            }
            bounds.X = Position.X - 22;
            bounds.Y = Position.Y - 30;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 spriteOrigin = new Vector2(0, 64 * curAnimationFrame);
            Rectangle source = new Rectangle((int)spriteOrigin.X, (int)spriteOrigin.Y, 64, 64);
            spriteBatch.Draw(knight, Position, source, color, 0, new Vector2(32, 32), 2f, SpriteEffects.None, 1);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            //Update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frame
            if (animationTimer > 0.15)
            {
                switch (curAnimationFrame)
                {
                    case 0:
                        if (prevAnimationFrame == 1)
                        {
                            curAnimationFrame = 2;
                            prevAnimationFrame = 2;
                        }
                        else
                        {
                            curAnimationFrame = 1;
                            prevAnimationFrame = 1;
                        }
                        break;
                    case 1:
                    case 2:
                        curAnimationFrame = 0;
                        break;
                }
                animationTimer -= 0.15;
            }
        }
    }
}
