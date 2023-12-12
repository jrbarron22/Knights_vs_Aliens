using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knights_vs_Aliens;
using Knights_vs_Aliens.Collisions;

namespace Knights_vs_Aliens.Rooms
{
    public interface IGameplayRoom
    {
        Dictionary<Direction, int[]> Doors { get; set; }
        Dictionary<Direction, BoundingRectangle> DoorBoxes { get; set; }
        bool AreDoorsOpen { get; set; }
        void LoadContent(GraphicsDevice graphics, ContentManager content);
        void Update(GameTime gameTime, List<KeyValuePair<Direction, BoundingRectangle>> walls);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics);

        void GamePaused();
        void GameUnpaused();
    }
}
