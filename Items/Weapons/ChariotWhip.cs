using Highlander.Buffs;
using Highlander.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Weapons
{
    class ChariotWhip : ModItem
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chariot's Whip");
			Tooltip.SetDefault("Slashes bleed enemies\nLunges deal extra damage.");
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 28;
			item.useTime = 28;
			item.shootSpeed = 4.5f;
			item.knockBack = 1.2f;
			item.width = 32;
			item.height = 32;
			item.scale = 1.1f;
			item.rare = ItemRarityID.Green;
			item.crit = 4;
			item.value = Item.sellPrice(gold: 1, silver: 0);

			item.melee = true;
			item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
			item.noUseGraphic = true; // Important, it's kind of weird if people see two spears at one time. This prevents the melee animation of this item.
			item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

			item.useTurn = true;

			item.UseSound = SoundID.Item1;
			item.shoot = ProjectileType<ChariotWhipProjectile>();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.useTime = 25;
				item.useAnimation = 25;
				item.shoot = ProjectileType<ChariotWhipProjectile>();

				item.autoReuse = false;
				item.noMelee = true;
				item.noUseGraphic = true;

				item.useTurn = false;

			}
			else
			{
				item.useStyle = ItemUseStyleID.SwingThrow;
				item.useTime = 21;
				item.useAnimation = 21;
				item.shoot = ProjectileID.None;

				item.autoReuse = true;
				item.noMelee = false;
				item.noUseGraphic = false;

				item.useTurn = true;

			}
			if (player.itemAnimation == 0)
			{
				return player.ownedProjectileCounts[item.shoot] < 1;
			}
			else
			{
				return false;
			}
			
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			// Add the Onfire buff to the NPC for 1 second when the weapon hits an NPC
			// 60 frames = 1 second
			target.AddBuff(BuffType<Bleed>(), 60);
		}

		public override void OnHitPvp(Player player, Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Bleeding, 60);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return true;
		}


	}
}
