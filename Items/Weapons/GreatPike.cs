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
			item.damage = 30;
			item.useStyle = 5;
			item.useAnimation = 28;
			item.useTime = 28;
			item.shootSpeed = 4.5f;
			item.knockBack = 2.0f;
			item.width = 32;
			item.height = 32;
			item.scale = 1f;
			item.rare = ItemRarityID.LightRed;
			//item.crit = 4;
			item.value = Item.sellPrice(gold: 2, silver: 0);

			item.melee = true;
			item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
			item.noUseGraphic = true; // Important, it's kind of weird if people see two spears at one time. This prevents the melee animation of this item.
			item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

			item.UseSound = SoundID.Item1;
			item.shoot = ProjectileType<GreatPikeProjectile>();
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one spear can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[item.shoot] < 1;
		}


	}
}
