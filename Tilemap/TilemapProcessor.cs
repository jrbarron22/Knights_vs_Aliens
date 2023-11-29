﻿using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tilemap
{
    /// <summary>
    /// Processes a BasicTilemapContent object, building and linking the associated texture 
    /// and setting up the tile information.
    /// </summary>
    [ContentProcessor(DisplayName = "TilemapProcessor")]
    public class TilemapProcessor : ContentProcessor<TilemapContent, TilemapContent>
    {
        public override TilemapContent Process(TilemapContent map, ContentProcessorContext context)
        {
            // We need to build the tileset texture associated with this tilemap
            // This will create the binary texture file and link it to this tilemap so 
            // they get loaded together by the ContentProcessor.  
            //map.TilesetTexture = context.BuildAsset<Texture2DContent, Texture2DContent>(map.TilesetTexture, "Texture2DProcessor");
            map.TilesetTexture = context.BuildAndLoadAsset<TextureContent, Texture2DContent>(new ExternalReference<TextureContent>(map.TilesetImageFilename), "TextureProcessor");

            // Determine the number of rows and columns of tiles in the tileset texture            
            int tilesetColumns = map.TilesetTexture.Mipmaps[0].Width / map.TileWidth;
            int tilesetRows = map.TilesetTexture.Mipmaps[0].Height / map.TileWidth;

            // We need to create the bounds for each tile in the tileset image
            // These will be stored in the tiles array
            map.Tiles = new Rectangle[tilesetColumns * tilesetRows];
            context.Logger.LogMessage($"{map.Tiles.Length} Total tiles");
            for (int y = 0; y < tilesetRows; y++)
            {
                for (int x = 0; x < tilesetColumns; x++)
                {
                    map.Tiles[y * tilesetColumns + x] = new Rectangle(
                        x * map.TileWidth,
                        y * map.TileHeight,
                        map.TileWidth,
                        map.TileHeight
                        );
                }
            }

            //context.Logger.LogMessage($"{ map.Tiles[1].X} Test");
            // Return the fully processed tilemap
            return map;
        }
    }
}
