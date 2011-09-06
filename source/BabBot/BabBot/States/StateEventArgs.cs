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
    public class StateEventArgs<T> : EventArgs
    {
        public StateEventArgs(T Entity)
        {
            this.Entity = Entity;
        }

        public T Entity { get; private set; }

        public static StateEventArgs<T> GetArgs(T Entity)
        {
            var args = new StateEventArgs<T>(Entity);

            return args;
        }
    }
}