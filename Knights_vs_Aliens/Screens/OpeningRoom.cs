using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Knights_vs_Aliens.Sprites;
using Knights_vs_Aliens.Sprites.Enemies;
using CollisionExample.Collisions;

namespace Knights_vs_Aliens.Screens
{
    public class OpeningRoom : IScreen
    {
        private ContentManager content;

        private KnightSprite knight;

        private KeySprite[] keys;

        private AlienTurret[] turrets;

        private Texture2D backgroundLeft;

        private Texture2D backgroundRight;

        private BoundingRectangle topBounds;

        private BoundingRectangle bottomBounds;

        private BoundingRectangle leftBounds;

        private BoundingRectangle rightBounds;

        public OpeningRoom(GraphicsDevice graphics, KnightSprite knight, ContentManager content)
        {
            Random rand = new Random();
            this.knight = knight;
            keys = new KeySprite[]
            {
                new KeySprite(new Vector2(400, 300), Color.Purple),
                new KeySprite(new Vector2(200, 300), Color.Aqua),
                new KeySprite(new Vector2(600, 300), Color.Orange)
            };

            turrets = new AlienTurret[]
            {
                new AlienTurret(new Vector2((float)rand.NextDouble() * graphics.Viewport.Width, (float)rand.NextDouble() * graphics.Viewport.Height), content),
                new AlienTurret(new Vector2((float)rand.NextDouble() * graphics.Viewport.Width, (float)rand.NextDouble() * graphics.Viewport.Height), content)
            };
        }
        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            backgroundLeft = content.Load<Texture2D>("Dungeon_Tileset");
            backgroundRight = content.Load<Texture2D>("Dungeon_Tileset");
            foreach (var key in keys) key.LoadContent(content);
            foreach (var turret in turrets) turret.LoadContent();
            topBounds = new BoundingRectangle(0, 0, graphics.Viewport.Width, 30);
            leftBounds = new BoundingRectangle(0, 0, 30, graphics.Viewport.Height);
            bottomBounds = new BoundingRectangle(0, graphics.Viewport.Height - 50, graphics.Viewport.Width, 30);
            rightBounds = new BoundingRectangle(graphics.Viewport.Width - 45, 0, 45, graphics.Viewport.Height);
        }

        public void Update(GameTime gameTime)
        {
            knight.Update(gameTime);
            knight.color = Color.White;
            if (topBounds.CollidesWith(knight.Bounds))
            {
                knight.Position.Y = topBounds.Height + (knight.Bounds.Height/2) + 1;
            }
            if (bottomBounds.CollidesWith(knight.Bounds))
            {
                knight.Position.Y = bottomBounds.Y - (knight.Bounds.Height / 2) - 1;
            }
            if (leftBounds.CollidesWith(knight.Bounds))
            {
                knight.Position.X = leftBounds.Width + (knight.Bounds.Width / 2) + 1;
            }
            if (rightBounds.CollidesWith(knight.Bounds))
            {
                knight.Position.X = rightBounds.X - (knight.Bounds.Width / 2) - 1;
            }
            foreach (var key in keys) {
                if (!key.Collected && key.Bounds.CollidesWith(knight.Bounds))
                {
                    key.Collected = true;
                }
                key.Update(gameTime);
            }
            foreach (var turret in turrets)
            {
                turret.Update(gameTime, knight.Position);
                foreach(var projectile in turret.projectiles)
                {
                    if (projectile.bounds.CollidesWith(knight.Bounds))
                    {
                        projectile.hasCollided = true;
                        knight.color = Color.Red;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.Black);
            spriteBatch.Draw(backgroundLeft, new Vector2(0, 0), new Rectangle(10, 0, 68, 70), Color.White, 0, new Vector2(0, 0), graphics.Viewport.Height / 65, SpriteEffects.None, 0);
            spriteBatch.Draw(backgroundRight, new Vector2(graphics.Viewport.Width / 2, 0), new Rectangle(30, 0, 68, 70), Color.White, 0, new Vector2(0, 0), graphics.Viewport.Height / 65, SpriteEffects.None, 0);
            knight.Draw(gameTime, spriteBatch);
            foreach (var key in keys) key.Draw(gameTime, spriteBatch);
            foreach (var turret in turrets) turret.Draw(gameTime, spriteBatch);
        }
    }
}
