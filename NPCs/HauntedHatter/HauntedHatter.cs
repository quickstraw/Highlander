﻿using Highlander.Items.HauntedHatter;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.HauntedHatter
{
    [AutoloadBossHead]
    class HauntedHatter : ModNPC
    {
        private const float EXPERT_DAMAGE = 0.75f;
        private const int NEEDLE_DAMAGE = 18;
        private const int BALL_DAMAGE = 22;
        private const int BLAST_DAMAGE = 14;
        private const int UpdateRate = 30;
        private const int DASH_DEFAULT = 600;
        private const int BASE_DEF = 14;
        private int moveTimer = UpdateRate;
        private int dashTimer = DASH_DEFAULT;
        private float dashDistance;
        private float attackTimer2 = 60;
        private float attackTimer3 = 60;
        private BitsByte flags = new BitsByte();

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 12; // make sure to set this for your modnpcs.
        }

        public override void SetDefaults()
        {
            //npc.frame = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);
            drawOffsetY = 0;// -52;
            npc.aiStyle = -1;
            npc.lifeMax = 3800;
            npc.damage = 20;
            npc.defense = BASE_DEF;
            npc.knockBackResist = 0f;
            npc.width = 80;
            npc.height = 120;
            npc.npcSlots = 50f;
            npc.boss = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 100000;
            music = MusicID.Boss1;
            musicPriority = MusicPriority.BossMedium;
            bossBag = ItemType<HauntedHatterBag>();
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }


        public override void AI()
        {
            npc.velocity *= 0.85f;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.TargetClosest(false);
            }
            if (npc.HasValidTarget)
            {
                float percentHealth = ((float)npc.life / (float)npc.lifeMax);
                switch (stage)
                {
                    // Phase One:
                    case 0:
                        HandleMoving();
                        HandlePhaseOneShooting();

                        if (percentHealth < 0.95f && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            stage++;
                            npc.netUpdate = true;
                        }
                        break;
                    case 1:
                        HandleMoving();
                        HandlePhaseOneShooting();

                        if (percentHealth < 0.85f && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            stage++;
                            npc.netUpdate = true;
                        }
                        break;
                    case 2:
                        HandleMoving();
                        HandlePhaseOneShooting();

                        if (percentHealth < 0.70f && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            stage++;
                            npc.netUpdate = true;
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
                                npc.netUpdate = true;
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
                            npc.netUpdate = true;
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
                            npc.netUpdate = true;
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
            dashTimer--;
            moveTimer--;
        }

        public override void FindFrame(int frameHeight)
        {
            if (!smiling)
            {
                if (altTimer >= 16)
                {
                    npc.frame.Y = frameHeight * 4;
                }
                else if (altTimer >= 11)
                {
                    npc.frame.Y = frameHeight * 3;
                }
                else if (altTimer >= 6)
                {
                    npc.frame.Y = frameHeight * 2;
                }
                else if (altTimer >= 1)
                {
                    npc.frame.Y = frameHeight;
                }
                else
                {
                    npc.frame.Y = 0;
                }
            }
            if (smiling)
            {
                if (shooting)
                {
                    if (altTimer >= 24)
                    {
                        npc.frame.Y = frameHeight * 4;
                    }
                    else if (altTimer >= 22)
                    {
                        npc.frame.Y = frameHeight * 11;
                    }
                    else if (altTimer >= 19)
                    {
                        npc.frame.Y = frameHeight * 10;
                    }
                    else if (altTimer >= 13)
                    {
                        npc.frame.Y = frameHeight * 9;
                    }
                    else if (altTimer >= 10)
                    {
                        npc.frame.Y = frameHeight * 8;
                    }
                    else if (altTimer >= 4)
                    {
                        npc.frame.Y = frameHeight * 7;
                    }
                    else if (altTimer >= 2)
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                    else if (altTimer >= 0)
                    {
                        npc.frame.Y = frameHeight * 5;
                    }
                }
                else
                {
                    npc.frame.Y = frameHeight * 4;
                }
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            base.HitEffect(hitDirection, damage);
        }

        private void HandlePhaseOneShooting()
        {
            int ghostBlastTime = 45;

            switch (stage)
            {
                case 0:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(120f, 8f);
                        npc.netUpdate = true;
                        attackTimer += 15;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        GhostBlast();
                        npc.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(45);
                    }
                    break;
                case 1:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(105f, 16f);
                        npc.netUpdate = true;
                        attackTimer += 10;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        GhostBlast();
                        npc.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(45);
                    }
                    break;
                case 2:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(105f, 16f);
                        npc.netUpdate = true;
                        attackTimer += 10;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        GhostBlast();
                        npc.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(45);
                    }
                    break;
                case 3:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(105f, 16f);
                        npc.netUpdate = true;
                        attackTimer += 10;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        GhostBlast();
                        npc.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(45);
                    }
                    break;
            }
            attackTimer -= 1;
            attackTimer2 -= 1;
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
                        npc.netUpdate = true;
                        attackTimer += 15;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        HugeGhostBlast();
                        npc.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(30);
                    }
                    HandleShootingYarn(240);
                    break;
                case 6:
                    if (attackTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        ShootNeedle(360f + Main.rand.NextFloat(0, 60), 16f);
                        //ShootDoubleNeedle(180f, 16f);
                        npc.netUpdate = true;
                        attackTimer += 10;
                    }
                    if (attackTimer2 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        HugeGhostBlast();
                        npc.netUpdate = true;
                        attackTimer2 += ghostBlastTime + Main.rand.Next(30);
                    }
                    HandleShootingYarn(240);
                    break;
            }
            attackTimer -= 1;
            attackTimer2 -= 1;
            attackTimer3 -= 1;
        }

        private void HandleShootingYarn(int time)
        {
            if (attackTimer3 <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                shooting = true;
                npc.netUpdate = true;
            }
            if (!shot && shooting && altTimer > 16 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                canShoot = true;
                npc.netUpdate = true;
            }
            if (canShoot && Main.netMode != NetmodeID.MultiplayerClient)
            {
                shot = true;
                canShoot = false;
                ShootYarn();
                attackTimer3 += time + Main.rand.Next(60);
                npc.netUpdate = true;
            }
            if (shooting && altTimer > 24 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                shot = false;
                shooting = false;
                altTimer = 0;
                npc.netUpdate = true;
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
                    Vector2 distance = players[npc.target].Center - npc.Center;
                    dashDistance = distance.X;
                }
                Dash();
                if (dashTimer <= -20)
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
                npc.TargetClosest(false);
                npc.netUpdate = true;
                moveTimer = UpdateRate;
            }

            Player[] players = Main.player.ToArray();
            Vector2 distance = players[npc.target].Center - npc.Center;

            // Stay above player
            if (Math.Abs(distance.Y) < npc.height * 1.75f)
            {
                float yValue = (1 - Math.Abs(distance.Y) / (npc.height * 1.75f));
                if (yValue > 0.2f)
                {
                    yValue = 0.2f;
                }
                npc.velocity.Y -= yValue;
            } else if (distance.Y > npc.height * 2.0f)
            {
                float yValue = (1 - distance.Y / (npc.height * 2.0f));
                if (yValue > 0.5f)
                {
                    yValue = 0.5f;
                }
                npc.velocity.Y -= yValue;
            }

            if (distance.Length() > 360)
            {
                distance /= distance.Length();
                npc.velocity += distance;
            }
            else if (distance.Length() < 180)
            {
                distance /= distance.Length();
                npc.velocity -= distance / 2;
            }

            // Lean when moving
            if (npc.velocity.X < -1 && npc.rotation >= -0.3f)
            {
                npc.rotation -= 0.01f;
            } else if (npc.velocity.X > 1 && npc.rotation <= 0.3f)
            {
                npc.rotation += 0.01f;
            } else
            {
                if (Math.Abs(npc.rotation) > 0.05)
                {
                    npc.rotation *= 0.9f;
                }
                else
                {
                    npc.rotation = 0;
                }
            }
        }

        private void Dash()
        {
            if (moveTimer <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.TargetClosest(false);
                npc.netUpdate = true;
            }

            Player[] players = Main.player.ToArray();
            Vector2 distance = players[npc.target].Center - npc.Center;

            // Stay above player
            if (Math.Abs(distance.Y) < npc.height * 1.75f)
            {
                float yValue = (1 - Math.Abs(distance.Y) / (npc.height * 1.75f));
                if (yValue > 0.2f)
                {
                    yValue = 0.2f;
                }
                npc.velocity.Y -= yValue;
            }
            else if (distance.Y > npc.height * 2.0f)
            {
                float yValue = (1 - distance.Y / (npc.height * 2.0f));
                if (yValue > 0.5f)
                {
                    yValue = 0.5f;
                }
                npc.velocity.Y -= yValue;
            }

            //Dash
            //Player on Right
            if (dashDistance >= 0 && distance.X > -360)
            {
                npc.velocity.X += 5 * (dashDistance / 360);
            }
            //Player on Left
            else if (dashDistance < 0 && distance.X < 360)
            {
                npc.velocity.X += 5 * (dashDistance / 360);
            }

            if (distance.Length() > 360)
            {
                distance /= distance.Length();
                npc.velocity += distance;
            }
            else if (distance.Length() < 180)
            {
                distance /= distance.Length();
                npc.velocity -= distance / 2;
            }

            // Lean when moving
            if (npc.velocity.X < -1 && npc.rotation >= -0.3f)
            {
                npc.rotation -= 0.01f;
            }
            else if (npc.velocity.X > 1 && npc.rotation <= 0.3f)
            {
                npc.rotation += 0.01f;
            }
            else
            {
                if (Math.Abs(npc.rotation) > 0.05)
                {
                    npc.rotation *= 0.9f;
                }
                else
                {
                    npc.rotation = 0;
                }
            }
        }

        private void ExplodeTop()
        {
            Vector2 goreVelocity = new Vector2(0, -10);
            if (npc.life % 2 == 0)
            {
                goreVelocity.X = 3;
            }
            else
            {
                goreVelocity.X = -3;
            }
            Gore.NewGore(npc.position, goreVelocity, mod.GetGoreSlot("Gores/HauntedHatterTopGore"), 1f);

            // Code adapted from ExampleMod
            // Fire Dust spawn
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) + npc.velocity, npc.width, 8, 6, 0f, 0f, 100, default(Color), 3f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 5f;
                dustIndex = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) + npc.velocity, npc.width, 8, 6, 0f, 0f, 100, default(Color), 2f);
                Main.dust[dustIndex].velocity *= 3f;
            }
            // Large Smoke Gore spawn
            int goreIndex = Gore.NewGore(new Vector2(npc.Center.X, npc.position.Y) + npc.velocity, default(Vector2), Main.rand.Next(61, 64), 1f);
            Main.gore[goreIndex].scale = 1f;
            Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
            Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
            goreIndex = Gore.NewGore(new Vector2(npc.Center.X, npc.position.Y) + npc.velocity, default(Vector2), Main.rand.Next(61, 64), 1f);
            Main.gore[goreIndex].scale = 1f;
            Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
            Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;

            if (Main.netMode != NetmodeID.Server)
            {
                Main.PlaySound(SoundID.Item14.WithPitchVariance(0.1f).WithVolume(1.1f), new Vector2(npc.Center.X, npc.Center.Y + npc.height / 2));
            }
        }

        private void ShootNeedle(float timeBetweenVolleys, float maxOffset)
        {
            Vector2 velocity = new Vector2(0, 0); // Dummy gets overriden in projectile.
            int type = ModContent.ProjectileType<SewingNeedle>();
            float offsetX = 0;
            float offsetY = 0;

            float targetOffset = 0;

            switch (firePosition)
            {
                case 0:
                    offsetX = -npc.width * 1.5f;
                    firePosition = (firePosition + 1) % 3;
                    targetOffset = -maxOffset;
                    break;
                case 1:
                    offsetY = -npc.height * 1f;
                    firePosition = (firePosition + 1) % 3;
                    break;
                case 2:
                    offsetX = npc.width * 1.5f;
                    firePosition = (firePosition + 1) % 3;
                    targetOffset = maxOffset;
                    attackTimer += timeBetweenVolleys;
                    break;
                default:
                    firePosition = 0;
                    break;
            }

            offsetY -= npc.height * 0.5f;

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(NEEDLE_DAMAGE * expertMult);

            Vector2 origin = new Vector2(npc.Center.X + npc.velocity.X + offsetX, npc.Center.Y + npc.velocity.Y + offsetY);

            var projectile = Projectile.NewProjectileDirect(origin, velocity, type, damage, 0.5f);
            projectile.ai[1] = targetOffset;
            projectile.ai[2] = npc.target;
            projectile.ai[3] = npc.whoAmI;
        }

        private void ShootDoubleNeedle(float timeBetweenVolleys, float maxOffset)
        {
            Vector2 velocity = new Vector2(0, 0); // Dummy gets overriden in projectile.
            int type = ModContent.ProjectileType<SewingNeedle>();

            Vector2 point0 = new Vector2(npc.Center.X - npc.velocity.X - npc.width * 2.2f, npc.Center.Y - npc.velocity.Y - npc.height * 1);
            Vector2 point1 = new Vector2(npc.Center.X + npc.velocity.X + npc.width * 2.2f, npc.Center.Y - npc.velocity.Y - npc.height * 1);

            float offsetX0 = 0;
            float offsetY0 = 0;

            float offsetX1 = 0;
            float offsetY1 = 0;

            float targetOffset0 = 0;
            float targetOffset1 = 0;

            switch (firePosition)
            {
                case 0:
                    offsetX0 = -npc.width * 0.8f;
                    offsetX1 = npc.width * 0.8f;
                    firePosition = (firePosition + 1) % 3;
                    targetOffset0 = -maxOffset;
                    targetOffset1 = maxOffset;
                    break;
                case 1:
                    offsetY0 = -npc.height * 0.8f;
                    offsetY1 = -npc.height * 0.8f;
                    firePosition = (firePosition + 1) % 3;
                    break;
                case 2:
                    offsetX0 = npc.width * 0.8f;
                    offsetX1 = -npc.width * 0.8f;
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

            var projectile0 = Projectile.NewProjectileDirect(origin0, velocity, type, damage, 0.5f);
            projectile0.ai[1] = targetOffset0;
            projectile0.ai[2] = ClosestPlayerToPoint(point0);
            projectile0.ai[3] = npc.whoAmI;

            var projectile1 = Projectile.NewProjectileDirect(origin1, velocity, type, damage, 0.5f);
            projectile1.ai[1] = targetOffset1;
            projectile1.ai[2] = ClosestPlayerToPoint(point1);
            projectile1.ai[3] = npc.whoAmI;
        }

        private void ShootManyNeedle(float timeBetweenVolleys, float maxOffset)
        {
            Vector2 velocity = new Vector2(0, 0); // Dummy gets overriden in projectile.
            int type = ModContent.ProjectileType<SewingNeedle>();
            float offsetX = 0;
            float offsetY = 0;

            float targetOffset = 0;

            switch (firePosition)
            {
                case 0:
                    offsetX = -npc.width * 1.5f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset = -maxOffset;
                    break;
                case 1:
                    offsetX = -npc.width * 1.0f;
                    offsetY = -npc.height * 0.7f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset = -maxOffset * 0.5f;
                    break;
                case 2:
                    offsetY = -npc.height * 1f;
                    firePosition = (firePosition + 1) % 5;
                    break;
                case 3:
                    offsetX = npc.width * 1.0f;
                    offsetY = -npc.height * 0.7f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset = maxOffset * 0.5f;
                    break;
                case 4:
                    offsetX = npc.width * 1.5f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset = maxOffset;
                    attackTimer += timeBetweenVolleys;
                    break;
                default:
                    firePosition = 0;
                    break;
            }

            offsetY -= npc.height * 0.5f;

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(NEEDLE_DAMAGE * expertMult);

            Vector2 origin = new Vector2(npc.Center.X + npc.velocity.X + offsetX, npc.Center.Y + npc.velocity.Y + offsetY);

            var projectile = Projectile.NewProjectileDirect(origin, velocity, type, damage, 0.5f);
            projectile.ai[1] = targetOffset;
            projectile.ai[2] = npc.target;
            projectile.ai[3] = npc.whoAmI;
        }

        private void ShootManyManyNeedle(float timeBetweenVolleys, float maxOffset)
        {
            Vector2 velocity = new Vector2(0, 0); // Dummy gets overriden in projectile.
            int type = ModContent.ProjectileType<SewingNeedle>();

            Vector2 point0 = new Vector2(npc.Center.X - npc.velocity.X - npc.width * 2.2f, npc.Center.Y - npc.velocity.Y - npc.height * 1);
            Vector2 point1 = new Vector2(npc.Center.X + npc.velocity.X + npc.width * 2.2f, npc.Center.Y - npc.velocity.Y - npc.height * 1);

            float offsetX0 = 0;
            float offsetY0 = 0;

            float offsetX1 = 0;
            float offsetY1 = 0;

            float targetOffset0 = 0;
            float targetOffset1 = 0;

            switch (firePosition)
            {
                case 0:
                    offsetX0 = -npc.width * 0.8f;
                    offsetX1 = npc.width * 0.8f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset0 = -maxOffset;
                    targetOffset1 = maxOffset;
                    break;
                case 1:
                    offsetX0 = npc.width * 0.8f;
                    offsetX1 = -npc.width * 0.8f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset0 = +maxOffset;
                    targetOffset1 = -maxOffset;
                    break;
                case 2:
                    offsetX0 = -npc.width * 0.6f;
                    offsetX1 = npc.width * 0.6f;
                    offsetY0 = npc.height * 0.8f;
                    offsetY1 = npc.height * 0.8f;
                    firePosition = (firePosition + 1) % 5;
                    targetOffset0 = maxOffset * 0.5f;
                    targetOffset1 = -maxOffset * 0.5f;
                    break;
                case 3:
                    offsetY0 = -npc.height * 0.8f;
                    offsetY1 = -npc.height * 0.8f;
                    firePosition = (firePosition + 1) % 5;
                    break;
                case 4:
                    offsetX0 = npc.width * 0.6f;
                    offsetX1 = -npc.width * 0.6f;
                    offsetY0 = npc.height * 0.8f;
                    offsetY1 = npc.height * 0.8f;
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

            var projectile0 = Projectile.NewProjectileDirect(origin0, velocity, type, damage, 0.5f);
            projectile0.ai[1] = targetOffset0;
            projectile0.ai[2] = ClosestPlayerToPoint(point0);
            projectile0.ai[3] = npc.whoAmI;

            var projectile1 = Projectile.NewProjectileDirect(origin1, velocity, type, damage, 0.5f);
            projectile1.ai[1] = targetOffset1;
            projectile1.ai[2] = ClosestPlayerToPoint(point1);
            projectile1.ai[3] = npc.whoAmI;

        }

        private void ShootYarn()
        {
            int type = ModContent.ProjectileType<EnchantedYarn>();

            Player player = Main.player[npc.target];
            float diff = player.Center.X - npc.Center.X;
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
            Vector2 origin = new Vector2(npc.Center.X, npc.position.Y + 26);

            var projectile = Projectile.NewProjectileDirect(origin, velocity, type, damage, 0.5f);
        }

        private void GhostBlast()
        {
            int type = ModContent.ProjectileType<GhostBlast>();

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(BLAST_DAMAGE * expertMult);

            Player target = Main.player[npc.target];

            float CoolAngle = (float)Math.Atan2(target.Center.Y - ghostHead.Y, target.Center.X - ghostHead.X) + MathHelper.PiOver2;

            float rotation = CoolAngle - MathHelper.PiOver2;
            Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            velocity.Normalize();
            velocity *= 8;

            var projectile = Projectile.NewProjectileDirect(ghostHead + npc.velocity + new Vector2(0, -8), velocity, type, damage, 0.5f);

            Main.PlaySound(SoundID.Item12, projectile.position);
        }

        private void HugeGhostBlast()
        {
            int type = ModContent.ProjectileType<HugeGhostBlast>();

            float expertMult = Main.expertMode ? EXPERT_DAMAGE : 1;
            int damage = (int)(BLAST_DAMAGE * 1.2f * expertMult);

            Player target = Main.player[npc.target];

            float CoolAngle = (float)Math.Atan2(target.Center.Y - ghostHead.Y, target.Center.X - ghostHead.X) + MathHelper.PiOver2;

            float rotation = CoolAngle - MathHelper.PiOver2;
            Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            velocity.Normalize();
            velocity *= 8;

            var projectile = Projectile.NewProjectileDirect(ghostHead + npc.velocity + new Vector2(0, -8), velocity, type, damage, 0.5f);

            Main.PlaySound(SoundID.Item12, projectile.position);
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

        private void UpdateDefense()
        {
            //bool alive0 = Main.npc[spawns[0]].life > 0;
            //bool alive1 = Main.npc[spawns[1]].life > 0;

            //int def0 = alive0 ? 1 : 0;
            //int def1 = alive1 ? 1 : 0;

            //npc.defense = BASE_DEF + 8 * (def0 + def1);
        }

        private int stage
        {
            get => (int)npc.ai[0];
            set => npc.ai[0] = value;
        }

        private float attackTimer
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }

        private int firePosition
        {
            get => (int)npc.ai[2];
            set => npc.ai[2] = value;
        }

        private bool smiling
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

        private Vector2 ghostHead {
            get => npc.position + new Vector2(70, 20);
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
            get => npc.ai[3];
            private set => npc.ai[3] = value;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((short)moveTimer);
            writer.Write(flags);
            writer.Write((short)attackTimer2);
            writer.Write((short)attackTimer3);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            moveTimer = reader.ReadInt16();
            flags = reader.ReadByte();
            attackTimer2 = reader.ReadInt16();
            attackTimer3 = reader.ReadInt16();
        }

        public override void NPCLoot()
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {

            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.NextBool(2))
                {
                    Item.NewItem(npc.getRect(), ItemType<SpiritShears>());
                }
                Item.NewItem(npc.getRect(), ItemType<GhostlyGibus>());
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Haunted Hatter";
            potionType = ItemID.HealingPotion;
        }


    }
}
