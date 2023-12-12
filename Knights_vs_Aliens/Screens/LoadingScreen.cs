using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knights_vs_Aliens.Screens
{
    public class LoadingScreen : IScreen
    {
        private SpriteFont phudu;

        private switchScreen swap;
        private ScreenName screenToSwitchTo;

        private double loadingTimer;

        public LoadingScreen(switchScreen swap, ScreenName newScreen)
        {
            this.swap = swap;
            screenToSwitchTo = newScreen;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.DarkGray);
            Vector2 spriteLength;
            spriteBatch.Begin();

            //Draw Title
            string text = "";
            switch (screenToSwitchTo)
            {
                case ScreenName.GameplayScreen: 
                    text = "Defend the Castle!";
                    break;
                default:
                    text = "Loading";
                    break;
            }

            spriteLength = phudu.MeasureString(text);
            spriteBatch.DrawString(phudu, text, new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height/2), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 1.35f, SpriteEffects.None, 0);

            //Draw Instructions
            spriteLength = phudu.MeasureString("Collect all of the keys!");
            spriteBatch.DrawString(phudu, "Collect all of the keys!", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height - 80), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 0.4f, SpriteEffects.None, 0);

            spriteLength = phudu.MeasureString("Press Esc to Pause");
            spriteBatch.DrawString(phudu, "Press Esc to Pause", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height - 40), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 0.4f, SpriteEffects.None, 0);


            spriteBatch.End();
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            phudu = content.Load<SpriteFont>("phudu");
        }

        public void Update(GameTime gameTime)
        {
            loadingTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (loadingTimer > 2)
            {
                loadingTimer = 0;
                swap(screenToSwitchTo, ScreenName.LoadingScreen);
            }
        }
        public void GameUnpaused() { }
        public void LevelReset() { }
    }
}
