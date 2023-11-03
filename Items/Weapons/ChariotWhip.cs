using Highlander.Buffs;
using Highlander.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Weapons
{
    class ChariotWhip : ModItem
    {

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.shootSpeed = 4.5f;
			Item.knockBack = 1.2f;
			Item.width = 32;
			Item.height = 32;
			Item.scale = 1.0f;
			Item.rare = ItemRarityID.Blue;
			Item.crit = 4;
			Item.value = Item.sellPrice(gold: 1, silver: 50);

			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
			Item.noUseGraphic = true; // Important, it's kind of weird if people see two spears at one time. This prevents the melee animation of this item.
			Item.autoReuse = false; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

			Item.useTurn = true;

			Item.UseSound = SoundID.Item1;
			Item.shoot = ProjectileType<ChariotWhipProjectile>();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.useTime = 25;
				Item.useAnimation = 25;
				Item.shoot = ProjectileType<ChariotWhipProjectile>();

				Item.autoReuse = false;
				Item.noMelee = true;
				Item.noUseGraphic = true;

				Item.useTurn = false;

			}
			else
			{
				Item.useStyle = ItemUseStyleID.Swing;
				Item.useTime = 21;
				Item.useAnimation = 21;
				Item.shoot = ProjectileID.None;

				Item.autoReuse = false;
				Item.noMelee = false;
				Item.noUseGraphic = false;

				Item.useTurn = true;

			}
			if (player.itemAnimation == 0)
			{
				return player.ownedProjectileCounts[Item.shoot] < 1;
			}
			else
			{
				return false;
			}
			
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Add the Onfire buff to the NPC for 1 second when the weapon hits an NPC
            // 60 frames = 1 second
            target.AddBuff(BuffType<Bleed>(), 60);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Bleeding, 60);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }


    }
}
