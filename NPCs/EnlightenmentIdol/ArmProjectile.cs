using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.EnlightenmentIdol
{
    class ArmProjectile : ModProjectile
    {

		private Texture2D portalF;
		private Texture2D portalB;
		private Texture2D arm;
		private Vector2 initPos = new Vector2();
		private BitsByte flags;
		private byte timer;
		private byte waitTimer = 20;
		private bool flip;

		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 0;
			projectile.timeLeft = 6000;
			projectile.penetrate = -1;
			//drawOriginOffsetY = -4;
			//drawOffsetX = -4;
			portalF = GetTexture("Highlander/NPCs/EnlightenmentIdol/Portal_Front");
			portalB = GetTexture("Highlander/NPCs/EnlightenmentIdol/Portal_Back");
			arm = GetTexture("Highlander/NPCs/EnlightenmentIdol/ArmProjectile");
			flip = (projectile.rotation > MathHelper.PiOver2 && projectile.rotation < 3 * MathHelper.PiOver2);
		}

		public override void AI()
		{
			flip = projectile.rotation > MathHelper.PiOver2 && projectile.rotation < 3 * MathHelper.PiOver2;

			if (!flags[1])
			{
					flags[1] = true;
					initPos = projectile.position;
					projectile.netUpdate = true;
			}

			if (flags[3])
			{
				projectile.ai[1]--;
				if(projectile.ai[1] <= 0)
				{
					projectile.Kill();
				}
			}
			else if ((projectile.ai[1] >= 10 || flags[2]) && waitTimer <= 0)
			{
				if (!flags[2])
				{
					flags[2] = true;
					projectile.hostile = true;
					projectile.netUpdate = true;
				}

				int drawLength = (int)projectile.ai[0];

				if (drawLength > arm.Width)
				{
					flags[4] = true;
					projectile.velocity *= 0;
					projectile.netUpdate = true;
				}

				if (flags[4])
				{
					timer++;
				}

				if (timer >= 20 || flags[0])
				{
					if (!flags[0])
					{
						flags[0] = true;
						projectile.netUpdate = true;
					}
					projectile.velocity = -forward * 12.0f;

					float length = projectile.velocity.Length();

					

					if (projectile.ai[0] - length <= 0)
					{
						projectile.velocity = -forward * projectile.ai[0];
						projectile.ai[0] = 0;
						flags[3] = true;
						projectile.hostile = false;
						projectile.netUpdate = true;
					}
					else
					{
						projectile.ai[0] -= length;
					}
					
				}
				else if (timer > 0)
				{
					projectile.velocity *= 0;
				}
				else
				{
					//projectile.rotation += 0.01f;
					projectile.velocity = forward * 16;
					//projectile.velocity *= 1.05f;
					projectile.ai[0] += projectile.velocity.Length();
				}
			}
			else if (projectile.ai[1] >= 10 && waitTimer > 0)
			{
				waitTimer--;
			}
			else
			{
				projectile.ai[1]++;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = projectile.position - Main.screenPosition;
			Vector2 portalPos;

			Vector2 armPos = drawPos + new Vector2(projectile.width / 2, projectile.height / 2);

			if(projectile.velocity.LengthSquared() == 0)
			{
				armPos += forward * Main.rand.NextFloat(2, 4);
			}

			int drawLength = (int) projectile.ai[0];

			if(drawLength > arm.Width)
			{
				drawLength = arm.Width;
			}
			if(drawLength <= 0)
			{
				drawLength = 0;
			}

			if (initPos.LengthSquared() != 0)
			{
				portalPos = initPos - Main.screenPosition + new Vector2(projectile.width / 2, projectile.height / 2) + forward * 34;
				spriteBatch.Draw(portalB, portalPos, new Rectangle(0, 0, portalB.Width, portalB.Height), Color.White, projectile.rotation, new Vector2(portalB.Width / 2, portalB.Height / 2), 0.1f + projectile.ai[1] * 9f / 100f, 0, 0);
				if (projectile.ai[1] >= 10 && !flags[3])
				{
					drawArm(spriteBatch, lightColor);//spriteBatch.Draw(arm, armPos, new Rectangle(0, 0, drawLength, arm.Height / 2), lightColor, projectile.rotation, new Vector2(22, 22), 1.0f, 0, 0);
				}
				spriteBatch.Draw(portalF, portalPos, new Rectangle(0, 0, portalF.Width, portalF.Height), Color.White, projectile.rotation, new Vector2(portalF.Width / 2, portalF.Height / 2), 0.1f + projectile.ai[1] * 9f / 100f, 0, 0);
			}
			else
			{
				if (projectile.ai[1] >= 10 && !flags[3])
				{
					spriteBatch.Draw(arm, armPos, new Rectangle(0, 0, drawLength, arm.Height / 2), lightColor, projectile.rotation, new Vector2(22, 22), 1.0f, 0, 0);
				}
			}
			
			return false;
		}

		private void drawArm(SpriteBatch spriteBatch, Color lightColor)
		{
			int sourceX = 0;
			int num = 0;
			float rotation = projectile.rotation;


			Vector2 drawPos = projectile.position - Main.screenPosition;
			Vector2 armPos = drawPos + new Vector2(projectile.width / 2, projectile.height / 2);

			if (projectile.velocity.LengthSquared() == 0)
			{
				armPos += forward * Main.rand.NextFloat(2, 4);
			}

			int drawLength = (int)projectile.ai[0];

			if (drawLength > arm.Width)
			{
				drawLength = arm.Width;
			}
			if (drawLength <= 0)
			{
				drawLength = 0;
			}
			if (drawLength > arm.Width)
			{
				drawLength = arm.Width;
			}

			if (flip)
			{
				num = 1;
				rotation += MathHelper.Pi;
				armPos -= forward * (arm.Width * 0 - 44 + drawLength);
				sourceX = arm.Width - drawLength;
			}

			if (projectile.ai[1] >= 10 && !flags[3])
			{
				spriteBatch.Draw(arm, armPos, new Rectangle(sourceX, num * arm.Height / 2, drawLength, arm.Height / 2), lightColor, rotation, new Vector2(22, 22), 1.0f, 0, 0);
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			//writer.Write(initPos.X);
			//writer.Write(initPos.Y);
			writer.Write(flags);
			writer.Write(timer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			//initPos.X = reader.ReadByte();
			//initPos.Y = reader.ReadByte();
			flags = reader.ReadByte();
			timer = reader.ReadByte();
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

	}
}
