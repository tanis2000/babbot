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
using BabBot.Common;
using BabBot.Wow;

namespace BabBot.States.Samples
{
    public class HangingAroundState : State<WowPlayer>
    {
        private DateTime _LastJumpCheck = DateTime.Now;

        protected override void DoEnter(WowPlayer Entity)
        {
            //on enter we will do nothing for now
            return;
        }

        protected override void DoExecute(WowPlayer Entity)
        {
            //if it has been more then/equal to 5 seconds since the last jump attempt, then test
            if (DateTime.Now.Subtract(_LastJumpCheck).TotalSeconds >= 5)
            {
                _LastJumpCheck = DateTime.Now;

                //if two random numbers between 1 and 5 equal each other then jump
                if (MathFuncs.RandomNumber(1, 5) == MathFuncs.RandomNumber(1, 5))
                {
                    Entity.PlayerCM.SendKeys(" ");
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