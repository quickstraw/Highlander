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
using Terraria.Audio;
using Terraria.DataStructures;

namespace Highlander.Projectiles
{
	class AggressiveAleProjectile : ModProjectile
	{

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
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
			if(Projectile.ai[1] <= 0)
			{
				Explode();
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.ai[1] > 0)
			{
				return true;
			} else if (Projectile.owner == Main.myPlayer)
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
			Projectile.spriteDirection = Projectile.direction;
			if(Projectile.ai[0] + 1 >= 15f)
			{
				Projectile.velocity.Y += 0.4f;
			}
			else
			{
				Projectile.ai[0] += 1f;
			}
			if(Projectile.velocity.X > 0)
			{
				Projectile.rotation += 0.2f;
			}
			else
			{
				Projectile.rotation -= 0.2f;
			}

			if (Projectile.ai[1] > 0)
			{
				Projectile.Kill();
			}
			Lighting.AddLight(Projectile.position + Projectile.velocity, 0.23f, 0.097f, 0.003f);
			Dust.NewDust(Projectile.Center + Projectile.velocity, 1, 1, ModContent.DustType<AggressiveAleDustExplosive>());

		}

		public override void Kill(int timeLeft)
		{
			if(Projectile.ai[1] > 0)
			{
				// reset size to normal width and height.
				Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
				Projectile.width = 10;
				Projectile.height = 10;
				Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			}

			//SoundEngine.PlaySound(SoundID.Item27.WithPitchVariance(0.2f).WithVolume(0.9f), Projectile.position);
			SoundEngine.PlaySound(SoundID.Item27 with { PitchVariance = 0.2f, Volume = 0.9f}, Projectile.position);
			SoundEngine.PlaySound(SoundID.Splash, Projectile.position);
			for (int i = 0; i < 60; i++)
			{
				Dust.NewDust(Projectile.Center, 15, 10, ModContent.DustType<AggressiveAleDustExplosive>());
			}
			for (int i = 1; i <= 3; i++)
			{
				float velX = Main.rand.NextFloat(-1, 1);
				float velY = Main.rand.NextFloat(-1, 1);
				Vector2 goreVelocity = new Vector2(velX, velY);
				var source = Projectile.GetSource_Death();
				Gore.NewGore(source, Projectile.position, goreVelocity, Mod.Find<ModGore>("MugGore" + i).Type, 1f);
			}

		}

		private void Explode()
		{
			Projectile.tileCollide = false;
			// Set to transparent. This Projectile technically lives as  transparent for about 3 frames
			Projectile.alpha = 255;
			// change the hitbox size, centered about the original Projectile center. This makes the Projectile damage enemies during the explosion.
			Projectile.position = Projectile.Center;
			Projectile.width = 120;
			Projectile.height = 120;
			Projectile.Center = Projectile.position;
			Projectile.damage = Projectile.damage * 2 / 3;
			Projectile.knockBack = 2f;

			Projectile.ai[1]++;
		}

	}
}
