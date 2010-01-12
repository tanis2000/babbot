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
    public enum StateConditions : byte
    {
        NEW,
        STARTED,
        FINISHED,
        TERMINATED,
    }

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

        /// <summary>
        /// State condition
        /// </summary>
        public StateConditions Status = StateConditions.NEW;

        /// <summary>The state the entity was in previous to this state being started;</summary>
        public State<T> PreviousState { get; set; }

        public bool HasChangeStateEventHookup
        {
            get { return (ChangeStateRequest == null) ? false : true; }
        }

        public bool Started
        {
            get { return Status == StateConditions.STARTED; }
        }

        public bool Completed
        {
            get
            {
                return (Status == StateConditions.FINISHED) ||
                  (Status == StateConditions.TERMINATED);
            }
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

            // Last call. Change status
            Status = StateConditions.STARTED;
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

            //call DoExecute
            DoExit(Entity);

            //Raise Exited
            if (Exited != null)
                Exited(this, StateEventArgs<T>.GetArgs(Entity));

            //Update Exit date/time at the end
            ExitTime = DateTime.Now;

            // Last call. Change status
            Status = StateConditions.TERMINATED;
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

            //Update Finish date/time at the end
            FinishTime = DateTime.Now;

            // Last call. Change status
            Status = StateConditions.FINISHED;
        }

        protected virtual void DoFinish(T Entity) { }

        protected bool CallChangeStateEvent(T Entity, State<T> NewState)
        {
            return CallChangeStateEvent(Entity, NewState, true, false);
        }

        /// <summary>
        /// Call this method when current state decide it need swith to different state,
        /// For ex travel state after it find destination coordinates switch to navigation
        /// state to actually start moving to destination
        /// </summary>
        /// <param name="Entity">Object this state interact with</param>
        /// <param name="NewState">New State</param>
        /// <param name="TrackPrevious">True if need track previous state
        /// (i.e state that calling change)</param>
        /// <param name="ExitPrevious">True if need exit previous
        /// (i.e state that calling change)</param>
        /// <returns>True if change accepted and false if not</returns>
        protected bool CallChangeStateEvent(T Entity, State<T> NewState, 
                                                bool TrackPrevious, bool ExitPrevious)
        {
            if (HasChangeStateEventHookup)
            {
                ChangeStateRequest(this, ChangeStateEventArgs<T>.GetArgs(Entity, 
                                                NewState, TrackPrevious, ExitPrevious));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Logging utility
        /// </summary>
        /// <param name="lfs">Logging facility</param>
        /// <param name="msg">Logging message</param>
        protected void Log(string lfs, string msg)
        {
            Output.Instance.Log(lfs, msg);
        }

        /// <summary>
        /// Debug utility
        /// </summary>
        /// <param name="lfs">Debug facility</param>
        /// <param name="msg">Debug message</param>
        protected void Debug(string lfs, string msg)
        {
            Output.Instance.Debug(lfs, msg, this);
        }
    }
}