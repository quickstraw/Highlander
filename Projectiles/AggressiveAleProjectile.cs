using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Highlander.Dusts;

namespace Highlander.Projectiles
{
	class AggressiveAleProjectile : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.penetrate = 1;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			// Vanilla explosions do less damage to Eater of Worlds in expert mode, so we will too.
			if (Main.expertMode)
			{
				if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
				{
					damage /= 5;
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(projectile.ai[1] <= 0)
			{
				Explode();
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.ai[1] > 0)
			{
				return true;
			} else if (projectile.owner == Main.myPlayer)
			{
				Explode();

				return false;
			}
			else
			{
				return false;
			}
		}

		public override void AI()
		{
			projectile.spriteDirection = projectile.direction;
			if(projectile.ai[0] + 1 >= 15f)
			{
				projectile.velocity.Y += 0.4f;
			}
			else
			{
				projectile.ai[0] += 1f;
			}
			if(projectile.velocity.X > 0)
			{
				projectile.rotation += 0.2f;
			}
			else
			{
				projectile.rotation -= 0.2f;
			}

			if (projectile.ai[1] > 0)
			{
				projectile.Kill();
			}
			Lighting.AddLight(projectile.position + projectile.velocity, 0.23f, 0.097f, 0.003f);
			Dust.NewDust(projectile.Center + projectile.velocity, 1, 1, ModContent.DustType<AggressiveAleDustExplosive>());

		}

		public override void Kill(int timeLeft)
		{
			if(projectile.ai[1] > 0)
			{
				// reset size to normal width and height.
				projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
				projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
				projectile.width = 10;
				projectile.height = 10;
				projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
				projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			}

			Main.PlaySound(SoundID.Item27.WithPitchVariance(0.2f).WithVolume(0.9f), projectile.position);
			Main.PlaySound(SoundID.Splash, projectile.position);
			for (int i = 0; i < 60; i++)
			{
				Dust.NewDust(projectile.Center, 15, 10, ModContent.DustType<AggressiveAleDustExplosive>());
			}
			for (int i = 1; i <= 3; i++)
			{
				float velX = Main.rand.NextFloat(-1, 1);
				float velY = Main.rand.NextFloat(-1, 1);
				Vector2 goreVelocity = new Vector2(velX, velY);
				Gore.NewGore(projectile.position, goreVelocity, mod.GetGoreSlot("Gores/MugGore" + i), 1f);
			}

		}

		private void Explode()
		{
			projectile.tileCollide = false;
			// Set to transparent. This projectile technically lives as  transparent for about 3 frames
			projectile.alpha = 255;
			// change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
			projectile.position = projectile.Center;
			projectile.width = 120;
			projectile.height = 120;
			projectile.Center = projectile.position;
			projectile.damage = projectile.damage * 2 / 3;
			projectile.knockBack = 2f;

			projectile.ai[1]++;
		}

	}
}
