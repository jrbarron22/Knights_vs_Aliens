using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Knights_vs_Aliens.Sprites.Enemies
{
    public class AlienTurret
    {
        private Texture2D texture;

        public Vector2 Position;

        private double animationTimer;

        private bool hasShot;

        private int animationFrame = 0;

        private ContentManager content;

        public List<AlienProjectile> projectiles = new List<AlienProjectile>();

        public AlienTurret(Vector2 position, ContentManager content)
        {
            Position = position;
            this.content = content;
        }

        public void LoadContent()
        {
            texture = content.Load<Texture2D>("Alien Turret");
        }

        public void Update(GameTime gameTime, Vector2 knightPosition)
        {
            if(animationFrame == 5 && !hasShot)
            {
                Shoot(knightPosition);
                hasShot = true;
            }
            foreach (var projectile in projectiles) projectile.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTimer > 0.5)
            {
                animationFrame++;
                if (animationFrame > 5)
                {
                    hasShot = false;
                    animationFrame = 0;
                }
                animationTimer -= 0.5;
            }

            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color.White, 0, new Vector2(16, 16), 2.5f, SpriteEffects.None, 0);
            foreach (var projectile in projectiles) projectile.Draw(gameTime, spriteBatch);
        }

        //Don't want bullets tied to turrets in long run
        private void Shoot(Vector2 knightPosition)
        {
            AlienProjectile newProjectile = new AlienProjectile(new Vector2(Position.X + 16, Position.Y + 4), knightPosition - Position);
            newProjectile.LoadContent(content);
            projectiles.Add(newProjectile);
        }
    }
}
