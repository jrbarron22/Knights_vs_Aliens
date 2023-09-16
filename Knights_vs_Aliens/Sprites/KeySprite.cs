using CollisionExample.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knights_vs_Aliens.Sprites
{
    public class KeySprite
    {
        private Texture2D texture;

        private double animationTimer;

        private double directionTimer;

        private Direction direction;

        private int animationFrame = 0;

        private Vector2 position;

        private Color color;

        private BoundingRectangle bounds;

        public BoundingRectangle Bounds => bounds;

        public bool Collected = false;

        public KeySprite(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
            bounds = new BoundingRectangle(new Vector2(position.X - 16, position.Y - 16), 16, 32);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Key");
        }

        public void Update(GameTime gameTime)
        {
            //Update Direction Timer
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Switch Timer every 1 second
            if (directionTimer > 1)
            {
                switch (direction)
                {
                    case Direction.Up:
                        direction = Direction.Down;
                        break;
                    case Direction.Down:
                        direction = Direction.Up;
                        break;
                }
                directionTimer -= 1;
            }

            switch (direction)
            {
                case Direction.Up:
                    position += new Vector2(0, -1) * 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case Direction.Down:
                    position += new Vector2(0, 1) * 5 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }
            bounds.X = position.X - 16;
            bounds.Y = position.Y - 16;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Collected) return;
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 0.4)
            {
                animationFrame++;
                if (animationFrame > 2) animationFrame = 0;
                animationTimer -= 0.4;
            }

            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(texture, position, source, color, 0, new Vector2(16, 16), 1.5f, SpriteEffects.None, 0);
        }
    }
}
