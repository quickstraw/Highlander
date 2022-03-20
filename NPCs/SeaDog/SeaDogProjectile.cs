using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.NPCs.SeaDog
{
    class SeaDogProjectile : ModProjectile
    {

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			DisplayName.SetDefault("Sea Dog's Powder Keg");
		}

		public override void SetDefaults()
		{
			// while the sprite is actually bigger than 14x14, we use 14x14 since it lets the projectile clip into tiles as it bounces. It looks better.
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.hostile = true;
			Projectile.penetrate = -1;

			// 5 second fuse.
			Projectile.timeLeft = 240;

			// These 2 help the Projectile hitbox be centered on the Projectile sprite.
			//drawOffsetX = 5;
			//drawOriginOffsetY = 5;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.ai[1] != 1)
			{
				Explode();
				Projectile.timeLeft = 2;
			}
			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Projectile.ai[1] != 1)
			{
				Explode();
				Projectile.timeLeft = 2;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.ai[1] != 1)
			{
				Explode();
				Projectile.timeLeft = 2;
			}
		}

		public override void AI()
		{
			// Loop through the 4 animation frames, spending 5 ticks on each.
			if (++Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
			}

			if (Projectile.timeLeft < 3 && Projectile.ai[1] != 1)
			{
				Explode();
			}
			else
			{
				// Smoke and fuse dust spawn.
				if (Main.rand.NextBool())
				{
					int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
					Main.dust[dustIndex].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[dustIndex].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].position = Projectile.Center + new Vector2(0f, (float)(-(float)Projectile.height / 2)).RotatedBy((double)Projectile.rotation, default(Vector2)) * 1.1f;
					dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
					Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].position = Projectile.Center + new Vector2(0f, (float)(-(float)Projectile.height / 2 - 6)).RotatedBy((double)Projectile.rotation, default(Vector2)) * 1.1f;
				}
			}
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > 5f)
			{
				Projectile.ai[0] = 10f;
				// Roll speed dampening.
				if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
				{
					Projectile.velocity.X = Projectile.velocity.X * 0.97f;
					//if (Projectile.type == 29 || Projectile.type == 470 || Projectile.type == 637)
					{
						//Projectile.velocity.X = Projectile.velocity.X * 0.99f;
					}
					if ((double)Projectile.velocity.X > -0.01 && (double)Projectile.velocity.X < 0.01)
					{
						Projectile.velocity.X = 0f;
						Projectile.netUpdate = true;
					}
				}
				Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
			}
			// Rotation increased by velocity.X 
			Projectile.rotation += Projectile.velocity.X * 0.1f;
			return;
		}

		private void Explode()
		{
			Projectile.tileCollide = false;
			// Set to transparent. This Projectile technically lives as  transparent for about 3 frames
			Projectile.alpha = 255;
			// change the hitbox size, centered about the original Projectile center. This makes the Projectile damage enemies during the explosion.
			Projectile.position = Projectile.Center;

			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.Center = Projectile.position;

			Projectile.damage = (int) (Projectile.damage * 1.1);
			Projectile.knockBack = 10f;

			Projectile.ai[1] = 1;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				SoundEngine.PlaySound(SoundID.Item14.SoundId, (int)Projectile.Center.X, (int)Projectile.Center.Y, SoundID.Item14.Style, 0.80f, 0.6f);
			}

			// Smoke Dust spawn
			for (int i = 0; i < 10; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
				Main.dust[dustIndex].velocity *= 1.4f;
			}
			// Fire Dust spawn
			for (int i = 0; i < 18; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity *= 5f;
				dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
				Main.dust[dustIndex].velocity *= 3f;
			}
			// Large Smoke Gore spawn
			/**for (int g = 0; g < 1; g++)
			{
				int goreIndex = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[goreIndex].scale = 1.5f;
				Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
				Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
				goreIndex = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[goreIndex].scale = 1.5f;
				Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
				Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
				goreIndex = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[goreIndex].scale = 1.5f;
				Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
				Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
				goreIndex = Gore.NewGore(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[goreIndex].scale = 1.5f;
				Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
				Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
			}**/
			// reset size to normal width and height.
			Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
		}
	}
}
