using Highlander.Items.Weapons;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.SeaDog
{
    class SeaDogBag : ModItem
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
            
            items.Add(ItemType<FeralFrenzy>());
            items.Add(ItemType<BrokenBlunderbuss>());

            int chance;
            int drops = 1;
            for (int i = 0; i < drops; i++)
            {
                chance = Main.rand.Next(0, items.Count);
                player.QuickSpawnItem(items[chance]);
                items.RemoveAt(chance);
            }
            player.QuickSpawnItem(ItemType<BarnacleBarrier>());
            if (Main.rand.NextBool(7))
            {
                player.QuickSpawnItem(ItemType<SeaDogMask>());
            }
            if (Main.rand.NextBool())
            {
                player.QuickSpawnItem(ItemID.SpelunkerPotion, 2);
            }
            else
            {
                player.QuickSpawnItem(ItemID.GillsPotion, 2);
            }
            int rand = Main.rand.Next(30, 50);
            player.QuickSpawnItem(ItemID.GoldOre, rand);
        }

        public override int BossBagNPC => NPCType<NPCs.SeaDog.SeaDog>();

    }
}
