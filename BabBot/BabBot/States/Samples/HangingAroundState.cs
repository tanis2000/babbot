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
using System.Text;
using BabBot.Wow;
using BabBot.Manager;

namespace BabBot.States.Samples
{
    public class HangingAroundState : State<WowPlayer>
    {
        protected override void DoEnter(WowPlayer Entity)
        {
            //on enter we will do nothing for now
            return;
        }

        protected override void DoExecute(WowPlayer Entity)
        {
            //get all objects around player
            List<WowObject> lwo = ProcessManager.ObjectManager.GetAllObjectsAroundLocalPlayer();
            //on execute lets find the npc that is farthest away and have the player walk to it
            WowObject far = Entity;


            foreach (WowObject wo in lwo)
            {
                //if a unit, and not me
                if (wo.Type == Descriptor.eObjType.OT_UNIT && wo.Guid != Entity.Guid)
                {
                    

                    //if not me
                    
                    
                }
            }            
        }

        protected override void DoExit(WowPlayer Entity)
        {
            //on exit we will do nothing
            return;
        }

        protected override void DoFinish(WowPlayer Entity)
        {
            return;
        }
    }
}
