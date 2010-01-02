using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Wow;
using BabBot.States;
using BabBot.Wow.Helpers;

namespace BabBot.States.Common
{
    abstract public class AbstractQuestState : State<WowPlayer>
    {
        /// <summary>
        /// Pointer on current quest
        /// </summary>
        protected Quest q;

        /// <summary>
        /// Log facility
        /// </summary>
        protected string lfs;

        /// <summary>
        /// Quest Giver/Taker
        /// </summary>
        protected GameObject obj;

        /// <summary>
        /// Accept/Delivery request used to invoke methods from QuestHelper
        /// </summary>
        protected QuestReq req;

        private readonly QuestStates _start_state;
        private readonly QuestStates _end_state;

        public AbstractQuestState(Quest quest, string lfs,
            QuestStates start_state, QuestStates end_state)
        {
            this.q = quest;
            this.lfs = lfs;

            _start_state = start_state;
            _end_state = end_state;
        }

        protected override void DoExecute(WowPlayer player)
        {
            try
            {
                if (q.State == _start_state)
                    obj = QuestHelper.FindQuestGameObj(req, q, lfs);
                else if (q.State == _end_state)
                    Finish(player);
                else
                {
                    switch (q.State)
                    {
                        case QuestStates.OBJ_FOUND:
                            Vector3D dest = NpcHelper.GetGameObjCoord(obj, lfs);

                            // Switch to Travel State
                            string msg = "Moving to quest " + 
                                    req.NpcDestText + " " + obj.Name;
                            Log(lfs, msg + " ...");
                            NavigationState ns = new NavigationState(dest, lfs, msg);

                            ns.Finished += SetQuestStateReached;
                            CallChangeStateEvent(player, ns, true, false);

                            q.State = QuestStates.MOVING_TO_OBJ;
                            break;

                        case QuestStates.MOVING_TO_OBJ:
                            // Do nothing
                            break;

                        // Keep it for now but works for NPC only
                        case QuestStates.OBJ_REACHED:
                            // Target game object
                            NpcHelper.TargetGameObj(obj, lfs);
                            q.State = QuestStates.OBJ_TARGETED;
                            break;

                        case QuestStates.OBJ_TARGETED:
                            QuestHelper.SelectGameObjQuest(req, obj, q, lfs);
                            break;

                        case QuestStates.SELECTED:
                            QuestHelper.DoActionEx(req, q, lfs);
                            break;

                        default:
                            throw new QuestSkipException("Unknown quest state: " +
                                Enum.GetName(typeof(QuestStates), q.State));
                    }
                }
            }
            catch // For now skip quest on any exception
            {
                SkipQuest(player);
            }
        }

        private void SetQuestStateReached(object sm, EventArgs arg)
        {
            q.State = QuestStates.OBJ_REACHED;
        }

        /// <summary>
        /// Skip Quest
        /// </summary>
        protected void SkipQuest(WowPlayer player)
        {
            q.State = QuestStates.SKIPPED;
            Finish(player);
        }
    }
}
