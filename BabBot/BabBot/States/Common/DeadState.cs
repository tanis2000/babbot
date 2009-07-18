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
using System.Linq;
using System.Text;
using Pather.Graph;
using BabBot.Wow;
using BabBot.Manager;

namespace BabBot.States.Common
{
    public class DeadState : State<Wow.WowPlayer>
    {
        protected Vector3D _CorpseLocation;

        protected override void DoEnter(BabBot.Wow.WowPlayer Entity)
        {
            //on enter, get location of corpose
            _CorpseLocation = Entity.CorpseLocation;
        }

        protected override void DoExecute(BabBot.Wow.WowPlayer Entity)
        {
            //on execute, if the distance to our corpose is more then 5 yards, we need to get there
            if (Entity.DistanceFromCorpse() > 5f)
            {
                // so we make a new move to state that will take us to our corpose
                MoveToState mtsCorpse = new MoveToState(_CorpseLocation);

                //request that we move to this location
                CallChangeStateEvent(Entity, mtsCorpse, true, false);

                return;
            }

            //we should now web close to our corpse so rez!
            // but since we can't yet... just finish and exit...
            Finish(Entity);
            Exit(Entity);
        }

        protected override void DoExit(BabBot.Wow.WowPlayer Entity)
        {
            //on exit, if there is a previous state, go back to it
            if (PreviousState != null)
            {
                CallChangeStateEvent(Entity, PreviousState, false, false);
            }
        }

        protected override void DoFinish(BabBot.Wow.WowPlayer Entity)
        {
            //finish
        }
    }
}
