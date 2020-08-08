using Highlander.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.EnlightenmentIdol
{
    class EnlightenmentIdolBag : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = 9;
            item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor();
            List<int> items = new List<int>();
            //items.Add(ItemType<AncientStoneBlaster>());
            int chance;
            int drops = 1;
            for (int i = 0; i < drops; i++)
            {
                chance = Main.rand.Next(0, items.Count);
                player.QuickSpawnItem(items[chance]);
                items.RemoveAt(chance);
            }
            //player.QuickSpawnItem(ItemType<EnchantedNeedleHook>());
            if (Main.rand.NextBool(7))
            {
                //player.QuickSpawnItem(ItemType<GhostlyGibus>());
            }
        }

        public override int BossBagNPC => NPCType<NPCs.EnlightenmentIdol.EnlightenmentIdol>();

    }
}
