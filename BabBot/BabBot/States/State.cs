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

namespace BabBot.States
{
    /// <summary>
    /// Represents a generic state in the fininte state machine
    /// </summary>
    /// <typeparam name="T">T is the type of object that this state will interact with</typeparam>
    public abstract class State<T>
    {
        public DateTime EnterTime { get; protected set; }
        public DateTime LastExecuteTime { get; protected set; }
        public DateTime ExitTime { get; protected set; }
        public DateTime FinishTime { get; protected set; }

        /// <summary>The state the entity was in previous to this state being started;</summary>
        public State<T> PreviousState { get; set; }

        public bool HasChangeStateEventHookup
        {
            get { return (ChangeStateRequest == null) ? false : true; }
        }

        /// <summary>Event fires before Enter code runs</summary>
        public event EventHandler<StateEventArgs<T>> Entering;

        /// <summary>Event fires after Enter code runs</summary>
        public event EventHandler<StateEventArgs<T>> Entered;

        /// <summary>Event fires before Execute code runs</summary>
        public event EventHandler<StateEventArgs<T>> Executing;

        /// <summary>Event fires after Execute code runs</summary>
        public event EventHandler<StateEventArgs<T>> Executed;

        /// <summary>Event fires before Exit code runs</summary>
        public event EventHandler<StateEventArgs<T>> Exiting;

        /// <summary>Event fires after Exit code runs</summary>
        public event EventHandler<StateEventArgs<T>> Exited;

        /// <summary>Event fires before the State finishes</summary>
        public event EventHandler<StateEventArgs<T>> Finishing;

        /// <summary>Event fires after the State finishes</summary>
        public event EventHandler<StateEventArgs<T>> Finished;

        /// <summary>Event fires when this state whants to kick-off another state as part of it's process</summary>
        public event EventHandler<ChangeStateEventArgs<T>> ChangeStateRequest;

        /// <summary>Enter this State</summary>
        public void Enter(T Entity)
        {
            //on enter, clear out last execute time, exit time, finish time
            LastExecuteTime = DateTime.MinValue;
            ExitTime = DateTime.MinValue;
            FinishTime = DateTime.MinValue;

            //Raise Entering event
            if (Entering != null)
                Entering(this, StateEventArgs<T>.GetArgs(Entity));

            //Update Enter Date/time
            EnterTime = DateTime.Now;

            //call DoEnter
            DoEnter(Entity);

            //Raise Entered
            if (Entered != null)
                Entered(this, StateEventArgs<T>.GetArgs(Entity));
        }

        protected abstract void DoEnter(T Entity);

        /// <summary>Execute State</summary>
        public void Execute(T Entity)
        {
            //Raise Executing event
            if (Executing != null)
                Executing(this, StateEventArgs<T>.GetArgs(Entity));

            //Update last executed Date/time
            LastExecuteTime = DateTime.Now;

            //call DoExecute
            DoExecute(Entity);

            //Raise Executed
            if (Executed != null)
                Executed(this, StateEventArgs<T>.GetArgs(Entity));
        }

        protected abstract void DoExecute(T Entity);

        /// <summary>Exit State</summary>
        public void Exit(T Entity)
        {
            //Raise Exiting event
            if (Exiting != null)
                Exiting(this, StateEventArgs<T>.GetArgs(Entity));

            //Update Exit date/time
            ExitTime = DateTime.Now;

            //call DoExecute
            DoExit(Entity);

            //Raise Exited
            if (Exited != null)
                Exited(this, StateEventArgs<T>.GetArgs(Entity));
        }

        protected virtual void DoExit(T Entity) { }

        /// <summary>Finish State</summary>
        public void Finish(T Entity)
        {
            //Raise Finishing event
            if (Finishing != null)
                Finishing(this, StateEventArgs<T>.GetArgs(Entity));

            //call DoFinish
            DoFinish(Entity);

            //Raise Finished
            if (Finished != null)
                Finished(this, StateEventArgs<T>.GetArgs(Entity));

            //Update Finish date/time. Always last
            FinishTime = DateTime.Now;
        }


        protected virtual void DoFinish(T Entity) { }

        protected bool CallChangeStateEvent(T Entity, State<T> NewState, bool TrackPrevious, bool ExitPrevious)
        {
            if (HasChangeStateEventHookup)
            {
                ChangeStateRequest(this, ChangeStateEventArgs<T>.GetArgs(Entity, NewState, TrackPrevious, ExitPrevious));
                return true;
            }

            return false;
        }

        protected void Log(string lfs, string msg)
        {
            Output.Instance.Log(lfs, msg);
        }

        protected void Debug(string lfs, string msg)
        {
            Output.Instance.Debug(lfs, msg, this);
        }
    }
}