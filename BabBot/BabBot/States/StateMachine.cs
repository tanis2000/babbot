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
using System.Threading;

namespace BabBot.States
{
    public sealed class StateMachine<T>
    {
        /// <summary>The object that owns this finite state engine</summary>
        public T Entity { get; private set; }

        /// <summary>
        /// The overal "goal" state (what this state machine is ultimately trying to accomplish.
        /// This will also manage what the current state is by changing states as needed
        /// </summary>
        /// <remarks>
        /// ie: Level up, fish, farm, w/e
        ///</remarks>
        public State<T> GlobalState { get; private set; }

        /// <summary>The currently active state for this state machine.</summary>
        /// <remarks>This will be set by the global state (most of the time).</remarks>
        public State<T> CurrentState { get; private set; }

        /// <summary>Date/Time when last update was executed</summary>
        public DateTime LastUpdated { get; private set; }

        /// <summary>True/False whether the state machine is running</summary>
        public bool IsRunning { get; set; }

        /// <summary>Create a new state machine for the given entity.</summary>
        /// <param name="Entity">The entity that this state machine belongs too</param>
        public StateMachine(T Entity)
        {
            //make sure entity is not null, if null then throw an exception
            if (Entity == null) throw new NullReferenceException("Entity must not be null.");

            //Capture Entity
            this.Entity = Entity;

            //set states to nulls
            GlobalState = null;
            CurrentState = null;
        }

        public void SetGlobalState(State<T> NewGlobalState)
        {
            //if a global state currently exists then exit i
            if (GlobalState != null) GlobalState.Exit(Entity);

            //set new global state
            GlobalState = NewGlobalState;

            //enter new global state
            GlobalState.Enter(Entity);
        }

        /// <summary>Update states in state machine</summary>
        public void Update()
        {
            //if state machine is not active, then skip
            if (!IsRunning) return;

            //update the global state (if it exists and has not already exited or finished)
            if (GlobalState != null && GlobalState.ExitTime == DateTime.MinValue && GlobalState.FinishTime == DateTime.MinValue)
                GlobalState.Execute(Entity);

            //update the current state (if it exists and has not already exited or finished)
            if (CurrentState != null && CurrentState.ExitTime == DateTime.MinValue && CurrentState.FinishTime == DateTime.MinValue) 
                CurrentState.Execute(Entity);

            //update last updated variable
            LastUpdated = DateTime.Now;
        }

        /// <summary>Change the current state to the new specified state.</summary>
        public void ChangeState(State<T> NewState)
        {
            //if new state is null ignore change request
            if (NewState == null) return;

            //Capture current state as previous state
            NewState.PreviousState = CurrentState;

            //Exit Current State
            CurrentState.Exit(Entity);

            //capture new current state
            CurrentState = NewState;

            //Enter new state
            CurrentState.Enter(Entity);
        }

        /// <summary>Change back to the previous state</summary>
        public void RevertToPreviousState()
        {
            ChangeState(CurrentState.PreviousState);
        }

        /// <summary>Is state machine in the specified state?</summary>
        public bool IsInState(Type State)
        {
            if (CurrentState.GetType() == State) return true;
            else return false;
        }
    }
}