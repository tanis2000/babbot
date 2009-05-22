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
using BabBot.Scripting;
using BabBotScript;

namespace BabBotScript
{
    public class Paladin : ClassScript
    {
         
        public Paladin(Script iScript) : base(iScript)
        {
            // TODO: get that stuff out of the way and load bindings from the appropriate XML file
            Binding b = new Binding("melee", 1, "1");
            Bindings.Add(b.Name, b);
            b = new Binding("test", 1, "2");
            Bindings.Add(b.Name, b);

            PlayerAction a = new PlayerAction("attack", Bindings["melee"], 0.0f, 0.0f, false, true);
            Actions.Add(a.Name, a);
        }

        // TODO: add an interface for all common functions like this
        public new void Fight()
        {
            // TODO: Implement actual fight logic (only by PlayAction)
            script.player.PlayAction(Actions["attack"], true);
        }
    }
}
