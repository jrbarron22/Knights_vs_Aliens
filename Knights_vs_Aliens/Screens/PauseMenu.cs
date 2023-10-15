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
    public enum PauseOptions
    {
        Resume,
        Controls,
        MainMenu
    }
    public class PauseMenu : IScreen
    {
        private KeyboardState curKeyboardState;
        private KeyboardState prevKeyboardState;

        private PauseOptions curPauseOption = PauseOptions.Resume;

        private switchScreen resumePressed;
        private switchScreen  mainMenuPressed;
        private string[] menuButtons = { "Resume", "Controls", "Main Menu" };

        private SpriteFont phudu;
        private Texture2D alienTube;

        public PauseMenu(switchScreen changeScreen)
        {
            resumePressed = changeScreen;
            mainMenuPressed = changeScreen;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.DarkGray);
            Vector2 spriteLength;
            spriteBatch.Begin();

            //Draw Menu Buttons
            for (int i = 0; i < menuButtons.Length; i++)
            {
                float fontScale = 1;
                if ((int)curPauseOption == i)
                {

                    fontScale = 1.2f;
                }
                spriteLength = phudu.MeasureString(menuButtons[i]);
                spriteBatch.DrawString(phudu, menuButtons[i], new Vector2(graphics.Viewport.Width / 2, 250 + 50 * i), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), fontScale, SpriteEffects.None, 0);

                if ((int)curPauseOption == i)
                {
                    spriteBatch.Draw(alienTube, new Vector2(graphics.Viewport.Width / 2 - spriteLength.X / 2 - 40, 250 + 50 * i), null, Color.White, 0, new Vector2(16, 16), 2.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(alienTube, new Vector2(graphics.Viewport.Width / 2 + spriteLength.X / 2 + 30, 250 + 50 * i), null, Color.White, 0, new Vector2(16, 16), 2.0f, SpriteEffects.None, 0);
                }
            }

            //Draw Title
            spriteLength = phudu.MeasureString("Paused");
            spriteBatch.DrawString(phudu, "Paused", new Vector2(graphics.Viewport.Width / 2, 50), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 1.35f, SpriteEffects.None, 0);

            //Draw Exit Instructions
            spriteLength = phudu.MeasureString("Press 'Space' on the Main Menu button to exit the game");
            spriteBatch.DrawString(phudu, "Press 'Space' on the Main Menu button to exit the game", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height - 50), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 0.4f, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        public void GameUnpaused(){}

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            phudu = content.Load<SpriteFont>("phudu");
            alienTube = content.Load<Texture2D>("Alien Tube");
        }

        public void Update(GameTime gameTime)
        {
            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();

            if ((curPauseOption == PauseOptions.Resume) && ((curKeyboardState.IsKeyDown(Keys.Space) || (curKeyboardState.IsKeyDown(Keys.Enter)))))
            {
                resumePressed(2);
            }
            else if ((curPauseOption == PauseOptions.MainMenu) && ((curKeyboardState.IsKeyDown(Keys.Space) || (curKeyboardState.IsKeyDown(Keys.Enter)))))
            {
                mainMenuPressed(0);
            }

            if (curKeyboardState.IsKeyDown(Keys.W) && prevKeyboardState.IsKeyUp(Keys.W))
            {
                switch (curPauseOption)
                {
                    case PauseOptions.Resume:
                        break;
                    case PauseOptions.Controls:
                    case PauseOptions.MainMenu:
                        curPauseOption--;
                        break;
                }
            }
            if (curKeyboardState.IsKeyDown(Keys.S) && prevKeyboardState.IsKeyUp(Keys.S))
            {
                switch (curPauseOption)
                {
                    case PauseOptions.MainMenu:
                        break;
                    case PauseOptions.Controls:
                    case PauseOptions.Resume:
                        curPauseOption++;
                        break;
                }
            }
        }
    }
}
