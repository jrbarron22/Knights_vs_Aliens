using Knights_vs_Aliens.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Knights_vs_Aliens.Screens
{
    public delegate void switchScreen();

    public class ScreenManager
    {
        private IScreen[] screens;
        private IScreen curScreen;
        private int curIndex;

        private GraphicsDevice graphics;
        private ContentManager content;

        private KnightSprite knight;

        public ScreenManager(GraphicsDevice graphics, ContentManager content)
        {
            this.graphics = graphics;
            this.content = content;
            Initialize();
        }

        private void Initialize()
        {
            knight = new KnightSprite();
            screens = new IScreen[2];
            screens[0] = new TitleMenu(new switchScreen(SwitchScreen));
            screens[1] = new OpeningRoom(graphics, knight, content);
            curIndex = 0;
            curScreen = screens[curIndex];
            foreach (IScreen screen in screens) screen.LoadContent(graphics, content);
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            knight.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            curScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            curScreen.Draw(gameTime, spriteBatch, graphics);
        }

        private void SwitchScreen()
        {
            curScreen = screens[curIndex++];
        }
    }
}
