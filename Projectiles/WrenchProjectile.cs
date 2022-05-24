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
	class WrenchProjectile : ModProjectile
	{

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
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
			SoundEngine.PlaySound(SoundID.Item53.WithPitchVariance(0.2f).WithVolume(0.6f), Projectile.position);
			Projectile.velocity.Y = -oldVelocity.Y * 0.5f;
			Projectile.velocity.X = oldVelocity.X * 0.5f;
			Projectile.ai[1]++;
			return false;
		}

		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;
			if(Projectile.ai[0] + 1 >= 15f)
			{
				Projectile.velocity.Y += 0.4f;
			}
			else
			{
				Projectile.ai[0] += 1f;
			}
			if(Projectile.velocity.X > 0)
			{
				Projectile.rotation += 0.2f;
			}
			else
			{
				Projectile.rotation -= 0.2f;
			}

			if (Projectile.ai[1] > 2)
			{
				Projectile.Kill();
			}

		}

		public override void Kill(int timeLeft)
		{
			if(Projectile.ai[1] > 0)
			{
				// reset size to normal width and height.
				Projectile.position.X = Projectile.position.X + (float)(Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y + (float)(Projectile.height / 2);
				Projectile.width = 10;
				Projectile.height = 10;
				Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
				Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
			}

			SoundEngine.PlaySound(SoundID.Item53.WithPitchVariance(0.2f).WithVolume(0.9f), Projectile.position);

			if(Main.netMode != NetmodeID.Server)
            {
				var source = Projectile.GetSource_Death();
				var gore = Gore.NewGoreDirect(source, Projectile.position, Projectile.velocity * 0.4f, Mod.Find<ModGore>("WrenchProjectile").Type, 1f);
				gore.rotation = Projectile.rotation;
			}
		}

	}
}
