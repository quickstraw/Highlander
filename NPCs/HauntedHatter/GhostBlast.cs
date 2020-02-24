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
            Main.projFrames[projectile.type] = 5;
        }

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 0;
			projectile.timeLeft = 600;
		}

		public override void AI()
		{
			projectile.rotation = forward.ToRotation();

			// Loop through the 5 animation frames, spending 6 ticks on each.
			if (++projectile.frameCounter >= 6)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 5)
				{
					projectile.frame = 0;
				}
			}

			float strength = 0.3f;

			Lighting.AddLight(projectile.position + projectile.velocity * 8, 0.15f * strength, 0.54f * strength, 0.31f * strength);
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
				float rotation = projectile.rotation - MathHelper.PiOver2;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

	}
}
