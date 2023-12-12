using Knights_vs_Aliens.Collisions;
using Knights_vs_Aliens.Sprites;
using Knights_vs_Aliens.Sprites.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Knights_vs_Aliens.Rooms
{
    public class BasicRoom : IGameplayRoom
    {
        public Dictionary<Direction, int[]> Doors { get; set ; } = new Dictionary<Direction, int[]>();
        public Dictionary<Direction, BoundingRectangle> DoorBoxes { get; set; } = new Dictionary<Direction, BoundingRectangle>();

        private WallLaser[] wallLasers;
        private AlienTurret[] alienTurrets;
        private KeySprite key;

        private SoundEffect keyPickup;
        private SoundEffect knightHit;

        private bool[,] tileArray = new bool[25, 15];

        public bool AreDoorsOpen { get; set; } = false;

        Random rand = new Random();

        private Tileset tileset;
        private KnightSprite knight;
        private Game game;
        private ContentManager content;

        public BasicRoom (int difficulty, int numKeys, Tileset tileset, KnightSprite knight, Game game, ContentManager content)
        {
            this.tileset = tileset;
            this.knight = knight;
            this.game = game;
            this.content = content;

            InitializeTileArray();

            switch (difficulty)
            {
                case 1:
                    InitializeWallLasers(rand.Next(1));
                    InitializeTurrets(rand.Next(2) + 1);
                    break;
                case 2:
                    InitializeWallLasers(rand.Next(1) + 1);
                    InitializeTurrets(rand.Next(2) + 2);
                    break;
            }

            if(numKeys > 0)
            {
                key = new KeySprite(new Vector2(400, 240), Color.Gold);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            foreach (var laser in wallLasers) laser.Draw(gameTime, spriteBatch);

            foreach (var turret in alienTurrets)
            {
                if (turret.CurHealth != 0)
                {
                    turret.Draw(gameTime, spriteBatch, graphics);
                }
            }

            if (key != null) key.Draw(gameTime, spriteBatch);
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            keyPickup = content.Load<SoundEffect>("Pickup_Key");
            knightHit = content.Load<SoundEffect>("Knight_Hit");

            InitializeDoorBoxes();

            foreach (var laser in wallLasers) laser.LoadContent();
            foreach (var turret in alienTurrets) turret.LoadContent();
            if (key != null) key.LoadContent(content);
        }

        public void Update(GameTime gameTime, List<KeyValuePair<Direction, BoundingRectangle>> walls)
        {
            //Check Knight Collisions with Laser
            foreach (var laser in wallLasers)
            {
                laser.Update(gameTime);
                if (laser.LaserParticles.IsActive && laser.LaserBounds.CollidesWith(knight.Bounds))
                {
                    if (!knight.Invulnerable)
                    {
                        knight.KnightHit(gameTime);
                        knightHit.Play();
                    }
                }
            }

            int numTurretsDead = 0;
            foreach (var turret in alienTurrets)
            {
                if (turret.CurHealth == 0) numTurretsDead++;
                else
                {
                    turret.Update(gameTime, knight.Position);
                    foreach (var projectile in turret.projectiles)
                    {
                        //Check Knight Collisions with Projectiles
                        if (projectile.bounds.CollidesWith(knight.Bounds) && !projectile.hasCollided)
                        {
                            projectile.hasCollided = true;
                            if (!knight.Invulnerable)
                            {
                                knight.KnightHit(gameTime);
                            }
                        }
                        //Check Bullet collisions with walls
                        foreach (var wall in walls)
                        {
                            if (wall.Value.CollidesWith(projectile.bounds))
                            {
                                projectile.hasCollided = true;
                            }
                        }                        
                    }
                    //Check Weapon collision with turrets
                    if (knight.Weapon.IsAttackActive() && knight.Weapon.GetAttackBounds().CollidesWith(turret.Bounds) && !turret.Invulnerable)
                    {
                        turret.TurretHit();
                    }
                }
            }

            if (numTurretsDead == alienTurrets.Length) AreDoorsOpen = true;

            if (key != null)
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

        private void InitializeTileArray()
        {
            for (int x = 0; x < tileset.MapWidth; x++)
            {
                for (int y = 0; y < tileset.MapHeight; y++)
                {
                    //Boundaries
                    if (x == 0 || x == 24 || y == 14) tileArray[x, y] = true;
                    //Left Door
                    else if (x > 0 && x <= 3 && y >= 6 && y <= 8) tileArray[x, y] = true;
                    //Right Door
                    else if (x >= 22 && x < 24 && y >= 6 && y <= 8) tileArray[x, y] = true;
                    //Top and Bottom Door
                    else if ((x >= 12 && x <= 14) && ((y >= 0 && y <= 5) || (y <= 14 && y >= 12))) tileArray[x, y] = true;
                    else tileArray[x, y] = false;
                }
            }
        }

        private void InitializeTurrets(int num)
        {
            alienTurrets = new AlienTurret[num];
            if (num == 0) return;
            else
            {
                for (int i = 0; i < num; i++)
                {
                    int x;
                    int y;
                    do
                    {
                        x = rand.Next(20) + 2;
                        y = rand.Next(10) + 2;
                    }
                    while (tileArray[x, y] || tileArray[x, y + 1] || tileArray[x, y - 1]);
                    alienTurrets[i] = new AlienTurret(new Vector2(x * tileset.TileWidth, y * tileset.TileHeight), content);
                    tileArray[x, y] = true;
                    tileArray[x, y + 1] = true;
                    tileArray[x, y - 1] = true;
                }
            }
        }

        private void InitializeWallLasers(int num)
        {
            wallLasers = new WallLaser[num];
            if (num == 0) return;
            else
            {
                for (int i = 0; i < num; i++)
                {
                    int y = 50;
                    int x;
                    do
                    {
                        x = rand.Next(20) + 2;
                    }
                    while (tileArray[x, 0] || tileArray[x - 1, 0] || tileArray[x + 1, 0]);
                    wallLasers[i] = new WallLaser(game, new Vector2(x * tileset.TileWidth, y), content);
                    tileArray[x, 0] = true;
                    tileArray[x - 1, 0] = true;
                    tileArray[x + 1, 0] = true;
                }
            }
        }

        public void GamePaused()
        {
            foreach (var laser in wallLasers) laser.GamePaused();
        }

        public void GameUnpaused()
        {
            foreach (var laser in wallLasers) laser.GameUnpaused();
        }
    }
}
