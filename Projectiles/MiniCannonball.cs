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
				if(projectile.ai[1] != 1)
				{
					projectile.ai[1] = 1;
					projectile.netUpdate = true;
				}
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

		public override void Kill(int timeLeft)
		{
			var gore = Gore.NewGoreDirect(projectile.position, projectile.velocity * 0.4f, mod.GetGoreSlot("Gores/MiniCannonball"), 1f);
			gore.rotation = projectile.rotation;
			gore.timeLeft = 60;
			//Main.PlaySound(SoundID.Item53.SoundId, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.Item53.Style, 0.9f, -2f);
			if(Main.netMode != NetmodeID.Server)
			{
				Main.PlaySound(SoundID.Item10.SoundId, (int)projectile.Center.X, (int)projectile.Center.Y, SoundID.Item10.Style, 1.0f, -0.8f);
			}
			/**for (int i = 0; i < 5; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 1, 1, ModContent.DustType<MiniCannonballDust>());
				var unit = projectile.velocity;
				unit.Normalize();
				Main.dust[dustIndex].velocity *= 0.8f;
				Main.dust[dustIndex].velocity += -unit * 3;
				Main.dust[dustIndex].scale = 1.6f;
				Main.dust[dustIndex].rotation = (-unit).ToRotation() + MathHelper.PiOver2;
			}**/
			//Point tileCoords = projectile.position.ToTileCoordinates();
			Vector2 tileCoords = new Vector2(projectile.position.X / 16, projectile.position.Y / 16);
			Tile tileSafely = Framing.GetTileSafely((int)tileCoords.X, (int)tileCoords.Y);
			Vector2 norm = projectile.velocity;
			norm.Normalize();
			norm *= 16;
			Vector2 otherTileCoords = new Vector2((projectile.position.X + norm.X) / 16, (projectile.position.Y + norm.Y) / 16);
			Tile otherTile = Framing.GetTileSafely((int)otherTileCoords.X, (int)otherTileCoords.Y);
			if (tileSafely.nactive())
			{
				tileSafely = otherTile;
				if (tileSafely.nactive())
				{
					otherTileCoords = otherTileCoords + (otherTileCoords - tileCoords);
					tileSafely = Framing.GetTileSafely((int)(otherTileCoords.X), (int)otherTileCoords.Y);
				}
			}
			//WorldGen.KillTile_MakeTileDust((int)tileCoords.X, (int)tileCoords.Y, tileSafely);

			for (int i = 0; i < 5; i++)
			{
				int dustIndex = WorldGen.KillTile_MakeTileDust((int)tileCoords.X, (int)tileCoords.Y, tileSafely);
				var unit = projectile.velocity;
				unit.Normalize();
				Main.dust[dustIndex].velocity *= 0.8f;
				Main.dust[dustIndex].velocity += -unit * 3;
				Main.dust[dustIndex].scale = 1.6f;
				Main.dust[dustIndex].rotation = (-unit).ToRotation() + MathHelper.PiOver2;
			}
		}

	}
}
