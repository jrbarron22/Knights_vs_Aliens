﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;


namespace Tilemap
{
    [ContentSerializerRuntimeType("Knights_vs_Aliens.Tileset, Knights_vs_Aliens")]
    public class TilemapContent
    {
        /// <summary>Map dimensions</summary>
        public int MapWidth, MapHeight;

        /// <summary>Tile dimensions</summary>
        public int TileWidth, TileHeight;

        /// <summary>The tileset texture</summary>
        public Texture2DContent TilesetTexture;

        /// <summary>The tileset data</summary>
        public Rectangle[] Tiles;

        /// <summary>The map data</summary>
        public int[] TileIndices;

        /// <summary>The map filename</summary>
        [ContentSerializerIgnore]
        public string mapFilename;

        /// <summary> The tileset image filename </summary>
        [ContentSerializerIgnore]
        public String TilesetImageFilename;
    }
}
