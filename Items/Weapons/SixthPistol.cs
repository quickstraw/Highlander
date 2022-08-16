using Highlander.Projectiles.Bullets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Highlander.Items.Weapons
{
    class SixthPistol : AmmoGun
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sixth Pistol");
			Tooltip.SetDefault("Receives higher damage bonuses from better ammo");
		}


		public override void SetDefaults()
		{
			Item.damage = 38;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 5;
			Item.useAnimation = 5;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 4;
			Item.value = Item.sellPrice(silver: 60);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item41;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
			ammo = MaxAmmo;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			bool canShoot = false;
			if (Item.useAmmo == AmmoID.None)
			{
				ammo = MaxAmmo;
				Item.useAmmo = AmmoID.Bullet;
				Item.useTime = 5;
				Item.useAnimation = 5;
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.UseSound = SoundID.Item41;
			}
			else if (ammo > 0)
			{
				damage = (int)((damage - Item.damage) * 2.5 + Item.damage);
				type = ModContent.ProjectileType<TargetBullet>();
				ammo--;
				canShoot = true;
			}
			if (ammo <= 0)
			{
				Item.useAmmo = AmmoID.None;
				Item.useTime = 60;
				Item.useAnimation = 60;
				Item.useStyle = 2;
				Item.UseSound = null;
			}

			//Code to makse sure the bullet matches the muzzle.
			Vector2 velocityUnit = new Vector2(-velocity.Y, velocity.X);
			velocityUnit.Normalize();

			if (player.direction == -1)
			{
				position += velocityUnit * 5;
			}
			else
			{
				position -= velocityUnit * 5;
			}

            if (canShoot)
            {
				Vector2 mouse = Main.MouseWorld;
				int npcIndex = GetNPC(mouse);

				var bullet = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, ai0: (float) npcIndex + 1);
			}
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1.0f, 0);
		}

		public override byte GetMaxAmmo()
		{
			return 6;
		}

		private int GetNPC(Vector2 pos)
        {
			var npcs = Main.npc;
			float min = 360000;
			int target = -1;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i] != null && !Main.npc[i].friendly)
				{
					float distance = (Main.npc[i].Center - pos).LengthSquared();
					if (distance < min)
					{
						min = distance;
						target = i;
					}
				}
			}
			return target;
		}

	}
}
