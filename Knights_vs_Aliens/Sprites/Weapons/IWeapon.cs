using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knights_vs_Aliens.Sprites.Weapons
{
    public interface IWeapon
    {
        void UpdatePosition(Direction knightDirection, Vector2 knightPosition);

        void UpdateAttack(GameTime gameTime, Direction knightDirection);
        void LoadContent(ContentManager content);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics);
    }
}
