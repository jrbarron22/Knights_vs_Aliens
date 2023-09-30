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
using Microsoft.Xna.Framework.Audio;

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

        private SoundEffect keyPickup;
        private SoundEffect knightHit;

        private BoundingRectangle topBounds;
        private BoundingRectangle bottomBounds;
        private BoundingRectangle leftBounds;
        private BoundingRectangle rightBounds;

        private switchScreen pause;

        public OpeningRoom(GraphicsDevice graphics, KnightSprite knight, ContentManager content, switchScreen swapScreen)
        {
            Random rand = new Random();
            this.knight = knight;
            pause = swapScreen;

            keys = new KeySprite[]
            {
                new KeySprite(new Vector2(325, 300), Color.Purple),
                new KeySprite(new Vector2(150, 300), Color.Aqua),
                new KeySprite(new Vector2(500, 300), Color.Orange)
            };

            turrets = new AlienTurret[]
            {
                //TODO: Change to fixed positions at some point
                new AlienTurret(new Vector2(300, 150), content),
                new AlienTurret(new Vector2(400, 400), content)
            };
        }
        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            backgroundLeft = content.Load<Texture2D>("Dungeon_Tileset");
            backgroundRight = content.Load<Texture2D>("Dungeon_Tileset");
            foreach (var key in keys) key.LoadContent(content);
            foreach (var turret in turrets) turret.LoadContent();
            keyPickup = content.Load<SoundEffect>("Pickup_Key");
            knightHit = content.Load<SoundEffect>("Knight_Hit");

            //Walls
            topBounds = new BoundingRectangle(0, 0, graphics.Viewport.Width, 70);
            leftBounds = new BoundingRectangle(0, 0, 40, graphics.Viewport.Height);
            bottomBounds = new BoundingRectangle(0, graphics.Viewport.Height - 50, graphics.Viewport.Width, 50);
            rightBounds = new BoundingRectangle(graphics.Viewport.Width - 45, 0, 45, graphics.Viewport.Height);
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) pause(1);
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
                    keyPickup.Play();
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
                        knightHit.Play();
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.Black);
            spriteBatch.Draw(backgroundLeft, new Vector2(0, 0), new Rectangle(10, 0, 68, 70), Color.White, 0, new Vector2(0, 0), graphics.Viewport.Height / 65, SpriteEffects.None, 0);
            spriteBatch.Draw(backgroundRight, new Vector2(graphics.Viewport.Width / 2, 0), new Rectangle(40, 0, 68, 70), Color.White, 0, new Vector2(0, 0), graphics.Viewport.Height / 65, SpriteEffects.None, 0);
            knight.Draw(gameTime, spriteBatch, graphics);
            foreach (var key in keys) key.Draw(gameTime, spriteBatch);
            foreach (var turret in turrets) turret.Draw(gameTime, spriteBatch);

            //Debugging
            /*
            Texture2D blankTexture = new Texture2D(graphics, 1, 1);
            blankTexture.SetData(new Color[] { Color.DarkSlateGray });
            spriteBatch.Draw(blankTexture, new Vector2(topBounds.X, topBounds.Y), topBounds.Bounds(), Color.White);
            spriteBatch.Draw(blankTexture, new Vector2(leftBounds.X, leftBounds.Y), leftBounds.Bounds(), Color.White);
            spriteBatch.Draw(blankTexture, new Vector2(rightBounds.X, rightBounds.Y), rightBounds.Bounds(), Color.White);
            spriteBatch.Draw(blankTexture, new Vector2(bottomBounds.X, bottomBounds.Y), bottomBounds.Bounds(), Color.White);
            */
        }
    }
}
