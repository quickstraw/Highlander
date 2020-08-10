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
    class ArmProjectileNew : ModProjectile
    {

		public override string Texture => "Highlander/NPCs/EnlightenmentIdol/ArmProjectile";

		private Texture2D portalF;
		private Texture2D portalB;
		private Texture2D arm;
		private Vector2 initPos = new Vector2();
		private BitsByte flags;
		private byte stopTimer;
		private byte portalTimer;
		private byte waitTimer;

		private bool flip;

		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 2;
			DisplayName.SetDefault("Fist of the Idol");
		}

		public override void SetDefaults()
		{
			projectile.width = 36;
			projectile.height = 36;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 0;
			//projectile.timeLeft = 180;
			projectile.penetrate = -1;
			portalF = GetTexture("Highlander/NPCs/EnlightenmentIdol/Portal_Front");
			portalB = GetTexture("Highlander/NPCs/EnlightenmentIdol/Portal_Back");
			arm = GetTexture("Highlander/NPCs/EnlightenmentIdol/ArmProjectile");
		}

		public override void AI()
		{
			Init();
			if (projectile.rotation == 0)
			{
				initialized = false;
				Init();
			}

			projectile.active = true;

			flip = projectile.rotation > MathHelper.PiOver2 && projectile.rotation < 3 * MathHelper.PiOver2;

			if (!portal)
			{
				if (portalTimer >= 10)
				{
					portalTimer = 10;
					portal = true;
					projectile.netUpdate = true;
				}
			} else if (waitTimer > 0){
			} else if(waitTimer <= 0 && !startForward) // Arm spawns //
			{
				startForward = true;
				projectile.velocity = forward * 16;
				projectile.hostile = true;
				projectile.netUpdate = true;
				if (Main.netMode != NetmodeID.Server)
				{
					Main.PlaySound(SoundID.Item1.SoundId, (int)projectile.position.X, (int)projectile.position.Y, SoundID.Item1.Style, 0.70f, -0.9f);
				}
			}

			if (startForward && !stopped) // Arm moves forward //
			{
				float length = projectile.velocity.Length();

				if (projectile.ai[0] + length >= arm.Width) // Check if arm should stop
				{
					projectile.velocity = forward * (projectile.ai[0] + length - arm.Width);
					projectile.ai[0] = arm.Width;

					stopped = true;

					projectile.netUpdate = true;
				}
				else
				{
					projectile.ai[0] += length;
				}
			} else if (stopped && stopTimer < 20) // Arm stops //
			{
				if(stopTimer <= 0)
				{
					projectile.velocity *= 0;

					stopTimer++;

					projectile.netUpdate = true;
				}
			} else if (stopTimer >= 20 && !finished) // Arm reverses //
			{
				if (!startReverse) // Initialize reverse
				{
					projectile.velocity = -forward * 12.0f;

					startReverse = true;
					projectile.netUpdate = true;
				}

				float length = projectile.velocity.Length();

				if (projectile.ai[0] - length <= 0) // Check if arm has finished reversing
				{
					projectile.velocity = -forward * projectile.ai[0];
					projectile.ai[0] = 0;
					projectile.hostile = false;
					finished = true;

					projectile.netUpdate = true;
				}
				else
				{
					projectile.ai[0] -= length;
				}
			} else if (finished)
			{
				if (portalTimer <= 0 && projectile.ai[0] == 0)
				{
					projectile.Kill();
				}
			}

			UpdateTimers();
		}

		private void Init()
		{
			if (!initialized)
			{
				initPos = projectile.position;

				initialized = true;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					waitTimer = 40;
				}
				Player target = Main.player[(int)projectile.ai[1]];

				projectile.ai[0] = 0;

				projectile.rotation = (float)Math.Atan2(target.Center.Y - projectile.Center.Y, target.Center.X - projectile.Center.X) + MathHelper.Pi;
				projectile.netUpdate = true;
				if (Main.netMode != NetmodeID.Server)
				{
					Main.PlaySound(SoundID.Item45.SoundId, (int)projectile.position.X, (int)projectile.position.Y, SoundID.Item45.Style, 0.40f, -0.5f);
				}
			}
		}

		private void UpdateTimers()
		{
			if(portalTimer < 10 && !finished)
			{
				portalTimer++;
			} else if (portalTimer > 0 && finished)
			{
				portalTimer--;
			}
			if (!startForward)
			{
				waitTimer--;
			}
			if (stopTimer > 0 && stopTimer < 20)
			{
				stopTimer++;
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
				portalPos = initPos - Main.screenPosition + new Vector2(projectile.width / 2, projectile.height / 2) + forward * 30;
				spriteBatch.Draw(portalB, portalPos, new Rectangle(0, 0, portalB.Width, portalB.Height), Color.White, projectile.rotation, new Vector2(portalB.Width / 2, portalB.Height / 2), 0.1f + portalTimer * 9f / 100f, 0, 0);
				if (true || portalTimer >= 10 && !finished)
				{
					drawArm(spriteBatch, lightColor);//spriteBatch.Draw(arm, armPos, new Rectangle(0, 0, drawLength, arm.Height / 2), lightColor, projectile.rotation, new Vector2(22, 22), 1.0f, 0, 0);
				}
				spriteBatch.Draw(portalF, portalPos, new Rectangle(0, 0, portalF.Width, portalF.Height), Color.White, projectile.rotation, new Vector2(portalF.Width / 2, portalF.Height / 2), 0.1f + portalTimer * 9f / 100f, 0, 0);
			}
			else
			{
				if (portalTimer >= 10 && !flags[3])
				{
					spriteBatch.Draw(arm, armPos, new Rectangle(0, 0, drawLength, arm.Height / 2), lightColor, projectile.rotation, new Vector2(22, 22), 1.0f, 0, 0);
				}
			}
			
			return false;
		}

		public override void Kill(int timeLeft)
		{
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

			if (portalTimer >= 10 && !flags[3])
			{
				spriteBatch.Draw(arm, armPos, new Rectangle(sourceX, num * arm.Height / 2, drawLength, arm.Height / 2), lightColor, rotation, new Vector2(22, 22), 1.0f, 0, 0);
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			//writer.Write(initPos.X);
			//writer.Write(initPos.Y);
			writer.Write(flags);
			writer.Write(stopTimer);
			writer.Write(portalTimer);
			writer.Write(waitTimer);
			//writer.Write((short) (projectile.rotation * 10000));
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			//initPos.X = reader.ReadByte();
			//initPos.Y = reader.ReadByte();
			flags = reader.ReadByte();
			stopTimer = reader.ReadByte();
			portalTimer = reader.ReadByte();
			waitTimer = reader.ReadByte();
			//projectile.rotation = (float)reader.ReadInt16() / 10000f;
			
		}

		private int ClosestPlayerToPoint(Vector2 point)
		{
			int closestPlayer = 0;
			float closestDistanceSquared = float.MaxValue;
			foreach (Player player in Main.player)
			{
				float distanceSquared = Vector2.DistanceSquared(player.position, point);
				if (distanceSquared < closestDistanceSquared)
				{
					closestDistanceSquared = distanceSquared;
					closestPlayer = player.whoAmI;
				}
			}
			if (closestDistanceSquared == float.MaxValue)
			{
				closestPlayer = -1;
			}
			return closestPlayer;
		}

		private bool portal
		{
			get => flags[0];
			set => flags[0] = value;
		}

		private bool stopped
		{
			get => flags[4];
			set => flags[4] = value;
		}

		private bool initialized
		{
			get => flags[1];
			set => flags[1] = value;
		}
		public bool finished
		{
			get => flags[3];
			set => flags[3] = value;
		}

		private bool startForward
		{
			get => flags[5];
			set => flags[5] = value;
		}

		private bool startReverse
		{
			get => flags[6];
			set => flags[6] = value;
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
