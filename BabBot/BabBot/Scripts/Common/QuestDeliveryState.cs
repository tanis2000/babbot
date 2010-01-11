using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Wow;
using BabBot.Wow.Helpers;

namespace BabBot.States.Common
{
    class QuestDeliveryState : AbstractQuestState
    {
        /// <summary>
        /// Choice if reward required choice
        /// </summary>
        int _choice;

        /// <summary>
        /// Quest Delivery state. While in this state I assumed all verification done i.e
        /// a) Quest already in toon's log
        /// b) Quest completed
        /// </summary>
        /// <param name="quest">Quest Object</param>
        /// <param name="lfs">Name of logging facility</param>
        public QuestDeliveryState(Quest quest, string lfs, int reward_choice)
            : base(quest, lfs, QuestStates.COMPLETED, QuestStates.DELIVERED)
        {
            _choice = reward_choice;
        }

        protected override void DoEnter(WowPlayer player)
        {
            req = QuestHelper.MakeDeliveryQuestReq(_choice);
        }
    }
}
