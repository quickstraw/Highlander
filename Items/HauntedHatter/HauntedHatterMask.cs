using Terraria;
using Terraria.ModLoader;


namespace Highlander.Items.HauntedHatter
{
    [AutoloadEquip(EquipType.HandsOff)]
    class HauntedHatterMask : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = 3;
            Item.vanity = true;
            Item.accessory = true;
        }

    }
}
