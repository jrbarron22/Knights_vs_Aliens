using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;

namespace Knights_vs_Aliens.Sprites.Weapons
{
    public class Spear : IWeapon
    {
        private Texture2D texture;

        private Vector2 position;

        private float scale;

        private int curAnimationFrame = 0;

        private int prevAnimationFrame;

        private double animationTimer;

        private float weaponRotation = 0;

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
                    position.X = knightPosition.X - (8 * scale);
                    position.Y = knightPosition.Y + (3 * scale);
                    break;
                case Direction.Right:
                    position.X = knightPosition.X + (15 * scale);
                    position.Y = knightPosition.Y + (1 * scale);
                    break;
            }
        }

        //Need more sprite art before I can incorporate attacks
        public void UpdateAttack(GameTime gameTime, Direction knightDirection)
        {
            return;
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 0.15)
            {
                ProgressAnimation();
                switch (knightDirection)
                {
                    case Direction.Down:
                        break;
                    case Direction.Up:
                        break;
                    case Direction.Left:
                        if (curAnimationFrame == 0) weaponRotation = 0;
                        else
                        {
                            weaponRotation = 3 * (float)Math.PI / 2;
                        }
                        break;
                    case Direction.Right:
                        weaponRotation = (float)Math.PI / 2;
                        break;
                }
                animationTimer -= 0.15;
            }
        }
        private void ProgressAnimation()
        {
            switch (curAnimationFrame)
            {
                case 0:
                    if (prevAnimationFrame == 1) break;
                    else
                    {
                        prevAnimationFrame = 0;
                        curAnimationFrame = 1;
                    }
                    break;
                case 1:
                    if (prevAnimationFrame == 0) curAnimationFrame = 2;
                    else curAnimationFrame = 0;
                    prevAnimationFrame = 1;
                    break;
                case 2:
                    prevAnimationFrame = 2;
                    curAnimationFrame = 1;
                    break;
            }
            animationTimer -= 0.15;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Spear");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Draw(texture, position, null, Color.White, weaponRotation, new Vector2(32, 32), scale, SpriteEffects.None, 0);
        }
    }
}
