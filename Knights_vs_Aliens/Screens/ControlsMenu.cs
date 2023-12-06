using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knights_vs_Aliens.Screens
{
    public class ControlsMenu : IScreen
    {
        private SpriteFont phudu;

        private switchScreen back;

        public ControlsMenu(switchScreen swap)
        {
            back = swap;
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            phudu = content.Load<SpriteFont>("phudu");
        }
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                back(ScreenName.TitleScreen, ScreenName.ControlsScreen);
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.DarkGray);
            Vector2 spriteLength;
            spriteBatch.Begin();
            spriteLength = phudu.MeasureString("Movement - WASD");
            spriteBatch.DrawString(phudu, "Movement - WASD", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2 - 50), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 1f, SpriteEffects.None, 0);
            spriteLength = phudu.MeasureString("Attack - Left Mouse Click");
            spriteBatch.DrawString(phudu, "Attack - Left Mouse Click", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2 + 50), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 1f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public void GameUnpaused() { }

        public void LevelReset() { }
    }
}
