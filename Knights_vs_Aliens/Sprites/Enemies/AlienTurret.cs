using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Knights_vs_Aliens.Collisions;
using SharpDX.Direct2D1.Effects;

namespace Knights_vs_Aliens.Sprites.Enemies
{
    public class AlienTurret
    {
        private Texture2D texture;
        private Texture2D blankTexture;
        private Texture2D heartTexture;

        public Vector2 Position;

        private double animationTimer;

        private bool hasShot;

        private int animationFrame = 0;

        public bool Invulnerable = false;

        private double invulnerabilityTimer;

        private ContentManager content;

        private SoundEffect laserShot;

        public Color color = Color.White;

        public BoundingRectangle Bounds;

        private int maxHealth = 2;
        public int CurHealth = 2;

        public List<AlienProjectile> projectiles = new List<AlienProjectile>();

        public AlienTurret(Vector2 position, ContentManager content)
        {
            Position = position;
            this.content = content;
            Bounds = new BoundingRectangle(position.X - (12 * 2.5f), position.Y - (16 * 2.5f), 23 * 2.5f, 30 * 2.5f);
        }

        public void LoadContent()
        {
            texture = content.Load<Texture2D>("Alien Turret");
            laserShot = content.Load<SoundEffect>("Laser_Shoot");
            heartTexture = content.Load<Texture2D>("Heart");
        }

        public void Update(GameTime gameTime, Vector2 knightPosition)
        {
            if(animationFrame == 5 && !hasShot)
            {
                Shoot(knightPosition);
                hasShot = true;
            }

            if (Invulnerable)
            {
                invulnerabilityTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                color = Color.Red;

                if (invulnerabilityTimer < 0)
                {
                    Invulnerable = false;
                    color = Color.White;
                }
            }

            foreach (var projectile in projectiles) projectile.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            blankTexture = new Texture2D(graphics, 1, 1);
            blankTexture.SetData(new Color[] { Color.DarkSlateGray });

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

            //Draw Health
            var source = new Rectangle();
            int tempCounter = CurHealth;
            for (int i = 0; i < maxHealth; i++)
            {
                if (tempCounter > 0)
                {
                    source = new Rectangle(0, 0, 64, 64);
                    tempCounter--;
                }
                else
                {
                    source = new Rectangle(128, 0, 64, 64);
                }
                spriteBatch.Draw(heartTexture, new Vector2((int)(Position.X - (6 * 2.5) + (16 * i)), (int)(Position.Y + (14 * 2.5))), source, Color.White, 0, new Vector2(16, 16), 0.5f, SpriteEffects.None, 0);
            }

            source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(texture, Position, source, color, 0, new Vector2(16, 16), 2.5f, SpriteEffects.None, 0);
            //spriteBatch.Draw(blankTexture, new Vector2(Bounds.X, Bounds.Y), Bounds.Bounds(), Color.White);
            foreach (var projectile in projectiles) projectile.Draw(gameTime, spriteBatch);
        }

        //Don't want bullets tied to turrets in long run
        private void Shoot(Vector2 knightPosition)
        {
            laserShot.Play();
            AlienProjectile newProjectile = new AlienProjectile(new Vector2(Position.X + 16, Position.Y + 4), knightPosition - Position);
            newProjectile.LoadContent(content);
            projectiles.Add(newProjectile);
        }

        public void TurretHit()
        {
            color = Color.Red;
            CurHealth--;
            invulnerabilityTimer = 0.5;
            Invulnerable = true;
        }
    }
}
