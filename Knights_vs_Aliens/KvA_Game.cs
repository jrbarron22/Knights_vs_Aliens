using Knights_vs_Aliens.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using System.Drawing.Drawing2D;

namespace Knights_vs_Aliens
{
    public delegate void quitGame();
    public class KvA_Game : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        private ScreenManager screenManager;

        public KvA_Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            screenManager = new ScreenManager(this, GraphicsDevice, Content, new quitGame(Quit));
            
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            screenManager.LoadContent(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.CapsLock)) Exit();
            // TODO: Add your update logic here
            screenManager.Update(gameTime);
           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            screenManager.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }

        private void Quit()
        {
            this.Exit();
        }
    }
}