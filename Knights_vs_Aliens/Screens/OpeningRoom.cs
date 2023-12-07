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
using Microsoft.Xna.Framework.Audio;
using Knights_vs_Aliens.Collisions;

namespace Knights_vs_Aliens.Screens
{
    public class OpeningRoom : IScreen
    {
        private ContentManager content;

        private KnightSprite knight;

        private KeySprite[] keys;

        private AlienTurret[] turrets;

        private Tileset tileset;

        private Texture2D background;
        private SpriteFont phudu;

        private SoundEffect keyPickup;
        private SoundEffect knightHit;

        private int keysCollected = 0;

        private BoundingRectangle topBounds;
        private BoundingRectangle bottomBounds;
        private BoundingRectangle leftBounds;
        private BoundingRectangle rightBounds;

        private switchScreen pause;
        private switchScreen victory;
        private switchScreen defeat;

        private GraphicsDevice graphics;

        public OpeningRoom(Game game, GraphicsDevice graphics, KnightSprite knight, ContentManager content, switchScreen swapScreen)
        {
            Random rand = new Random();
            this.knight = knight;
            pause = swapScreen;
            victory = swapScreen;
            defeat = swapScreen;
            this.graphics = graphics;
            tileset = new Tileset();

            keys = new KeySprite[]
            {
                new KeySprite(new Vector2(600, 200), Color.Purple),
                new KeySprite(new Vector2(150, 250), Color.Aqua),
                new KeySprite(new Vector2(500, 350), Color.Orange)
            };

            turrets = new AlienTurret[]
            {
                new AlienTurret(new Vector2(300, 150), content),
                new AlienTurret(new Vector2(400, 300), content),
            };
        }
        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            tileset = content.Load<Tileset>("tilemap");
            //background = content.Load<Texture2D>("Dungeon_Tileset");
            foreach (var key in keys) key.LoadContent(content);
            foreach (var turret in turrets) turret.LoadContent();
            keyPickup = content.Load<SoundEffect>("Pickup_Key");
            knightHit = content.Load<SoundEffect>("Knight_Hit");
            phudu = content.Load<SpriteFont>("phudu");

            InitializeWalls();

            //Walls
            /*
            topBounds = new BoundingRectangle(0, 0, (1.5f) * graphics.Viewport.Width, 70);
            leftBounds = new BoundingRectangle(0, 0, 40, graphics.Viewport.Height);
            bottomBounds = new BoundingRectangle(0, graphics.Viewport.Height - 50, (1.5f) * graphics.Viewport.Width, 50);
            rightBounds = new BoundingRectangle(graphics.Viewport.Width + (38 * graphics.Viewport.Height / 65), 0, 45, graphics.Viewport.Height);
            */
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                pause(ScreenName.PauseScreen, ScreenName.OpeningRoom);
                return;
            }

            knight.Update(gameTime);
            
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
            if (keysCollected == 3) victory(ScreenName.SecondRoom, ScreenName.OpeningRoom);

            //Check Knight Collisions with Projectiles
            foreach (var turret in turrets)
            {
                if (turret.CurHealth != 0)
                {
                    turret.color = Color.White;
                    turret.Update(gameTime, knight.Position);
                    foreach (var projectile in turret.projectiles)
                    {
                        if (projectile.bounds.CollidesWith(knight.Bounds) && !projectile.hasCollided)
                        {
                            projectile.hasCollided = true;
                            if (!knight.Invulnerable)
                            {
                                knight.KnightHit(gameTime);
                                knightHit.Play();
                            }
                        }
                        if (projectile.bounds.CollidesWith(topBounds) || projectile.bounds.CollidesWith(rightBounds) || projectile.bounds.CollidesWith(leftBounds) || projectile.bounds.CollidesWith(bottomBounds))
                        {
                            projectile.hasCollided = true;
                        }
                    }
                    //Check Weapon collision with turrets
                    if (knight.Weapon.IsAttackActive() && knight.Weapon.GetAttackBounds().CollidesWith(turret.Bounds) && !turret.Invulnerable)
                    {
                        turret.TurretHit();
                    }
                }
            }

            //Check Defeat
            if (knight.CurHealth == 0) defeat(ScreenName.DefeatScreen, ScreenName.OpeningRoom);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.Black);

            spriteBatch.Begin();
            tileset.Draw(gameTime, spriteBatch);
            
            foreach (var key in keys) key.Draw(gameTime, spriteBatch);
            foreach (var turret in turrets)
            {
                if (turret.CurHealth != 0)
                {
                    turret.Draw(gameTime, spriteBatch, graphics);
                }
            }
            knight.Draw(gameTime, spriteBatch, graphics);
            spriteBatch.End();

            //Debugging
            /*
            spriteBatch.Begin(transformMatrix: transform);
            Texture2D blankTexture = new Texture2D(graphics, 1, 1);
            blankTexture.SetData(new Color[] { Color.DarkSlateGray });
            spriteBatch.Draw(blankTexture, new Vector2(topBounds.X, topBounds.Y), topBounds.Bounds(), Color.White);
            spriteBatch.Draw(blankTexture, new Vector2(leftBounds.X, leftBounds.Y), leftBounds.Bounds(), Color.White);
            spriteBatch.Draw(blankTexture, new Vector2(rightBounds.X, rightBounds.Y), rightBounds.Bounds(), Color.White);
            spriteBatch.Draw(blankTexture, new Vector2(bottomBounds.X, bottomBounds.Y), bottomBounds.Bounds(), Color.White);
            spriteBatch.End();
             */
        }

        public void GameUnpaused()
        {
        }

        private void InitializeWalls()
        {
            //Walls
            topBounds = new BoundingRectangle(0, 0, tileset.TileWidth * tileset.MapWidth, 30);
            leftBounds = new BoundingRectangle(0, 0, 20, tileset.TileHeight * tileset.MapHeight);
            bottomBounds = new BoundingRectangle(0, tileset.TileHeight * tileset.MapHeight - 30, tileset.TileWidth * tileset.MapWidth, 20);
            rightBounds = new BoundingRectangle(tileset.TileWidth * tileset.MapWidth - 20, 0, 20, tileset.TileHeight * tileset.MapHeight);
        }

        public void LevelReset()
        {
            keysCollected = 0;
            foreach (var key in keys) key.Collected = false;
            foreach (var turret in turrets) turret.CurHealth = 2;
            knight.Position = new Vector2(200, 200);
        }
    }
}
