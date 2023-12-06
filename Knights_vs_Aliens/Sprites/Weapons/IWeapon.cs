using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knights_vs_Aliens.Collisions;

namespace Knights_vs_Aliens.Sprites.Weapons
{
    public interface IWeapon
    {
        bool IsAttackActive();
        void UpdatePosition(Direction knightDirection, Vector2 knightPosition);
        void UpdateAttack(GameTime gameTime, Direction knightDirection, Vector2 knightPosition);
        BoundingCircle GetAttackBounds();
        void LoadContent(ContentManager content);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics, Direction knightDirection);
    }
}
