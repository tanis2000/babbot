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

namespace BabBot.States
{
    [Serializable]
    public class ChangeStateEventArgs<T> : StateEventArgs<T>
    {
        public ChangeStateEventArgs(T Entity, State<T> NewState, bool TrackPrevious, bool ExitPrevious) : base(Entity)
        {
            this.NewState = NewState;
            this.TrackPrevious = TrackPrevious;
            this.ExitPrevious = ExitPrevious;
        }

        public State<T> NewState { get; private set; }
        public bool TrackPrevious { get; private set; }
        public bool ExitPrevious { get; private set; }

        public static ChangeStateEventArgs<T> GetArgs(T Entity, State<T> NewState, bool TrackPrevious, bool ExitPrevious)
        {
            var args = new ChangeStateEventArgs<T>(Entity, NewState, TrackPrevious, ExitPrevious);

            return args;
        }
    }
}