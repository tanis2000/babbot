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
    public class SpellList : List<Spell>
    {
    }

    public class Spell
    {
        public int Weight;
        public string Name;

        public Spell(string iName, int iWeight)
        {
            Name = iName;
            Weight = iWeight;
        }

        public int Cost
        {
            get
            {
                ProcessManager.Injector.Lua_DoString(string.Format("name, rank, icon, cost, isFunnel, powerType, castTime, minRange, maxRange = GetSpellInfo(\"{0}\");", Name));
                string cost = ProcessManager.Injector.Lua_GetLocalizedText(3);

                return Convert.ToInt32(cost);
            }
        }

        public int CastTime
        {
            get
            {
                ProcessManager.Injector.Lua_DoString(string.Format("name, rank, icon, cost, isFunnel, powerType, castTime, minRange, maxRange = GetSpellInfo(\"{0}\");", Name));
                string castTime = ProcessManager.Injector.Lua_GetLocalizedText(6);

                return Convert.ToInt32(castTime); // milliseconds
            }
        }

        public bool IsOnCooldown()
        {
            ProcessManager.Injector.Lua_DoString(string.Format("start, duration, enabled = GetSpellCooldown(\"{0}\");", Name));
            string duration = ProcessManager.Injector.Lua_GetLocalizedText(1);

            return duration == "0" ? false : true;
        }
    }
}