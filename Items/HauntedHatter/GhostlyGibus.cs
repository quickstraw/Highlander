using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Highlander.Items.HauntedHatter
{
    [AutoloadEquip(EquipType.Head)]
    class GhostlyGibus : ModItem
    {
        public override string Texture => "Highlander/Items/HauntedHatter/GhostlyGibus";

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}
