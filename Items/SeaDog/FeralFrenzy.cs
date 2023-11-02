
using Highlander.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.SeaDog
{
	public class FeralFrenzy : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("Casts a slashing claw");
			//Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 4;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 4;
			Item.value = Item.sellPrice(silver: 75);
			Item.rare = ItemRarityID.Blue;
			//item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ProjectileType<FeralFrenzyProjectile>();
			Item.shootSpeed = 16f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 mouse = Main.MouseWorld;
			Vector2 vectorToMouse = mouse - player.Center;
			Projectile proj;
			if (vectorToMouse.LengthSquared() > 300 * 300)
			{
				Vector2 unit = vectorToMouse;
				unit.Normalize();
				proj = Projectile.NewProjectileDirect(source, player.Center + unit * 300, new Vector2(), type, damage, knockback, player.whoAmI, vectorToMouse.ToRotation());
			}
			else
			{
				proj = Projectile.NewProjectileDirect(source, Main.MouseWorld, new Vector2(), type, damage, knockback, player.whoAmI, vectorToMouse.ToRotation());
			}
			if (Main.netMode != NetmodeID.Server)
			{
				//SoundEngine.PlaySound(SoundID.Item1.SoundId, (int)proj.position.X, (int)proj.position.Y, SoundID.Item1.Style, 1.0f, +1.0f);
				//SoundEngine.PlaySound(SoundID.Item20.SoundId, (int)player.Center.X, (int)player.Center.Y, SoundID.Item20.Style, 0.8f, 0.3f);
				SoundEngine.PlaySound(SoundID.Item1 with { Volume = 1.0f, Pitch = 1.0f }, proj.position);
				SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.8f, Pitch = 0.3f }, proj.position);
			}
			return false;
        }

    }
}