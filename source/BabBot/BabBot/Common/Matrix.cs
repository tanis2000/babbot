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
using BabBot.Wow;
namespace BabBot.Common
{
    internal class Matrix
    {
        private readonly float _x1;
        private readonly float _x2;
        private readonly float _x3;
        private readonly float _y1;
        private readonly float _y2;
        private readonly float _y3;
        private readonly float _z1;
        private readonly float _z2;
        private readonly float _z3;

        public Matrix(float x1, float x2, float x3, float y1, float y2, float y3, float z1, float z2, float z3)
        {
            _x1 = x1;
            _x2 = x2;
            _x3 = x3;
            _y1 = y1;
            _y2 = y2;
            _y3 = y3;
            _z1 = z1;
            _z2 = z2;
            _z3 = z3;
        }

        public Matrix()
        {
        }

        public Vector3D getFirstColumn
        {
            get { return new Vector3D(_x1, _y1, _z1); }
        }

        public Matrix inverse()
        {
            Matrix inv;
            float d = 1/det();
            inv = new Matrix(d*(_y2*_z3 - _y3*_z2), d*(_x3*_z2 - _x2*_z3), d*(_x2*_y3 - _x3*_y2),
                             d*(_y3*_z1 - _y1*_z3), d*(_x1*_z3 - _x3*_z1), d*(_x3*_y1 - _x1*_y3),
                             d*(_y1*_z2 - _y2*_z1), d*(_x2*_z1 - _x1*_z2), d*(_x1*_y2 - _x2*_y1));
            return inv;
        }

        public float det()
        {
            float det = (_x1*_y2*_z3) + (_x2*_y3*_z1) + (_x3*_y1*_z2)
                        - (_x3*_y2*_z1) - (_x2*_y1*_z3) - (_x1*_y3*_z2);
            return det;
        }

        public static Vector3D operator *(Vector3D v, Matrix m)
        {
            var res = new Vector3D(m._x1*v.X + m._y1*v.Y + m._z1*v.Z,
                                 m._x2*v.X + m._y2*v.Y + m._z2*v.Z,
                                 m._x3*v.X + m._y3*v.Y + m._z3*v.Z);
            return res;
        }
    }
}