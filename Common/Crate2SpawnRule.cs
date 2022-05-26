using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;

namespace Highlander.Common
{
    public class Crate2SpawnRule : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            var npc = info.npc;
            bool passive = npc.aiStyle == 7 || npc.aiStyle == 24 || npc.aiStyle == 64 || npc.aiStyle == 65 || npc.aiStyle == 66 || npc.aiStyle == 67 || npc.aiStyle == 68;
            bool downedHatter = HighlanderWorld.downedHauntedHatter;
            bool downedIdol = HighlanderWorld.downedEnlightenmentIdol;
            bool boss = npc.boss;
            return !passive && downedHatter && downedIdol;
        }

        public bool CanShowItemDropInUI()
        {
            return false;
        }

        public string GetConditionDescription()
        {
            return "Spawns after defeating the Haunted Hatter and the Idol of Enlightenment.";
        }
    }
}
