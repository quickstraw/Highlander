
using Highlander.Projectiles;
using Highlander.Projectiles.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Weapons.Spears
{
    class VerminSpear : ModItem
    {

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Vermin Spear");
			//Tooltip.SetDefault("Crits on poisoned targets");
		}

		public override void SetDefaults()
		{
			Item.damage = 13;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 14;
			Item.useTime = 28;
			Item.shootSpeed = 4.5f;
			Item.knockBack = 2.0f;
			Item.width = 32;
			Item.height = 32;
			Item.scale = 1f;
			Item.rare = ItemRarityID.Blue;
			//item.crit = 4;
			Item.value = Item.sellPrice(gold: 0, silver: 25);

			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
			Item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this item.
									  //item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

			Item.UseSound = SoundID.Item1;
			Item.shoot = ProjectileType<VerminSpearProjectile>();
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one spear can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

	}
}
