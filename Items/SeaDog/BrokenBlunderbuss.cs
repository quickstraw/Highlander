using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.SeaDog
{
    class BrokenBlunderbuss : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(silver: 20);
        }

	}
}
