
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
			DisplayName.SetDefault("Vermin Spear");
			//Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.damage = 13;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 14;
			item.useTime = 28;
			item.shootSpeed = 4.5f;
			item.knockBack = 2.0f;
			item.width = 32;
			item.height = 32;
			item.scale = 1f;
			item.rare = ItemRarityID.Blue;
			//item.crit = 4;
			item.value = Item.sellPrice(gold: 0, silver: 25);

			item.melee = true;
			item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
			item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this item.
			//item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

			item.UseSound = SoundID.Item1;
			item.shoot = ProjectileType<VerminSpearProjectile>();
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one spear can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[item.shoot] < 1;
		}

	}
}
