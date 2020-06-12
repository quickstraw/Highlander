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
	class WrenchProjectile : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.penetrate = 2;
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

			if (projectile.ai[1] > 2)
			{
				projectile.Kill();
			}

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

			Main.PlaySound(SoundID.Item53.WithPitchVariance(0.2f).WithVolume(0.9f), projectile.position);

			var gore = Gore.NewGoreDirect(projectile.position, projectile.velocity * 0.4f, mod.GetGoreSlot("Gores/WrenchProjectile"), 1f);
			gore.rotation = projectile.rotation;

		}

	}
}
