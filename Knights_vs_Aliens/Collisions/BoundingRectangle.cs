﻿using System;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;

namespace CollisionExample.Collisions
{
    /// <summary>
    /// A bounding rectangle for collision detection
    /// Skeleton Code sourced from the Collision Tutorial made by Nathan Bean
    /// </summary>
    public struct BoundingRectangle
    {
        public float X;

        public float Y; 

        public float Width;

        public float Height;

        public float Left => X;

        public float Right => X + Width;

        public float Top => Y;

        public float Bottom => Y + Height;


        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }

        public Rectangle Bounds()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }
    }
}
