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

namespace BabBot.Bot
{
    [Serializable]
    public class BindingList : Dictionary<string, Binding>
    {
    }

    [Serializable]
    public class Binding
    {
        public int Bar;
        public string Key;
        public string Name;

        public Binding(string iName, int iBar, string iKey)
        {
            Name = iName;
            Bar = iBar;
            Key = iKey;
        }
    }
}