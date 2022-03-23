using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.NPCs.HauntedHatter
{
	class HugeGhostBlastCharge : ModProjectile
	{

		private BitsByte flags = new BitsByte();
		private int timer;
		private Vector2 offset;

		public override string Texture => "Highlander/NPCs/HauntedHatter/HugeGhostBlast";

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.width = 52;
			Projectile.height = 52;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
			Projectile.timeLeft = 150;
		}

		public override void AI()
		{
			if (!flags[0])
			{
				Projectile.scale = 0.10f;
				flags[0] = true;
				offset = Projectile.position - owner.position;
				Projectile.velocity *= 0;
				timer = 30;
				if (Main.netMode != NetmodeID.Server)
				{
					SoundEngine.PlaySound(SoundID.Item45.SoundId, (int)Projectile.position.X, (int)Projectile.position.Y, SoundID.Item45.Style, 0.50f, -0.5f);
				}
			}

			if (timer > 0)
			{
				Projectile.scale += 0.03f;
				timer--;

				Projectile.position = offset + owner.position;

				if (timer <= 0)
				{
					float CoolAngle = (float)Math.Atan2(target.Center.Y - Projectile.position.Y, target.Center.X - Projectile.position.X) + MathHelper.PiOver2;

					float rotation = CoolAngle - MathHelper.PiOver2;
					Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
					velocity.Normalize();
					velocity *= 8;

					Projectile.scale = 1.0f;

					Projectile.velocity = velocity;
					Projectile.netUpdate = true;
				}
			}
			else
			{
				if (!flags[1])
				{
					flags[1] = true;
					if (Main.netMode != NetmodeID.Server)
					{
						SoundEngine.PlaySound(SoundID.Item12.SoundId, (int)Projectile.position.X, (int)Projectile.position.Y, SoundID.Item12.Style, 1f, -0.2f);
					}
				}
				Projectile.rotation = forward.ToRotation();
				Projectile.timeLeft--;
			}

			// Loop through the 5 animation frames, spending 6 ticks on each.
			if (++Projectile.frameCounter >= 6)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 5)
				{
					Projectile.frame = 0;
				}
			}

			float strength = 0.9f;

			Lighting.AddLight(Projectile.position + Projectile.velocity * 8, 0.15f * strength, 0.54f * strength, 0.31f * strength);
		}

		// Shoots 8 projectiles when killed.
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				var source = Projectile.GetNPCSource_FromThis();

				int type = ModContent.ProjectileType<GhostBlast>();
				int damage = (int)(Projectile.damage * 0.8333333f);
				var curr = forward;
				for (int i = 0; i < 8; i++)
				{
					Projectile.NewProjectileDirect(source, Projectile.Center, curr * 6, type, damage, 0.5f);
					curr = curr.RotatedBy(MathHelper.PiOver4);
				}
				if (Main.netMode != NetmodeID.Server)
				{
					SoundEngine.PlaySound(SoundID.Item72.SoundId, (int)Projectile.position.X, (int)Projectile.position.Y, SoundID.Item72.Style, 0.60f, -0.2f);
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
			writer.Write((short)timer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			flags = reader.ReadByte();
			timer = reader.ReadInt16();
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

		private Player target
		{
			get
			{
				return Main.player[(int)Projectile.ai[1]];
			}
		}

		private NPC owner
		{
			get
			{
				return Main.npc[(int)Projectile.ai[0]];
			}
		}

	}
}