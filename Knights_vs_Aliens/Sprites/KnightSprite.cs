using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CollisionExample.Collisions;
using Knights_vs_Aliens.Sprites.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Knights_vs_Aliens.Sprites
{
    public class KnightSprite
    {
        private KeyboardState curKeyboardState;
        private KeyboardState prevKeyboardState;

        private const float SPEED = 110;

        private const float SCALE = 1.5f;

        public Vector2 Position = new Vector2(200, 200);

        private Direction direction;

        private bool isAttackActive = false;

        private IWeapon weapon;

        private Texture2D knight;

        private Texture2D blankTexture;

        private double animationTimer;

        private short prevAnimationFrame;

        private short curAnimationFrame = 0;

        public Color color = Color.White;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - 32, 200 - 32), 22 * SCALE, 47 * SCALE);

        public BoundingRectangle Bounds => bounds;

        private Texture2D debugRectangle;

        public KnightSprite()
        {
            weapon = new Spear(SCALE);
        }

        public void LoadContent(ContentManager content)
        {
            knight = content.Load<Texture2D>("Knight");
            debugRectangle = content.Load<Texture2D>("Debug_Rectangle");
            weapon.LoadContent(content);
        }
        public void Update(GameTime gameTime)
        {
            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();

            UpdateMovement(gameTime);
            
            //Attacks not ready yet
            //UpdateAttacks(gameTime);

            //Updates actual collision box but does not draw in right spot
            bounds.X = Position.X - (11 * SCALE);
            bounds.Y = Position.Y - (23 * SCALE);

            weapon.UpdatePosition(direction, Position);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            //For Debugging
            blankTexture = new Texture2D(graphics, 1, 1);
            blankTexture.SetData(new Color[] { Color.DarkSlateGray });
            
            Vector2 spriteOrigin;
            switch (direction)
            {
                case Direction.Up:
                    spriteOrigin = new Vector2(64 * curAnimationFrame, 64);
                    break;
                case Direction.Left:
                    spriteOrigin = new Vector2(64 * curAnimationFrame, 128);
                    break;
                case Direction.Right:
                    spriteOrigin = new Vector2((64 * curAnimationFrame) + 192, 0);
                    break;
                case Direction.Down:
                default:
                    spriteOrigin = new Vector2(64 * curAnimationFrame, 0);
                    break;
            }

            Rectangle source = new Rectangle((int)spriteOrigin.X, (int)spriteOrigin.Y, 64, 64);

            if (direction == Direction.Up || direction == Direction.Left)
            {
                weapon.Draw(gameTime, spriteBatch, graphics);
                spriteBatch.Draw(knight, Position, source, color, 0, new Vector2(32, 32), SCALE, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(knight, Position, source, color, 0, new Vector2(32, 32), SCALE, SpriteEffects.None, 1);
                weapon.Draw(gameTime, spriteBatch, graphics);
            }


            //Draw Hitbox
            //spriteBatch.Draw(blankTexture, new Vector2(bounds.X, bounds.Y), bounds.Bounds(), Color.White);
        }

        private void UpdateMovement(GameTime gameTime)
        {
            float diagonalVelocity = SPEED / (float)Math.Sqrt(2);

            //TODO: Maybe only change direction if you are attacking in that direction, not based on movement
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
                direction = Direction.Up;
                Position += new Vector2(0, -SPEED) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.S))
            {
                direction = Direction.Down;
                Position += new Vector2(0, SPEED) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.A))
            {
                direction = Direction.Left;
                Position += new Vector2(-SPEED, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.D))
            {
                direction = Direction.Right;
                Position += new Vector2(SPEED, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else
            {
                curAnimationFrame = 0;
            }
        }

        private void UpdateAttacks(GameTime gameTime)
        {
            if (isAttackActive)
            {
                weapon.UpdateAttack(gameTime, direction);
            }
            else if (curKeyboardState.IsKeyDown(Keys.Up))
            {
                direction = Direction.Up;
                isAttackActive = true;
                weapon.UpdateAttack(gameTime, direction);
            }
            else if (curKeyboardState.IsKeyDown(Keys.Left))
            {
                direction = Direction.Left;
                isAttackActive = true;
                weapon.UpdateAttack(gameTime, direction);
            }
            else if (curKeyboardState.IsKeyDown(Keys.Right))
            {
                direction = Direction.Right;
                isAttackActive = true;
                weapon.UpdateAttack(gameTime, direction);
            }
            else if (curKeyboardState.IsKeyDown(Keys.Down))
            {
                direction = Direction.Down;
                isAttackActive = true;
                weapon.UpdateAttack(gameTime, direction);
            }
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
