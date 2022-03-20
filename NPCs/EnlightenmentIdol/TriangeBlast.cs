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
            Main.projFrames[Projectile.type] = 4;
        }

		public override void SetDefaults()
		{
			Projectile.width = 54;
			Projectile.height = 54;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 600;
			//projectile.penetrate = -1;
			Projectile.scale = 0.8f;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;

			if (Projectile.spriteDirection == 1) {
				Projectile.rotation = Projectile.velocity.ToRotation();// + MathHelper.Pi;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
			}

			// Loop through the 5 animation frames, spending 6 ticks on each.
			if (++Projectile.frameCounter >= 6)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
			}

			if(Projectile.alpha > 0)
			{
				if (Projectile.alpha - 32 < 0) {
					Projectile.alpha = 0;
					Projectile.netUpdate = true;
				}
				else
				{
					Projectile.alpha -= 32;
				}
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((float)(255 - Projectile.alpha) / 255f);
		}

		private Vector2 forward
		{
			get
			{
				float rotation = Projectile.rotation - MathHelper.Pi;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

	}
}
