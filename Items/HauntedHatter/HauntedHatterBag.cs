using Highlander.Items.Weapons;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.HauntedHatter
{
    class HauntedHatterBag : GrabBag
    {
        public override bool IsPreHardMode() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.OneFromOptions(1, ItemType<SpiritShears>(), ItemType<AncientStoneBlaster>()));
            itemLoot.Add(ItemDropRule.Common(ItemType<EnchantedNeedleHook>()));
            itemLoot.Add(ItemDropRule.Common(ItemType<GhostlyGibus>(), 7));
        }

    }
}
