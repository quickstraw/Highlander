﻿using Terraria.ID;
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
	class MiniCannonball : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
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
			/**Main.PlaySound(SoundID.Item53.WithPitchVariance(0.2f).WithVolume(0.6f), projectile.position);
			projectile.velocity.Y = -oldVelocity.Y * 0.5f;
			projectile.velocity.X = oldVelocity.X * 0.5f;
			projectile.ai[1]++;
			return false;**/
			return true;
		}

		public override void AI()
		{
			
			projectile.spriteDirection = projectile.direction;
			projectile.ai[0]++;

			if(projectile.ai[0] > 30)
			{
				projectile.velocity *= 0.98f;
				projectile.velocity.Y += 0.3f;
			}
			if(projectile.alpha > 0)
			{
				if (projectile.alpha - 20 < 0)
				{
					projectile.alpha = 0;
				}
				else
				{
					projectile.alpha -= 20;
				}
			}
		}

	}
}
