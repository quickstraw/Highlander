using Highlander.Items;
using Highlander.Items.HauntedHatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs
{
    class HighlanderGlobalNPC : GlobalNPC
    {

        public override bool InstancePerEntity => true;

        public override void NPCLoot(NPC npc)
        {
            bool passive = npc.aiStyle == 7 || npc.aiStyle == 24 || npc.aiStyle == 64 || npc.aiStyle == 65 || npc.aiStyle == 66 || npc.aiStyle == 67 || npc.aiStyle == 68;
            bool drop = Main.rand.NextBool(50);

            if (HighlanderWorld.downedHauntedHatter && !passive && drop)
            {
                if (Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].ZoneSnow)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<WinterHatSupplyLockBox>());
                }
                else
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<HatSupplyLockBox>());
                }
            }

            if (NPC.downedBoss2 && !HighlanderWorld.downedHauntedHatter)
            {
                bool dropSpookyHeadwear = Main.rand.NextBool(100);

                if (dropSpookyHeadwear)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<SpookyHeadwear>());
                }
            }

        }

    }
}
