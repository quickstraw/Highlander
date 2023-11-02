using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.SeaDog
{
    class TrustyBlunderbuss : ModItem
    {

		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("Couldn't hit the broad side of a barn");
		}

		public override void SetDefaults()
		{
			Item.damage = 21;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 46;
			Item.useAnimation = 46;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 7;
			Item.value = Item.sellPrice(gold: 3);
			Item.rare = ItemRarityID.Green;
			//item.UseSound = SoundID.Item38;
			Item.autoReuse = true;
			Item.shoot = 10; //idk why but all the guns in the vanilla source have this
			Item.shootSpeed = 14f;
			Item.useAmmo = AmmoID.Bullet;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 offset = velocity;
			offset.Normalize();
			offset *= 10;
			int numberProjectiles = 6; // 6 shots
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(60)); // 25 degree spread.
																												// If you want to randomize the speed to stagger the projectiles
				float scale = 1f - (Main.rand.NextFloat() * .4f);
				perturbedSpeed = perturbedSpeed * scale;
				Projectile.NewProjectile(source, position.X + offset.X, position.Y + offset.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
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
				//SoundEngine.PlaySound(SoundID.Item38.SoundId, (int)player.Center.X, (int)player.Center.Y, SoundID.Item38.Style, 0.9f, -0.3f);
				SoundEngine.PlaySound(SoundID.Item38 with { Volume = 0.9f, Pitch = -0.3f }, player.Center);
			}
			return false;
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-17, 0);
		}
	}
}
