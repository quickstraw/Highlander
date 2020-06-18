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
	class UnusualFireworkProjectile : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = false;
			projectile.penetrate = 2;
			projectile.ai[0] = 0;
			projectile.ai[1] = 0;
			projectile.tileCollide = false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Main.expertMode)
			{
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			base.OnHitNPC(target, damage, knockback, crit);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void AI()
		{
			if(projectile.ai[0] == 0)
			{
				projectile.ai[0] = 1;

				projectile.velocity.Y = Main.rand.NextFloat(-5, -4);
				projectile.velocity.X = Main.rand.NextFloat(-1, 1);
				
				float pitch = Main.rand.NextFloat(-0.05f, 0.05f);
				//Main.PlaySound(SoundLoader.customSoundType, -1 , -1, 1, pitch, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/UnusualOpen"));
				Main.PlaySound(SoundLoader.customSoundType, (int) projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/UnusualOpen"));
			}
			projectile.spriteDirection = projectile.direction;
			projectile.velocity.Y *= 1.005f;

			projectile.rotation = (float) Math.Atan2(projectile.velocity.X, -projectile.velocity.Y);

			if(projectile.ai[1] % 3 == 0)
			{
				Dust.NewDustPerfect(new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y) - forward * 30, 6);
			}

			if (projectile.ai.Length >= 2 && projectile.ai[1] > 75)
			{
				projectile.Kill();
			}

			projectile.ai[1]++;
		}

		public override void Kill(int timeLeft)
		{
			//float pitch = Main.rand.NextFloat(-0.05f, 0.05f);
			//Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/UnusualPop"));

			Main.PlaySound(SoundID.Item14.WithPitchVariance(0.1f).WithVolume(0.7f), projectile.position);

			if (Main.netMode != NetmodeID.Server) {
				// Spawn firework dust
				for (int i = 0; i < 20; i++)
				{
					int type = 130; // Red

					if(Main.rand.NextBool()){
						type = 132; // Blue and White
					}

					//Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<UnusualFireworkDust>());
					Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, type);
					d.velocity *= 0.4f;
					d.noGravity = true;
				}
				for (int i = 0; i < 60; i++) {
					int type = 130; // Red

					if (Main.rand.NextBool())
					{
						type = 132;
					}

					//Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<UnusualFireworkDust>());
					Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, type);
					d.velocity.Normalize();
					d.velocity *= 8;
					d.noGravity = true;
				}
				for (int i = 0; i < 180; i++)
				{
					int type = 130; // Red

					if (Main.rand.NextBool())
					{
						type = 132;
					}

					//Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<UnusualFireworkDust>());
					Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, type);
					d.velocity.Normalize();
					d.velocity *= 16;
					d.noGravity = true;
				}
			}

			// Red Firework Implementation
			/**
			Main.PlaySound(SoundID.Item14, projectile.position);
			for (int index1 = 0; index1 < 400; ++index1)
			{
				float num1 = 16f;
				if (index1 < 300)
					num1 = 12f;
				if (index1 < 200)
					num1 = 8f;
				if (index1 < 100)
					num1 = 4f;
				int index2 = Dust.NewDust(new Vector2((float)projectile.position.X, (float)projectile.position.Y), 6, 6, ModContent.DustType<FireworkCloneDust>(), 0.0f, 0.0f, 100);
				float num2 = (float)Main.dust[index2].velocity.X;
				float y = (float)Main.dust[index2].velocity.Y;
				if ((double)num2 == 0.0 && (double)y == 0.0)
					num2 = 1f;
				float num3 = (float)Math.Sqrt((double)num2 * (double)num2 + (double)y * (double)y);
				float num4 = num1 / num3;
				float num5 = num2 * num4;
				float num6 = y * num4;
				Dust dust = Main.dust[index2];
				dust.velocity = dust.velocity * 0.5f;
				// ISSUE: explicit reference operation
				// ISSUE: cast to a reference type
				// ISSUE: variable of a reference type
				Main.dust[index2].velocity.X += num5;
				// ISSUE: explicit reference operation
				// ISSUE: cast to a reference type
				// ISSUE: variable of a reference type
				Main.dust[index2].velocity.Y += num6;
				// ISSUE: explicit reference operation
				// ISSUE: explicit reference operation
				Main.dust[index2].scale = 1.3f;
				Main.dust[index2].noGravity = true;
			}**/

		}

		private Vector2 forward
		{
			get
			{
				float rotation = projectile.rotation - MathHelper.PiOver2;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

	}
}
