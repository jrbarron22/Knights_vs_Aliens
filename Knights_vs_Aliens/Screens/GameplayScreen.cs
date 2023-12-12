using CppNet;
using Knights_vs_Aliens.Collisions;
using Knights_vs_Aliens.Rooms;
using Knights_vs_Aliens.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Knights_vs_Aliens.Screens
{
    public class GameplayScreen : IScreen
    {
        Random rand = new Random();
        private IGameplayRoom[,] roomGrid = new IGameplayRoom[5,5];
        private List<int[]> pickedRooms;
        private List<int[]> availableRooms;
        private int[] openingRoom;
        private int numRooms;
        private IGameplayRoom curRoom;

        private Tileset tileset;
        private Texture2D doorTexture;

        private List<KeyValuePair<Direction, BoundingRectangle>> walls = new List<KeyValuePair<Direction, BoundingRectangle>>();

        private Game game;
        private GraphicsDevice graphics;
        private KnightSprite knight;
        private ContentManager content;

        private switchScreen pause;
        private switchScreen victory;
        private switchScreen defeat;

        public GameplayScreen(Game game, GraphicsDevice graphics, KnightSprite knight, ContentManager content, switchScreen changeScreen)
        {
            this.game = game;
            this.graphics = graphics;
            this.knight = knight;
            this.content = content;

            tileset = new Tileset();

            pause = changeScreen;
            victory = changeScreen;
            defeat = changeScreen;

            //Has to have more than 5 rooms
            numRooms = rand.Next(2) + 11;
            pickedRooms = new List<int[]> { new int[] { 2, 2 } };
            availableRooms = new List<int[]> { new int[] { 2, 2 } };
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            tileset = content.Load<Tileset>("tilemap");
            doorTexture = content.Load<Texture2D>("Dungeon_Tileset_Rescaled");
            openingRoom = pickedRooms[0];
            roomGrid[openingRoom[0], openingRoom[1]] = new OpeningRoom(knight, tileset);
            curRoom = roomGrid[openingRoom[0], openingRoom[1]];
            CreateFloorLayout(numRooms - 1, 2);
            CreateDoors();

            foreach (var room in pickedRooms)
            {
                roomGrid[room[0], room[1]].LoadContent(graphics, content);
            }
            
            InitializeWalls();
        }

        public void Update(GameTime gameTime)
        {
            knight.Update(gameTime);
            curRoom.Update(gameTime);

            //Wall-Knight collisions
            foreach (var wall in walls)
            {
                if (wall.Value.CollidesWith(knight.Bounds))
                {
                    HandleBoundaryCollisions(wall.Key);
                }
            }

            foreach(var door in curRoom.DoorBoxes)
            {
                //There is not a door in this direction or if the door is closed
                if ((!curRoom.Doors.ContainsKey(door.Key) || !curRoom.AreDoorsOpen) && door.Value.CollidesWith(knight.Bounds))
                {
                    HandleBoundaryCollisions(door.Key);
                }
                //If knight collides with an open door
                else if(door.Value.CollidesWith(knight.Bounds))
                {  
                    switch (door.Key)
                    {
                        case Direction.Up:
                            knight.Position = new Vector2(400, (tileset.MapHeight * tileset.TileHeight) - tileset.TileHeight - (knight.Bounds.Height + 20));
                            break;
                        case Direction.Down:
                            knight.Position = new Vector2(400, tileset.TileHeight + knight.Bounds.Height + 20);
                            break;
                        case Direction.Left:
                            knight.Position = new Vector2((tileset.MapWidth * tileset.TileWidth) - tileset.TileWidth - (knight.Bounds.Width + 20), 240);
                            break;
                        case Direction.Right:
                            knight.Position = new Vector2(tileset.TileWidth + knight.Bounds.Width + 20, 240);
                            break;
                    }
                    curRoom = roomGrid[curRoom.Doors[door.Key][0], curRoom.Doors[door.Key][1]];
                    break;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            graphics.Clear(Color.Black);

            spriteBatch.Begin();
            tileset.Draw(gameTime, spriteBatch);
            curRoom.Draw(gameTime, spriteBatch, graphics);

            foreach (var door in curRoom.DoorBoxes)
            {
                Rectangle source = new Rectangle();
                if (curRoom.AreDoorsOpen && curRoom.Doors.ContainsKey(door.Key))
                {
                    source = new Rectangle(456, 60, 50, 50);
                }
                else
                {
                    if (door.Key == Direction.Up && curRoom.Doors.ContainsKey(Direction.Up))
                    {
                        source = new Rectangle(386, 194, 122, 58);
                    }
                    else if (door.Key == Direction.Down && curRoom.Doors.ContainsKey(Direction.Down))
                    {
                        source = new Rectangle(386, 194, 122, 58);
                    }
                    else if (door.Key == Direction.Left && curRoom.Doors.ContainsKey(Direction.Left))
                    {
                        source = new Rectangle(404, 260, 24, 120);
                    }
                    else if (door.Key == Direction.Right && curRoom.Doors.ContainsKey(Direction.Right))
                    {
                        source = new Rectangle(404, 260, 24, 120);
                    }
                }
                Vector2 scale = new Vector2(door.Value.Width / source.Width, door.Value.Height / source.Height);
                if (door.Key == Direction.Up || door.Key == Direction.Down)
                {
                    scale.Y *= 2;
                    spriteBatch.Draw(doorTexture, new Vector2(door.Value.X, door.Value.Y), source, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
                }
                else if (door.Key == Direction.Right)
                {
                    scale.X *= 1.2f;
                    source.Width += 15;
                    spriteBatch.Draw(doorTexture, new Vector2(door.Value.X - 10, door.Value.Y), source, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
                }
                else if (door.Key == Direction.Left)
                {
                    scale.X *= 1.2f;
                    source.Width += 15;
                    spriteBatch.Draw(doorTexture, new Vector2(door.Value.X, door.Value.Y), source, Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
                }

            }
            
            knight.Draw(gameTime, spriteBatch, graphics);

            //For debugging
            /*
            Texture2D blankTexture = new Texture2D(graphics, 1, 1);
            blankTexture.SetData(new Color[] { Color.SlateBlue });
            foreach (var wall in walls) spriteBatch.Draw(blankTexture, new Vector2(wall.Value.X, wall.Value.Y), wall.Value.Bounds(), Color.White);
            curRoom.Draw(gameTime, spriteBatch, graphics);
            */
            spriteBatch.End();
        }

        public void GameUnpaused()
        {
            throw new NotImplementedException();
        }

        public void LevelReset()
        {
        }

        private void HandleBoundaryCollisions(Direction curDirection)
        {
            switch (curDirection)
            {
                case Direction.Up:
                    knight.Position.Y = 30 + (knight.Bounds.Height / 2) + 1;
                    break;
                case Direction.Down:
                    knight.Position.Y = (tileset.MapHeight * tileset.TileHeight) - 30 - (knight.Bounds.Height / 2) - 1;
                    break;
                case Direction.Left:
                    knight.Position.X = 20 + (knight.Bounds.Width / 2) + 1;
                    break;
                case Direction.Right:
                    knight.Position.X = (tileset.MapWidth * tileset.TileWidth) - 20 - (knight.Bounds.Width / 2) - 1;
                    break;
            }
        }

        private void CreateDoors()
        {
            foreach (var room in pickedRooms)
            {
                try
                {
                    //Door to the right
                    if (roomGrid[room[0] + 1, room[1]] != null)
                    {
                        roomGrid[room[0], room[1]].Doors.Add(Direction.Right, new int[] { room[0] + 1, room[1] });
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    //Door to the left
                    if (roomGrid[room[0] - 1, room[1]] != null)
                    {
                        roomGrid[room[0], room[1]].Doors.Add(Direction.Left, new int[] { room[0] - 1, room[1] });
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    //Door down
                    if (roomGrid[room[0], room[1] + 1] != null)
                    {
                        roomGrid[room[0], room[1]].Doors.Add(Direction.Down, new int[] { room[0], room[1] + 1 });
                    }
                }
                catch (IndexOutOfRangeException) { }
                try
                {
                    //Door Up
                    if (roomGrid[room[0], room[1] - 1] != null)
                    {
                        roomGrid[room[0], room[1]].Doors.Add(Direction.Up, new int[] { room[0], room[1] - 1 });
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
        }

        private void CreateFloorLayout(int numRoomsRemaining, int keysToPlace)
        {
            if (numRoomsRemaining == 0) return;

            int[] nextRoom = FindAvailableRoom();
            pickedRooms.Add(nextRoom);
            availableRooms.Add(nextRoom);
            int difficulty = 1;
            int numRoomKeys = 0;
            if (nextRoom[0] == 0 || nextRoom[0] == 4 || nextRoom[1] == 0 || nextRoom[1] == 4) difficulty = 2;
            if (keysToPlace > 0 && difficulty == 2) numRoomKeys = 1;
            roomGrid[nextRoom[0], nextRoom[1]] = new BasicRoom(difficulty, numRoomKeys, tileset, knight, game, content);
            CreateFloorLayout(numRoomsRemaining - 1, keysToPlace - numRoomKeys);
        }

        /// <summary>
        /// Ensures the opening room has at least 2 adjacent rooms, then selects a random room
        /// </summary>
        /// <param name="numRoomsPlaced"></param>
        /// <returns>The new room's coordinates to be added to the room grid</returns>
        private int[] FindAvailableRoom()
        {
            int[] room;
            int index = 0;
            List<int[]> availableAdjancency;
            do {
                index = rand.Next(availableRooms.Count);
                //Check if opening room has atleast two adjacent rooms
                if (pickedRooms.Count < 3)
                {
                    room = openingRoom;
                }
                //Pick a random available room
                else
                {
                    room = availableRooms[index];
                }
                availableAdjancency = CheckRoomAvailability(room);
            }
            while (availableAdjancency.Count == 0);

            index = rand.Next(availableAdjancency.Count);
            return availableAdjancency[index];
        }

        /// <summary>
        /// Makes sure there is an adjacent room that is empty
        /// </summary>
        /// <param name="room">The room we are checking</param>
        /// <returns>The list of available rooms adjacent to the selected room</returns>
        private List<int[]> CheckRoomAvailability(int[] room)
        {
            List<int[]> availableAdjacency = new List<int[]>();
            try
            {
                if (roomGrid[room[0] + 1, room[1]] == null) availableAdjacency.Add(new int[2] { room[0] + 1, room[1] });
            }
            catch (IndexOutOfRangeException) { }
            try
            {
                if (roomGrid[room[0] - 1, room[1]] == null) availableAdjacency.Add(new int[2] { room[0] - 1, room[1] });
            }
            catch (IndexOutOfRangeException) { }
            try
            {
                if (roomGrid[room[0], room[1] + 1] == null) availableAdjacency.Add(new int[2] { room[0], room[1] + 1 });
            }
            catch (IndexOutOfRangeException) { }
            try
            {
                if (roomGrid[room[0], room[1] - 1] == null) availableAdjacency.Add(new int[2] { room[0], room[1] - 1 });
            }
            catch (IndexOutOfRangeException) { }

            if (availableAdjacency.Count == 0) availableRooms.Remove(room);
            return availableAdjacency;
        }

        private bool IsRoomInList(List<int[]> list, int[] room)
        {
            foreach (var item in list)
            {
                if (item[0] == room[0] && item[1] == room[1]) return true;
            }
            return false;
        }

        private void InitializeWalls()
        {
            //Top Wall 1
            walls.Add(new KeyValuePair<Direction, BoundingRectangle>(Direction.Up, new BoundingRectangle(0, 0, tileset.TileWidth * 11, 30)));
            //Top Wall 2
            walls.Add(new KeyValuePair<Direction, BoundingRectangle>(Direction.Up, new BoundingRectangle(tileset.TileWidth * 14, 0, tileset.TileWidth * 11, 30)));
            //Left Wall 1
            walls.Add(new KeyValuePair<Direction, BoundingRectangle>(Direction.Left, new BoundingRectangle(0, 0, 20, tileset.TileHeight * 6)));
            //Left Wall 2
            walls.Add(new KeyValuePair<Direction, BoundingRectangle>(Direction.Left, new BoundingRectangle(0, tileset.TileHeight * 9, 20, tileset.TileHeight * 6)));
            //Bottom Wall 1
            walls.Add(new KeyValuePair<Direction, BoundingRectangle>(Direction.Down, new BoundingRectangle(0, tileset.TileHeight * tileset.MapHeight - 30, tileset.TileWidth * 11, 20)));
            //Bottom Wall 2
            walls.Add(new KeyValuePair<Direction, BoundingRectangle>(Direction.Down, new BoundingRectangle(tileset.TileWidth * 14, tileset.TileHeight * tileset.MapHeight - 30, tileset.TileWidth * 11, 20)));
            //Right Wall 1
            walls.Add(new KeyValuePair<Direction, BoundingRectangle>(Direction.Right, new BoundingRectangle(tileset.TileWidth * tileset.MapWidth - 20, 0, 20, tileset.TileHeight * 6)));
            //Right Wall 2
            walls.Add(new KeyValuePair<Direction, BoundingRectangle>(Direction.Right, new BoundingRectangle(tileset.TileWidth * tileset.MapWidth - 20, tileset.TileHeight * 9, 20, tileset.TileHeight * 6)));
        }
    }
}
