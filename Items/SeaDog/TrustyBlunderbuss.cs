﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.SeaDog
{
    class TrustyBlunderbuss : ModItem
    {

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Couldn't hit the broad side of a barn");
		}

		public override void SetDefaults()
		{
			item.damage = 21;
			item.ranged = true;
			item.width = 40;
			item.height = 20;
			item.useTime = 46;
			item.useAnimation = 46;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 7;
			item.value = Item.sellPrice(gold: 3);
			item.rare = ItemRarityID.Green;
			//item.UseSound = SoundID.Item38;
			item.autoReuse = true;
			item.shoot = 10; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 14f;
			item.useAmmo = AmmoID.Bullet;
		}

		// What if I wanted it to shoot like a shotgun?
		// Shotgun style: Multiple Projectiles, Random spread 
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 offset = new Vector2(speedX, speedY);
			offset.Normalize();
			offset *= 10;
			int numberProjectiles = 6; // 6 shots
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(60)); // 25 degree spread.
				// If you want to randomize the speed to stagger the projectiles
				float scale = 1f - (Main.rand.NextFloat() * .4f);
				perturbedSpeed = perturbedSpeed * scale; 
				Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			offset *= 5f;
			// Smoke Dust spawn
			for (int i = 0; i < 2; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(position.X + offset.X, position.Y + offset.Y), 1, 1, DustID.Smoke, player.velocity.X, player.velocity.Y, 100, default(Color), 2f);
				Main.dust[dustIndex].velocity *= 1.4f;
			}
			// Fire Dust spawn
			for (int i = 0; i < 2; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(position.X + offset.X, position.Y + offset.Y), 1, 1, 6, player.velocity.X, player.velocity.Y, 100, default(Color), 3f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity *= 5f;
				dustIndex = Dust.NewDust(new Vector2(position.X + offset.X, position.Y + offset.Y), 1, 1, 6, player.velocity.X, player.velocity.Y, 100, default(Color), 2f);
				Main.dust[dustIndex].velocity *= 3f;
			}
			if (Main.netMode != NetmodeID.Server)
			{
				//Main.PlaySound(SoundID.Item14.SoundId, (int)player.Center.X, (int)player.Center.Y, SoundID.Item14.Style, 0.9f, -0.3f);
				Main.PlaySound(SoundID.Item38.SoundId, (int)player.Center.X, (int)player.Center.Y, SoundID.Item38.Style, 0.9f, -0.3f);
			}
			return false; // return false because we don't want tmodloader to shoot projectile
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-17, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<BrokenBlunderbuss>(), 1);
			recipe.SetResult(this, 1);
			recipe.AddIngredient(ItemID.Musket);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<BrokenBlunderbuss>(), 1);
			recipe.SetResult(this, 1);
			recipe.AddIngredient(ItemID.TheUndertaker);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();
		}
	}
}
