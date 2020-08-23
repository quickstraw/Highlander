
using Highlander.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.SeaDog
{
	public class FeralFrenzy : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Casts a slashing claw");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 23;
			item.magic = true;
			item.mana = 4;
			item.width = 40;
			item.height = 40;
			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.Blue;
			//item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ProjectileType<FeralFrenzyProjectile>();
			item.shootSpeed = 16f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 mouse = Main.MouseWorld;
			Vector2 vectorToMouse = mouse - player.Center;
			Projectile proj;
			if(vectorToMouse.LengthSquared() > 300 * 300)
			{
				Vector2 unit = vectorToMouse;
				unit.Normalize();
				proj = Projectile.NewProjectileDirect(player.Center + unit * 300, new Vector2(), type, damage, knockBack, player.whoAmI, vectorToMouse.ToRotation());
			}
			else
			{
				proj = Projectile.NewProjectileDirect(Main.MouseWorld, new Vector2(), type, damage, knockBack, player.whoAmI, vectorToMouse.ToRotation());
			}
			if (Main.netMode != NetmodeID.Server)
			{
				Main.PlaySound(SoundID.Item1.SoundId, (int)proj.position.X, (int)proj.position.Y, SoundID.Item1.Style, 1.0f, +1.0f);
				Main.PlaySound(SoundID.Item20.SoundId, (int)player.Center.X, (int)player.Center.Y, SoundID.Item20.Style, 0.8f, 0.3f);
			}
			return false;
		}

	}
}