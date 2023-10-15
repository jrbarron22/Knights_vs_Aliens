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
    public class VictoryScreen : IScreen
    {
        private KeyboardState curKeyboardState;

        private SpriteFont phudu;

        private switchScreen titleScreen;

        public VictoryScreen(switchScreen changeScreen)
        {
            titleScreen = changeScreen;
        }

        public void GameUnpaused() { }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            phudu = content.Load<SpriteFont>("phudu");
        }

        public void Update(GameTime gameTime)
        {
            curKeyboardState = Keyboard.GetState();

            if(curKeyboardState.IsKeyDown(Keys.Enter) || curKeyboardState.IsKeyDown(Keys.Space))
            {
                titleScreen(0);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.DarkGray);
            Vector2 spriteLength;
            spriteBatch.Begin();

            //Draw Title
            spriteLength = phudu.MeasureString("Victory!");
            spriteBatch.DrawString(phudu, "Victory!", new Vector2(graphics.Viewport.Width / 2, 150), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 1.35f, SpriteEffects.None, 0);


            //Draw Exit Instructions
            spriteLength = phudu.MeasureString("Press 'Space' or 'Enter' to exit the game");
            spriteBatch.DrawString(phudu, "Press 'Space' or 'Enter' to exit the game", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height - 50), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 0.4f, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
