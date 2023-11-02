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

namespace Highlander.Projectiles
{
	class FeralFrenzyProjectile : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 20;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			SoundEngine.PlaySound(SoundID.Item53 with { PitchVariance = 0.2f, Volume = 0.6f }, Projectile.position);
			Projectile.velocity.Y = -oldVelocity.Y * 0.5f;
			Projectile.velocity.X = oldVelocity.X * 0.5f;
			Projectile.ai[1]++;
			return false;
		}

		public override void AI()
		{
			Projectile.velocity *= 0;

			Projectile.rotation = Projectile.ai[0] + MathHelper.Pi;

			if(Projectile.rotation < MathHelper.PiOver2 || Projectile.rotation > 3 * MathHelper.PiOver2)
			{
				Projectile.spriteDirection = 1;
			}
			else
			{
				Projectile.spriteDirection = -1;
				Projectile.rotation -= MathHelper.Pi;
			}

			if(Projectile.frameCounter < 2)
			{
				Projectile.frame = 0;
			}
			else if(Projectile.frameCounter < 4)
			{
				Projectile.frame = 1;
			}
			else if (Projectile.frameCounter < 6)
			{
				Projectile.frame = 2;
			}
			else if (Projectile.frameCounter < 10)
			{
				Projectile.frame = 3;
			}
			else if (Projectile.frameCounter < 12)
			{
				Projectile.frame = 4;
			}
			else if (Projectile.frameCounter < 14)
			{
				Projectile.frame = 5;
			}
			else if (Projectile.frameCounter < 16)
			{
				Projectile.frame = 6;
			}
			else if (Projectile.frameCounter < 18)
			{
				Projectile.frame = 7;
			}

			Projectile.frameCounter++;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

	}
}
