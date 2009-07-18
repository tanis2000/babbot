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
using BabBot.Wow;

namespace BabBot.Common
{
    public static class MathFuncs
    {
        private static Random _Random = new Random();

        public static float GetDistance(Vector3D dest, Vector3D currentPos, bool UseZ)
        {
            float num = currentPos.X - dest.X;
            float num2 = currentPos.Y - dest.Y;
            float num3 = (dest.Z != 0f) ? (currentPos.Z - dest.Z) : 0f;
            if (UseZ)
            {
                return (float) Math.Sqrt((double) (((num * num) + (num2 * num2)) + (num3 * num3)));
            }
            return (float) Math.Sqrt((double) ((num * num) + (num2 * num2)));
        }

        public static float GetFaceRadian(Vector3D dest, Vector3D currentPos)
        {
            return negativeAngle((float) Math.Atan2((double) (dest.Y - currentPos.Y), (double) (dest.X - currentPos.X)));
        }

        public static float negativeAngle(float angle)
        {
            if (angle < 0f)
            {
                angle += 6.283185f;
            }
            return angle;
        }

        public static float RadianToDegree(float value)
        {
            return (float) Math.Round((double) (value * 57.295779513082323));
        }

        public static int RandomNumber(int min, int max)
        {
            return _Random.Next(min, max + 1);
        }
    }
}

