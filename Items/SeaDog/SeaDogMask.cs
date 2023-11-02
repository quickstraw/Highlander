using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Highlander.Items.SeaDog
{
    [AutoloadEquip(EquipType.Head)]
    class SeaDogMask : ModItem
    {
        public override string Texture => "Highlander/Items/SeaDog/SeaDogMask";

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
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
