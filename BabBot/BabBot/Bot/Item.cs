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
using System.Collections.Generic;
using BabBot.Manager;

namespace BabBot.Bot
{
    public class ItemList : List<Item>
    {
    }

    public class Item : IComparable<Item>, IComparer<Item>
    {
        public string Name;
        public int LevelRequired;
        public int Priority;
        public string Kind;
        public int Price;
        public int Quantity;

        public Item(string iName, int iLevelRequired, int iPriority)
        {
            Name = iName;
            LevelRequired = iLevelRequired;
            Priority = iPriority;
        }

        #region Comparer
        // We sort by level required and priority, highest to lowest 
        public int CompareTo(Item other)
        {
            int l = -LevelRequired.CompareTo(other.LevelRequired);
            if (l == 0)
            {
                int p = -Priority.CompareTo(other.Priority);
                return p;
            }
            return l;
        }

        public int Compare(Item x, Item y)
        {
            int l = -x.LevelRequired.CompareTo(y.LevelRequired);
            if (l == 0)
            {
                int p = -x.Priority.CompareTo(y.Priority);
                return p;
            }
            return l;
        }
        #endregion
    }
}