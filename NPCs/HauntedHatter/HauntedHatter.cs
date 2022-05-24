using Highlander.Items.HauntedHatter;
using Highlander.Items.Weapons;
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
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.HauntedHatter
{
    [AutoloadBossHead]
    class HauntedHatter : ModNPC
    {
        private const float EXPERT_DAMAGE = 0.8f;
        private const int NEEDLE_DAMAGE = 18;
        private const int BALL_DAMAGE = 22;
        private const int BLAST_DAMAGE = 14;
        private const int UpdateRate = 30;
        private const int DASH_DEFAULT = 600;
        private const int BASE_DEF = 14;

        Vector2 leftHand = new Vector2(-62, -15);
        Vector2 rightHand = new Vector2(62, -15);
        Vector2 currHand;

        private int moveTimer = UpdateRate;
        private int dashTimer = DASH_DEFAULT;
        private int deathTimer = 0;
        private byte alpha = 255;
        private float dashDistance;
        private float attackTimer2 = 60;
        private float attackTimer3 = 60;
        private byte leftArmTimer = 0;
        private byte rightArmTimer = 0;
        private byte leftFrame = 0;
        private byte rightFrame = 0;
        private BitsByte flags = new BitsByte();
        private Texture2D Hat;
        private Texture2D Bottom;
        private Texture2D Left;
        private Texture2D Right;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2; // make sure to set this for your modnpcs.
        }

        public override void SetDefaults()
        {
            //NPC.frame = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);
            DrawOffsetY = 0;// -52;
            NPC.aiStyle = -1;
            NPC.lifeMax = 3800;
            NPC.damage = 20;
            NPC.defense = BASE_DEF;
            NPC.knockBackResist = 0f;
            NPC.width = 60;
            NPC.height = 120;
            NPC.npcSlots = 50f;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 60000;
            Music = MusicID.Boss3;
            //musicPriority = MusicPriority.BossMedium;
            //bossBag = ItemType<HauntedHatterBag>();
            Hat = Request<Texture2D>("Highlander/NPCs/HauntedHatter/HauntedHatterHat").Value;
            Bottom = Request<Texture2D>("Highlander/NPCs/HauntedHatter/HauntedHatterBottom").Value;
            Left = Request<Texture2D>("Highlander/NPCs/HauntedHatter/HauntedHatterLeft").Value;
            Right = Request<Texture2D>("Highlander/NPCs/HauntedHatter/HauntedHatterRight").Value;
            currHand = leftHand;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.7f);
        }


        public override void AI()
        {
            NPC.velocity *= 0.85f;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest(false);
                if (!initialized)
                {
                    initialized = true;

                    //NPC.NewNPC((int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<HauntedHatterHat>(), 0, 0, 0, NPC.whoAmI);
                    //NPC.NewNPC((int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<HauntedHatterBottom>(), 0, 0, 0, NPC.whoAmI);
                    NPC.netUpdate = true;
                }
            }
            if (NPC.HasValidTarget)
            {
                if(deathTimer != 0)
                {
                    deathTimer = 0;
                    alpha = 255;
                    NPC.netUpdate = true;
                }
                float percentHealth = ((float)NPC.life / (float)NPC.lifeMax);
                switch (stage)
                {
                    // Phase One:
                    case 0:
                        HandleMoving();
                        HandlePhaseOneShooting();

                        if (percentHealth < 0.95f && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            stage++;
                            NPC.netUpdate = true;
                        }
                        break;
                    case 1:
                        HandleMoving();
                        HandlePhaseOneShooting();

                        if (percentHealth < 0.85f && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            stage++;
                            NPC.netUpdate = true;
                        }
                        break;
                    case 2:
                        HandleMoving();
                        HandlePhaseOneShooting();

                        if (percentHealth < 0.70f && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            stage++;
                            NPC.netUpdate = true;
                        }
                        break;
                    case 3:
                        HandleMoving();
                        HandlePhaseOneShooting();

                        if (percentHealth < 0.50f)
                        {
                            ExplodeTop();
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                stage++;
                                NPC.netUpdate = true;
                            }
                        }
                        break;
                    // End Phase One; Start Phase Two:
                    case 4:
                        if (altTimer >= 60 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            smiling = true;
                            altTimer = 0;
                            stage++;
                            NPC.netUpdate = true;
                            break;
                        }
                        UpdateDefense();
                        altTimer += 1;
                        break;
                    // Phase Two:
                    case 5:
                        HandleMoving();
                        UpdateDefense();
                        HandlePhaseTwoShooting();
                        if (percentHealth < 0.35f && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            stage++;
                            NPC.netUpdate = true;
                        }
                        break;
                    case 6:
                        HandleMoving();
                        UpdateDefense();
                        HandlePhaseTwoShooting();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                deathTimer++;
                if(deathTimer > 240)
                {
                    alpha -= 2;
                }
                if(deathTimer > 360)
                {
                    NPC.active = false;
                    NPC.netUpdate = true;
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                FindPostFrame();

                // Move timer
                if (leftArm && leftArmTimer < 11)
                {
                    leftArmTimer++;
                }
                else if (!leftArm && leftArmTimer > 0)
                {
                    leftArmTimer--;
                }

                // Move timer
                if (rightArm && rightArmTimer < 11)
                {
                    rightArmTimer++;
                }
                else if (!rightArm && rightArmTimer > 0)
                {
                    rightArmTimer--;
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                dashTimer--;
                moveTimer--;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frameHeight;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 drawPos = NPC.position - screenPos;

            int HatFrameHeight = Hat.Height / 8;
            int HatFrame = 0;
            int BottomFrameHeight = Bottom.Height / 6;
            int BottomFrame = 0;
            int LeftFrameHeight = Left.Height / 5;
            int RightFrameHeight = Right.Height / 5;

            BottomFrame = (int)(BottomFrameHeight * (int)(NPC.frameCounter / 10));

            if (smiling)
            {
                HatFrame = (int)(HatFrameHeight * 1);

                if (shooting)
                {
                    if (altTimer >= 42)
                    {
                        HatFrame = HatFrameHeight * 1;
                    }
                    else if (altTimer >= 36)
                    {
                        HatFrame = HatFrameHeight * 7;
                    }
                    else if (altTimer >= 30)
                    {
                        HatFrame = HatFrameHeight * 6;
                    }
                    else if (altTimer >= 24)
                    {
                        HatFrame = HatFrameHeight * 6;
                    }
                    else if (altTimer >= 18)
                    {
                        HatFrame = HatFrameHeight * 5;
                    }
                    else if (altTimer >= 12)
                    {
                        HatFrame = HatFrameHeight * 4;
                    }
                    else if (altTimer >= 6)
                    {
                        HatFrame = HatFrameHeight * 3;
                    }
                    else if (altTimer >= 0)
                    {
                        HatFrame = HatFrameHeight * 2;
                    }
                }
            }
            else
            {
                HatFrame = 0;

                if(altTimer > 0)
                {
                    HatFrame = (int)(BottomFrameHeight * 1);
                }
            }

            Vector2 HatOrigin = new Vector2(Hat.Width / 2, HatFrameHeight / 2);
            Vector2 BottomOrigin = new Vector2(Bottom.Width / 2, BottomFrameHeight / 2);
            Vector2 LeftOrigin = new Vector2(Left.Width / 2, LeftFrameHeight / 2);

            Vector2 HatDrawPos = drawPos + new Vector2(30, 60);
            Vector2 BottomDrawPos = drawPos + new Vector2(30, 60);

            spriteBatch.Draw(Hat, HatDrawPos, new Rectangle(0, HatFrame, Hat.Width, HatFrameHeight), drawColor * ((float) alpha / 255f), NPC.rotation, HatOrigin, 1.0f, 0, 0);
            spriteBatch.Draw(Bottom, BottomDrawPos, new Rectangle(0, BottomFrame, Bottom.Width, BottomFrameHeight), drawColor * ((float) alpha / 255f), NPC.rotation, BottomOrigin, 1.0f, 0, 0);
            spriteBatch.Draw(Left, BottomDrawPos, new Rectangle(0, leftFrame * LeftFrameHeight, Left.Width, LeftFrameHeight), drawColor * ((float) alpha / 255f), NPC.rotation, LeftOrigin, 1.0f, 0, 0);
            spriteBatch.Draw(Right, BottomDrawPos, new Rectangle(0, rightFrame * RightFrameHeight, Right.Width, RightFrameHeight), drawColor * ((float) alpha / 255f), NPC.rotation, LeftOrigin, 1.0f, 0, 0);

            NPC.frameCounter = (NPC.frameCounter + 1.75) % 60;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            base.HitEffect(hitDirection, damage);
        }

        private void HandlePhaseOneShooting()
        {
            int ghostBlastTime = 90;

            switch (stage)
            {
                case 0:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(120f, 8f);
                        NPC.netUpdate = true;
                        attackTimer += 15;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        GhostBlast(false);
                        NPC.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(30);
                    }
                    break;
                case 1:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(105f, 16f);
                        NPC.netUpdate = true;
                        attackTimer += 10;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        GhostBlast(false);
                        NPC.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(30);
                    }
                    break;
                case 2:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(105f, 16f);
                        NPC.netUpdate = true;
                        attackTimer += 10;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        GhostBlast(false);
                        NPC.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(30);
                    }
                    break;
                case 3:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(105f, 16f);
                        NPC.netUpdate = true;
                        attackTimer += 10;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        GhostBlast(false);
                        NPC.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(30);
                    }
                    break;
            }

            if(attackTimer2 <= ghostBlastTime - 26)
            {
                leftArm = false;
                rightArm = false;
            }

            attackTimer -= 1;
            attackTimer2 -= 1;

            if(attackTimer2 <= 20)
            {
                if (TargetDirection())
                {
                    currHand = rightHand;
                    rightArm = true;
                }
                else
                {
                    currHand = leftHand;
                    leftArm = true;
                }
            }
        }

        private void HandlePhaseTwoShooting()
        {
            int ghostBlastTime = 90;

            switch (stage)
            {
                case 5:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(360f + Main.rand.NextFloat(0, 60), 16f);
                        //ShootDoubleNeedle(180f, 16f);
                        NPC.netUpdate = true;
                        attackTimer += 15;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        HugeGhostBlast();
                        NPC.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(30);
                    }
                    HandleShootingYarn(240);
                    break;
                case 6:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(360f + Main.rand.NextFloat(0, 60), 16f);
                        //ShootDoubleNeedle(180f, 16f);
                        NPC.netUpdate = true;
                        attackTimer += 10;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        HugeGhostBlast();
                        NPC.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(30);
                    }
                    HandleShootingYarn(240);
                    break;
            }

            if (attackTimer2 == ghostBlastTime - 26)
            {
                leftArm = false;
                rightArm = false;
                NPC.netUpdate = true;
            }

            attackTimer -= 1;
            attackTimer2 -= 1;
            attackTimer3 -= 1;

            if (attackTimer2 == 20)
            {
                if (TargetDirection())
                {
                    currHand = rightHand;
                    rightArm = true;
                }
                else
                {
                    currHand = leftHand;
                    leftArm = true;
                }
                NPC.netUpdate = true;
            }
        }

        private void HandleShootingYarn(int time)
        {
            if (attackTimer3 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                shooting = true;
                NPC.netUpdate = true;
            }
            if (!shot && shooting && altTimer > 26 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                canShoot = true;
                NPC.netUpdate = true;
            }
            if (canShoot && Main.netMode != NetmodeID.MultiplayerClient)
            {
                shot = true;
                canShoot = false;
                ShootYarn();
                attackTimer3 += time + Main.rand.Next(60);
                NPC.netUpdate = true;
            }
            if (shooting && altTimer > 42 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                shot = false;
                shooting = false;
                altTimer = 0;
                NPC.netUpdate = true;
            }
            if (shooting)
            {
                altTimer += 1;
            }
        }

        private void HandleMoving()
        {
            if (dashTimer <= 0)
            {
                if (dashTimer == 0)
                {
                    Player[] players = Main.player.ToArray();
                    Vector2 distance = players[NPC.target].Center - NPC.Center;
                    dashDistance = distance.X;
                }
                Dash();
                if (dashTimer <= -20 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    dashTimer = DASH_DEFAULT;
                }
            }
            else
            {
                NormalMove();
            }
        }

        private void NormalMove()
        {
            if (moveTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest(false);
                NPC.netUpdate = true;
                moveTimer = UpdateRate;
            }

            Player[] players = Main.player.ToArray();
            Vector2 distance = players[NPC.target].Center - NPC.Center;

            // Stay above player
            if (Math.Abs(distance.Y) < NPC.height * 1.75f)
            {
                float yValue = (1 - Math.Abs(distance.Y) / (NPC.height * 1.75f));
                if (yValue > 0.2f)
                {
                    yValue = 0.2f;
                }
                NPC.velocity.Y -= yValue;
            } else if (distance.Y > NPC.height * 2.0f)
            {
                float yValue = (1 - distance.Y / (NPC.height * 2.0f));
                if (yValue > 0.5f)
                {
                    yValue = 0.5f;
                }
                NPC.velocity.Y -= yValue;
            }

            if (distance.Length() > 360)
            {
                distance /= distance.Length();
                NPC.velocity += distance;
            }
            else if (distance.Length() < 180)
            {
                distance /= distance.Length();
                NPC.velocity -= distance / 2;
            }

            // Lean when moving
            if (NPC.velocity.X < -1 && NPC.rotation >= -0.15f)
            {
                NPC.rotation -= 0.01f;
            } else if (NPC.velocity.X > 1 && NPC.rotation <= 0.15f)
            {
                NPC.rotation += 0.01f;
            } else
            {
                if (Math.Abs(NPC.rotation) > 0.05)
                {
                    NPC.rotation *= 0.9f;
                }
                else
                {
                    NPC.rotation = 0;
                }
            }
        }

        private void Dash()
        {
            if (moveTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest(false);
                NPC.netUpdate = true;
            }

            Player[] players = Main.player.ToArray();
            Vector2 distance = players[NPC.target].Center - NPC.Center;

            // Stay above player
            if (Math.Abs(distance.Y) < NPC.height * 1.75f)
            {
                float yValue = (1 - Math.Abs(distance.Y) / (NPC.height * 1.75f));
                if (yValue > 0.2f)
                {
                    yValue = 0.2f;
                }
                NPC.velocity.Y -= yValue;
            }
            else if (distance.Y > NPC.height * 2.0f)
            {
                float yValue = (1 - distance.Y / (NPC.height * 2.0f));
                if (yValue > 0.5f)
                {
                    yValue = 0.5f;
                }
                NPC.velocity.Y -= yValue;
            }

            //Dash
            //Player on Right
            if (dashDistance >= 0 && distance.X > -360)
            {
                NPC.velocity.X += 5 * (dashDistance / 360);
            }
            //Player on Left
            else if (dashDistance < 0 && distance.X < 360)
            {
                NPC.velocity.X += 5 * (dashDistance / 360);
            }

            if (distance.Length() > 360)
            {
                distance /= distance.Length();
                NPC.velocity += distance;
            }
            else if (distance.Length() < 180)
            {
                distance /= distance.Length();
                NPC.velocity -= distance / 2;
            }

            // Lean when moving
            if (NPC.velocity.X < -1 && NPC.rotation >= -0.3f)
            {
                NPC.rotation -= 0.01f;
            }
            else if (NPC.velocity.X > 1 && NPC.rotation <= 0.3f)
            {
                NPC.rotation += 0.01f;
            }
            else
            {
                if (Math.Abs(NPC.rotation) > 0.05)
                {
                    NPC.rotation *= 0.9f;
                }
                else
                {
                    NPC.rotation = 0;
                }
            }
        }

        private void ExplodeTop()
        {
            Vector2 goreVelocity = new Vector2(0, -10);
            if (NPC.life % 2 == 0)
            {
                goreVelocity.X = 3;
            }
            else
            {
                goreVelocity.X = -3;
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, goreVelocity, Mod.Find<ModGore>("HauntedHatterTopGore").Type, 1f);
            }

            // Code adapted from ExampleMod
            // Fire Dust spawn
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y) + NPC.velocity, NPC.width, 8, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y) + NPC.velocity, NPC.width, 8, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
            // Large Smoke Gore spawn
            int goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X, NPC.position.Y) + NPC.velocity, default(Vector2), Main.rand.Next(61, 64), 1f);
            Main.gore[goreIndex].scale = 1f;
            Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
            Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
            goreIndex = Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.Center.X, NPC.position.Y) + NPC.velocity, default(Vector2), Main.rand.Next(61, 64), 1f);
            Main.gore[goreIndex].scale = 1f;
            Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
            Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;

            if (Main.netMode != NetmodeID.Server)
            {
                SoundEngine.PlaySound(SoundID.Item14.WithPitchVariance(0.1f).WithVolume(1.1f), new Vector2(NPC.Center.X, NPC.Center.Y + NPC.height / 2));
            }
        }

        private void ShootNeedle(float timeBetweenVolleys, float maxOffset)
        {
            var source = NPC.GetSource_FromAI();
            Vector2 velocity = new Vector2(0, 0); // Dummy gets overriden in projectile.
            int type = ModContent.ProjectileType<SewingNeedle>();
            float offsetX = 0;
            float offsetY = 0;

            float targetOffset = 0;

            switch (firePosition)
            {
                case 0:
                    offsetX = -NPC.width * 1.5f;
                    firePosition = (firePosition + 1) % 3;
                    targetOffset = -maxOffset;
                    break;
                case 1:
                    offsetY = -NPC.height * 1f;
                    firePosition = (firePosition + 1) % 3;
                    break;
                case 2:
                    offsetX = NPC.width * 1.5f;
                    firePosition = (firePosition + 1) % 3;
                    targetOffset = maxOffset;
                    attackTimer += timeBetweenVolleys;
                    break;
                default:
                    firePosition = 0;
                    break;
            }

            offsetY -= NPC.height * 0.5f;

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(NEEDLE_DAMAGE * expertMult);

            Vector2 origin = new Vector2(NPC.Center.X + NPC.velocity.X + offsetX, NPC.Center.Y + NPC.velocity.Y + offsetY);

            var projectile = Projectile.NewProjectileDirect(source, origin, velocity, type, damage, 0.5f);
            projectile.ai[1] = targetOffset;
            projectile.ai[2] = NPC.target;
            projectile.ai[3] = NPC.whoAmI;
        }

        private void ShootDoubleNeedle(float timeBetweenVolleys, float maxOffset)
        {
            var source = NPC.GetSource_FromAI();
            Vector2 velocity = new Vector2(0, 0); // Dummy gets overriden in projectile.
            int type = ModContent.ProjectileType<SewingNeedle>();

            Vector2 point0 = new Vector2(NPC.Center.X - NPC.velocity.X - NPC.width * 2.2f, NPC.Center.Y - NPC.velocity.Y - NPC.height * 1);
            Vector2 point1 = new Vector2(NPC.Center.X + NPC.velocity.X + NPC.width * 2.2f, NPC.Center.Y - NPC.velocity.Y - NPC.height * 1);

            float offsetX0 = 0;
            float offsetY0 = 0;

            float offsetX1 = 0;
            float offsetY1 = 0;

            float targetOffset0 = 0;
            float targetOffset1 = 0;

            switch (firePosition)
            {
                case 0:
                    offsetX0 = -NPC.width * 0.8f;
                    offsetX1 = NPC.width * 0.8f;
                    firePosition = (firePosition + 1) % 3;
                    targetOffset0 = -maxOffset;
                    targetOffset1 = maxOffset;
                    break;
                case 1:
                    offsetY0 = -NPC.height * 0.8f;
                    offsetY1 = -NPC.height * 0.8f;
                    firePosition = (firePosition + 1) % 3;
                    break;
                case 2:
                    offsetX0 = NPC.width * 0.8f;
                    offsetX1 = -NPC.width * 0.8f;
                    firePosition = (firePosition + 1) % 3;
                    targetOffset0 = +maxOffset;
                    targetOffset1 = -maxOffset;
                    attackTimer += timeBetweenVolleys;
                    break;
                default:
                    firePosition = 0;
                    break;
            }

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(NEEDLE_DAMAGE * expertMult);

            Vector2 origin0 = new Vector2(point0.X + offsetX0, point0.Y + offsetY0);
            Vector2 origin1 = new Vector2(point1.X + offsetX1, point1.Y + offsetY1);

            var projectile0 = Projectile.NewProjectileDirect(source, origin0, velocity, type, damage, 0.5f);
            projectile0.ai[1] = targetOffset0;
            projectile0.ai[2] = ClosestPlayerToPoint(point0);
            projectile0.ai[3] = NPC.whoAmI;

            var projectile1 = Projectile.NewProjectileDirect(source, origin1, velocity, type, damage, 0.5f);
            projectile1.ai[1] = targetOffset1;
            projectile1.ai[2] = ClosestPlayerToPoint(point1);
            projectile1.ai[3] = NPC.whoAmI;
        }

        private void ShootManyNeedle(float timeBetweenVolleys, float maxOffset)
        {
            var source = NPC.GetSource_FromAI();
            Vector2 velocity = new Vector2(0, 0); // Dummy gets overriden in projectile.
            int type = ModContent.ProjectileType<SewingNeedle>();
            float offsetX = 0;
            float offsetY = 0;

            float targetOffset = 0;

            switch (firePosition)
            {
                case 0:
                    offsetX = -NPC.width * 1.5f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset = -maxOffset;
                    break;
                case 1:
                    offsetX = -NPC.width * 1.0f;
                    offsetY = -NPC.height * 0.7f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset = -maxOffset * 0.5f;
                    break;
                case 2:
                    offsetY = -NPC.height * 1f;
                    firePosition = (firePosition + 1) % 5;
                    break;
                case 3:
                    offsetX = NPC.width * 1.0f;
                    offsetY = -NPC.height * 0.7f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset = maxOffset * 0.5f;
                    break;
                case 4:
                    offsetX = NPC.width * 1.5f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset = maxOffset;
                    attackTimer += timeBetweenVolleys;
                    break;
                default:
                    firePosition = 0;
                    break;
            }

            offsetY -= NPC.height * 0.5f;

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(NEEDLE_DAMAGE * expertMult);

            Vector2 origin = new Vector2(NPC.Center.X + NPC.velocity.X + offsetX, NPC.Center.Y + NPC.velocity.Y + offsetY);

            var projectile = Projectile.NewProjectileDirect(source, origin, velocity, type, damage, 0.5f);
            projectile.ai[1] = targetOffset;
            projectile.ai[2] = NPC.target;
            projectile.ai[3] = NPC.whoAmI;
        }

        private void ShootManyManyNeedle(float timeBetweenVolleys, float maxOffset)
        {
            var source = NPC.GetSource_FromAI();
            Vector2 velocity = new Vector2(0, 0); // Dummy gets overriden in projectile.
            int type = ModContent.ProjectileType<SewingNeedle>();

            Vector2 point0 = new Vector2(NPC.Center.X - NPC.velocity.X - NPC.width * 2.2f, NPC.Center.Y - NPC.velocity.Y - NPC.height * 1);
            Vector2 point1 = new Vector2(NPC.Center.X + NPC.velocity.X + NPC.width * 2.2f, NPC.Center.Y - NPC.velocity.Y - NPC.height * 1);

            float offsetX0 = 0;
            float offsetY0 = 0;

            float offsetX1 = 0;
            float offsetY1 = 0;

            float targetOffset0 = 0;
            float targetOffset1 = 0;

            switch (firePosition)
            {
                case 0:
                    offsetX0 = -NPC.width * 0.8f;
                    offsetX1 = NPC.width * 0.8f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset0 = -maxOffset;
                    targetOffset1 = maxOffset;
                    break;
                case 1:
                    offsetX0 = NPC.width * 0.8f;
                    offsetX1 = -NPC.width * 0.8f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset0 = +maxOffset;
                    targetOffset1 = -maxOffset;
                    break;
                case 2:
                    offsetX0 = -NPC.width * 0.6f;
                    offsetX1 = NPC.width * 0.6f;
                    offsetY0 = NPC.height * 0.8f;
                    offsetY1 = NPC.height * 0.8f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset0 = maxOffset * 0.5f;
                    targetOffset1 = -maxOffset * 0.5f;
                    break;
                case 3:
                    offsetY0 = -NPC.height * 0.8f;
                    offsetY1 = -NPC.height * 0.8f;
                    firePosition = (firePosition + 1) % 5;
                    break;
                case 4:
                    offsetX0 = NPC.width * 0.6f;
                    offsetX1 = -NPC.width * 0.6f;
                    offsetY0 = NPC.height * 0.8f;
                    offsetY1 = NPC.height * 0.8f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset0 = -maxOffset * 0.5f;
                    targetOffset1 = maxOffset * 0.5f;
                    attackTimer += timeBetweenVolleys;
                    break;
                default:
                    firePosition = 0;
                    break;
            }

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(NEEDLE_DAMAGE * expertMult);

            Vector2 origin0 = new Vector2(point0.X + offsetX0, point0.Y + offsetY0);
            Vector2 origin1 = new Vector2(point1.X + offsetX1, point1.Y + offsetY1);

            var projectile0 = Projectile.NewProjectileDirect(source, origin0, velocity, type, damage, 0.5f);
            projectile0.ai[1] = targetOffset0;
            projectile0.ai[2] = ClosestPlayerToPoint(point0);
            projectile0.ai[3] = NPC.whoAmI;

            var projectile1 = Projectile.NewProjectileDirect(source, origin1, velocity, type, damage, 0.5f);
            projectile1.ai[1] = targetOffset1;
            projectile1.ai[2] = ClosestPlayerToPoint(point1);
            projectile1.ai[3] = NPC.whoAmI;

        }

        private void ShootYarn()
        {
            var source = NPC.GetSource_FromAI();
            int type = ModContent.ProjectileType<EnchantedYarn>();

            Player player = Main.player[NPC.target];
            float diff = player.Center.X - NPC.Center.X;
            float xVelocity = 0;
            if (diff > 0)
            {
                xVelocity += 3;
            }
            else
            {
                xVelocity -= 3;
            }

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(BALL_DAMAGE * expertMult);

            Vector2 velocity = new Vector2(xVelocity, -12);
            Vector2 origin = new Vector2(NPC.Center.X, NPC.position.Y + 26);

            var projectile = Projectile.NewProjectileDirect(source, origin, velocity, type, damage, 0.5f);
        }

        private void GhostBlast()
        {
            var source = NPC.GetSource_FromAI();
            int type = ModContent.ProjectileType<GhostBlast>();

            Vector2 leftHand = new Vector2(-60, -10) + NPC.Center;

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(BLAST_DAMAGE * expertMult);

            Player target = Main.player[NPC.target];

            float CoolAngle = (float)Math.Atan2(target.Center.Y - leftHand.Y, target.Center.X - leftHand.X) + MathHelper.PiOver2;

            float rotation = CoolAngle - MathHelper.PiOver2;
            Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            velocity.Normalize();
            velocity *= 8;

            var projectile = Projectile.NewProjectileDirect(source, leftHand + NPC.velocity + new Vector2(0, -8), velocity, type, damage, 0.5f);

            SoundEngine.PlaySound(SoundID.Item12, projectile.position);
        }

        private void GhostBlast(bool isRight)
        {
            var source = NPC.GetSource_FromAI();
            int type = ModContent.ProjectileType<GhostBlastCharge>();
            
            Vector2 hand = currHand + NPC.Center;

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(BLAST_DAMAGE * expertMult);

            int id = NPC.whoAmI;
            var projectile = Projectile.NewProjectileDirect(source, hand, new Vector2(), type, damage, 0.5f, 255, id, NPC.target);
            projectile.ai[0] = id;
        }

        private void HugeGhostBlast()
        {
            var source = NPC.GetSource_FromAI();
            int type = ModContent.ProjectileType<HugeGhostBlastCharge>();

            Vector2 hand = currHand + NPC.Center + new Vector2(0, -20);

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(BLAST_DAMAGE * 1.2f * expertMult);

            int id = NPC.whoAmI;
            var projectile = Projectile.NewProjectileDirect(source, hand, new Vector2(), type, damage, 0.5f, 255, id, NPC.target);
            projectile.ai[0] = id;
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

        /// <summary>
        /// Returns true if right and false if left
        /// </summary>
        /// <returns></returns>
        private bool TargetDirection()
        {
            Player player = Main.player[NPC.target];
            if(player.position.X > NPC.Center.X)
            {
                return true;
            }
            return false;
        }

        private void FindPostFrame()
        {
            byte newLeftFrame = 0;
            byte newRightFrame = 0;

            // Change Frames based on Timer
            if (leftArmTimer > 0)
            {
                if (leftArmTimer > 10)
                {
                    newLeftFrame = 4;
                }
                else if (leftArmTimer >= 7)
                {
                    newLeftFrame = 3;
                }
                else if (leftArmTimer >= 4)
                {
                    newLeftFrame = 2;
                }
                else if (leftArmTimer >= 1)
                {
                    newLeftFrame = 1;
                }
            }

            if (rightArmTimer > 0)
            {
                if (rightArmTimer > 10)
                {
                    newRightFrame = 4;
                }
                else if (rightArmTimer >= 7)
                {
                    newRightFrame = 3;
                }
                else if (rightArmTimer >= 4)
                {
                    newRightFrame = 2;
                }
                else if (rightArmTimer >= 1)
                {
                    newRightFrame = 1;
                }
            }

            if(newRightFrame != rightFrame || newLeftFrame != leftFrame)
            {
                NPC.netUpdate = true;
                rightFrame = newRightFrame;
                leftFrame = newLeftFrame;
            }

        }

        private void UpdateDefense()
        {
            //bool alive0 = Main.NPC[spawns[0]].life > 0;
            //bool alive1 = Main.NPC[spawns[1]].life > 0;

            //int def0 = alive0 ? 1 : 0;
            //int def1 = alive1 ? 1 : 0;

            //NPC.defense = BASE_DEF + 8 * (def0 + def1);
        }

        private int stage
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private float attackTimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        private int firePosition
        {
            get => (int)NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        public bool smiling
        {
            get => flags[0];
            set => flags[0] = value;
        }

        private bool shooting
        {
            get => flags[1];
            set => flags[1] = value;
        }
        private bool canShoot
        {
            get => flags[2];
            set => flags[2] = value;
        }

        private bool shot
        {
            get => flags[3];
            set => flags[3] = value;
        }

        private bool initialized
        {
            get => flags[4];
            set => flags[4] = value;
        }

        private bool leftArm
        {
            get => flags[5];
            set => flags[5] = value;
        }

        private bool rightArm
        {
            get => flags[6];
            set => flags[6] = value;
        }

        private Vector2 ghostHead {
            get => NPC.position + new Vector2(70, 20);
        }

        /// <summary>
        /// False is left. True is right.
        /// </summary>
        private bool dashDirection
        {
            get => flags[4];
            set => flags[4] = value;
        }

        internal float altTimer
        {
            get => NPC.ai[3];
            private set => NPC.ai[3] = value;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((short)moveTimer);
            writer.Write((short)dashTimer);
            writer.Write(flags);
            writer.Write((short)attackTimer2);
            writer.Write((short)attackTimer3);
            writer.Write(leftArmTimer);
            writer.Write(rightArmTimer);
            writer.Write(leftFrame);
            writer.Write(rightFrame);
            writer.Write((short)deathTimer);
            writer.Write(alpha);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            moveTimer = reader.ReadInt16();
            dashTimer = reader.ReadInt16();
            flags = reader.ReadByte();
            attackTimer2 = reader.ReadInt16();
            attackTimer3 = reader.ReadInt16();
            leftArmTimer = reader.ReadByte();
            rightArmTimer = reader.ReadByte();
            leftFrame = reader.ReadByte();
            rightFrame = reader.ReadByte();
            deathTimer = reader.ReadInt16();
            alpha = reader.ReadByte();
        }

        public override void OnKill()
        {
            if (!HighlanderWorld.downedHauntedHatter)
            {
                HighlanderWorld.downedHauntedHatter = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ItemType<HauntedHatterBag>()));

            npcLoot.Add(ItemDropRule.Common(ItemType<HauntedHatterTrophy>(), 10));

            // All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            // Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            // Boss masks are spawned with 1/7 chance
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<GhostlyGibus>(), 7));
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ItemType<SpiritShears>(), ItemType<AncientStoneBlaster>()));
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Haunted Hatter";
            potionType = ItemID.HealingPotion;
        }


    }

}
