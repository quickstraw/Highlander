using Highlander.NPCs.EnlightenmentIdol;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.EnlightenmentIdol
{
	class PortalCharge : ModProjectile
	{

		private BitsByte flags = new BitsByte();

		public override string Texture => "Highlander/NPCs/EnlightenmentIdol/Portal_Front";

		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 0;
			projectile.timeLeft = 20;
		}

		public override void AI()
		{
			if (!flags[0])
			{
				flags[0] = true;
				projectile.velocity *= 0;
				timer = 0;
				projectile.rotation = (float)Math.Atan2(target.Center.Y - projectile.Center.Y, target.Center.X - projectile.Center.X) + MathHelper.Pi;
				if (Main.netMode != NetmodeID.Server)
				{
					Main.PlaySound(SoundID.Item45.SoundId, (int)projectile.position.X, (int)projectile.position.Y, SoundID.Item45.Style, 0.40f, -0.5f);
				}
			}

			if (timer < 10)
			{
				timer++;
			}
			else
			{
				//projectile.timeLeft--;
			}
		}

		// Shoots arm when killed.
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int type = ModContent.ProjectileType<ArmProjectileCharge>();
				int damage = (int)(projectile.damage);
				var curr = forward;

				Projectile.NewProjectileDirect(projectile.Center, curr * 16, type, damage, 0.5f);

				if (Main.netMode != NetmodeID.Server)
				{
					Main.PlaySound(SoundID.Item1.SoundId, (int)projectile.position.X, (int)projectile.position.Y, SoundID.Item1.Style, 0.70f, -0.9f);
				}
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(flags);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			flags = reader.ReadByte();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 portalPos = projectile.position - Main.screenPosition + new Vector2(projectile.width / 2, projectile.height / 2) + forward * 30;

			Texture2D portalF = GetTexture("Highlander/NPCs/EnlightenmentIdol/Portal_Front");
			Texture2D portalB = GetTexture("Highlander/NPCs/EnlightenmentIdol/Portal_Back");

			spriteBatch.Draw(portalB, portalPos, new Rectangle(0, 0, portalB.Width, portalB.Height), Color.White, projectile.rotation, new Vector2(portalB.Width / 2, portalB.Height / 2), 0.1f + timer * 9f / 100f, 0, 0);
			spriteBatch.Draw(portalF, portalPos, new Rectangle(0, 0, portalF.Width, portalF.Height), Color.White, projectile.rotation, new Vector2(portalF.Width / 2, portalF.Height / 2), 0.1f + timer * 9f / 100f, 0, 0);

			return false;
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

		private Player target
		{
			get
			{
				return Main.player[(int)projectile.ai[1]];
			}
		}

		private float timer
		{
			get
			{
				return (int)projectile.ai[0];
			}
			set => projectile.ai[0] = value;
		}

	}
}