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

        private WallLaser[] lasers;

        private Texture2D background;

        private SoundEffect keyPickup;
        private SoundEffect knightHit;

        private int keysCollected = 0;

        private BoundingRectangle topBounds;
        private BoundingRectangle bottomBounds;
        private BoundingRectangle leftBounds;
        private BoundingRectangle rightBounds;

        private switchScreen pause;
        private switchScreen victory;

        public OpeningRoom(Game game, GraphicsDevice graphics, KnightSprite knight, ContentManager content, switchScreen swapScreen)
        {
            Random rand = new Random();
            this.knight = knight;
            pause = swapScreen;
            victory = swapScreen;

            keys = new KeySprite[]
            {
                new KeySprite(new Vector2(800, 200), Color.Purple),
                new KeySprite(new Vector2(150, 250), Color.Aqua),
                new KeySprite(new Vector2(500, 400), Color.Orange)
            };

            turrets = new AlienTurret[]
            {
                //TODO: Change to fixed positions at some point
                new AlienTurret(new Vector2(300, 150), content),
                new AlienTurret(new Vector2(400, 400), content),
                new AlienTurret(new Vector2(900, 200), content)
            };

            lasers = new WallLaser[]{
                new WallLaser(game, new Vector2(200, 50), content),
                new WallLaser(game, new Vector2(700, 50), content)
            };
        }
        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            background = content.Load<Texture2D>("Dungeon_Tileset");
            foreach (var key in keys) key.LoadContent(content);
            foreach (var turret in turrets) turret.LoadContent();
            foreach (var laser in lasers) laser.LoadContent();
            keyPickup = content.Load<SoundEffect>("Pickup_Key");
            knightHit = content.Load<SoundEffect>("Knight_Hit");

            //Walls
            topBounds = new BoundingRectangle(0, 0, (1.5f) * graphics.Viewport.Width, 70);
            leftBounds = new BoundingRectangle(0, 0, 40, graphics.Viewport.Height);
            bottomBounds = new BoundingRectangle(0, graphics.Viewport.Height - 50, (1.5f) * graphics.Viewport.Width, 50);
            rightBounds = new BoundingRectangle(graphics.Viewport.Width + (38 * graphics.Viewport.Height / 65), 0, 45, graphics.Viewport.Height);
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                foreach (var laser in lasers) laser.GamePaused();
                pause(1);
                return;
            }
            knight.Update(gameTime);
            knight.color = Color.White;

            //Check Knight Collisions with Walls
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

            //Check Knight Collisions with Keys

            foreach (var key in keys) {
                if (!key.Collected && key.Bounds.CollidesWith(knight.Bounds))
                {
                    key.Collected = true;
                    keysCollected++;
                    keyPickup.Play();
                }
                key.Update(gameTime);
            }

            //Check Win Condition
            if (keysCollected == 3) victory(3);

            //Check Knight Collisions with Projectiles
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

            //Check Knight Collisions with Laser
            foreach(var laser in lasers)
            {
                laser.Update(gameTime);
                if (laser.LaserParticles.IsActive && laser.LaserBounds.CollidesWith(knight.Bounds))
                {
                    knight.color = Color.Red;
                    knightHit.Play();
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.Black);

            //Calculate our offset vector
            float playerX = MathHelper.Clamp(knight.Position.X, 200, graphics.Viewport.Width - 300);
            float offsetX = 200 - playerX;

            Matrix transform = Matrix.CreateTranslation(offsetX, 0, 0);
            spriteBatch.Begin(transformMatrix: transform);            

            //Draw left side of background
            spriteBatch.Draw(background, new Vector2(0, 0), new Rectangle(10, 0, 68, 70), Color.White, 0, new Vector2(0, 0), graphics.Viewport.Height / 65, SpriteEffects.None, 0);

            //Draw middle of background
            spriteBatch.Draw(background, new Vector2(graphics.Viewport.Width/2, 0), new Rectangle(20, 0, 68, 70), Color.White, 0, new Vector2(0, 0), graphics.Viewport.Height / 65, SpriteEffects.None, 0);

            //Draw right side of background
            spriteBatch.Draw(background, new Vector2(graphics.Viewport.Width, 0), new Rectangle(40, 0, 45, 70), Color.White, 0, new Vector2(0, 0), graphics.Viewport.Height / 65, SpriteEffects.None, 0);

            knight.Draw(gameTime, spriteBatch, graphics);
            foreach (var key in keys) key.Draw(gameTime, spriteBatch);
            foreach (var turret in turrets) turret.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            foreach (var laser in lasers) laser.Draw(gameTime, spriteBatch, transform);
            

            //Debugging
            /*
            spriteBatch.Begin(transformMatrix: transform);
            Texture2D blankTexture = new Texture2D(graphics, 1, 1);
            blankTexture.SetData(new Color[] { Color.DarkSlateGray });
            spriteBatch.Draw(blankTexture, new Vector2(topBounds.X, topBounds.Y), topBounds.Bounds(), Color.White);
            spriteBatch.Draw(blankTexture, new Vector2(leftBounds.X, leftBounds.Y), leftBounds.Bounds(), Color.White);
            spriteBatch.Draw(blankTexture, new Vector2(rightBounds.X, rightBounds.Y), rightBounds.Bounds(), Color.White);
            spriteBatch.Draw(blankTexture, new Vector2(bottomBounds.X, bottomBounds.Y), bottomBounds.Bounds(), Color.White);
            */
        }

        public void GameUnpaused()
        {
            foreach (var laser in lasers) laser.GameUnpaused();
        }
    }
}
