using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Knights_vs_Aliens
{
    public class Tileset
    {
        /// <summary>
        /// The map width
        /// </summary>
        public int MapWidth { get; init; }

        /// <summary>
        /// The map height
        /// </summary>
        public int MapHeight { get; init; }

        /// <summary>
        /// The width of a tile in the map
        /// </summary>
        public int TileWidth { get; init; }

        /// <summary>
        /// The height of a tile in the map
        /// </summary>
        public int TileHeight { get; init; }

        /// <summary>
        /// The texture containing the tiles
        /// </summary>
        public Texture2D TilesetTexture { get; init; }

        public Rectangle[] Tiles { get; init; }

        public int[] TileIndices { get; init; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int index = TileIndices[y * MapWidth + x];

                    // Draw the current tile
                    Vector2 source = RetrieveTile(index);
                    spriteBatch.Draw(
                        TilesetTexture,
                        new Vector2(x * TileWidth, y * TileHeight),
                        new Rectangle((int)source.X, (int)source.Y, TileWidth, TileHeight),
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        1f,
                        SpriteEffects.None,
                        0
                        );
                }
            }
        }

        public Vector2 RetrieveTile(int index)
        {
            // 0 = Top Left Corner
            // 1 = Top 1/2 Top Wall
            // 2 = Bottom 1/2 Top Wall

            // 3 = Left Wall 1
            // 4 = Left Wall 2

            // 5 = Top Right Corner
            // 6 = Right Side Wall 1
            // 7 Right Side Wall 2

            // 8 = Bottom Wall 1
            // 9 = Bottom Wall 2
            //Default = Floor
            switch (index)
            {
                case 0: return new Vector2(36, 0);
                case 1: return new Vector2(68, 0);
                case 2: return new Vector2(68, 32);
                case 3: return new Vector2(36, 32);
                case 4: return new Vector2(36, 64);
                case 5: return new Vector2(316, 0);
                case 6: return new Vector2(316, 32);
                case 7: return new Vector2(316, 64);
                case 8: return new Vector2(68, 256);
                case 9: return new Vector2(100, 256);
            }
            return new Vector2(128, 128);
        }
    }
}
