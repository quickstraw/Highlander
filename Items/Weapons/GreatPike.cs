using Highlander.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Weapons
{
    class GreatPike : ModItem
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Great Pike");
			Tooltip.SetDefault("Long reach has trouble hitting close enemies but deals more damage to bosses");
		}

		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.useStyle = 5;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.shootSpeed = 4.5f;
			Item.knockBack = 2.0f;
			Item.width = 32;
			Item.height = 32;
			Item.scale = 1f;
			Item.rare = ItemRarityID.LightRed;
			//item.crit = 4;
			Item.value = Item.sellPrice(gold: 2, silver: 0);

			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
			Item.noUseGraphic = true; // Important, it's kind of weird if people see two spears at one time. This prevents the melee animation of this item.
			Item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

			Item.UseSound = SoundID.Item1;
			Item.shoot = ProjectileType<GreatPikeProjectile>();
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one spear can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}


	}
}
