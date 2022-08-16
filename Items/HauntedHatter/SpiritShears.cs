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

namespace Highlander.Items.HauntedHatter
{
    class SpiritShears : ModItem
    {

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Perfect size for stabbing.");
		}

		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.useStyle = 5;
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.shootSpeed = 4.5f;
			Item.knockBack = 4.5f;
			Item.width = 32;
			Item.height = 32;
			Item.scale = 1f;
			Item.rare = ItemRarityID.Green;
			Item.crit = 4;
			Item.value = Item.sellPrice(silver: 60);

			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
			Item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this item.
			Item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

			Item.UseSound = SoundID.Item1;
			Item.shoot = ProjectileType<SpiritShearsProjectile>();
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one spear can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}


	}
}
