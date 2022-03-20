using Highlander.Buffs;
using Highlander.Dusts;
using Highlander.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.Weapons
{
    class AggressiveAle : ModItem
    {

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("'Ale for throwing or for fun!'");
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 10;

			Item.holdStyle = 1;
			Item.useTurn = true;
			Item.autoReuse = true;

			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.UseSound = SoundID.Item1;

			Item.consumable = true;
			Item.maxStack = 999;
			Item.useTime = 15;

			Item.shootSpeed = 9f;
			Item.shoot = ModContent.ProjectileType<AggressiveAleProjectile>();
			Item.damage = 30;

			Item.noMelee = true;
			Item.DamageType = DamageClass.Throwing;

			Item.rare = ItemRarityID.Green;

			Item.value = 80;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useStyle = 2;
				Item.useTime = 17;
				Item.useAnimation = 17;
				Item.damage = 0;
				Item.shoot = 0;
				Item.ammo = 353;
				Item.notAmmo = true;
				Item.buffType = ModContent.BuffType<DrunkenAnger>();
				Item.buffTime = 1800;
				Item.UseSound = SoundID.Item3;
			}
			else
			{
				Item.useStyle = 1;
				Item.useTime = 15;
				Item.useAnimation = 15;
				Item.damage = 20;
				Item.shoot = ModContent.ProjectileType<AggressiveAleProjectile>();
				Item.ammo = 0;
				Item.notAmmo = false;
				Item.buffType = 0;
				Item.buffTime = 0;
				Item.UseSound = SoundID.Item1;
			}
			return base.CanUseItem(player);
		}

		public override void HoldItem(Player player)
		{
			player.itemLocation.X = (float)(player.Center.X + 8 * player.direction);
			player.itemLocation.Y = (float)(player.MountedCenter.Y + 11.0);

			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position + player.velocity, 0.23f, 0.097f, 0.003f);

			Vector2 dustLoc = player.itemLocation + player.velocity;
			dustLoc.Y -= 14;

			if (Main.rand.NextBool(20))
			{
				if (player.direction > 0)
				{
					Dust.NewDust(dustLoc, 16, 1, ModContent.DustType<AggressiveAleDust>());
				}
				else
				{
					dustLoc.X -= 20;
					Dust.NewDust(dustLoc, 16, 1, ModContent.DustType<AggressiveAleDust>());
				}
			}
		}
	}
}
