using Highlander.Items.HauntedHatter;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.SeaDog
{
    class SeaDogBag : GrabBag
    {
        public override bool IsPreHardMode() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ItemID.GoldOre, 1, 30, 50));
            itemLoot.Add(ItemDropRule.OneFromOptions(1, ItemID.SpelunkerPotion, ItemID.GillsPotion));
            itemLoot.Add(ItemDropRule.OneFromOptions(1, ItemType<FeralFrenzy>(), ItemType<BrokenBlunderbuss>()));
            itemLoot.Add(ItemDropRule.Common(ItemType<BarnacleBarrier>()));
            itemLoot.Add(ItemDropRule.Common(ItemType<SeaDogMask>(), 7));
        }

    }
}
