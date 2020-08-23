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
	class FeralFrenzyProjectile : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.width = 42;
			projectile.height = 42;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.timeLeft = 20;
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
			Main.PlaySound(SoundID.Item53.WithPitchVariance(0.2f).WithVolume(0.6f), projectile.position);
			projectile.velocity.Y = -oldVelocity.Y * 0.5f;
			projectile.velocity.X = oldVelocity.X * 0.5f;
			projectile.ai[1]++;
			return false;
		}

		public override void AI()
		{
			projectile.velocity *= 0;

			projectile.rotation = projectile.ai[0] + MathHelper.Pi;

			if(projectile.rotation < MathHelper.PiOver2 || projectile.rotation > 3 * MathHelper.PiOver2)
			{
				projectile.spriteDirection = 1;
			}
			else
			{
				projectile.spriteDirection = -1;
				projectile.rotation -= MathHelper.Pi;
			}

			if(projectile.frameCounter < 2)
			{
				projectile.frame = 0;
			}
			else if(projectile.frameCounter < 4)
			{
				projectile.frame = 1;
			}
			else if (projectile.frameCounter < 6)
			{
				projectile.frame = 2;
			}
			else if (projectile.frameCounter < 10)
			{
				projectile.frame = 3;
			}
			else if (projectile.frameCounter < 12)
			{
				projectile.frame = 4;
			}
			else if (projectile.frameCounter < 14)
			{
				projectile.frame = 5;
			}
			else if (projectile.frameCounter < 16)
			{
				projectile.frame = 6;
			}
			else if (projectile.frameCounter < 18)
			{
				projectile.frame = 7;
			}

			projectile.frameCounter++;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

	}
}
