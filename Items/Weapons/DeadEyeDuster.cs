using Highlander.Projectiles.Bullets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace Highlander.Items.Weapons
{
    class DeadEyeDuster : AmmoGun
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dead Eye Duster");
			Tooltip.SetDefault("Receives higher damage bonuses from better ammo\nFires Ricochet Bullets");
		}


		public override void SetDefaults()
		{
			Item.damage = 31;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 5;
			Item.useAnimation = 5;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 4;
			Item.value = Item.sellPrice(silver: 60);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item41;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			Item.shootSpeed = 18f;
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
				type = ModContent.ProjectileType<RicochetingBullet>();
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
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			}
			return false;
		}

        public override void UpdateInventory(Player player)
		{
		}

		public override byte GetMaxAmmo()
		{
			return 6;
		}

	}
}
