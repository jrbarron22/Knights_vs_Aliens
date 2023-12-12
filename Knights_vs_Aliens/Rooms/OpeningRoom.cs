using Knights_vs_Aliens.Collisions;
using Knights_vs_Aliens.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knights_vs_Aliens.Rooms
{
    public class OpeningRoom : IGameplayRoom
    {
        private KeySprite[] keys;

        private KnightSprite knight;

        private Tileset tileset;

        private SoundEffect keyPickup;

        public bool AreDoorsOpen { get; set; } = true;

        public Dictionary<Direction, int[]> Doors { get; set; } = new Dictionary<Direction, int[]>();

        public Dictionary<Direction, BoundingRectangle> DoorBoxes { get; set; } = new Dictionary<Direction, BoundingRectangle>();
        public OpeningRoom(KnightSprite knight, Tileset tileset)
        {
            this.knight = knight;
            this.tileset = tileset;

            keys = new KeySprite[]
            {
                new KeySprite(new Vector2(400, 240), Color.Gold)
            };
            this.tileset = tileset;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            foreach (var key in keys) key.Draw(gameTime, spriteBatch);

            //For debugging
            /*
            Texture2D blankTexture = new Texture2D(graphics, 1, 1);
            blankTexture.SetData(new Color[] { Color.DarkRed });
            foreach (var door in doorBoxes.Values) spriteBatch.Draw(blankTexture, new Vector2(door.X, door.Y), door.Bounds(), Color.White);
            */
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            foreach (var key in keys) key.LoadContent(content);
            keyPickup = content.Load<SoundEffect>("Pickup_Key");

            InitializeDoorBoxes();
        }

        public void Update(GameTime gameTime, List<KeyValuePair<Direction, BoundingRectangle>> walls)
        {
            //Check Knight collision with Keys
            foreach (var key in keys)
            {
                if (!key.Collected && key.Bounds.CollidesWith(knight.Bounds))
                {
                    key.Collected = true;
                    knight.KeysCollected++;
                    keyPickup.Play();
                }
                key.Update(gameTime);
            }
        }
        private void InitializeDoorBoxes()
        {
            //Top Door
            DoorBoxes.Add(Direction.Up, new BoundingRectangle(tileset.TileWidth * 11, 0, tileset.TileWidth * 3, 30));
            //Left Door
            DoorBoxes.Add(Direction.Left, new BoundingRectangle(0, tileset.TileHeight * 6, 20, tileset.TileHeight * 3));
            //Bottom Door
            DoorBoxes.Add(Direction.Down, new BoundingRectangle(tileset.TileWidth * 11, tileset.TileHeight * tileset.MapHeight - 30, tileset.TileWidth * 3, 30));
            //Right Door
            DoorBoxes.Add(Direction.Right, new BoundingRectangle(tileset.TileWidth * tileset.MapWidth - 20, tileset.TileHeight * 6, 20, tileset.TileHeight * 3));
        }

        public void GamePaused() { }
        public void GameUnpaused() { }
    }
}
