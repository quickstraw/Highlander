using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.NPCs.EnlightenmentIdol
{
    class TriangleBlast : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

		public override void SetDefaults()
		{
			projectile.width = 54;
			projectile.height = 54;
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;
			//projectile.penetrate = -1;
			projectile.scale = 0.8f;
			projectile.alpha = 255;
		}

		public override void AI()
		{
			projectile.spriteDirection = projectile.direction;

			if (projectile.spriteDirection == 1) {
				projectile.rotation = projectile.velocity.ToRotation();// + MathHelper.Pi;
			}
			else
			{
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
			}

			// Loop through the 5 animation frames, spending 6 ticks on each.
			if (++projectile.frameCounter >= 6)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}

			if(projectile.alpha > 0)
			{
				if (projectile.alpha - 32 < 0) {
					projectile.alpha = 0;
					projectile.netUpdate = true;
				}
				else
				{
					projectile.alpha -= 32;
				}
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((float)(255 - projectile.alpha) / 255f);
		}

		private Vector2 forward
		{
			get
			{
				float rotation = projectile.rotation - MathHelper.Pi;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

	}
}
