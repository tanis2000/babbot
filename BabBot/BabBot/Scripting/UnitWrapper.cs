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
using BabBot.Bot;
using BabBot.Manager;
using BabBot.Wow;

namespace BabBot.Scripting
{
    public interface IUnitWrapper
    {
        string Test();
    }

    public class UnitWrapper : IUnitWrapper
    {
        // Reference to the Player object
        private readonly WowUnit unit;

        public UnitWrapper(WowUnit iUnit)
        {
            unit = iUnit;
        }

        #region IUnitWrapper Members

        public string Test()
        {
            return "unit.Test()";
        }

        #endregion
    }
}