using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Highlander.Items.EnlightenmentIdol
{
    [AutoloadEquip(EquipType.Head)]
    class EnlightenedMask : ModItem
    {
        public override string Texture => "Highlander/Items/EnlightenmentIdol/EnlightenedMask";

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
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
