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
	class MiniCannonball : ModProjectile
	{

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
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
			/**Main.PlaySound(SoundID.Item53.WithPitchVariance(0.2f).WithVolume(0.6f), Projectile.position);
			Projectile.velocity.Y = -oldVelocity.Y * 0.5f;
			Projectile.velocity.X = oldVelocity.X * 0.5f;
			Projectile.ai[1]++;
			return false;**/
			return true;
		}

		public override void AI()
		{
			
			Projectile.spriteDirection = Projectile.direction;
			Projectile.ai[0]++;

			if(Projectile.ai[0] > 30)
			{
				Projectile.velocity *= 0.98f;
				Projectile.velocity.Y += 0.3f;
				if(Projectile.ai[1] != 1)
				{
					Projectile.ai[1] = 1;
					Projectile.netUpdate = true;
				}
			}
			if(Projectile.alpha > 0)
			{
				if (Projectile.alpha - 20 < 0)
				{
					Projectile.alpha = 0;
				}
				else
				{
					Projectile.alpha -= 20;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			var gore = Gore.NewGoreDirect(Projectile.position, Projectile.velocity * 0.4f, Mod.Find<ModGore>("Gores/MiniCannonball").Type, 1f);
			gore.rotation = Projectile.rotation;
			gore.timeLeft = 60;
			//Main.PlaySound(SoundID.Item53.SoundId, (int)Projectile.Center.X, (int)Projectile.Center.Y, SoundID.Item53.Style, 0.9f, -2f);
			if(Main.netMode != NetmodeID.Server)
			{
				SoundEngine.PlaySound(SoundID.Item10.SoundId, (int)Projectile.Center.X, (int)Projectile.Center.Y, SoundID.Item10.Style, 1.0f, -0.8f);
			}
			/**for (int i = 0; i < 5; i++)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 1, 1, ModContent.DustType<MiniCannonballDust>());
				var unit = Projectile.velocity;
				unit.Normalize();
				Main.dust[dustIndex].velocity *= 0.8f;
				Main.dust[dustIndex].velocity += -unit * 3;
				Main.dust[dustIndex].scale = 1.6f;
				Main.dust[dustIndex].rotation = (-unit).ToRotation() + MathHelper.PiOver2;
			}**/
			//Point tileCoords = Projectile.position.ToTileCoordinates();
			Vector2 tileCoords = new Vector2(Projectile.position.X / 16, Projectile.position.Y / 16);
			Tile tileSafely = Framing.GetTileSafely((int)tileCoords.X, (int)tileCoords.Y);
			Vector2 norm = Projectile.velocity;
			norm.Normalize();
			norm *= 16;
			Vector2 otherTileCoords = new Vector2((Projectile.position.X + norm.X) / 16, (Projectile.position.Y + norm.Y) / 16);
			Tile otherTile = Framing.GetTileSafely((int)otherTileCoords.X, (int)otherTileCoords.Y);
			if (tileSafely.HasUnactuatedTile)
			{
				tileSafely = otherTile;
				if (tileSafely.HasUnactuatedTile)
				{
					otherTileCoords = otherTileCoords + (otherTileCoords - tileCoords);
					tileSafely = Framing.GetTileSafely((int)(otherTileCoords.X), (int)otherTileCoords.Y);
				}
			}
			//WorldGen.KillTile_MakeTileDust((int)tileCoords.X, (int)tileCoords.Y, tileSafely);

			for (int i = 0; i < 5; i++)
			{
				int dustIndex = WorldGen.KillTile_MakeTileDust((int)tileCoords.X, (int)tileCoords.Y, tileSafely);
				var unit = Projectile.velocity;
				unit.Normalize();
				Main.dust[dustIndex].velocity *= 0.8f;
				Main.dust[dustIndex].velocity += -unit * 3;
				Main.dust[dustIndex].scale = 1.6f;
				Main.dust[dustIndex].rotation = (-unit).ToRotation() + MathHelper.PiOver2;
			}
		}

	}
}
