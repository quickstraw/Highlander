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
using Highlander.Sounds.Custom;

namespace Highlander.Projectiles
{
	class UnusualFireworkProjectile : ModProjectile
	{

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = false;
			Projectile.penetrate = 2;
			Projectile.ai[0] = 0;
			Projectile.ai[1] = 0;
			Projectile.tileCollide = false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void AI()
		{
			if(Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;

				Projectile.velocity.Y = Main.rand.NextFloat(-5, -4);
				Projectile.velocity.X = Main.rand.NextFloat(-1, 1);
				
				float pitch = Main.rand.NextFloat(-0.05f, 0.05f);
				//Main.PlaySound(SoundLoader.customSoundType, -1 , -1, 1, pitch, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/UnusualOpen"));
				SoundEngine.PlaySound(HighlanderSounds.UnusualOpenSound, Projectile.position);
			}
			Projectile.spriteDirection = Projectile.direction;
			Projectile.velocity.Y *= 1.005f;

			Projectile.rotation = (float) Math.Atan2(Projectile.velocity.X, -Projectile.velocity.Y);

			if(Projectile.ai[1] % 3 == 0)
			{
				Dust.NewDustPerfect(new Vector2(Projectile.position.X + Projectile.width / 2, Projectile.position.Y) - forward * 30, 6);
			}

			if (Projectile.ai.Length >= 2 && Projectile.ai[1] > 75)
			{
				Projectile.Kill();
			}

			Projectile.ai[1]++;
		}

		public override void Kill(int timeLeft)
		{
			//float pitch = Main.rand.NextFloat(-0.05f, 0.05f);
			//Main.PlaySound(SoundLoader.customSoundType, (int)Projectile.position.X, (int)Projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/UnusualPop"));

			SoundEngine.PlaySound(HighlanderSounds.UnusualPopVanilla, Projectile.position);

			if (Main.netMode != NetmodeID.Server) {
				// Spawn firework dust
				for (int i = 0; i < 20; i++)
				{
					int type = 130; // Red

					if(Main.rand.NextBool()){
						type = 132; // Blue and White
					}

					//Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<UnusualFireworkDust>());
					Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, type);
					d.velocity *= 0.4f;
					d.noGravity = true;
				}
				for (int i = 0; i < 60; i++) {
					int type = 130; // Red

					if (Main.rand.NextBool())
					{
						type = 132;
					}

					//Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<UnusualFireworkDust>());
					Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, type);
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

					//Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<UnusualFireworkDust>());
					Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, type);
					d.velocity.Normalize();
					d.velocity *= 16;
					d.noGravity = true;
				}
			}

			// Red Firework Implementation
			/**
			Main.PlaySound(SoundID.Item14, Projectile.position);
			for (int index1 = 0; index1 < 400; ++index1)
			{
				float num1 = 16f;
				if (index1 < 300)
					num1 = 12f;
				if (index1 < 200)
					num1 = 8f;
				if (index1 < 100)
					num1 = 4f;
				int index2 = Dust.NewDust(new Vector2((float)Projectile.position.X, (float)Projectile.position.Y), 6, 6, ModContent.DustType<FireworkCloneDust>(), 0.0f, 0.0f, 100);
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
				float rotation = Projectile.rotation - MathHelper.PiOver2;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

	}
}
