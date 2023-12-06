using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tilemap;
using Knights_vs_Aliens.Sprites;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Knights_vs_Aliens.Sprites.Enemies;
using Knights_vs_Aliens.Collisions;

namespace Knights_vs_Aliens.Screens
{
    public class SecondRoom : IScreen
    {
        private Game game;
        private ContentManager content;
        private GraphicsDevice graphics;

        private switchScreen pause;
        private switchScreen victory;
        private switchScreen defeat;

        private KnightSprite knight;

        private Tileset tileset;

        private KeySprite[] keys;
        private int keysCollected = 0;

        private WallLaser[] lasers;
        private AlienTurret[] turrets;

        private SoundEffect keyPickup;
        private SoundEffect knightHit;

        private BoundingRectangle topBounds;
        private BoundingRectangle bottomBounds;
        private BoundingRectangle leftBounds;
        private BoundingRectangle rightBounds;

        public SecondRoom(Game game, GraphicsDevice graphics, ContentManager content, switchScreen swapScreen, KnightSprite knight) 
        {
            this.content = content;
            this.game = game;
            pause = swapScreen;
            victory = swapScreen;
            defeat = swapScreen;
            this.knight = knight;
            this.graphics = graphics;
            tileset = new Tileset();

            keys = new KeySprite[]
            {
                new KeySprite(new Vector2(600, 200), Color.Purple),
                new KeySprite(new Vector2(150, 250), Color.Aqua),
                new KeySprite(new Vector2(500, 350), Color.Orange)
            };

            lasers = new WallLaser[]{
                new WallLaser(game, new Vector2(200, 50), content),
                new WallLaser(game, new Vector2(500, 50), content)
            };

            turrets = new AlienTurret[]
            {
                new AlienTurret(new Vector2(300, 150), content),
                new AlienTurret(new Vector2(400, 400), content),
                new AlienTurret(new Vector2(600, 200), content)
            };
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            tileset = content.Load<Tileset>("tilemap");
            foreach (var key in keys) key.LoadContent(content);
            keyPickup = content.Load<SoundEffect>("Pickup_Key");
            knightHit = content.Load<SoundEffect>("Knight_Hit");
            foreach (var laser in lasers) laser.LoadContent();
            foreach (var turret in turrets) turret.LoadContent();

            InitializeWalls();
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                foreach (var laser in lasers) laser.GamePaused();
                pause(ScreenName.PauseScreen, ScreenName.SecondRoom);
                return;
            }

            knight.Update(gameTime);
            knight.color = Color.White;

            //Check Knight Collisions with Walls
            if (topBounds.CollidesWith(knight.Bounds))
            {
                knight.Position.Y = topBounds.Height + (knight.Bounds.Height / 2) + 1;
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

            foreach (var key in keys)
            {
                if (!key.Collected && key.Bounds.CollidesWith(knight.Bounds))
                {
                    key.Collected = true;
                    keysCollected++;
                    keyPickup.Play();
                }
                key.Update(gameTime);
            }

            //Check Win Condition
            if (keysCollected == 3)
            {
                foreach (var laser in lasers) laser.GamePaused();
                victory(ScreenName.VictoryScreen, ScreenName.SecondRoom);
            }

            //Check Knight Collisions with Laser
            foreach (var laser in lasers)
            {
                laser.Update(gameTime);
                if (laser.LaserParticles.IsActive && laser.LaserBounds.CollidesWith(knight.Bounds))
                {
                    if (!knight.Invulnerable)
                    {
                        knight.KnightHit(gameTime);
                        knightHit.Play();
                    }
                }
            }

            
            foreach (var turret in turrets)
            {
                turret.Update(gameTime, knight.Position);
                foreach (var projectile in turret.projectiles)
                {
                    //Check Knight Collisions with Projectiles
                    if (projectile.bounds.CollidesWith(knight.Bounds))
                    {
                        projectile.hasCollided = true;
                        if (!knight.Invulnerable)
                        {
                            knight.color = Color.Red;
                            knightHit.Play();
                            knight.CurHealth--;
                            knight.Invulnerable = true;
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

            //Check Defeat
            if (knight.CurHealth == 0) defeat(ScreenName.DefeatScreen, ScreenName.SecondRoom);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.Black);

            //Calculate our offset vector
            //TODO: Calculate clamp without guessing

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

                foreach (var laser in lasers) laser.Draw(gameTime, spriteBatch); 
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
            foreach (var laser in lasers) laser.GameUnpaused();
        }

        public void LevelReset()
        {
            keysCollected = 0;
            foreach (var key in keys) key.Collected = false;
            knight.Position = new Vector2(200, 200);
        }

        private void InitializeWalls()
        {
            //Walls
            topBounds = new BoundingRectangle(0, 0, tileset.TileWidth * tileset.MapWidth, 30);
            leftBounds = new BoundingRectangle(0, 0, 20, tileset.TileHeight * tileset.MapHeight);
            bottomBounds = new BoundingRectangle(0, tileset.TileHeight * tileset.MapHeight - 30, tileset.TileWidth * tileset.MapWidth, 20);
            rightBounds = new BoundingRectangle(tileset.TileWidth * tileset.MapWidth - 20, 0, 20, tileset.TileHeight * tileset.MapHeight);
        }
    }
}
