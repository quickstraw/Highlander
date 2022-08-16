﻿using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Highlander.Tiles;
using Terraria;

namespace Highlander.Items.SeaDog
{
    class SeaDogTrophy : ModItem
	{
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
			Item.placeStyle = 2;
		}
	}
}
