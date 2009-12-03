/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/
using System;
using System.Xml.Serialization;

namespace BabBot.Wow
{
    public class Vector3D : ICloneable
    {
        [XmlAttribute("x")]
        public float X;

        [XmlAttribute("y")]
        public float Y;

        [XmlAttribute("z")]
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

        public bool IsValid()
        {
            return ((X == 0) && (Y == 0) && (Z == 0)) ? false : true;
        }

        public Vector3D Normalize()
        {
            double length = Math.Sqrt(X*X + Y*Y + Z*Z);
            var v = new Vector3D();
            v.X = (float) (X/length);
            v.Y = (float) (Y/length);
            v.Z = (float) (Z/length);
            return v;
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}", X, Y, Z);
        }

        public static bool operator ==(Vector3D v1, Vector3D v2)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(v1, v2))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)v1 == null) || ((object)v2 == null))
            {
                return false;
            }

            // Return true if the fields match:
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }

        public static bool operator !=(Vector3D v1, Vector3D v2)
        {
            return !(v1 == v2);
        }

        public static Vector3D operator *(Vector3D v, float n)
        {
            return new Vector3D(v.X*n, v.Y*n, v.Z*n);
        }

        public static float operator *(Vector3D v1, Vector3D v2)
        {
            float f;
            f = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
            return f;
        }

        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            var v3 = new Vector3D((v1.X - v2.X), (v1.Y - v2.Y), (v1.Z - v2.Z));
            return v3;
        }

        public bool Equals(Vector3D obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.X == X && obj.Y == Y && obj.Z == Z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (Vector3D))
            {
                return false;
            }
            return Equals((Vector3D) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result*397) ^ Y.GetHashCode();
                result = (result*397) ^ Z.GetHashCode();
                return result;
            }
        }

        public float GetDistanceTo(Vector3D l)
        {
            float dx = X - l.X;
            float dy = Y - l.Y;
            float dz = Z - l.Z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}