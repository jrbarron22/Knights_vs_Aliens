using Knights_vs_Aliens.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Knights_vs_Aliens.Screens
{
    public delegate void switchScreen(ScreenName newScreen, ScreenName prevScreen);

    public class ScreenManager
    {
        private IScreen[] screens;
        private IScreen curScreen;

        private GraphicsDevice graphics;
        private ContentManager content;
        private Game game;

        private KnightSprite knight;

        private quitGame exitGame;

        private Song titleMusic;
        private Song gameplayMusic;

        public ScreenManager(Game game, GraphicsDevice graphics, ContentManager content, quitGame quit)
        {
            this.game = game;
            this.graphics = graphics;
            this.content = content;
            exitGame = quit;
            Initialize();
        }

        private void Initialize()
        {
            knight = new KnightSprite();
            screens = new IScreen[8];

            screens[(int)ScreenName.TitleScreen] = new TitleMenu(new switchScreen(SwitchScreen), exitGame);
            screens[(int)ScreenName.OpeningRoom] = new OpeningRoom(game, graphics, knight, content, new switchScreen(SwitchScreen));
            screens[(int)ScreenName.VictoryScreen] = new VictoryScreen(new switchScreen(SwitchScreen));
            screens[(int)ScreenName.SecondRoom] = new SecondRoom(game, graphics, content, new switchScreen(SwitchScreen), knight);
            screens[(int)ScreenName.ControlsScreen] = new ControlsMenu(new switchScreen(SwitchScreen));
            screens[(int)ScreenName.DefeatScreen] = new DefeatScreen(new switchScreen(SwitchScreen));

            curScreen = screens[(int)ScreenName.TitleScreen];
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            foreach (IScreen screen in screens)
            {
                if(screen != null) screen.LoadContent(graphics, content);
            }
            knight.LoadContent(content);
            titleMusic = content.Load<Song>("Denys Kyshchuk - Wide Flower Fields");
            gameplayMusic = content.Load<Song>("Aldous Ichnite - The Rise of the 3D Accelerator");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(titleMusic);
        }

        public void Update(GameTime gameTime)
        {
            curScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            curScreen.Draw(gameTime, spriteBatch, graphics);
        }

        private void SwitchScreen(ScreenName newScreen, ScreenName prevScreen)
        {
            
            MediaPlayer.Stop();

            if(prevScreen == ScreenName.DefeatScreen)
            {
                knight.Reset();
            }


            if ((newScreen == ScreenName.OpeningRoom || newScreen == ScreenName.SecondRoom) && (prevScreen == ScreenName.TitleScreen || prevScreen == ScreenName.OpeningRoom))
            {
                //Switching to a gameplay screen but the previous screen is not a loading screen
                screens[(int)ScreenName.LoadingScreen] = new LoadingScreen(new switchScreen(SwitchScreen), newScreen);
                screens[(int)ScreenName.LoadingScreen].LoadContent(graphics, content);
                curScreen = screens[(int)ScreenName.LoadingScreen];
                return;
            }
            else if (newScreen == ScreenName.PauseScreen)
            {
                screens[(int)ScreenName.PauseScreen] = new PauseMenu(new switchScreen(SwitchScreen), prevScreen);
                screens[(int)ScreenName.PauseScreen].LoadContent(graphics, content);
            }

            //TODO Tie music to the different screens?
            curScreen = screens[(int)newScreen];

            if (newScreen == ScreenName.OpeningRoom || newScreen == ScreenName.SecondRoom)
            {
                MediaPlayer.Play(gameplayMusic);
                if (prevScreen == ScreenName.PauseScreen)
                {
                    //Switching to gameplay screen from a loading screen
                    screens[(int)newScreen].GameUnpaused();
                }
                else if(prevScreen == ScreenName.LoadingScreen)
                {
                    curScreen.LevelReset();
                }
            }
            else if (newScreen == ScreenName.TitleScreen)
            {
                MediaPlayer.Play(titleMusic);
            }
        }
    }
}
