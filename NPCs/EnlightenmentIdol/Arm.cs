using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.EnlightenmentIdol
{
    class Arm : ModNPC
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
			Main.npcFrameCount[NPC.type] = 2; // make sure to set this for your modnpcs.
			DisplayName.SetDefault("Fist of the Idol");
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.lifeMax = 20;
			NPC.width = 36;
			NPC.height = 36;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			//NPC.alpha = 255;
			//NPC.timeLeft = 180;
			NPC.damage = 48;
			NPC.knockBackResist = 0f;
			NPC.dontTakeDamage = true;
			portalF = Request<Texture2D>("Highlander/NPCs/EnlightenmentIdol/Portal_Front").Value;
			portalB = Request<Texture2D>("Highlander/NPCs/EnlightenmentIdol/Portal_Back").Value;
			arm = Request<Texture2D>("Highlander/NPCs/EnlightenmentIdol/ArmProjectile").Value;
		}

		public override void AI()
		{
			Init();
			if (NPC.rotation == 0)
			{
				initialized = false;
				Init();
			}

			flip = NPC.rotation > MathHelper.PiOver2 && NPC.rotation < 3 * MathHelper.PiOver2;

			if (!portal)
			{
				if (portalTimer >= 10)
				{
					portalTimer = 10;
					portal = true;
					NPC.netUpdate = true;
				}
			} else if (waitTimer > 0){
			} else if(waitTimer <= 0 && !startForward) // Arm spawns //
			{
				startForward = true;
				NPC.velocity = forward * 16;
				//NPC.hostile = true;
				NPC.netUpdate = true;
				if (Main.netMode != NetmodeID.Server)
				{
					//SoundEngine.PlaySound(SoundID.Item1.SoundId, (int)NPC.position.X, (int)NPC.position.Y, SoundID.Item1.Style, 0.70f, -0.9f);
					SoundEngine.PlaySound(SoundID.Item1 with { Volume = 0.70f, Pitch = -0.9f}, NPC.position);
				}
			}

			if (startForward && !stopped) // Arm moves forward //
			{
				float length = NPC.velocity.Length();

				if (NPC.ai[0] + length >= arm.Width) // Check if arm should stop
				{
					NPC.velocity = forward * (NPC.ai[0] + length - arm.Width);
					NPC.ai[0] = arm.Width;

					stopped = true;

					NPC.netUpdate = true;
				}
				else
				{
					NPC.ai[0] += length;
				}
			} else if (stopped && stopTimer < 20) // Arm stops //
			{
				if(stopTimer <= 0)
				{
					NPC.velocity *= 0;

					stopTimer++;

					NPC.netUpdate = true;
				}
			} else if (stopTimer >= 20 && !finished) // Arm reverses //
			{
				if (!startReverse) // Initialize reverse
				{
					NPC.velocity = -forward * 12.0f;

					startReverse = true;
					NPC.netUpdate = true;
				}

				float length = NPC.velocity.Length();

				if (NPC.ai[0] - length <= 0) // Check if arm has finished reversing
				{
					NPC.velocity = -forward * NPC.ai[0];
					NPC.ai[0] = 0;
					//NPC.hostile = false;
					finished = true;

					NPC.netUpdate = true;
				}
				else
				{
					NPC.ai[0] -= length;
				}
			} else if (finished)
			{
				if (portalTimer <= 0 && NPC.ai[0] == 0)
				{
					NPC.active = false;
					NPC.netUpdate = true;
				}
			}

			UpdateTimers();
		}

		private void Init()
		{
			if (!initialized)
			{
				initPos = NPC.position;

				initialized = true;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					waitTimer = 40;
				}
				Player target = Main.player[(int)NPC.ai[1]];

				NPC.ai[0] = 0;

				NPC.rotation = (float)Math.Atan2(target.Center.Y - NPC.Center.Y, target.Center.X - NPC.Center.X) + MathHelper.Pi;
				NPC.netUpdate = true;
				if (Main.netMode != NetmodeID.Server)
				{
					//SoundEngine.PlaySound(SoundID.Item45.SoundId, (int)NPC.position.X, (int)NPC.position.Y, SoundID.Item45.Style, 0.40f, -0.5f);
					SoundEngine.PlaySound(SoundID.Item45 with { Volume = 0.40f, Pitch = -0.5f });
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

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Vector2 drawPos = NPC.position - screenPos;
			Vector2 portalPos;

			Vector2 armPos = drawPos + new Vector2(NPC.width / 2, NPC.height / 2);

			if(NPC.velocity.LengthSquared() == 0)
			{
				armPos += forward * Main.rand.NextFloat(2, 4);
			}

			int drawLength = (int) NPC.ai[0];

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
				portalPos = initPos - screenPos + new Vector2(NPC.width / 2, NPC.height / 2) + forward * 30;
				spriteBatch.Draw(portalB, portalPos, new Rectangle(0, 0, portalB.Width, portalB.Height), Color.White, NPC.rotation, new Vector2(portalB.Width / 2, portalB.Height / 2), 0.1f + portalTimer * 9f / 100f, 0, 0);
				if (true || portalTimer >= 10 && !finished)
				{
					drawArm(spriteBatch, lightColor);//spriteBatch.Draw(arm, armPos, new Rectangle(0, 0, drawLength, arm.Height / 2), lightColor, NPC.rotation, new Vector2(22, 22), 1.0f, 0, 0);
				}
				spriteBatch.Draw(portalF, portalPos, new Rectangle(0, 0, portalF.Width, portalF.Height), Color.White, NPC.rotation, new Vector2(portalF.Width / 2, portalF.Height / 2), 0.1f + portalTimer * 9f / 100f, 0, 0);
			}
			else
			{
				if (portalTimer >= 10 && !flags[3])
				{
					spriteBatch.Draw(arm, armPos, new Rectangle(0, 0, drawLength, arm.Height / 2), lightColor, NPC.rotation, new Vector2(22, 22), 1.0f, 0, 0);
				}
			}
			
			return false;
		}

		private void drawArm(SpriteBatch spriteBatch, Color lightColor)
		{
			int sourceX = 0;
			int num = 0;
			float rotation = NPC.rotation;


			Vector2 drawPos = NPC.position - Main.screenPosition;
			Vector2 armPos = drawPos + new Vector2(NPC.width / 2, NPC.height / 2);

			if (NPC.velocity.LengthSquared() == 0)
			{
				armPos += forward * Main.rand.NextFloat(2, 4);
			}

			int drawLength = (int)NPC.ai[0];

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
				spriteBatch.Draw(arm, armPos, new Rectangle(sourceX, num * arm.Height / 2, drawLength, arm.Height / 2), Color.White, rotation, new Vector2(22, 22), 1.0f, 0, 0);
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
			//writer.Write((short) (NPC.rotation * 10000));
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			//initPos.X = reader.ReadByte();
			//initPos.Y = reader.ReadByte();
			flags = reader.ReadByte();
			stopTimer = reader.ReadByte();
			portalTimer = reader.ReadByte();
			waitTimer = reader.ReadByte();
			//NPC.rotation = (float)reader.ReadInt16() / 10000f;
			
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
				float rotation = NPC.rotation - MathHelper.Pi;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

	}
}
