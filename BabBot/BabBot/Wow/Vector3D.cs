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
    /// <summary>
    /// 3D position type
    /// ABSOLUTE is position on the continent map
    /// RELATIVE is position on zone map inside continent
    /// </summary>
    public enum VectorTypes : int
    {
        ABSOLUTE = 0,
        RELATIVE = 1
    }

    public class Vector3D : ICloneable
    {
        [XmlAttribute("x")]
        public float X;

        [XmlAttribute("y")]
        public float Y;

        [XmlAttribute("z")]
        public float Z;

        [XmlAttribute("type")]
        public int Type
        {
            set { VType = (VectorTypes)value; }
            get { return (int) VType; }
        }

        /// <summary>
        /// Vector length
        /// i.e distance from coordinate start point
        /// </summary>
        /// <returns></returns>
        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        /// <summary>
        /// Vector type (ABSOLUTE or RELATIVE)
        /// For now by default is ABSOLUTE until coordinates converter will be completed
        /// </summary>
        internal VectorTypes VType = VectorTypes.ABSOLUTE;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Vector3D() : this(0, 0, 0) { }

        /// <summary>
        /// Create 3D vector with given coordinates
        /// </summary>
        /// <param name="x">X axiz coordinate</param>
        /// <param name="y">Y axiz coordinate</param>
        /// <param name="z">Z axiz coordinate</param>
        public Vector3D(double x, double y, double z)
            : this((float)x, (float)y, (float)z) { }

        /// <summary>
        /// Create 3D vector with given coordinates
        /// </summary>
        /// <param name="x">X axiz coordinate</param>
        /// <param name="y">Y axiz coordinate</param>
        /// <param name="z">Z axiz coordinate</param>
        public Vector3D(float x, float y, float z)
            : this (x, y, z, VectorTypes.ABSOLUTE) { }

        /// <summary>
        /// Create 3D vector with given coordinates
        /// </summary>
        /// <param name="x">X axiz coordinate</param>
        /// <param name="y">Y axiz coordinate</param>
        /// <param name="z">Z axiz coordinate</param>
        /// <param name="type">Vector type (absolute or relative)</param>
        public Vector3D(float x, float y, float z, VectorTypes type)
        {
            // Keep 2 float digits
            X = (float)Math.Round(x, 2);
            Y = (float)Math.Round(y, 2);
            Z = (float)Math.Round(z, 2);
            VType = type;
        }

        /// <summary>
        /// Is vector not zero length
        /// However zero length might be valid if object located exactly in the center point
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return ((X == 0) && (Y == 0) && (Z == 0)) ? false : true;
        }

        

        /// <summary>
        /// Normalize vector i.e divide each coordinate on vector length (from center)
        /// so lenth of new vector is always 1
        /// </summary>
        /// <returns>Normalized vector</returns>
        public Vector3D Normalize()
        {
            double length = Length;
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
                return true;

            // If one is null, but not both, return false.
            if (((object)v1 == null) || ((object)v2 == null))
                return false;

            // Return true if the all fields match:
            return v1.IsEqualTo(v2);
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
                return false;

            return IsEqualTo(obj);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) || 
                (obj.GetType() != typeof (Vector3D)))
                    return false;

            return IsEqualTo((Vector3D)obj);
        }

        /// <summary>
        /// Compare itself with destination vector
        /// Used stricly internally assumed all verification done and
        /// destination vector not null and the same object type
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private bool IsEqualTo(Vector3D v)
        {
            return (ReferenceEquals(this, v) ||
               v.X == X && v.Y == Y && v.Z == Z && v.VType == VType);
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

        /// <summary>
        /// Get distance to destination vector
        /// </summary>
        /// <param name="l">Destination vector</param>
        /// <returns>Distance to destination</returns>
        public float GetDistanceTo(Vector3D l)
        {
            float dx = X - l.X;
            float dy = Y - l.Y;
            float dz = Z - l.Z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Check if 2 vectors located in 5 yards distance from each other
        /// </summary>
        /// <param name="v">Destination vector</param>
        /// <returns>TRUE if 2 vectors located in 5 yards range between each other
        /// and FALSE if not</returns>
        public bool IsClose(Vector3D v)
        {
            return IsClose(v, 5F);
        }

        /// <summary>
        /// Check if 2 vectors located close to each other
        /// </summary>
        /// <param name="v">Destination vector</param>
        /// <param name="distance">Distance between vectors 
        /// that considered to be "close distance"</param>
        /// <returns>TRUE if 2 vectors located in given distance between each other
        /// and FALSE if not</returns>
        public bool IsClose(Vector3D v, float distance)
        {
            return (GetDistanceTo(v) <= distance);
        }

        /// <summary>
        /// Clone itself
        /// </summary>
        /// <returns>Cloned vector</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Clone itself
        /// </summary>
        /// <returns>Cloned vector as Vector3D class </returns>
        public Vector3D CloneVector()
        {
            return (Vector3D)this.Clone();
        }
    }
}