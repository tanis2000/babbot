using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BabBot.Wow;
using BabBot.Wow.Helpers;

namespace BabBot.States.Common
{
    class QuestAcceptState : AbstractQuestState
    {
        public QuestAcceptState(Quest quest, string lfs)
            : base(quest, lfs, QuestStates.UNKNOWN, QuestStates.ACCEPTED) { }

        protected override void DoEnter(WowPlayer player)
        {
            // Check if it already accepted
            try
            {
                if (QuestHelper.CheckQuest(q, lfs))
                    Finish(player);
            }
            catch
            {
                SkipQuest(player);
            }

            req = QuestHelper.MakeAcceptQuestReq();
        }

    }
}
