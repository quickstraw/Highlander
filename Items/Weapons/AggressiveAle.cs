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
			item.width = 10;
			item.height = 10;

			item.holdStyle = 1;
			item.useTurn = true;
			item.autoReuse = true;

			item.useAnimation = 15;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;

			item.consumable = true;
			item.maxStack = 999;
			item.useTime = 15;

			item.shootSpeed = 9f;
			item.shoot = ModContent.ProjectileType<AggressiveAleProjectile>();
			item.damage = 30;

			item.noMelee = true;
			item.thrown = true;

			item.value = 80;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.useStyle = 2;
				item.useTime = 17;
				item.useAnimation = 17;
				item.damage = 0;
				item.shoot = 0;
				item.ammo = 353;
				item.notAmmo = true;
				item.buffType = mod.BuffType("DrunkenAnger");
				item.buffTime = 1800;
				item.UseSound = SoundID.Item3;
			}
			else
			{
				item.useStyle = 1;
				item.useTime = 15;
				item.useAnimation = 15;
				item.damage = 20;
				item.shoot = ModContent.ProjectileType<AggressiveAleProjectile>();
				item.ammo = 0;
				item.notAmmo = false;
				item.buffType = 0;
				item.buffTime = 0;
				item.UseSound = SoundID.Item1;
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
