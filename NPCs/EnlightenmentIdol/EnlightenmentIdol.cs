using Highlander.Dusts;
using Highlander.Items.EnlightenmentIdol;
using Highlander.NPCs.HauntedHatter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.EnlightenmentIdol
{
    [AutoloadBossHead]
    class EnlightenmentIdol : ModNPC
    {
        private const float EXPERT_DAMAGE = 0.8f;
        private const int BASE_DEF = 15;
        private const int TRIANGLE_DAMAGE = 44;
        private const int CHARGE_DAMAGE = 44;
        private const int SPHERE_RADIUS = 450;
        private Texture2D TopArms;
        private Texture2D MiddleArms;
        private Texture2D ChargeBlast;

        private BitsByte flags = new BitsByte();
        private int clapTimer = 0;
        private int deathTimer = 0;
        private int triangleTimer = 0;
        private byte fistTimer = 0;
        private byte floatTimer = 0;
        private byte blastTimer = 0;
        private byte topFrame;
        private byte middleFrame;
        private byte chargeBlastFrame;
        private byte chargeBlastCounter;
        private bool dontDamage;
        private byte blastRadius;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5; // make sure to set this for your modnpcs.
            DisplayName.SetDefault("Idol of Enlightenment");
        }

        public override void SetDefaults()
        {
            //NPC.frame = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);
            DrawOffsetY = 0;// -52;
            NPC.aiStyle = -1;
            NPC.lifeMax = 37000;
            NPC.damage = 40;
            NPC.defense = BASE_DEF;
            NPC.knockBackResist = 0f;
            NPC.width = 60;
            NPC.height = 120;
            NPC.npcSlots = 50f;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit42;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 150000;
            NPC.alpha = 0;
            Music = MusicID.Boss4;
            //MusicPriority = MusicPriority.BossMedium;
            TopArms = Request<Texture2D>("Highlander/NPCs/EnlightenmentIdol/EnlightenmentIdol_Triangle").Value;
            MiddleArms = Request<Texture2D>("Highlander/NPCs/EnlightenmentIdol/EnlightenmentIdol_Charge").Value;
            ChargeBlast = Request<Texture2D>("Highlander/NPCs/EnlightenmentIdol/ChargeBlast").Value;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.7f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.7f);
        }


        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest(false);
            }
            if (NPC.HasValidTarget)
            {
                var projSource = NPC.GetSpawnSource_ForProjectile();

                if(deathTimer != 0)
                {
                    deathTimer = 0;
                    NPC.alpha = 0;
                    NPC.netUpdate = true;
                }

                HandleStage();

                Player target = Main.player[NPC.target];
                if (NPC.position.X > target.position.X)
                {
                    NPC.spriteDirection = -1;
                }
                else if (NPC.position.X + NPC.width < target.position.X + target.width)
                {
                    NPC.spriteDirection = 1;
                }

                HandleMoving();
                HandleAttacking();

                FindPostFrame();
                if(blastRadius > 40)
                {
                    foreach(Player p in Main.player)
                    {
                        float lengthSquared = (p.Center - NPC.Center).LengthSquared();
                        int innerRadius = (blastRadius - 40) * 2;
                        if(lengthSquared <= blastRadius * blastRadius * 4 && !(lengthSquared < innerRadius * innerRadius))
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                int damage = CHARGE_DAMAGE;
                                if (Main.expertMode)
                                {
                                    damage = (int)(damage * EXPERT_DAMAGE); // Should only ever be in expert
                                }
                                Projectile.NewProjectile(projSource, p.position, new Vector2(), ProjectileType<ChargeDummyProjectile>(), damage, 0);
                                
                                /**if (!p.immune) {
                                    p.statLife -= 80 - (int)(p.statDefense * 0.75f);
                                    Vector2 knockback = p.Center - NPC.Center;
                                    knockback.Normalize();
                                    p.velocity += knockback * 4;
                                    //p.immuneTime += 30;
                                }**/
                            }
                            Vector2 knockback = p.Center - NPC.Center;
                            //HighlanderPlayer modP = p.GetModPlayer<HighlanderPlayer>();
                            //modP.knockback = true;
                            //modP.knockbackDir = knockback.ToRotation();
                            knockback.Normalize();

                            p.velocity *= 0;
                            p.velocity += knockback * 16;
                        }
                    }
                }
            }
            else
            {
                NPC.velocity *= 0.85f;
                chargeBlastFrame = 0;
                deathTimer++;
                if (deathTimer > 240)
                {
                    NPC.alpha += 2;
                }
                if (deathTimer > 360)
                {
                    NPC.active = false;
                    NPC.netUpdate = true;
                }
            }
            if(Main.netMode != NetmodeID.Server)
            {
                if (Main.rand.NextBool(20))
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType<EnlightenmentIdolDust>());
                }
            }
        }

        private void HandleMoving()
        {
            NPC.velocity *= 0.85f;

            Player[] players = Main.player.ToArray();
            Vector2 distance = players[NPC.target].Center - NPC.Center;

            // Stay above player
            if (Math.Abs(distance.Y) < 100)
            {
                float yValue = (1 - Math.Abs(distance.Y) / (80));
                if (yValue > 0.2f)
                {
                    yValue = 0.2f;
                }
                NPC.velocity.Y -= yValue;
            }
            else if (distance.Y > 140)
            {
                float yValue = (1 - distance.Y / (110));
                if (yValue > 0.5f)
                {
                    yValue = 0.5f;
                }
                NPC.velocity.Y -= yValue;
            }

            if (distance.Length() > 280)
            {
                distance /= distance.Length();
                NPC.velocity += distance / 2;
            }
            else if (distance.Length() < 60)
            {
                distance /= distance.Length();
                NPC.velocity -= distance / 3;
            }

            // Lean when moving
            if (NPC.velocity.X < -1 && NPC.rotation >= -0.10f)
            {
                NPC.rotation -= 0.01f;
            }
            else if (NPC.velocity.X > 1 && NPC.rotation <= 0.10f)
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

            // Make a floating effect
            float floatVelocity = (float) (floatTimer / 10 - 3) / 40;

            NPC.velocity.Y += floatVelocity;

            if (!floatDirection)
            {
                floatTimer = (byte)(floatTimer + 1);
                if(floatTimer >= 60)
                {
                    floatDirection = !floatDirection;
                    NPC.netUpdate = true;
                }
            }
            else
            {
                floatTimer = (byte)(floatTimer - 1);
                if (floatTimer <= 0)
                {
                    floatDirection = !floatDirection;
                    NPC.netUpdate = true;
                }
            }
            

        }

        private void HandleAttacking()
        {
            switch (stage)
            {
                case 0:
                    blastTimer++;
                    if (blastTimer > 210)
                    {
                        triangleReady = true;
                        blastTimer = 0;
                        //triangleStance = false;
                        NPC.netUpdate = true;
                    } else if (blastTimer > 180 && triangleReady)
                    {
                        triangleReady = false;
                        NPC.netUpdate = true;
                        TriangleBlast();
                    } else if (blastTimer > 150)
                    {
                        triangleStance = true;
                        NPC.netUpdate = true;
                    }
                    
                    break;
                case 1:
                    if (clapped)
                    {
                        clapped = false;
                        NPC.netUpdate = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            FistAttack();
                        }
                    }
                    if (!clapping)
                    {
                        clapTimer++;
                    }

                    if (clapTimer >= 120)
                    {
                        clapTimer = 0;
                        Clap();
                    }
                    break;
                case 2:
                    blastTimer++;
                    if (blastTimer > 210)
                    {
                        triangleReady = true;
                        blastTimer = 0;
                        //triangleStance = false;
                        NPC.netUpdate = true;
                    }
                    else if (blastTimer > 180 && triangleReady)
                    {
                        triangleReady = false;
                        NPC.netUpdate = true;
                        TriangleBlast();
                    }
                    else if (blastTimer > 150)
                    {
                        triangleStance = true;
                        NPC.netUpdate = true;
                    }
                    if (clapped)
                    {
                        clapped = false;
                        NPC.netUpdate = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            FistAttack();
                        }
                    }
                    if (!clapping)
                    {
                        clapTimer++;
                    }

                    if (clapTimer >= 120)
                    {
                        clapTimer = 0;
                        Clap();
                    }
                    /**
                    if (clapped)
                    {
                        clapped = false;
                        fistTimer = 40;
                        NPC.netUpdate = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            FistAttack();
                        }
                    }
                    if (!clapping)
                    {
                        clapTimer++;
                    }

                    if (clapTimer >= 120)
                    {
                        clapTimer = 0;
                        Clap();
                    }
                    if (fistTimer > 0)
                    {
                        fistTimer--;
                        if (fistTimer % 15 == 0)
                        {
                            FistAttack();
                            NPC.netUpdate = true;
                        }
                    }**/
                    break;
                case 3:
                    if (Main.expertMode)
                    {
                        if (!charging)
                        {
                            chargeTimer++;
                        }
                        if (chargeTimer > 540)
                        {
                            charging = true;
                            chargeTimer = 0;
                            NPC.netUpdate = true;
                        }
                        if (charged)
                        {
                            charged = false;
                            NPC.netUpdate = true;
                        }
                    }

                    blastTimer++;
                    if (blastTimer > 110)
                    {
                        triangleReady = true;
                        blastTimer = 0;
                        NPC.netUpdate = true;
                    }
                    else if (blastTimer > 80 && triangleReady)
                    {
                        triangleReady = false;
                        NPC.netUpdate = true;
                        TriangleBlast();
                    }
                    else if (blastTimer > 50)
                    {
                        triangleStance = true;
                        NPC.netUpdate = true;
                    }
                    if (clapped)
                    {
                        clapped = false;
                        NPC.netUpdate = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            FistAttackMulti();
                        }
                    }
                    if (!clapping)
                    {
                        clapTimer++;
                    }

                    if (clapTimer >= 120)
                    {
                        clapTimer = 0;
                        Clap();
                    }
                    break;
            }
        }

        private void HandleStage()
        {
            float percentHealth = ((float)NPC.life / (float)NPC.lifeMax);
            if(percentHealth >= 0.85f)
            {
                if(stage != 0)
                {
                    if(Main.netMode != NetmodeID.MultiplayerClient && stage != 0)
                    {
                        stage = 0;
                        NPC.netUpdate = true;
                    }
                }
            } else if (percentHealth > 0.70f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && stage != 1)
                {
                    stage = 1;
                    NPC.netUpdate = true;
                }
            }
            else if (percentHealth > 0.40f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && stage != 2)
                {
                    stage = 2;
                    NPC.netUpdate = true;
                }
            }
            else if (percentHealth <= 0.40f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && stage != 3)
                {
                    stage = 3;
                    NPC.netUpdate = true;
                }
            }
        }

        private void Clap()
        {
            NPC.netUpdate = true;
            clapping = true;
        }

        private void FistAttack()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 up = new Vector2(0, -1);
                Vector2 down = new Vector2(0, 1);
                Vector2 left = new Vector2(-1, 0);
                Vector2 right = new Vector2(1, 0);

                var source = NPC.GetSpawnSourceForNPCFromNPCAI();

                Player target = Main.player[NPC.target];
                float distance = (target.position - NPC.position).Length();
                if (distance < 1350)
                {
                    // Get a random point with negative values and find its direction.
                    float rand = Main.rand.NextFloat(0, MathHelper.TwoPi);

                    NPC.NewNPC(source, (int)(target.position + (up * 250).RotatedBy(rand)).X, (int)(target.position + (up * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, NPC.target);
                    NPC.NewNPC(source, (int)(target.position + (down * 250).RotatedBy(rand)).X, (int)(target.position + (down * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, NPC.target);
                    NPC.NewNPC(source, (int)(target.position + (left * 250).RotatedBy(rand)).X, (int)(target.position + (left * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, NPC.target);
                    NPC.NewNPC(source, (int)(target.position + (right * 250).RotatedBy(rand)).X, (int)(target.position + (right * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, NPC.target);

                    //projectile.ai = new float[2];
                    //projectile.ai[0] = 0;
                    //projectile.ai[1] = NPC.target;
                }
            }
        }

        private void FistAttackMulti()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                var source = NPC.GetSpawnSourceForNPCFromNPCAI();
                foreach (Player p in Main.player) {
                    if (p.active && !p.dead)
                    {
                        float distance = (p.position - NPC.position).Length();
                        if (distance < 1350) {
                            Player target = p;

                            Vector2 up = new Vector2(0, -1);
                            Vector2 down = new Vector2(0, 1);
                            Vector2 left = new Vector2(-1, 0);
                            Vector2 right = new Vector2(1, 0);

                            // Get a random point with negative values and find its direction.
                            float rand = Main.rand.NextFloat(0, MathHelper.TwoPi);

                            NPC.NewNPC(source, (int)(target.position + (up * 250).RotatedBy(rand)).X, (int)(target.position + (up * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, p.whoAmI);
                            NPC.NewNPC(source, (int)(target.position + (down * 250).RotatedBy(rand)).X, (int)(target.position + (down * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, p.whoAmI);
                            NPC.NewNPC(source, (int)(target.position + (left * 250).RotatedBy(rand)).X, (int)(target.position + (left * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, p.whoAmI);
                            NPC.NewNPC(source, (int)(target.position + (right * 250).RotatedBy(rand)).X, (int)(target.position + (right * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, p.whoAmI);
                        }
                    }
                }
            }
        }

        private void TriangleBlast()
        {
            Vector2 up = new Vector2(0, -20);
            Vector2 spawn = NPC.Center + up;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                var source = NPC.GetSpawnSource_ForProjectile();

                Player target = Main.player[NPC.target];
                int type = ModContent.ProjectileType<TriangleBlast>();

                float rotation = (float)Math.Atan2(target.Center.Y - spawn.Y, target.Center.X - spawn.X) + MathHelper.PiOver2;
                int damage = TRIANGLE_DAMAGE;
                if (Main.expertMode)
                {
                    damage = (int) (damage * EXPERT_DAMAGE);
                }

                var projectile = Projectile.NewProjectile(source, spawn, up.RotatedBy(rotation - 0.4f) * 0.5f, type, damage, 9.5f, 255, 0, NPC.target);
                projectile = Projectile.NewProjectile(source, spawn, up.RotatedBy(rotation + 0.4f) * 0.5f, type, damage, 9.5f, 255, 0, NPC.target);
                projectile = Projectile.NewProjectile(source, spawn, up.RotatedBy(rotation) * 0.5f, type, damage, 9.5f, 255, 0, NPC.target);
                //projectile.ai = new float[2];
                //projectile.ai[0] = 0;
                //projectile.ai[1] = NPC.target;
            }
            if (Main.netMode != NetmodeID.Server)
            {
                SoundEngine.PlaySound(SoundID.Item72.SoundId, (int)spawn.X, (int)spawn.Y, SoundID.Item72.Style, 0.80f, -0.2f);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (clapping)
            {
                if(NPC.frameCounter < 6)
                {
                    NPC.frame.Y = frameHeight;
                } else if (NPC.frameCounter < 12)
                {
                    NPC.frame.Y = frameHeight * 2;
                }
                else if (NPC.frameCounter < 24)
                {
                    NPC.frame.Y = frameHeight * 3;
                }
                else if (NPC.frameCounter < 30)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
                else
                {
                    NPC.frame.Y = 0;
                    clapping = false;
                    clapped = true;
                    if (Main.netMode != NetmodeID.Server)
                    {
                        SoundEngine.PlaySound(SoundID.Item37.SoundId, (int)NPC.position.X, (int)NPC.position.Y, SoundID.Item37.Style, 0.90f, -0.5f);
                    }
                }
                NPC.frameCounter++;
            }
            else
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0;
            }
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            dontDamage = (player.Center - NPC.Center).Length() > SPHERE_RADIUS;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            dontDamage = player.active && (player.Center - NPC.Center).Length() > SPHERE_RADIUS;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (dontDamage)
            {
                damage = 0;
                crit = true;
                dontDamage = false;
                SoundEngine.PlaySound(SoundID.NPCHit4.SoundId, (int)NPC.position.X, (int)NPC.position.Y, SoundID.NPCHit4.Style, 1f, +0.3f);
                //Main.PlaySound(SoundID.NPCHit42.SoundId, (int)NPC.position.X, (int)NPC.position.Y, SoundID.NPCHit42.Style, 1f, 0.8f);
                //Main.PlaySound(NPC.HitSound, NPC.position);
                return false;
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D border = Request<Texture2D>("Highlander/NPCs/EnlightenmentIdol/SphereBorder").Value;

            spriteBatch.Draw(Request<Texture2D>("Highlander/NPCs/EnlightenmentIdol/IdolSphere").Value, NPC.Center - screenPos, null, Color.White * (40f / 255f) * ((255 - NPC.alpha) / 255f), 0f, new Vector2(SPHERE_RADIUS, SPHERE_RADIUS), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(border, NPC.Center - screenPos, null, Color.White * ((255 - NPC.alpha) / 255f), 0f, new Vector2(border.Width / 2, border.Height / 2), 1f, SpriteEffects.None, 0f);

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects flip = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Vector2 drawPos = NPC.position - screenPos + new Vector2(NPC.width / 2, NPC.height / 2 + 9);

            int TopArmsFrameHeight = TopArms.Height / 6;
            Vector2 TopArmsOrigin = new Vector2(TopArms.Width / 2, TopArmsFrameHeight / 2);
            
            Rectangle TopArmsFrame = new Rectangle(0, topFrame * TopArmsFrameHeight, TopArms.Width, TopArmsFrameHeight);

            spriteBatch.Draw(TopArms, drawPos, TopArmsFrame, Color.White * ((float)(255 - NPC.alpha) / 255f), NPC.rotation, TopArmsOrigin, 1.0f, flip, 0);

            int MiddleArmsFrameHeight = MiddleArms.Height / 27;
            Vector2 MiddleArmsOrigin = new Vector2(MiddleArms.Width / 2, MiddleArmsFrameHeight / 2);
            Rectangle MiddleArmsFrame = new Rectangle(0, middleFrame * MiddleArmsFrameHeight, MiddleArms.Width, MiddleArmsFrameHeight);

            spriteBatch.Draw(MiddleArms, drawPos, MiddleArmsFrame, Color.White * ((float)(255 - NPC.alpha) / 255f), NPC.rotation, MiddleArmsOrigin, 1.0f, flip, 0);

            int ChargeBlastFrameHeight = ChargeBlast.Height / 4;
            int ChargeBlastFrameWidth = ChargeBlast.Width / 5;
            Vector2 ChargeBlastOrigin = new Vector2(ChargeBlastFrameWidth / 2, ChargeBlastFrameHeight / 2);
            Rectangle ChargeBlastFrame = new Rectangle(chargeBlastFrame % 5 * ChargeBlastFrameWidth, chargeBlastFrame / 5 * ChargeBlastFrameHeight, ChargeBlastFrameWidth, ChargeBlastFrameHeight);

            spriteBatch.Draw(ChargeBlast, NPC.Center - screenPos, ChargeBlastFrame, Color.White * ((float)(255 - NPC.alpha) / 255f), 0f, ChargeBlastOrigin, 1.0f, flip, 0);
        }

        private void FindPostFrame()
        {
            byte newTopFrame = 0;

            if (triangleStance)
            {
                if (topArmsCounter < 4)
                {
                    newTopFrame = 1;
                }
                else if (topArmsCounter < 8)
                {
                    newTopFrame = 2;
                }
                else if (topArmsCounter < 12)
                {
                    newTopFrame = 3;
                }
                else if (topArmsCounter < 16)
                {
                    newTopFrame = 4;
                }
                else if (topArmsCounter < 44)
                {
                    newTopFrame = 5;
                }
                else if (topArmsCounter < 48)
                {
                    newTopFrame = 4;
                }
                else if (topArmsCounter < 53)
                {
                    newTopFrame = 3;
                }
                else if (topArmsCounter < 56)
                {
                    newTopFrame = 2;
                }
                else if (topArmsCounter < 60)
                {
                    newTopFrame = 1;
                }
                else
                {
                    triangleStance = false;
                    NPC.netUpdate = true;
                }
                topArmsCounter++;
            }
            else
            {
                newTopFrame = 0;
                topArmsCounter = 0;
                NPC.netUpdate = true;
            }
            if (newTopFrame != topFrame)
            {
                topFrame = newTopFrame;
                NPC.netUpdate = true;
            }

            byte newMiddleFrame = 0;

            if (charging)
            {
                if (middleArmsCounter < 6)
                {
                    newMiddleFrame = 1;
                }
                else if (middleArmsCounter < 12)
                {
                    newMiddleFrame = 2;
                }
                else if (middleArmsCounter < 17)
                {
                    newMiddleFrame = 3;
                }
                else if (middleArmsCounter < 23)
                {
                    newMiddleFrame = 4;
                }
                else if (middleArmsCounter < 28)
                {
                    newMiddleFrame = 5;
                }
                else if (middleArmsCounter < 33)
                {
                    if (middleArmsCounter == 29)
                    {
                        if (Main.netMode != NetmodeID.Server)
                        {
                            //SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)NPC.Center.X, (int)NPC.Center.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Thunder"));
                            SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)NPC.Center.X, (int)NPC.Center.Y, SoundLoader.GetSoundSlot(Mod, "Sounds/Custom/Thunder"));
                        }
                    }
                    newMiddleFrame = 6;
                }
                else if (middleArmsCounter < 38)
                {
                    newMiddleFrame = 7;
                }
                else if (middleArmsCounter < 45)
                {
                    newMiddleFrame = 8;
                }
                else if (middleArmsCounter < 50)
                {
                    newMiddleFrame = 9;
                }
                else if (middleArmsCounter < 55)
                {
                    newMiddleFrame = 10;
                }
                else if (middleArmsCounter < 60)
                {
                    newMiddleFrame = 11;
                }
                else if (middleArmsCounter < 65)
                {
                    newMiddleFrame = 12;
                }
                else if (middleArmsCounter < 72)
                {
                    newMiddleFrame = 13;
                }
                else if (middleArmsCounter < 77)
                {
                    newMiddleFrame = 14;
                }
                else if (middleArmsCounter < 82)
                {
                    newMiddleFrame = 15;
                }
                else if (middleArmsCounter < 87)
                {
                    newMiddleFrame = 16;
                }
                else if (middleArmsCounter < 92)
                {
                    newMiddleFrame = 17;
                }
                else if (middleArmsCounter < 97)
                {
                    newMiddleFrame = 18;
                }
                else if (middleArmsCounter < 104)
                {
                    newMiddleFrame = 19;
                }
                else if (middleArmsCounter < 109)
                {
                    newMiddleFrame = 20;
                }
                else if (middleArmsCounter < 114)
                {
                    newMiddleFrame = 21;
                }
                else if (middleArmsCounter < 119)
                {
                    newMiddleFrame = 22;
                }
                else if (middleArmsCounter < 126)
                {
                    if(middleArmsCounter == 120)
                    {
                        chargeBlast = true;
                        NPC.netUpdate = true;
                        if (Main.netMode != NetmodeID.Server)
                        {
                            //Main.PlaySound(SoundID.Item15.SoundId, (int)NPC.Center.X, (int)NPC.Center.Y, SoundID.Item15.Style, 0.80f, +0.5f);
                            //Main.PlaySound(SoundID.Item60.SoundId, (int)NPC.Center.X, (int)NPC.Center.Y, SoundID.Item60.Style, 0.80f, +0.5f);
                            SoundEngine.PlaySound(SoundID.Item74.SoundId, (int)NPC.Center.X, (int)NPC.Center.Y, SoundID.Item74.Style, 0.90f, +0.5f);
                        }
                    }
                    newMiddleFrame = 23;
                }
                else if (middleArmsCounter < 131)
                {
                    newMiddleFrame = 24;
                }
                else if (middleArmsCounter < 136)
                {
                    newMiddleFrame = 25;
                }
                else if (middleArmsCounter < 141)
                {
                    newMiddleFrame = 26;
                }
                else if (middleArmsCounter < 146)
                {
                    newMiddleFrame = 5;
                }
                else if (middleArmsCounter < 151)
                {
                    newMiddleFrame = 4;
                }
                else if (middleArmsCounter < 156)
                {
                    newMiddleFrame = 3;
                }
                else if (middleArmsCounter < 162)
                {
                    newMiddleFrame = 2;
                }
                else if (middleArmsCounter < 168)
                {
                    newMiddleFrame = 1;
                }
                else
                {
                    charging = false;
                    NPC.netUpdate = true;
                }
                middleArmsCounter++;
            }
            else
            {
                newMiddleFrame = 0;
                middleArmsCounter = 0;
                NPC.netUpdate = true;
            }
            if (newMiddleFrame != middleFrame)
            {
                middleFrame = newMiddleFrame;
                NPC.netUpdate = true;
            }

            byte newChargeBlastFrame = 0;
            byte newBlastRadius = 0;

            if (chargeBlast)
            {
                if (chargeBlastCounter < 2)
                {
                    newChargeBlastFrame = 1;
                    newBlastRadius = 8;
                }
                else if (chargeBlastCounter < 4)
                {
                    newChargeBlastFrame = 2;
                    newBlastRadius = 10;
                }
                else if (chargeBlastCounter < 6)
                {
                    newChargeBlastFrame = 3;
                    newBlastRadius = 12;
                }
                else if (chargeBlastCounter < 8)
                {
                    newChargeBlastFrame = 4;
                    newBlastRadius = 14;
                }
                else if (chargeBlastCounter < 10)
                {
                    newChargeBlastFrame = 5;
                    newBlastRadius = 16;
                }
                else if (chargeBlastCounter < 12)
                {
                    newChargeBlastFrame = 6;
                    newBlastRadius = 24;
                }
                else if (chargeBlastCounter < 14)
                {
                    newChargeBlastFrame = 7;
                    newBlastRadius = 32;
                }
                else if (chargeBlastCounter < 16)
                {
                    newChargeBlastFrame = 8;
                    newBlastRadius = 40;
                }
                else if (chargeBlastCounter < 18)
                {
                    newChargeBlastFrame = 9;
                    newBlastRadius = 48;
                }
                else if (chargeBlastCounter < 20)
                {
                    newChargeBlastFrame = 10;
                    newBlastRadius = 56;
                }
                else if (chargeBlastCounter < 22)
                {
                    newChargeBlastFrame = 11;
                    newBlastRadius = 64;
                }
                else if (chargeBlastCounter < 24)
                {
                    newChargeBlastFrame = 12;
                    newBlastRadius = 80;
                }
                else if (chargeBlastCounter < 26)
                {
                    newChargeBlastFrame = 13;
                    newBlastRadius = 96;
                }
                else if (chargeBlastCounter < 28)
                {
                    newChargeBlastFrame = 14;
                    newBlastRadius = 112;
                }
                else if (chargeBlastCounter < 30)
                {
                    newChargeBlastFrame = 15;
                    newBlastRadius = 128;
                }
                else if (chargeBlastCounter < 32)
                {
                    newChargeBlastFrame = 16;
                    newBlastRadius = 152;
                }
                else if (chargeBlastCounter < 34)
                {
                    newChargeBlastFrame = 17;
                    newBlastRadius = 174;
                }
                else if (chargeBlastCounter < 36)
                {
                    newChargeBlastFrame = 18;
                    newBlastRadius = 198;
                }
                else if (chargeBlastCounter < 38)
                {
                    newChargeBlastFrame = 19;
                    newBlastRadius = 220;
                }
                else
                {
                    newBlastRadius = 0;
                    chargeBlast = false;
                    NPC.netUpdate = true;
                }
                chargeBlastCounter++;
            }
            else
            {
                newBlastRadius = 0;
                newChargeBlastFrame = 0;
                chargeBlastCounter = 0;
                NPC.netUpdate = true;
            }
            if (newChargeBlastFrame != chargeBlastFrame)
            {
                chargeBlastFrame = newChargeBlastFrame;
                blastRadius = newBlastRadius;
                NPC.netUpdate = true;
            }
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White * ((float)(255 - NPC.alpha) / 255f);
        }

        public bool clapping
        {
            get => flags[0];
            set => flags[0] = value;
        }

        public bool clapped
        {
            get => flags[1];
            set => flags[1] = value;
        }

        public bool floatDirection
        {
            get => flags[2];
            set => flags[2] = value;
        }

        public bool triangleStance
        {
            get => flags[3];
            set => flags[3] = value;
        }

        public bool triangleReady
        {
            get => !flags[4];
            set => flags[4] = !value;
        }

        public bool chargeBlast
        {
            get => flags[5];
            set => flags[5] = value;
        }

        public bool charging
        {
            get => flags[6];
            set => flags[6] = value;
        }

        public bool charged
        {
            get => flags[7];
            set => flags[7] = value;
        }

        public float stage 
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public float topArmsCounter
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        public float middleArmsCounter
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        public float chargeTimer
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }


        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(flags);
            writer.Write((short)clapTimer);
            writer.Write(blastTimer);
            writer.Write((short)deathTimer);
            writer.Write((short)triangleTimer);
            writer.Write(topFrame);
            writer.Write(chargeBlastFrame);
            writer.Write(chargeBlastCounter);
            writer.Write(blastRadius);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            flags = reader.ReadByte();
            clapTimer = (int) reader.ReadInt16();
            blastTimer = reader.ReadByte();
            deathTimer = (int)reader.ReadInt16();
            triangleTimer = reader.ReadInt16();
            topFrame = reader.ReadByte();
            chargeBlastFrame = reader.ReadByte();
            chargeBlastCounter = reader.ReadByte();
            blastRadius = reader.ReadByte();
        }

        public override void OnKill()
        {
            if (!HighlanderWorld.downedEnlightenmentIdol)
            {
                HighlanderWorld.downedEnlightenmentIdol = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ItemType<EnlightenmentIdolBag>()));

            npcLoot.Add(ItemDropRule.Common(ItemType<EnlightenmentIdolTrophy>(), 10));

            // All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            // Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            // Boss masks are spawned with 1/7 chance
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EnlightenedMask>(), 7));
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ItemType<BlitzFist>(), ItemType<CommanderBlessing>()));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if(NPC.life <= 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        var gore = Gore.NewGoreDirect(NPC.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 5, Mod.Find<ModGore>("IdolGore" + i).Type, 1f);
                    } catch (Exception e)
                    {

                    }
                    
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Idol of Enlightenment";
            potionType = ItemID.GreaterHealingPotion;
        }

    }
}
