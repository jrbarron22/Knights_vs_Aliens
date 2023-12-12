using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Knights_vs_Aliens.Collisions;
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
        private MouseState curMouseState;

        private const float SPEED = 180;

        private const float SCALE = 1.5f;

        public Vector2 Position;

        public int CurHealth = 3;

        private int maxHealth = 3;

        public int KeysCollected = 0;

        private Direction direction;

        public IWeapon Weapon;

        private SpriteFont phudu;

        private Texture2D knight;

        private Texture2D heartTexture;

        private Texture2D blankTexture;

        public bool Invulnerable = false;

        private double invulnerabilityTimer;

        private double animationTimer;

        private short prevAnimationFrame;

        private short curAnimationFrame = 0;

        public Color color = Color.White;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200 - 32, 200 - 32), 22 * SCALE, 47 * SCALE);

        public BoundingRectangle Bounds => bounds;

        private Texture2D debugRectangle;

        public KnightSprite()
        {
            Weapon = new Spear(SCALE);
        }

        public void LoadContent(ContentManager content)
        {
            knight = content.Load<Texture2D>("Knight");
            debugRectangle = content.Load<Texture2D>("Debug_Rectangle");
            heartTexture = content.Load<Texture2D>("Heart");
            phudu = content.Load<SpriteFont>("phudu");
            Weapon.LoadContent(content);
        }
        public void Update(GameTime gameTime)
        {
            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();

            curMouseState = Mouse.GetState();

            //If in middle of attack animation, lock direction
            if (!Weapon.IsAttackActive())
            {
                UpdateDirection();
            }

            UpdateMovement(gameTime);

            Weapon.UpdatePosition(direction, Position);

            UpdateAttacks(gameTime);

            //Attacks not ready yet
            //UpdateAttacks(gameTime);
            if (Invulnerable)
            {
                invulnerabilityTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                color = Color.Red;

                if(invulnerabilityTimer < 0)
                {
                    Invulnerable = false;
                    color = Color.White;
                }
            }

            //Updates actual collision box but does not draw in right spot
            bounds.X = Position.X - (11 * SCALE);
            bounds.Y = Position.Y - (23 * SCALE);
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
                    spriteOrigin = new Vector2(0, 64 * curAnimationFrame);
                    break;
                case Direction.Left:
                    spriteOrigin = new Vector2(192, 64 * curAnimationFrame);
                    break;
                case Direction.Right:
                    spriteOrigin = new Vector2(128, 64 * curAnimationFrame);
                    break;
                case Direction.Down:
                default:
                    spriteOrigin = new Vector2(64, 64 * curAnimationFrame);
                    break;
            }

            Rectangle source = new Rectangle((int)spriteOrigin.X, (int)spriteOrigin.Y, 64, 64);

            //if (Invulnerable) color = Color.Gold;

            if (direction == Direction.Up || direction == Direction.Left)
            {
                Weapon.Draw(gameTime, spriteBatch, graphics, direction);
                spriteBatch.Draw(knight, Position, source, color, 0, new Vector2(32, 32), SCALE, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(knight, Position, source, color, 0, new Vector2(32, 32), SCALE, SpriteEffects.None, 1);
                Weapon.Draw(gameTime, spriteBatch, graphics, direction);
            }

            //Draw Health
            //spriteBatch.End();
            //spriteBatch.Begin();
            int tempCounter = CurHealth;
            for (int i = 0; i < maxHealth; i++)
            {
                if(tempCounter > 0)
                {
                    source = new Rectangle(0, 0, 64, 64);
                    spriteBatch.Draw(heartTexture, new Rectangle(8 + (32 * i), 8, 64, 64), source, Color.White);
                    tempCounter--;
                }
                else
                {
                    source = new Rectangle(128, 0, 64, 64);
                    spriteBatch.Draw(heartTexture, new Rectangle(8 + (32 * i), 8, 64, 64), source, Color.White);
                }
            }

            //Debugging Direction
            /*
            Vector2 mouseVector = new Vector2(curMouseState.X - Position.X, curMouseState.Y - Position.Y);
            double theta = Math.Atan(-mouseVector.Y / mouseVector.X);
            string str = curMouseState.X.ToString() + " " + curMouseState.Y.ToString() + " " + Position.X.ToString() + " " + Position.Y.ToString();
            spriteBatch.DrawString(phudu, str, new Vector2(graphics.Viewport.Width / 2, 50), Color.Black);
            */

            //spriteBatch.End();
            //spriteBatch.Begin(transformMatrix: transform);

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
                //direction = Direction.Up;
                Position += new Vector2(0, -SPEED) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.S))
            {
                //direction = Direction.Down;
                Position += new Vector2(0, SPEED) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.A))
            {
                //direction = Direction.Left;
                Position += new Vector2(-SPEED, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else if (curKeyboardState.IsKeyDown(Keys.D))
            {
                //direction = Direction.Right;
                Position += new Vector2(SPEED, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                UpdateAnimation(gameTime);
            }
            else
            {
                curAnimationFrame = 0;
            }
        }

        private void UpdateDirection()
        {
            //Need to update mouse to world coordinates
            Vector2 mouseVector = new Vector2(curMouseState.X - Position.X, curMouseState.Y - Position.Y);

            //Look up how to change from screen to world coordinates
            double theta = Math.Atan(-mouseVector.Y / mouseVector.X);

            if (curMouseState.X > Position.X) {
                if (theta > Math.PI / 4) direction = Direction.Up;
                else if (theta < -Math.PI / 4) direction = Direction.Down;
                else direction = Direction.Right;
            }
            else
            {
                if (theta > Math.PI / 4) direction = Direction.Down;
                else if (theta < -Math.PI / 4) direction = Direction.Up;
                else direction = Direction.Left;
            }
        }

        private void UpdateAttacks(GameTime gameTime)
        {
            if (!Weapon.IsAttackActive() && curMouseState.LeftButton == ButtonState.Pressed)
            {
                Weapon.UpdateAttack(gameTime, direction, Position);
            }
            else if (Weapon.IsAttackActive())
            {
                Weapon.UpdateAttack(gameTime, direction, Position);
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

        public void KnightHit(GameTime gameTime)
        {
            color = Color.Red;
            CurHealth--;
            invulnerabilityTimer = 0.50;
            Invulnerable = true;
        }

        public void Reset()
        {
            CurHealth = 3;
        }
    }
}
