using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Knights_vs_Aliens.Screens
{
    public interface IScreen
    {
        void Update(GameTime gameTime);
        void LoadContent(GraphicsDevice graphics, ContentManager content);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics);
        public void GameUnpaused();
        public void LevelReset();
    }
}
