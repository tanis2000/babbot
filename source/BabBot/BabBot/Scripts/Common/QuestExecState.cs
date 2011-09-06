using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Wow;
using BabBot.Wow.Helpers;

namespace BabBot.States.Common
{
    class QuestExecState : State<WowPlayer>
    {
        /// <summary>
        /// Pointer on current quest
        /// </summary>
        private Quest _q;

        /// <summary>
        /// Quest index in toon's quest log
        /// </summary>
        private int _idx;

        /// <summary>
        /// Log facility
        /// </summary>
        private string _lfs;

        /// <summary>
        /// Pointer on QuestAcceptState if this state raised
        /// </summary>
        QuestAcceptState _accept_state;

        /// <summary>
        /// Pointer on QuestDeliveryState if this state raised
        /// </summary>
        QuestDeliveryState _delivery_state;

        public QuestExecState(Quest quest, string lfs)
        {
            _q = quest;
            _lfs = lfs;
        }

        protected override void DoEnter(WowPlayer player)
        {
            if (_q.State != QuestStates.ACCEPTED)
            {
                // Check if quest accepted
                _accept_state = new QuestAcceptState(_q, _lfs);

                // Switch to QuestAcceptState
                player.StateMachine.ChangeState(_accept_state, true, false);
            }
        }

        protected override void DoExecute(WowPlayer player)
        {
            if ((_accept_state != null) && (_accept_state.FinishTime != null))
                // Wait accept state finish
                return;

            switch (_q.State)
            {
                case QuestStates.ACCEPTED:
                    if ((_q.Objectives.ObjList == null) ||
                        _q.Completed) { }

                    // Go to each objective
                    break;


            }
        }

        protected override void DoExit(WowPlayer player)
        {
            //on exit we will do nothing
            return;
        }

        protected override void DoFinish(WowPlayer player)
        {
            return;
        }
    }
}
