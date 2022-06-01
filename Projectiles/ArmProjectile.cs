using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Projectiles
{
	class ArmProjectile : ModProjectile
	{

		public override string Texture => "Highlander/NPCs/EnlightenmentIdol/ArmProjectile";

		private Texture2D portalF;
		private Texture2D portalB;
		private Texture2D arm;
		private Vector2 initPos = new Vector2();
		private BitsByte flags;
		private byte stopTimer;
		private byte portalTimer;
		private float rotation;

		//private bool flip;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			DisplayName.SetDefault("Fist of the Idol");
		}

		public override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.friendly = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
			//Projectile.timeLeft = 180;
			Projectile.penetrate = -1;
			portalF = Request<Texture2D>("Highlander/Projectiles/ArmProjectilePortal_Front").Value;
			portalB = Request<Texture2D>("Highlander/Projectiles/ArmProjectilePortal_Back").Value;
			arm = Request<Texture2D>("Highlander/Projectiles/ArmProjectile").Value;
		}

		public override void AI()
		{
			Init();

			Projectile.rotation = rotation;

			Projectile.active = true;

			if (!portal)
			{
				if (portalTimer >= 10)
				{
					portalTimer = 10;
					portal = true;
					Projectile.netUpdate = true;
				}
			}
			else if (portal && !startForward) // Arm spawns //
			{
				startForward = true;
				Projectile.velocity = forward * 16;
				Projectile.friendly = true;
				Projectile.netUpdate = true;
				if (Main.netMode != NetmodeID.Server)
				{
					//SoundEngine.PlaySound(SoundID.Item1.SoundId, (int)Projectile.position.X, (int)Projectile.position.Y, SoundID.Item1.Style, 0.70f, -0.9f);
					SoundEngine.PlaySound(SoundID.Item1 with { Volume = 0.70f, Pitch = -0.9f }, Projectile.position);
				}
			}
			if (startForward && !stopped) // Arm moves forward //
			{
				float length = Projectile.velocity.Length();

				if (Projectile.ai[0] + length >= arm.Width) // Check if arm should stop
				{
					Projectile.velocity = forward * (Projectile.ai[0] + length - arm.Width);
					Projectile.ai[0] = arm.Width;

					stopped = true;

					Projectile.netUpdate = true;
				}
				else
				{
					Projectile.ai[0] += length;
				}
			}
			else if (stopped && stopTimer < 20) // Arm stops //
			{
				if (stopTimer <= 0)
				{
					Projectile.velocity *= 0;

					stopTimer++;

					Projectile.netUpdate = true;
				}
			}
			else if (stopTimer >= 20 && !finished) // Arm reverses //
			{
				if (!startReverse) // Initialize reverse
				{
					Projectile.velocity = -forward * 12.0f;

					startReverse = true;
					Projectile.netUpdate = true;
				}

				float length = Projectile.velocity.Length();

				if (Projectile.ai[0] - length <= 0) // Check if arm has finished reversing
				{
					Projectile.velocity = -forward * Projectile.ai[0];
					Projectile.ai[0] = 0;
					Projectile.friendly = false;
					finished = true;

					Projectile.netUpdate = true;
				}
				else
				{
					Projectile.ai[0] -= length;
				}
			}
			else if (finished)
			{
				if (portalTimer <= 0 && Projectile.ai[0] == 0)
				{
					Projectile.Kill();
				}
			}

			UpdateTimers();
		}

		private void Init()
		{
			if (!initialized)
			{
				initPos = Projectile.position;

				initialized = true;

				Projectile.ai[0] = 0;

				Vector2 target = Main.MouseWorld;
				Projectile.rotation = (float)Math.Atan2(target.Y - Projectile.Center.Y, target.X - Projectile.Center.X) + MathHelper.Pi;
				rotation = Projectile.rotation;
				Projectile.netUpdate = true;
				if (Main.netMode != NetmodeID.Server)
				{
					//SoundEngine.PlaySound(SoundID.Item45.SoundId, (int)Projectile.position.X, (int)Projectile.position.Y, SoundID.Item45.Style, 0.40f, -0.5f);
					SoundEngine.PlaySound(SoundID.Item45 with { Volume = 0.40f, Pitch = -0.5f }, Projectile.position);
				}

				if(Projectile.rotation != 0)
				{
					if (Main.netMode != NetmodeID.SinglePlayer)
					{
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
					}
				}
				flip = rotation > MathHelper.PiOver2 && rotation < 3 * MathHelper.PiOver2;
			}
		}

		private void UpdateTimers()
		{
			if (portalTimer < 10 && !finished)
			{
				portalTimer++;
			}
			else if (portalTimer > 0 && finished)
			{
				portalTimer--;
			}
			if (stopTimer > 0 && stopTimer < 20)
			{
				stopTimer++;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawPos = Projectile.position - Main.screenPosition;
			Vector2 portalPos;

			Vector2 armPos = drawPos + new Vector2(Projectile.width / 2, Projectile.height / 2);

			if (Projectile.velocity.LengthSquared() == 0)
			{
				armPos += forward * Main.rand.NextFloat(2, 4);
			}

			int drawLength = (int)Projectile.ai[0];

			if (drawLength > arm.Width)
			{
				drawLength = arm.Width;
			}
			if (drawLength <= 0)
			{
				drawLength = 0;
			}

			if (initPos.LengthSquared() != 0)
			{
				portalPos = initPos - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.height / 2) + forward * 30;
				Main.EntitySpriteDraw(portalB, portalPos, new Rectangle(0, 0, portalB.Width, portalB.Height), Color.White, Projectile.rotation, new Vector2(portalB.Width / 2, portalB.Height / 2), 0.1f + portalTimer * 9f / 100f, 0, 0);
				if (true || portalTimer >= 10 && !finished)
				{
					drawArm(lightColor);//spriteBatch.Draw(arm, armPos, new Rectangle(0, 0, drawLength, arm.Height / 2), lightColor, Projectile.rotation, new Vector2(22, 22), 1.0f, 0, 0);
				}
				Main.EntitySpriteDraw(portalF, portalPos, new Rectangle(0, 0, portalF.Width, portalF.Height), Color.White, Projectile.rotation, new Vector2(portalF.Width / 2, portalF.Height / 2), 0.1f + portalTimer * 9f / 100f, 0, 0);
			}
			else
			{
				if (portalTimer >= 10 && !flags[3])
				{
					drawArm(lightColor);
					//spriteBatch.Draw(arm, armPos, new Rectangle(0, 0, drawLength, arm.Height / 2), lightColor, Projectile.rotation, new Vector2(22, 22), 1.0f, 0, 0);
				}
			}

			return false;
		}

		public override void Kill(int timeLeft)
		{
		}

		private void drawArm(Color lightColor)
		{
			int sourceX = 0;
			int num = 0;
			float rotation = Projectile.rotation;


			Vector2 drawPos = Projectile.position - Main.screenPosition;
			Vector2 armPos = drawPos + new Vector2(Projectile.width / 2, Projectile.height / 2);

			if (Projectile.velocity.LengthSquared() == 0)
			{
				armPos += forward * Main.rand.NextFloat(2, 4);
			}

			int drawLength = (int)Projectile.ai[0];

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

			if (flip || (rotation > MathHelper.PiOver2 && rotation < 3 * MathHelper.PiOver2))
			{
				num = 1;
				rotation += MathHelper.Pi;
				armPos -= forward * (arm.Width * 0 - 44 + drawLength);
				sourceX = arm.Width - drawLength;
			}

			if (portalTimer >= 10 && !flags[3])
			{
				Main.EntitySpriteDraw(arm, armPos, new Rectangle(sourceX, num * arm.Height / 2, drawLength, arm.Height / 2), Color.White, rotation, new Vector2(22, 22), 1.0f, 0, 0);
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			//writer.Write(initPos.X);
			//writer.Write(initPos.Y);
			writer.Write(flags);
			writer.Write(stopTimer);
			writer.Write(portalTimer);
			writer.WriteVector2(initPos);
			writer.Write(rotation);
			//writer.Write((short) (Projectile.rotation * 10000));
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			//initPos.X = reader.ReadByte();
			//initPos.Y = reader.ReadByte();
			flags = reader.ReadByte();
			stopTimer = reader.ReadByte();
			portalTimer = reader.ReadByte();
			initPos = reader.ReadVector2();

			float newRot = reader.ReadSingle();

			if (Projectile.rotation == 0 || Projectile.rotation < 0.01f)
			{
				rotation = newRot;
			}
			if (Projectile.velocity.LengthSquared() > 0)
			{
				float velRot = (Projectile.velocity.ToRotation() + MathHelper.Pi) % MathHelper.TwoPi;
				float velRotRev = (velRot + MathHelper.Pi) % MathHelper.TwoPi;
				if (Math.Abs(velRot - newRot) < 0.05f || Math.Abs(velRotRev - newRot) < 0.05f)
				{
					if(startForward && !startReverse)
					{
						rotation = velRot;
					}
					else if(startReverse)
					{
						rotation = velRotRev;
					}
				}
			}
			
			//Projectile.rotation = (float)reader.ReadInt16() / 10000f;
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

		private bool flip
		{
			get => flags[7];
			set => flags[7] = value;
		}

		private Vector2 forward
		{
			get
			{
				float rotation = Projectile.rotation - MathHelper.Pi;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

	}
}
