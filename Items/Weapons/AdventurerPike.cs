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
    class AdventurerPike : ModItem
    {

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Adventurer's Pike");
			//Tooltip.SetDefault("Long reach has trouble hitting close enemies but deals more damage to bosses");
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.useStyle = 5;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.shootSpeed = 4.5f;
			Item.knockBack = 1.2f;
			Item.width = 32;
			Item.height = 32;
			Item.scale = 1f;
			Item.rare = ItemRarityID.Green;
			//Item.crit = 4;
			Item.value = Item.sellPrice(gold: 1, silver: 0);

			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true; // Important because the spear is actually a projectile instead of an Item. This prevents the melee hitbox of this Item.
			Item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this Item.
			Item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

			Item.UseSound = SoundID.Item1;
			Item.shoot = ProjectileType<AdventurerPikeProjectile>();
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one spear can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}


	}
}
