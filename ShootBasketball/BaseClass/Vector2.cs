using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootBasketball.BaseClass
{
    class Vector2   
    {
        private float x, y;

        public Vector2(float setX, float setY)
        {
            x = setX;
            y = setY;
        }
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y 
        { 
            get { return y; }
            set { y = value; }
        }

        public static Vector2 operator + (Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v1.y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static float operator *(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }

        public static Vector2 operator *(float k, Vector2 v2)
        {
            return new Vector2(k * v2.x, k* v2.y);
        }

    }
}
