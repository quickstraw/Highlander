using Highlander.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

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
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            var source = player.GetItemSource_OpenItem(Type);

            player.TryGettingDevArmor(source);
            List<int> items = new List<int>();
            items.Add(ItemType<BlitzFist>());
            items.Add(ItemType<CommanderBlessing>());
            int chance;
            int drops = 1;
            for (int i = 0; i < drops; i++)
            {
                chance = Main.rand.Next(0, items.Count);
                player.QuickSpawnItem(source, items[chance]);
                items.RemoveAt(chance);
            }
            player.QuickSpawnItem(source, ItemType<DivinePresence>());
            if (Main.rand.NextBool(7))
            {
                player.QuickSpawnItem(source, ItemType<EnlightenedMask>());
            }
        }

        public override int BossBagNPC => NPCType<NPCs.EnlightenmentIdol.EnlightenmentIdol>();

    }
}
