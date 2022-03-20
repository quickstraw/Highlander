using Highlander.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.EnlightenmentIdol
{
	public class BlitzFist : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 3, silver: 60);
			Item.rare = ItemRarityID.Pink;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.knockBack = 7f;
			Item.damage = 72;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<BlitzFistProjectile>();
			Item.shootSpeed = 32.0f;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			//item.crit = 9;
			Item.channel = true;
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.shoot = ModContent.ProjectileType<BlitzFistAltProjectile>();
			}
			else
			{
				Item.shoot = ModContent.ProjectileType<BlitzFistProjectile>();
			}
			return base.CanUseItem(player);
		}
	}
}