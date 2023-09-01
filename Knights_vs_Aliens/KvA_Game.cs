using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using System.Drawing.Drawing2D;

namespace Knights_vs_Aliens
{
    public class KvA_Game : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        private TitleMenu menu;

        private SpriteFont phudu;
        private AlienOrbSprite[] alienOrbs;
        private Texture2D alienTube;
        private Texture2D castle;
        private Texture2D sword;

        public KvA_Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            menu = new TitleMenu();
            alienOrbs = new AlienOrbSprite[]
            {
                new AlienOrbSprite(){Position = new Vector2(35, (GraphicsDevice.Viewport.Height / 2) + 100), Direction = Direction.Up },
                new AlienOrbSprite(){Position = new Vector2(GraphicsDevice.Viewport.Width - 35, (GraphicsDevice.Viewport.Height / 2) - 100), Direction = Direction.Down }
            };

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            phudu = Content.Load<SpriteFont>("phudu");
            foreach (var orb in alienOrbs) orb.LoadContent(Content);
            alienTube = Content.Load<Texture2D>("Alien Tube");
            sword = Content.Load<Texture2D>("Sword");
            castle = Content.Load<Texture2D>("Castle");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)
                || (menu.curMenuOption == menuOptions.Exit && ((Keyboard.GetState().IsKeyDown(Keys.Space) || (Keyboard.GetState().IsKeyDown(Keys.Enter))))))
                Exit();

            // TODO: Add your update logic here
            menu.Update();
            foreach (var orb in alienOrbs) orb.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            Vector2 spriteLength;

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //Draw Menu Buttons
            for(int i = 0; i < menu.menuButtons.Length; i++)
            {
                float fontScale = 1;
                
                if ((int)menu.curMenuOption == i)
                {
                    
                    fontScale = 1.2f;
                }
                spriteLength = phudu.MeasureString(menu.menuButtons[i]);
                spriteBatch.DrawString(phudu, menu.menuButtons[i], new Vector2(GraphicsDevice.Viewport.Width/2, 250 + 50 * i), Color.Black, 0, new Vector2((spriteLength.X / 2), (spriteLength.Y / 2)), fontScale, SpriteEffects.None, 0);
                
                if((int) menu.curMenuOption == i)
                {
                    spriteBatch.Draw(alienTube, new Vector2((GraphicsDevice.Viewport.Width / 2) - (spriteLength.X / 2) - 40, 250 + 50 * i), null, Color.White, 0, new Vector2(16, 16), 2.0f, SpriteEffects.None, 0);
                    spriteBatch.Draw(alienTube, new Vector2((GraphicsDevice.Viewport.Width / 2) + (spriteLength.X / 2) + 30, 250 + 50 * i), null, Color.White, 0, new Vector2(16, 16), 2.0f, SpriteEffects.None, 0);
                }

            }

            //Draw Alien Orbs
            foreach (var orb in alienOrbs) orb.Draw(gameTime, spriteBatch);

            //Draw Title
            spriteLength = phudu.MeasureString("Knights vs Aliens");
            spriteBatch.DrawString(phudu, "Knights vs Aliens", new Vector2(GraphicsDevice.Viewport.Width / 2, 50), Color.Black, 0, new Vector2(spriteLength.X /2, spriteLength.Y/2), 1.35f, SpriteEffects.None, 0);

            //Draw Castle
            spriteBatch.Draw(castle, new Vector2((GraphicsDevice.Viewport.Width / 2), 175), null, Color.White, 0, new Vector2(64, 64), 1.0f, SpriteEffects.None, 0);

            //Draw Swords
            spriteBatch.Draw(sword, new Vector2((GraphicsDevice.Viewport.Width / 2) + 96, 175), null, Color.White, 0, new Vector2(8, 8), 2.0f, SpriteEffects.None, 0);
            spriteBatch.Draw(sword, new Vector2((GraphicsDevice.Viewport.Width / 2) - 128, 175), null, Color.White, 0, new Vector2(8, 8), 2.0f, SpriteEffects.None, 0);

            spriteLength = phudu.MeasureString("Press 'Space' or 'Exit' on the Exit button to exit the game");
            spriteBatch.DrawString(phudu, "Press 'Space' or 'Exit' on the Exit button to exit the game", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - 50), Color.Black, 0, new Vector2(spriteLength.X / 2, spriteLength.Y / 2), 0.4f, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}