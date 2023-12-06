using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Knights_vs_Aliens.Particles;
using System.ComponentModel;
using Knights_vs_Aliens.Collisions;

namespace Knights_vs_Aliens.Sprites.Enemies
{
    public class WallLaser
    {
        private Game game;

        private Texture2D texture;

        public Vector2 Position;

        private double animationTimer;

        private int animationFrame = 0;

        private ContentManager content;

        public WallLaserParticleSystem LaserParticles;

        public BoundingRectangle LaserBounds;

        public WallLaser(Game game, Vector2 position, ContentManager content)
        {
            this.game = game;
            Position = position;
            this.content = content;
            Initialize();
        }

        private void Initialize()
        {
            LaserParticles = new WallLaserParticleSystem(game, new Rectangle((int)Position.X - 15, (int)Position.Y, 30, 8));
            LaserBounds = new BoundingRectangle((int)Position.X - 15, (int)Position.Y, 30, 2000);
            game.Components.Add(LaserParticles);
        }

        public void LoadContent()
        {
            texture = content.Load<Texture2D>("Wall Laser");
        }

        public void Update(GameTime gameTime)
        {
            if (animationFrame == 2)
            {
                LaserParticles.IsActive = true;
            }
            LaserParticles.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 1.2 && animationFrame != 2)
            {
                animationFrame++;
                animationTimer -= 1.2;
            }
            else if(animationTimer > 2)
            {
                animationFrame = 0;
                animationTimer -= 2;
                LaserParticles.IsActive = false;
            }

            var source = new Rectangle(animationFrame * 64, 0, 64, 64);

            spriteBatch.Draw(texture, Position, source, Color.White, 0, new Vector2(32, 32), 1f, SpriteEffects.None, 0);
            LaserParticles.Draw(gameTime);
        }

        public void GamePaused()
        {
            if(animationFrame == 2) LaserParticles.IsActive = false;
        }

        public void GameUnpaused()
        {
            if (animationFrame == 2) LaserParticles.IsActive = true;
        }
    }
}
