using Highlander.Projectiles.Bullets;
using Microsoft.Xna.Framework;
using Terraria;
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
			item.damage = 31;
			item.ranged = true;
			item.width = 40;
			item.height = 20;
			item.useTime = 5;
			item.useAnimation = 5;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4;
			item.value = Item.sellPrice(silver: 60);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item41;
			item.autoReuse = false;
			item.shoot = ProjectileID.PurificationPowder; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 18f;
			item.useAmmo = AmmoID.Bullet;
			ammo = MaxAmmo;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			bool canShoot = false;
			if(item.useAmmo == AmmoID.None)
			{
				ammo = MaxAmmo;
				item.useAmmo = AmmoID.Bullet;
				item.useTime = 5;
				item.useAnimation = 5;
				item.useStyle = ItemUseStyleID.HoldingOut;
				item.UseSound = SoundID.Item41;
			}
			else if(ammo > 0)
			{
				damage = (int)((damage - item.damage) * 2.5 + item.damage);
				type = ModContent.ProjectileType<RicochetingBullet>();
				ammo--;
				canShoot = true;
			}
			if(ammo <= 0)
			{
				item.useAmmo = AmmoID.None;
				item.useTime = 60;
				item.useAnimation = 60;
				item.useStyle = 2;
				item.UseSound = null;
			}

			//Code to makse sure the bullet matches the muzzle.
			Vector2 velocityUnit = new Vector2(-speedY, speedX);
			velocityUnit.Normalize();

			if (player.direction == -1)
			{
				position += velocityUnit * 5;
			}
			else
			{
				position -= velocityUnit * 5;
			}

			return canShoot;
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
