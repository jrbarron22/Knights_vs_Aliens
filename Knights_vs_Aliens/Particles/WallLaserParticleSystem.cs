using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knights_vs_Aliens.Particles
{
    public class WallLaserParticleSystem : ParticleSystem
    {
        private Rectangle _source;

        private bool _isActive = false;

        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                base.isActive = value;
            }
        }

        public WallLaserParticleSystem(Game game, Rectangle source) : base(game, 5000)
        {
            _source = source;
        }

        protected override void InitializeConstants()
        {
            textureFilename = "Particle";
            minNumParticles = 10;
            maxNumParticles = 20;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where, Vector2.UnitY * 500, Vector2.Zero, Color.LightGreen, scale: RandomHelper.NextFloat(0.1f, 0.4f), lifetime: 3);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsActive) AddParticles(_source);
        }
    }
}
