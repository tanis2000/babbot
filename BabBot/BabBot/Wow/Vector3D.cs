using System;
using System.Collections.Generic;
using System.Text;

namespace BabBot.Wow
{
    public class Vector3D
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
    
    }
}
