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
    public delegate void switchScreen(int index);

    public class ScreenManager
    {
        private IScreen[] screens;
        private IScreen curScreen;
        private int curIndex;

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
            //TODO: Make Screens an enum
            knight = new KnightSprite();
            screens = new IScreen[4];

            screens[0] = new TitleMenu(new switchScreen(SwitchScreen), exitGame);
            screens[1] = new PauseMenu(new switchScreen(SwitchScreen));
            screens[2] = new OpeningRoom(game, graphics, knight, content, new switchScreen(SwitchScreen));
            screens[3] = new VictoryScreen(new switchScreen(SwitchScreen));

            curScreen = screens[0];
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            foreach (IScreen screen in screens) screen.LoadContent(graphics, content);
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

        private void SwitchScreen(int index)
        {
            curScreen = screens[index];
            if(index == 2)
            {
                screens[2].GameUnpaused();
            }
            MediaPlayer.Stop();
            if (index == 0)
            {
                MediaPlayer.Play(titleMusic);
            }
            if(index == 2)
            {
                MediaPlayer.Play(gameplayMusic);
            }
        }
    }
}
