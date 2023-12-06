using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knights_vs_Aliens.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;

namespace Knights_vs_Aliens.Sprites.Weapons
{
    public class Spear : IWeapon
    {
        private Texture2D texture;
        private Texture2D circleTexture;

        private Vector2 position;

        private float scale;

        public bool isAttackActive = false;

        private int curAnimationFrame = 0;

        private int prevAnimationFrame;

        private double animationTimer;

        private float weaponRotation = 0;

        private BoundingCircle attackBounds;
        public BoundingCircle AttackBounds => attackBounds;

        public Spear(float scale)
        {
            this.scale = scale;
        }
        
        public void UpdatePosition(Direction knightDirection, Vector2 knightPosition)
        {
            switch (knightDirection)
            {
                case Direction.Down:
                    position.X = knightPosition.X - (11 * scale);
                    position.Y = knightPosition.Y;
                    break;
                case Direction.Up:
                    position.X = knightPosition.X + (11 * scale);
                    position.Y = knightPosition.Y;
                    break;
                case Direction.Left:
                    position.X = knightPosition.X - (3 * scale);
                    position.Y = knightPosition.Y + (3 * scale);
                    break;
                case Direction.Right:
                    position.X = knightPosition.X + (15 * scale);
                    position.Y = knightPosition.Y + (1 * scale);
                    break;
            }
        }

        public void UpdateAttack(GameTime gameTime, Direction knightDirection, Vector2 knightPosition)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (curAnimationFrame == 0 && prevAnimationFrame == 0)
            {
                ProgressAnimation();
                isAttackActive = true;
            }
            else if (animationTimer > 0.13)
            {
                ProgressAnimation();
                animationTimer -= 0.13;
            }
            if (isAttackActive) UpdateAttackPosition(knightDirection, knightPosition);
            if (curAnimationFrame == 2) CreateCollisionBox(knightDirection);
        }
        private void ProgressAnimation()
        {
            switch (curAnimationFrame)
            {
                case 0:
                    if (prevAnimationFrame == 1)
                    {
                        prevAnimationFrame = 0;
                        isAttackActive = false;
                        break;
                    }
                    else
                    {
                        prevAnimationFrame = 0;
                        curAnimationFrame = 1;
                    }
                    break;
                case 1:
                    if (prevAnimationFrame == 0) curAnimationFrame = 2;
                    else
                    {
                        curAnimationFrame = 0;
                    }
                    prevAnimationFrame = 1;
                    break;
                case 2:
                    prevAnimationFrame = 2;
                    curAnimationFrame = 1;
                    attackBounds.Radius = 0;
                    break;
            }
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Spear");
            circleTexture = content.Load<Texture2D>("Circle");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics, Direction knightDirection)
        {
            Rectangle source = new Rectangle(0, 0, 64, 64);
            switch (knightDirection)
            {
                case Direction.Down:
                    source = new Rectangle(64 * curAnimationFrame, 0, 64, 64);
                    break;
                case Direction.Left:
                    source = new Rectangle(64 * curAnimationFrame, 128, 64, 64);
                    break;
                case Direction.Right:
                    source = new Rectangle(64 * curAnimationFrame, 64, 64, 64);
                    break;
                case Direction.Up:
                    source = new Rectangle(64 * curAnimationFrame, 192, 64, 64);
                    break;
            }
            spriteBatch.Draw(texture, position, source, Color.White, weaponRotation, new Vector2(32, 32), scale, SpriteEffects.None, 0);
            //spriteBatch.Draw(circleTexture, AttackBounds.Center, null, Color.DarkBlue, 0, new Vector2(7, 7), AttackBounds.Radius / 13, SpriteEffects.None, 0);
        }

        public bool IsAttackActive() { return isAttackActive; }

        public void UpdateAttackPosition(Direction knightDirection, Vector2 knightPosition)
        {
            if (curAnimationFrame == 0) return;
            else if (curAnimationFrame == 1)
            {
                switch (knightDirection)
                {
                    case Direction.Left:
                    {
                        position.X = knightPosition.X + (5 * scale);
                        position.Y = knightPosition.Y + (10 * scale);
                        break;
                    }
                    case Direction.Right:
                    {
                        position.X = knightPosition.X - (5 * scale);
                        position.Y = knightPosition.Y + (10 * scale);
                        break;
                    }
                    case Direction.Up:
                    {
                        position.X = knightPosition.X + (10 * scale);
                        position.Y = knightPosition.Y + (2 * scale);
                        break;
                    }
                    case Direction.Down:
                    {
                        position.X = knightPosition.X - (12 * scale);
                        position.Y = knightPosition.Y;
                        break;
                    }
                }
            }
            else if (curAnimationFrame == 2)
            {
                switch (knightDirection)
                {
                    case Direction.Left:
                    {
                        position.X = knightPosition.X - (10 * scale);
                        position.Y = knightPosition.Y + (7 * scale);
                        break;
                    }
                    case Direction.Right:
                    {
                        position.X = knightPosition.X + (10 * scale);
                        position.Y = knightPosition.Y + (7 * scale);
                        break;
                    }
                    case Direction.Up:
                    {
                        position.X = knightPosition.X + (10 * scale);
                        position.Y = knightPosition.Y - (2 * scale);
                        break;
                    }
                    case Direction.Down:
                    {
                        position.X = knightPosition.X - (12 * scale);
                        position.Y = knightPosition.Y + (6 * scale);
                        break;
                    }
                }
            }
        }

        private void CreateCollisionBox(Direction knightDirection)
        {
            switch (knightDirection)
            {
                case Direction.Left:
                {
                     attackBounds = new BoundingCircle(new Vector2(position.X - (20 * scale), position.Y), 20 * scale);
                     break;
                }
                case Direction.Right:
                    attackBounds = new BoundingCircle(new Vector2(position.X + (20 * scale), position.Y), 20 * scale);
                    break;
                case Direction.Up:
                    attackBounds = new BoundingCircle(new Vector2(position.X + (4 * scale), position.Y - (20 * scale)), 20 * scale);
                    break;
                case Direction.Down:
                    attackBounds = new BoundingCircle(new Vector2(position.X - (4 * scale), position.Y + (20 * scale)), 20 * scale);
                    break;
            }
        }

        public BoundingCircle GetAttackBounds()
        {
            return AttackBounds;
        }
    }
}
