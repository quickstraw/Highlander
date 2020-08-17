﻿using Highlander.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.EnlightenmentIdol
{
	public class BlitzFist : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = Item.sellPrice(gold: 3, silver: 60);
			item.rare = ItemRarityID.Pink;
			item.noMelee = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 20;
			item.useTime = 20;
			item.knockBack = 7f;
			item.damage = 72;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<BlitzFistProjectile>();
			item.shootSpeed = 32.0f;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			//item.crit = 9;
			item.channel = true;
			item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.shoot = ModContent.ProjectileType<BlitzFistAltProjectile>();
			}
			else
			{
				item.shoot = ModContent.ProjectileType<BlitzFistProjectile>();
			}
			return base.CanUseItem(player);
		}
	}
}