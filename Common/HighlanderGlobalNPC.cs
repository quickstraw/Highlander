using Highlander.Items;
using Highlander.Items.LockBoxes;
using Highlander.Items.HauntedHatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Common
{
    class HighlanderGlobalNPC : GlobalNPC
    {

        public override bool InstancePerEntity => true;

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            CrateSpawnRule CrateSpawn = new CrateSpawnRule();
            Crate2SpawnRule Crate2Spawn = new Crate2SpawnRule();
            SpookyCrateSpawnRule SpookyCrateSpawn = new SpookyCrateSpawnRule();
            WinterCrateSpawnRule WinterCrateSpawn = new WinterCrateSpawnRule();
            ReserveCrateSpawnRule ReserveCrateSpawn = new ReserveCrateSpawnRule();

            IItemDropRule crateRule = new LeadingConditionRule(CrateSpawn);
            IItemDropRule crate2Rule = new LeadingConditionRule(Crate2Spawn);
            IItemDropRule spookyCrateRule = new LeadingConditionRule(SpookyCrateSpawn);
            IItemDropRule winterCrateRule = new LeadingConditionRule(WinterCrateSpawn);
            IItemDropRule reserveCrateRule = new LeadingConditionRule(ReserveCrateSpawn);

            

            crateRule.OnSuccess(new CommonDrop(ItemType<HatSupplyLockBox>(), 50));
            crate2Rule.OnSuccess(ItemDropRule.OneFromOptions(50, ItemType<HatSupplyLockBox>(), ItemType<HatLockBox2>()));
            spookyCrateRule.OnSuccess(new CommonDrop(ItemType<SpookyHatLockBox>(), 50));
            winterCrateRule.OnSuccess(new CommonDrop(ItemType<WinterHatSupplyLockBox>(), 50));
            reserveCrateRule.OnSuccess(new CommonDrop(ItemType<ReserveHatSupplyLockBox>(), 50));

            npcLoot.Add(crateRule);
            npcLoot.Add(crate2Rule);
            npcLoot.Add(spookyCrateRule);
            npcLoot.Add(winterCrateRule);
            npcLoot.Add(reserveCrateRule);
        }

    }
}
