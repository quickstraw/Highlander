using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.NPCs.HauntedHatter
{
	class HugeGhostBlast : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 52;
			projectile.height = 52;
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 0;
			projectile.timeLeft = 150;
		}

		public override void AI()
		{

			// Loop through the 5 animation frames, spending 6 ticks on each.
			if (++projectile.frameCounter >= 6)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 5)
				{
					projectile.frame = 0;
				}
			}

			float strength = 0.9f;
			projectile.timeLeft--;

			Lighting.AddLight(projectile.position + projectile.velocity * 8, 0.15f * strength, 0.54f * strength, 0.31f * strength);
		}

		// Shoots 8 projectiles when killed.
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int type = ModContent.ProjectileType<GhostBlast>();
				int damage = (int)(projectile.damage * 0.8333333f);
				var curr = forward;
				for (int i = 0; i < 8; i++)
				{
					Projectile.NewProjectileDirect(projectile.Center, curr * 6, type, damage, 0.5f);
					curr = curr.RotatedBy(MathHelper.PiOver4);
				}
			}
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