using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.HauntedHatter
{

	internal class NeedleWormHead : NeedleWorm
	{
		//public override string Texture => "Terraria/NPC_" + NPCID.DiggerHead;
		public override string Texture => "Highlander/NPCs/HauntedHatter/NeedleWormHead";
		private int extension;

		public override void SetDefaults()
		{
			// Head is 10 defence, body 20, tail 30.
			SetWormDefaults();
			NPC.HitSound = SoundID.NPCHit42;
			NPC.DeathSound = SoundID.NPCDeath3;
			NPC.dontTakeDamage = false;
			NPC.scale = 1.1f;
			NPC.defense += 8;
			NPC.aiStyle = -1;
		}

		public override void Init()
		{
			var source = NPC.GetSource_FromAI();
			base.Init();
			head = true;
			Vector2 NewPosition = new Vector2(NPC.Center.X, NPC.Center.Y);
			NewPosition += forward * NPC.width;
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.NewNPC(source, (int)NewPosition.X, (int)NewPosition.Y, ModContent.NPCType<NeedleWormExtension>(), 0, 0, NPC.whoAmI, NPC.whoAmI);
				NPC.netUpdate = true;
			}
		}

		public override void CustomBehavior()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Player target = Main.player[NPC.target];
				float angle = (float)Math.Atan2(target.Center.Y - NPC.Center.Y, target.Center.X - NPC.Center.X) + MathHelper.PiOver2;
				float rotation = NPC.rotation % MathHelper.TwoPi;
				if (!(rotation + MathHelper.PiOver2 + MathHelper.PiOver4 >= angle && rotation - MathHelper.PiOver2 - MathHelper.PiOver4 <= angle))
				{
					NPC.velocity *= 0.99f;
					NPC.netUpdate = true;
				}
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
		{
			Lighting.AddLight(NPC.position + NPC.velocity + forward * NPC.width, 0.36f, 0.24f, 0.40f);
		}

		private Vector2 forward
		{
			get
			{
				float rotation = NPC.rotation - MathHelper.PiOver2;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}
	}

	internal class NeedleWormExtension : NeedleWorm
	{
		public override string Texture => "Highlander/NPCs/HauntedHatter/NeedleWormExtension";

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.npcFrameCount[NPC.type] = 5;
		}

		public override void SetDefaults()
		{
			SetWormDefaults();
			NPC.HitSound = SoundID.NPCHit42;
			NPC.scale = 1.1f;
			NPC.dontTakeDamage = false;
			NPC.defense += 14;
			NPC.dontCountMe = true;
			NPC.aiStyle = -1;
		}

		public override void AI()
		{
			if (!Main.npc[(int)NPC.ai[2]].active)
			{
				NPC.active = false;
			}
			if((int)NPC.ai[3] == 0)
			{
				var source = NPC.GetSource_FromAI();

				if((int)NPC.ai[0] < 4)
				{
					Vector2 NewPosition = new Vector2(NPC.Center.X, NPC.Center.Y);
					NewPosition += forward * NPC.width;
					NPC.NewNPC(source, (int)NewPosition.X, (int)NewPosition.Y, ModContent.NPCType<NeedleWormExtension>(), 0, (int)NPC.ai[0] + 1, NPC.whoAmI, NPC.ai[2]);
				}
				NPC.rotation = Main.npc[(int)NPC.ai[1]].rotation;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[3]++;
					NPC.realLife = Main.npc[(int)NPC.ai[2]].realLife;
					NPC.netUpdate = true;
				}
			}
			else
			{
				NPC.rotation = Main.npc[(int)NPC.ai[2]].rotation;
				NPC.position = Main.npc[(int)NPC.ai[2]].position + forward * NPC.width * (1 + (int) NPC.ai[0]);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.realLife = Main.npc[(int)NPC.ai[2]].realLife;
					NPC.netUpdate = true;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
				NPC.frame.Y = frameHeight * (int)NPC.ai[0];
		}

		private Vector2 forward
		{
			get
			{
				float rotation = NPC.rotation - MathHelper.PiOver2;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

	}

	internal class NeedleWormBody : NeedleWorm
	{
		public override string Texture => "Highlander/NPCs/HauntedHatter/NeedleWormBody";

		public override void SetDefaults()
		{
			SetWormDefaults();
			NPC.damage = (int)(NPC.damage * 0.75);
			NPC.defense += 14;
			NPC.dontCountMe = true;
			NPC.aiStyle = -1;
		}

		public override void Init()
		{
			base.Init();
			/**NPC curr = NPC;
			for(int i = 0; i < 4; i++)
			{
				if((int)curr.ai[1] == (int)NPC.ai[3])
				{
					NPC.dontTakeDamage = false;
					break;
				}
				else
				{
					curr = Main.NPC[(int)curr.ai[1]];
				}
			}**/
			/**if((int)NPC.ai[2] % 3 == 0)
			{
				NPC.dontTakeDamage = false;
			}**/
		}
	}

	internal class NeedleWormTail : NeedleWorm
	{
		public override string Texture => "Highlander/NPCs/HauntedHatter/NeedleWormTail";

		public override void SetDefaults()
		{
			SetWormDefaults();
			NPC.damage = (int)(NPC.damage * 0.75);
			NPC.defense += 20;
			NPC.dontCountMe = true;
			NPC.aiStyle = -1;
		}

		public override void Init()
		{
			base.Init();
			tail = true;
		}
	}

	public abstract class NeedleWorm : Worm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Needle");
		}

		public override void Init()
		{
			minLength = 35;
			maxLength = 40;
			tailType = NPCType<NeedleWormTail>();
			bodyType = NPCType<NeedleWormBody>();
			headType = NPCType<NeedleWormHead>();
			//drawOffsetY = 32;
			flies = true;
			speed = 8.0f;
			turnSpeed = 0.06f;
		}

		public void SetWormDefaults()
		{
			NPC.lifeMax = 1000;
			NPC.width = 14;
			NPC.height = 14;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.damage = 24;
			NPC.defense = 2;
			NPC.knockBackResist = 0;
			NPC.value = 300;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.scale = 1f;
			NPC.behindTiles = false;
			NPC.lavaImmune = true;
			NPC.dontTakeDamage = true;
		}
		public override bool PreKill()
		{
			return false;
		}

	}

	// This abstract class can be used for non splitting worm type NPC.
	public abstract class Worm : ModNPC
	{
		/* ai[0] = follower
		 * ai[1] = following
		 * ai[2] = distanceFromTail
		 * ai[3] = head
		 */
		public bool head;
		public bool tail;
		public int minLength;
		public int maxLength;
		public int headType;
		public int bodyType;
		public int tailType;
		public bool flies = false;
		public bool directional = false;
		public float speed;
		public float turnSpeed;

		public override void AI()
		{
			if (NPC.localAI[1] == 0f)
			{
				NPC.localAI[1] = 1f;
				Init();
			}
			if (headIndex > 0f)
			{
				NPC.realLife = headIndex;
			}
			if (!head && NPC.timeLeft < 300)
			{
				NPC.timeLeft = 300;
			}
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead)
			{
				NPC.TargetClosest(true);
			}
			if (Main.player[NPC.target].dead && NPC.timeLeft > 300)
			{
				NPC.timeLeft = 300;
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				var source = NPC.GetSource_FromAI();

				if (!tail && nextSegment == 0f)
				{
					if (head)
					{
						headIndex = NPC.whoAmI;
						NPC.realLife = NPC.whoAmI;
						NPC.ai[2] = (float)Main.rand.Next(minLength, maxLength + 1);
						nextSegment = NPC.NewNPC(source, (int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), bodyType, NPC.whoAmI);
					}
					else if (NPC.ai[2] > 0f)
					{
						nextSegment = NPC.NewNPC(source, (int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), NPC.type, NPC.whoAmI);
					}
					else
					{
						nextSegment = NPC.NewNPC(source, (int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), tailType, NPC.whoAmI);
					}
					Main.npc[nextSegment].ai[3] = headIndex;
					Main.npc[nextSegment].realLife = NPC.realLife;
					Main.npc[nextSegment].ai[1] = (float)NPC.whoAmI;
					Main.npc[nextSegment].ai[2] = NPC.ai[2] - 1f;
					NPC.netUpdate = true;
				}
				if (!head && (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].type != headType && Main.npc[(int)NPC.ai[1]].type != bodyType))
				{
					NPC.life = 0;
					NPC.HitEffect(0, 10.0);
					NPC.active = false;
				}
				if (!tail && (!Main.npc[nextSegment].active || Main.npc[nextSegment].type != bodyType && Main.npc[nextSegment].type != tailType))
				{
					NPC.life = 0;
					NPC.HitEffect(0, 10.0);
					NPC.active = false;
				}
				if (!NPC.active && Main.netMode == 2)
				{
					NetMessage.SendData(28, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
				}
			}
			int num180 = (int)(NPC.position.X / 16f) - 1;
			int num181 = (int)((NPC.position.X + (float)NPC.width) / 16f) + 2;
			int num182 = (int)(NPC.position.Y / 16f) - 1;
			int num183 = (int)((NPC.position.Y + (float)NPC.height) / 16f) + 2;
			if (num180 < 0)
			{
				num180 = 0;
			}
			if (num181 > Main.maxTilesX)
			{
				num181 = Main.maxTilesX;
			}
			if (num182 < 0)
			{
				num182 = 0;
			}
			if (num183 > Main.maxTilesY)
			{
				num183 = Main.maxTilesY;
			}
			bool currFlying = flies;
			if (!currFlying)
			{
				for (int num184 = num180; num184 < num181; num184++)
				{
					for (int num185 = num182; num185 < num183; num185++)
					{
						if (Main.tile[num184, num185] != null && (Main.tile[num184, num185].HasUnactuatedTile && (Main.tileSolid[(int)Main.tile[num184, num185].BlockType] || Main.tileSolidTop[(int)Main.tile[num184, num185].BlockType] && Main.tile[num184, num185].TileFrameY == 0) || Main.tile[num184, num185].LiquidType > 64))
						{
							Vector2 vector17;
							vector17.X = (float)(num184 * 16);
							vector17.Y = (float)(num185 * 16);
							if (NPC.position.X + (float)NPC.width > vector17.X && NPC.position.X < vector17.X + 16f && NPC.position.Y + (float)NPC.height > vector17.Y && NPC.position.Y < vector17.Y + 16f)
							{
								currFlying = true;
								if (Main.rand.NextBool(100) && NPC.behindTiles && Main.tile[num184, num185].HasUnactuatedTile)
								{
									WorldGen.KillTile(num184, num185, true, true, false);
								}
								if (Main.netMode != NetmodeID.MultiplayerClient && ((int)Main.tile[num184, num185].BlockType) == 2)
								{
									ushort arg_BFCA_0 = ((ushort)Main.tile[num184, num185 - 1].BlockType);
								}
							}
						}
					}
				}
			}
			if (!currFlying && head)
			{
				Rectangle rectangle = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
				int num186 = 1000;
				bool headShouldFly = true;
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active)
					{
						Rectangle rectangle2 = new Rectangle((int)Main.player[i].position.X - num186, (int)Main.player[i].position.Y - num186, num186 * 2, num186 * 2);
						if (rectangle.Intersects(rectangle2))
						{
							headShouldFly = false;
							break;
						}
					}
				}
				if (headShouldFly)
				{
					currFlying = true;
				}
			}
			if (directional)
			{
				if (NPC.velocity.X < 0f)
				{
					NPC.spriteDirection = 1;
				}
				else if (NPC.velocity.X > 0f)
				{
					NPC.spriteDirection = -1;
				}
			}
			float num188 = speed;
			float num189 = turnSpeed;
			Vector2 vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
			float num191 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2);
			float num192 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2);
			num191 = (float)((int)(num191 / 16f) * 16);
			num192 = (float)((int)(num192 / 16f) * 16);
			vector18.X = (float)((int)(vector18.X / 16f) * 16);
			vector18.Y = (float)((int)(vector18.Y / 16f) * 16);
			num191 -= vector18.X;
			num192 -= vector18.Y;
			float num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
			if (NPC.ai[1] > 0f && NPC.ai[1] < (float)Main.npc.Length)
			{
				try
				{
					vector18 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					num191 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector18.X;
					num192 = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - vector18.Y;
				}
				catch
				{
				}
				NPC.rotation = (float)System.Math.Atan2((double)num192, (double)num191) + 1.57f;
				num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
				int num194 = NPC.width;
				num193 = (num193 - (float)num194) / num193;
				num191 *= num193;
				num192 *= num193;
				NPC.velocity = Vector2.Zero;
				NPC.position.X = NPC.position.X + num191;
				NPC.position.Y = NPC.position.Y + num192;
				if (directional)
				{
					if (num191 < 0f)
					{
						NPC.spriteDirection = 1;
					}
					if (num191 > 0f)
					{
						NPC.spriteDirection = -1;
					}
				}
			}
			else
			{
				if (!currFlying)
				{
					NPC.TargetClosest(true);
					NPC.velocity.Y = NPC.velocity.Y + 0.11f;
					if (NPC.velocity.Y > num188)
					{
						NPC.velocity.Y = num188;
					}
					if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.4)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X = NPC.velocity.X - num189 * 1.1f;
						}
						else
						{
							NPC.velocity.X = NPC.velocity.X + num189 * 1.1f;
						}
					}
					else if (NPC.velocity.Y == num188)
					{
						if (NPC.velocity.X < num191)
						{
							NPC.velocity.X = NPC.velocity.X + num189;
						}
						else if (NPC.velocity.X > num191)
						{
							NPC.velocity.X = NPC.velocity.X - num189;
						}
					}
					else if (NPC.velocity.Y > 4f)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X = NPC.velocity.X + num189 * 0.9f;
						}
						else
						{
							NPC.velocity.X = NPC.velocity.X - num189 * 0.9f;
						}
					}
				}
				else
				{
					if (!flies && NPC.behindTiles && NPC.soundDelay == 0)
					{
						float num195 = num193 / 40f;
						if (num195 < 10f)
						{
							num195 = 10f;
						}
						if (num195 > 20f)
						{
							num195 = 20f;
						}
						NPC.soundDelay = (int)num195;
						SoundEngine.PlaySound(SoundID.Roar, NPC.position);
					}
					num193 = (float)System.Math.Sqrt((double)(num191 * num191 + num192 * num192));
					float num196 = System.Math.Abs(num191);
					float num197 = System.Math.Abs(num192);
					float num198 = num188 / num193;
					num191 *= num198;
					num192 *= num198;
					if (ShouldRun())
					{
						bool flag20 = true;
						for (int num199 = 0; num199 < 255; num199++)
						{
							if (Main.player[num199].active && !Main.player[num199].dead && Main.player[num199].ZoneCorrupt)
							{
								flag20 = false;
							}
						}
						if (flag20)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient && (double)(NPC.position.Y / 16f) > (Main.rockLayer + (double)Main.maxTilesY) / 2.0)
							{
								NPC.active = false;
								int num200 = (int)nextSegment;
								while (num200 > 0 && num200 < 200 && Main.npc[num200].active && Main.npc[num200].aiStyle == NPC.aiStyle)
								{
									int num201 = (int)Main.npc[num200].ai[0];
									Main.npc[num200].active = false;
									NPC.life = 0;
									if (Main.netMode == 2)
									{
										NetMessage.SendData(23, -1, -1, null, num200, 0f, 0f, 0f, 0, 0, 0);
									}
									num200 = num201;
								}
								if (Main.netMode == 2)
								{
									NetMessage.SendData(23, -1, -1, null, NPC.whoAmI, 0f, 0f, 0f, 0, 0, 0);
								}
							}
							num191 = 0f;
							num192 = num188;
						}
					}
					bool flag21 = false;
					if (NPC.type == 87)
					{
						if ((NPC.velocity.X > 0f && num191 < 0f || NPC.velocity.X < 0f && num191 > 0f || NPC.velocity.Y > 0f && num192 < 0f || NPC.velocity.Y < 0f && num192 > 0f) && System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y) > num189 / 2f && num193 < 300f)
						{
							flag21 = true;
							if (System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y) < num188)
							{
								NPC.velocity *= 1.1f;
							}
						}
						if (NPC.position.Y > Main.player[NPC.target].position.Y || (double)(Main.player[NPC.target].position.Y / 16f) > Main.worldSurface || Main.player[NPC.target].dead)
						{
							flag21 = true;
							if (System.Math.Abs(NPC.velocity.X) < num188 / 2f)
							{
								if (NPC.velocity.X == 0f)
								{
									NPC.velocity.X = NPC.velocity.X - (float)NPC.direction;
								}
								NPC.velocity.X = NPC.velocity.X * 1.1f;
							}
							else
							{
								if (NPC.velocity.Y > -num188)
								{
									NPC.velocity.Y = NPC.velocity.Y - num189;
								}
							}
						}
					}
					if (!flag21)
					{
						if (NPC.velocity.X > 0f && num191 > 0f || NPC.velocity.X < 0f && num191 < 0f || NPC.velocity.Y > 0f && num192 > 0f || NPC.velocity.Y < 0f && num192 < 0f)
						{
							if (NPC.velocity.X < num191)
							{
								NPC.velocity.X = NPC.velocity.X + num189;
							}
							else
							{
								if (NPC.velocity.X > num191)
								{
									NPC.velocity.X = NPC.velocity.X - num189;
								}
							}
							if (NPC.velocity.Y < num192)
							{
								NPC.velocity.Y = NPC.velocity.Y + num189;
							}
							else
							{
								if (NPC.velocity.Y > num192)
								{
									NPC.velocity.Y = NPC.velocity.Y - num189;
								}
							}
							if ((double)System.Math.Abs(num192) < (double)num188 * 0.2 && (NPC.velocity.X > 0f && num191 < 0f || NPC.velocity.X < 0f && num191 > 0f))
							{
								if (NPC.velocity.Y > 0f)
								{
									NPC.velocity.Y = NPC.velocity.Y + num189 * 2f;
								}
								else
								{
									NPC.velocity.Y = NPC.velocity.Y - num189 * 2f;
								}
							}
							if ((double)System.Math.Abs(num191) < (double)num188 * 0.2 && (NPC.velocity.Y > 0f && num192 < 0f || NPC.velocity.Y < 0f && num192 > 0f))
							{
								if (NPC.velocity.X > 0f)
								{
									NPC.velocity.X = NPC.velocity.X + num189 * 2f;
								}
								else
								{
									NPC.velocity.X = NPC.velocity.X - num189 * 2f;
								}
							}
						}
						else
						{
							if (num196 > num197)
							{
								if (NPC.velocity.X < num191)
								{
									NPC.velocity.X = NPC.velocity.X + num189 * 1.1f;
								}
								else if (NPC.velocity.X > num191)
								{
									NPC.velocity.X = NPC.velocity.X - num189 * 1.1f;
								}
								if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5)
								{
									if (NPC.velocity.Y > 0f)
									{
										NPC.velocity.Y = NPC.velocity.Y + num189;
									}
									else
									{
										NPC.velocity.Y = NPC.velocity.Y - num189;
									}
								}
							}
							else
							{
								if (NPC.velocity.Y < num192)
								{
									NPC.velocity.Y = NPC.velocity.Y + num189 * 1.1f;
								}
								else if (NPC.velocity.Y > num192)
								{
									NPC.velocity.Y = NPC.velocity.Y - num189 * 1.1f;
								}
								if ((double)(System.Math.Abs(NPC.velocity.X) + System.Math.Abs(NPC.velocity.Y)) < (double)num188 * 0.5)
								{
									if (NPC.velocity.X > 0f)
									{
										NPC.velocity.X = NPC.velocity.X + num189;
									}
									else
									{
										NPC.velocity.X = NPC.velocity.X - num189;
									}
								}
							}
						}
					}
				}
				NPC.rotation = (float)System.Math.Atan2((double)NPC.velocity.Y, (double)NPC.velocity.X) + 1.57f;
				if (head)
				{
					if (currFlying)
					{
						if (NPC.localAI[0] != 1f)
						{
							NPC.netUpdate = true;
						}
						NPC.localAI[0] = 1f;
					}
					else
					{
						if (NPC.localAI[0] != 0f)
						{
							NPC.netUpdate = true;
						}
						NPC.localAI[0] = 0f;
					}
					if ((NPC.velocity.X > 0f && NPC.oldVelocity.X < 0f || NPC.velocity.X < 0f && NPC.oldVelocity.X > 0f || NPC.velocity.Y > 0f && NPC.oldVelocity.Y < 0f || NPC.velocity.Y < 0f && NPC.oldVelocity.Y > 0f) && !NPC.justHit)
					{
						NPC.netUpdate = true;
						return;
					}
				}
			}
			CustomBehavior();
		}

		public virtual void Init()
		{
		}

		public virtual bool ShouldRun()
		{
			return false;
		}

		public virtual void CustomBehavior()
		{
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return head ? (bool?)null : false;
		}

		/// <summary>
		/// Holds the NPC index of the next segment.
		/// </summary>
		public int nextSegment
		{
			get => (int)NPC.ai[0];
			set => NPC.ai[0] = value;
		}

		/// <summary>
		/// Holds the NPC index of the head.
		/// </summary>
		public int headIndex
		{
			get => (int)NPC.ai[3];
			set => NPC.ai[3] = value;
		}
	}

}
