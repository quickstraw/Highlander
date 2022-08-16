using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.NPCs.HauntedHatter
{
    class GhostBlast : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
			Projectile.timeLeft = 600;
		}

		public override void AI()
		{
			Projectile.rotation = forward.ToRotation();

			// Loop through the 5 animation frames, spending 6 ticks on each.
			if (++Projectile.frameCounter >= 6)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 5)
				{
					Projectile.frame = 0;
				}
			}

			float strength = 0.3f;

			Lighting.AddLight(Projectile.position + Projectile.velocity * 8, 0.15f * strength, 0.54f * strength, 0.31f * strength);
		}

		public override void Kill(int timeLeft)
		{

		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
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
