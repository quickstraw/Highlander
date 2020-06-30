using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.NPCs.HauntedHatter
{
    class GhostBlastCharge : ModProjectile
    {

		private BitsByte flags = new BitsByte();
		private int timer;
		private Vector2 offset;

		public override string Texture => "Highlander/NPCs/HauntedHatter/GhostBlast";

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
			if (!flags[0])
			{
				projectile.scale = 0.10f;
				flags[0] = true;
				offset = projectile.position - owner.position;
				projectile.velocity *= 0;
				timer = 30;
				if (Main.netMode != NetmodeID.Server)
				{
					
					Main.PlaySound(SoundID.Item45.SoundId, (int) projectile.position.X, (int) projectile.position.Y, SoundID.Item45.Style, 0.40f, -0.5f);
					//Main.PlaySound(SoundLoader.customSoundType, (int)projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Charging"));
				}
			}

			if (timer > 0)
			{
				projectile.scale += 0.03f;
				timer--;

				projectile.position = offset + owner.position;

				if (timer <= 0)
				{
					float CoolAngle = (float)Math.Atan2(target.Center.Y - projectile.position.Y, target.Center.X - projectile.position.X) + MathHelper.PiOver2;

					float rotation = CoolAngle - MathHelper.PiOver2;
					Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
					velocity.Normalize();
					velocity *= 8;

					projectile.scale = 1.0f;

					projectile.velocity = velocity;
					projectile.netUpdate = true;
				}
			}
			else
			{
				if (!flags[1])
				{
					flags[1] = true;
					if (Main.netMode != NetmodeID.Server)
					{
						Main.PlaySound(SoundID.Item12, projectile.position);
					}
				}
				projectile.rotation = forward.ToRotation();
			}

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

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(flags);
			writer.Write((short) timer);
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
				float rotation = projectile.rotation - MathHelper.PiOver2;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

		private Player target
		{
			get
			{
				return Main.player[(int) projectile.ai[1]];
			}
		}

		private NPC owner
		{
			get
			{
				return Main.npc[(int) projectile.ai[0]];
			}
		}

	}
}
