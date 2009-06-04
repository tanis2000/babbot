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
namespace BabBot.Common
{
    public class Vector
    {
        private float _x, _y, _z;

        public Vector()
        {
            _x = 0;
            _y = 0;
            _z = 0;
        }

        public Vector(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Vector(Vector v)
        {
            _x = v._x;
            _y = v._y;
            _z = v._z;
        }

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public void SetVec(Vector v)
        {
            _x = v._x;
            _y = v._y;
            _z = v._z;
        }

        public void SetVec(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            var v3 = new Vector((v1._x + v2._x), (v1._y + v2._y), (v1._z + v2._z));
            return v3;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            var v3 = new Vector((v1._x - v2._x), (v1._y - v2._y), (v1._z - v2._z));
            return v3;
        }

        public static float operator *(Vector v1, Vector v2)
        {
            float f;
            f = v1._x*v2._x + v1._y*v2._y + v1._z*v2._z;
            return f;
        }
    }
}