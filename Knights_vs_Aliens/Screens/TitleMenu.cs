using Knights_vs_Aliens.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Knights_vs_Aliens.Screens
{
    public enum menuOptions
    {
        StartGame,
        Controls,
        Exit
    }

    public class TitleMenu : IScreen
    {
        private KeyboardState curKeyboardState;
        private KeyboardState prevKeyboardState;

        private switchScreen startPressed;
        private switchScreen loadGame;
        private switchScreen controls;
        private quitGame exit;

        private menuOptions curMenuOption = menuOptions.StartGame;

        public string[] menuButtons = { "Start Game", "Controls", "Exit" };

        private SpriteFont phudu;
        private AlienOrbSprite[] alienOrbs;
        private Texture2D alienTube;
        private Texture2D castle;
        private Texture2D sword;

        public TitleMenu(switchScreen changeScreen, quitGame exit)
        {
            startPressed = changeScreen;
            loadGame = changeScreen;
            controls = changeScreen;
            this.exit = exit;
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            alienOrbs = new AlienOrbSprite[]
            {
                new AlienOrbSprite(){Position = new Vector2(35, (graphics.Viewport.Height / 2) + 100), Direction = Direction.Up },
                new AlienOrbSprite(){Position = new Vector2(graphics.Viewport.Width - 35, (graphics.Viewport.Height / 2) - 100), Direction = Direction.Down }
            };
            phudu = content.Load<SpriteFont>("phudu");
            foreach (var orb in alienOrbs) orb.LoadContent(content);
            alienTube = content.Load<Texture2D>("Alien Tube");
            castle = content.Load<Texture2D>("Castle");
        }

        public void Update(GameTime gameTime)
        {
            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();

            foreach (var orb in alienOrbs) orb.Update(gameTime);

            if((curMenuOption == menuOptions.StartGame) && (((curKeyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space)) || ((curKeyboardState.IsKeyDown(Keys.Enter)) && prevKeyboardState.IsKeyUp(Keys.Enter)))))
            {
                startPressed(ScreenName.GameplayScreen, ScreenName.TitleScreen);
            }
            /*
            else if ((curMenuOption == menuOptions.LoadGame) && (((curKeyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space)) || ((curKeyboardState.IsKeyDown(Keys.Enter)) && prevKeyboardState.IsKeyUp(Keys.Enter)))))
            {
                try
                {
                    using (StreamReader sr = File.OpenText("SaveState.txt"))
                    {
                        int index = int.Parse(sr.ReadLine());
                        loadGame((ScreenName)index, ScreenName.TitleScreen);
                    }
                }
                catch(FileNotFoundException e)
                {
                    return;
                }
            }
            */
            else if ((curMenuOption == menuOptions.Controls) && (((curKeyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space)) || ((curKeyboardState.IsKeyDown(Keys.Enter)) && prevKeyboardState.IsKeyUp(Keys.Enter)))))
            {
                controls(ScreenName.ControlsScreen, ScreenName.TitleScreen);
            }
            else if ((curMenuOption == menuOptions.Exit) && (((curKeyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space)) || ((curKeyboardState.IsKeyDown(Keys.Enter)) && prevKeyboardState.IsKeyUp(Keys.Enter)))))
            {
                exit();
            }

            if (curKeyboardState.IsKeyDown(Keys.W) && prevKeyboardState.IsKeyUp(Keys.W))
            {
                switch (curMenuOption)
                {
                    case menuOptions.StartGame:
                        break;
                    case menuOptions.Controls:
                    case menuOptions.Exit:
                        curMenuOption--;
                        break;
                }
            }
            if (curKeyboardState.IsKeyDown(Keys.S) && prevKeyboardState.IsKeyUp(Keys.S))
            {
                switch (curMenuOption)
                {
                    case menuOptions.StartGame:
                    case menuOptions.Controls:
                        curMenuOption++;
                        break;
                    case menuOptions.Exit:
                        break;
                }
            }
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
                if ((int)curMenuOption == i)
                {

                    fontScale = 1.2f;
                }
                spriteLength = phudu.MeasureString(menuButtons[i]);
                spriteBatch.DrawString(phudu, menuButtons[i], new Vector2(graphics.Viewport.Width / 2, 250 + 50 * i), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), fontScale, SpriteEffects.None, 0);

                if ((int)curMenuOption == i)
                {
                    spriteBatch.Draw(alienTube, new Vector2(graphics.Viewport.Width / 2 - spriteLength.X / 2 - 40, 250 + 50 * i), null, Color.White, 0, new Vector2(16, 16), 2.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(alienTube, new Vector2(graphics.Viewport.Width / 2 + spriteLength.X / 2 + 30, 250 + 50 * i), null, Color.White, 0, new Vector2(16, 16), 2.0f, SpriteEffects.None, 0);
                }

            }

            //Draw Alien Orbs
            foreach (var orb in alienOrbs) orb.Draw(gameTime, spriteBatch);

            //Draw Title
            spriteLength = phudu.MeasureString("Knights vs Aliens");
            spriteBatch.DrawString(phudu, "Knights vs Aliens", new Vector2(graphics.Viewport.Width / 2, 50), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 1.35f, SpriteEffects.None, 0);

            //Draw Castle
            spriteBatch.Draw(castle, new Vector2(graphics.Viewport.Width / 2, 175), null, Color.White, 0, new Vector2(64, 64), 1.0f, SpriteEffects.None, 0);

            //Draw Exit Instructions
            spriteLength = phudu.MeasureString("Press 'Space' on the Exit button to exit the game");
            spriteBatch.DrawString(phudu, "Press 'Space' on the Exit button to exit the game", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height - 50), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 0.4f, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        public void GameUnpaused() { }

        public void LevelReset() { }
    }
}