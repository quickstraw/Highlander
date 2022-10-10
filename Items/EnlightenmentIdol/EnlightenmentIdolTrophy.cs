using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Highlander.Tiles;
using Terraria;
using Terraria.Localization;

namespace Highlander.Items.EnlightenmentIdol
{
    class EnlightenmentIdolTrophy : ModItem
	{
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault(Language.GetTextValue("Mods.Highlander.ItemName.EnlightenmentIdol." + GetType().Name));
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = TileType<BossTrophy>();
			Item.placeStyle = 1;
		}
	}
}
