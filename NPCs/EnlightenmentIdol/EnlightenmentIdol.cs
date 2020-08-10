using Highlander.Items.EnlightenmentIdol;
using Highlander.NPCs.HauntedHatter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.EnlightenmentIdol
{
    [AutoloadBossHead]
    class EnlightenmentIdol : ModNPC
    {
        private const int BASE_DEF = 20;
        private const int HAND_DAMAGE = 18;
        private const int SPHERE_RADIUS = 450;
        private Texture2D TopArms;
        private Texture2D MiddleArms;

        private BitsByte flags = new BitsByte();
        private int clapTimer = 0;
        private int deathTimer = 0;
        private int triangleTimer = 0;
        private byte fistTimer = 0;
        private byte floatTimer = 0;
        private byte blastTimer = 0;
        private byte topFrame;
        private byte middleFrame;
        private bool dontDamage;

        public override bool Autoload(ref string name)
        {
            return true;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5; // make sure to set this for your modnpcs.
            DisplayName.SetDefault("Idol of Enlightenment");
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
            npc.width = 60;
            npc.height = 120;
            npc.npcSlots = 50f;
            npc.boss = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 100000;
            npc.alpha = 0;
            music = MusicID.Boss1;
            musicPriority = MusicPriority.BossMedium;
            bossBag = ItemType<EnlightenmentIdolBag>();
            TopArms = GetTexture("Highlander/NPCs/EnlightenmentIdol/EnlightenmentIdol_Triangle");
            MiddleArms = GetTexture("Highlander/NPCs/EnlightenmentIdol/EnlightenmentIdol_Charge");
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.TargetClosest(false);
            }
            if (npc.HasValidTarget)
            {
                if(deathTimer != 0)
                {
                    deathTimer = 0;
                    npc.alpha = 0;
                    npc.netUpdate = true;
                }

                HandleStage();

                Player target = Main.player[npc.target];
                if (npc.position.X > target.position.X)
                {
                    npc.spriteDirection = -1;
                }
                else if (npc.position.X + npc.width < target.position.X + target.width)
                {
                    npc.spriteDirection = 1;
                }

                HandleMoving();
                HandleAttacking();

                FindPostFrame();
            }
            else
            {
                npc.velocity *= 0.85f;
                deathTimer++;
                if (deathTimer > 240)
                {
                    npc.alpha += 2;
                }
                if (deathTimer > 360)
                {
                    npc.active = false;
                    npc.netUpdate = true;
                }
            }
        }

        private void HandleMoving()
        {
            npc.velocity *= 0.85f;

            Player[] players = Main.player.ToArray();
            Vector2 distance = players[npc.target].Center - npc.Center;

            // Stay above player
            if (Math.Abs(distance.Y) < 100)
            {
                float yValue = (1 - Math.Abs(distance.Y) / (80));
                if (yValue > 0.2f)
                {
                    yValue = 0.2f;
                }
                npc.velocity.Y -= yValue;
            }
            else if (distance.Y > 140)
            {
                float yValue = (1 - distance.Y / (110));
                if (yValue > 0.5f)
                {
                    yValue = 0.5f;
                }
                npc.velocity.Y -= yValue;
            }

            if (distance.Length() > 280)
            {
                distance /= distance.Length();
                npc.velocity += distance / 2;
            }
            else if (distance.Length() < 60)
            {
                distance /= distance.Length();
                npc.velocity -= distance / 3;
            }

            // Lean when moving
            if (npc.velocity.X < -1 && npc.rotation >= -0.10f)
            {
                npc.rotation -= 0.01f;
            }
            else if (npc.velocity.X > 1 && npc.rotation <= 0.10f)
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

            // Make a floating effect
            float floatVelocity = (float) (floatTimer / 10 - 3) / 40;

            npc.velocity.Y += floatVelocity;

            if (!floatDirection)
            {
                floatTimer = (byte)(floatTimer + 1);
                if(floatTimer >= 60)
                {
                    floatDirection = !floatDirection;
                    npc.netUpdate = true;
                }
            }
            else
            {
                floatTimer = (byte)(floatTimer - 1);
                if (floatTimer <= 0)
                {
                    floatDirection = !floatDirection;
                    npc.netUpdate = true;
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
                        npc.netUpdate = true;
                    } else if (blastTimer > 180 && triangleReady)
                    {
                        triangleReady = false;
                        npc.netUpdate = true;
                        TriangleBlast();
                    } else if (blastTimer > 150)
                    {
                        triangleStance = true;
                        npc.netUpdate = true;
                    }
                    
                    break;
                case 1:
                    if (clapped)
                    {
                        clapped = false;
                        npc.netUpdate = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            FistAttackNew();
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
                        npc.netUpdate = true;
                    }
                    else if (blastTimer > 180 && triangleReady)
                    {
                        triangleReady = false;
                        npc.netUpdate = true;
                        TriangleBlast();
                    }
                    else if (blastTimer > 150)
                    {
                        triangleStance = true;
                        npc.netUpdate = true;
                    }
                    if (clapped)
                    {
                        clapped = false;
                        npc.netUpdate = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            FistAttackNew();
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
                        npc.netUpdate = true;
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
                            npc.netUpdate = true;
                        }
                    }**/
                    break;
                case 3:
                    if (!charging)
                    {
                        chargeTimer++;
                    }
                    if(chargeTimer > 600)
                    {
                        charging = true;
                        chargeTimer = 0;
                        npc.netUpdate = true;
                    }
                    if (charged)
                    {
                        charged = false;
                        npc.netUpdate = true;
                    }

                    blastTimer++;
                    if (blastTimer > 110)
                    {
                        triangleReady = true;
                        blastTimer = 0;
                        //triangleStance = false;
                        npc.netUpdate = true;
                    }
                    else if (blastTimer > 80 && triangleReady)
                    {
                        triangleReady = false;
                        npc.netUpdate = true;
                        TriangleBlast();
                    }
                    else if (blastTimer > 50)
                    {
                        triangleStance = true;
                        npc.netUpdate = true;
                    }
                    if (clapped)
                    {
                        clapped = false;
                        npc.netUpdate = true;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            FistAttackNewMulti();
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
            float percentHealth = ((float)npc.life / (float)npc.lifeMax);
            if(percentHealth >= 0.85f)
            {
                if(stage != 0)
                {
                    if(Main.netMode != NetmodeID.MultiplayerClient && stage != 0)
                    {
                        stage = 0;
                        npc.netUpdate = true;
                    }
                }
            } else if (percentHealth > 0.70f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && stage != 1)
                {
                    stage = 1;
                    npc.netUpdate = true;
                }
            }
            else if (percentHealth > 0.40f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && stage != 2)
                {
                    stage = 2;
                    npc.netUpdate = true;
                }
            }
            else if (percentHealth <= 0.40f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && stage != 3)
                {
                    stage = 3;
                    npc.netUpdate = true;
                }
            }
        }

        private void Clap()
        {
            npc.netUpdate = true;
            clapping = true;
        }

        private void FistAttack()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player target = Main.player[npc.target];
                int type = ModContent.ProjectileType<ArmProjectileNew>();

                // Get a random point with negative values and find its direction.
                Vector2 rand = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 0));
                rand.Normalize();

                var projectile = Projectile.NewProjectile(target.position + rand * 200, new Vector2(), type, HAND_DAMAGE, 9.5f, 255, 0, npc.target);
                //projectile.ai = new float[2];
                //projectile.ai[0] = 0;
                //projectile.ai[1] = npc.target;
            }
        }

        private void FistAttackNew()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 up = new Vector2(0, -1);
                Vector2 down = new Vector2(0, 1);
                Vector2 left = new Vector2(-1, 0);
                Vector2 right = new Vector2(1, 0);

                Player target = Main.player[npc.target];
                float distance = (target.position - npc.position).Length();
                if (distance < 1350)
                {
                    int type = ModContent.ProjectileType<PortalCharge>();

                    // Get a random point with negative values and find its direction.
                    float rand = Main.rand.NextFloat(0, MathHelper.TwoPi);
                    /**
                    var projectile = Projectile.NewProjectile(target.position + (up * 250).RotatedBy(rand), new Vector2(), type, HAND_DAMAGE, 9.5f, 255, 0, npc.target);
                    projectile = Projectile.NewProjectile(target.position + (down * 250).RotatedBy(rand), new Vector2(), type, HAND_DAMAGE, 9.5f, 255, 0, npc.target);
                    projectile = Projectile.NewProjectile(target.position + (left * 250).RotatedBy(rand), new Vector2(), type, HAND_DAMAGE, 9.5f, 255, 0, npc.target);
                    projectile = Projectile.NewProjectile(target.position + (right * 250).RotatedBy(rand), new Vector2(), type, HAND_DAMAGE, 9.5f, 255, 0, npc.target);**/

                    NPC.NewNPC((int)(target.position + (up * 250).RotatedBy(rand)).X, (int)(target.position + (up * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, npc.target);
                    NPC.NewNPC((int)(target.position + (down * 250).RotatedBy(rand)).X, (int)(target.position + (down * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, npc.target);
                    NPC.NewNPC((int)(target.position + (left * 250).RotatedBy(rand)).X, (int)(target.position + (left * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, npc.target);
                    NPC.NewNPC((int)(target.position + (right * 250).RotatedBy(rand)).X, (int)(target.position + (right * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, npc.target);

                    //projectile.ai = new float[2];
                    //projectile.ai[0] = 0;
                    //projectile.ai[1] = npc.target;
                }
            }
        }

        private void FistAttackNewMulti()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                foreach (Player p in Main.player) {
                    if (p.active && !p.dead)
                    {
                        float distance = (p.position - npc.position).Length();
                        if (distance < 1350) {
                            Player target = p;

                            Vector2 up = new Vector2(0, -1);
                            Vector2 down = new Vector2(0, 1);
                            Vector2 left = new Vector2(-1, 0);
                            Vector2 right = new Vector2(1, 0);

                            int type = ModContent.ProjectileType<PortalCharge>();

                            // Get a random point with negative values and find its direction.
                            float rand = Main.rand.NextFloat(0, MathHelper.TwoPi);

                            /**var projectile = Projectile.NewProjectile(p.position + (up * 250).RotatedBy(rand), new Vector2(), type, HAND_DAMAGE, 9.5f, 255, 0, p.whoAmI);
                            projectile = Projectile.NewProjectile(p.position + (down * 250).RotatedBy(rand), new Vector2(), type, HAND_DAMAGE, 9.5f, 255, 0, p.whoAmI);
                            projectile = Projectile.NewProjectile(p.position + (left * 250).RotatedBy(rand), new Vector2(), type, HAND_DAMAGE, 9.5f, 255, 0, p.whoAmI);
                            projectile = Projectile.NewProjectile(p.position + (right * 250).RotatedBy(rand), new Vector2(), type, HAND_DAMAGE, 9.5f, 255, 0, p.whoAmI);**/

                            NPC.NewNPC((int)(target.position + (up * 250).RotatedBy(rand)).X, (int)(target.position + (up * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, p.whoAmI);
                            NPC.NewNPC((int)(target.position + (down * 250).RotatedBy(rand)).X, (int)(target.position + (down * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, p.whoAmI);
                            NPC.NewNPC((int)(target.position + (left * 250).RotatedBy(rand)).X, (int)(target.position + (left * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, p.whoAmI);
                            NPC.NewNPC((int)(target.position + (right * 250).RotatedBy(rand)).X, (int)(target.position + (right * 250).RotatedBy(rand)).Y, NPCType<Arm>(), 0, 0, p.whoAmI);
                        }
                    }
                }
            }
        }

        private void TriangleBlast()
        {
            Vector2 up = new Vector2(0, -20);
            Vector2 spawn = npc.Center + up;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Player target = Main.player[npc.target];
                int type = ModContent.ProjectileType<TriangleBlast>();

                float rotation = (float)Math.Atan2(target.Center.Y - spawn.Y, target.Center.X - spawn.X) + MathHelper.PiOver2;

                var projectile = Projectile.NewProjectile(spawn, up.RotatedBy(rotation - 0.4f) * 0.5f, type, HAND_DAMAGE, 9.5f, 255, 0, npc.target);
                projectile = Projectile.NewProjectile(spawn, up.RotatedBy(rotation + 0.4f) * 0.5f, type, HAND_DAMAGE, 9.5f, 255, 0, npc.target);
                projectile = Projectile.NewProjectile(spawn, up.RotatedBy(rotation) * 0.5f, type, HAND_DAMAGE, 9.5f, 255, 0, npc.target);
                //projectile.ai = new float[2];
                //projectile.ai[0] = 0;
                //projectile.ai[1] = npc.target;
            }
            if (Main.netMode != NetmodeID.Server)
            {
                Main.PlaySound(SoundID.Item72.SoundId, (int)spawn.X, (int)spawn.Y, SoundID.Item72.Style, 0.80f, -0.2f);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (clapping)
            {
                if(npc.frameCounter < 6)
                {
                    npc.frame.Y = frameHeight;
                } else if (npc.frameCounter < 12)
                {
                    npc.frame.Y = frameHeight * 2;
                }
                else if (npc.frameCounter < 24)
                {
                    npc.frame.Y = frameHeight * 3;
                }
                else if (npc.frameCounter < 30)
                {
                    npc.frame.Y = frameHeight * 4;
                }
                else
                {
                    npc.frame.Y = 0;
                    clapping = false;
                    clapped = true;
                    if (Main.netMode != NetmodeID.Server)
                    {
                        Main.PlaySound(SoundID.Item37.SoundId, (int)npc.position.X, (int)npc.position.Y, SoundID.Item37.Style, 0.90f, -0.5f);
                    }
                }
                npc.frameCounter++;
            }
            else
            {
                npc.frame.Y = 0;
                npc.frameCounter = 0;
            }
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            dontDamage = (player.Center - npc.Center).Length() > SPHERE_RADIUS;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            dontDamage = player.active && (player.Center - npc.Center).Length() > SPHERE_RADIUS;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (dontDamage)
            {
                damage = 0;
                crit = true;
                dontDamage = false;
                Main.PlaySound(npc.HitSound, npc.position);
                return false;
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D border = mod.GetTexture("NPCs/EnlightenmentIdol/SphereBorder");

            spriteBatch.Draw(mod.GetTexture("NPCs/EnlightenmentIdol/IdolSphere"), npc.Center - Main.screenPosition, null, Color.White * (40f / 255f) * ((255 - npc.alpha) / 255f), 0f, new Vector2(SPHERE_RADIUS, SPHERE_RADIUS), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(border, npc.Center - Main.screenPosition, null, Color.White * ((255 - npc.alpha) / 255f), 0f, new Vector2(border.Width / 2, border.Height / 2), 1f, SpriteEffects.None, 0f);

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 drawPos = npc.position - Main.screenPosition + new Vector2(npc.width / 2, npc.height / 2 + 9);

            int TopArmsFrameHeight = TopArms.Height / 6;
            Vector2 TopArmsOrigin = new Vector2(TopArms.Width / 2, TopArmsFrameHeight / 2);
            
            Rectangle TopArmsFrame = new Rectangle(0, topFrame * TopArmsFrameHeight, TopArms.Width, TopArmsFrameHeight);

            spriteBatch.Draw(TopArms, drawPos, TopArmsFrame, Color.White * ((float)(255 - npc.alpha) / 255f), npc.rotation, TopArmsOrigin, 1.0f, 0, 0);

            int MiddleArmsFrameHeight = MiddleArms.Height / 27;
            Vector2 MiddleArmsOrigin = new Vector2(MiddleArms.Width / 2, MiddleArmsFrameHeight / 2);
            Rectangle MiddleArmsFrame = new Rectangle(0, middleFrame * MiddleArmsFrameHeight, MiddleArms.Width, MiddleArmsFrameHeight);

            spriteBatch.Draw(MiddleArms, drawPos, MiddleArmsFrame, Color.White * ((float)(255 - npc.alpha) / 255f), npc.rotation, MiddleArmsOrigin, 1.0f, 0, 0);

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
                    npc.netUpdate = true;
                }
                topArmsCounter++;
            }
            else
            {
                newTopFrame = 0;
                topArmsCounter = 0;
                npc.netUpdate = true;
            }
            if (newTopFrame != topFrame)
            {
                topFrame = newTopFrame;
                npc.netUpdate = true;
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
                            Main.PlaySound(SoundLoader.customSoundType, (int)npc.Center.X, (int)npc.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Thunder"));
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
                    npc.netUpdate = true;
                }
                middleArmsCounter++;
            }
            else
            {
                newMiddleFrame = 0;
                middleArmsCounter = 0;
                npc.netUpdate = true;
            }
            if (newMiddleFrame != middleFrame)
            {
                middleFrame = newMiddleFrame;
                npc.netUpdate = true;
            }
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White * ((float)(255 - npc.alpha) / 255f);
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

        public bool gotReady
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
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        public float topArmsCounter
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }

        public float middleArmsCounter
        {
            get => npc.ai[2];
            set => npc.ai[2] = value;
        }

        public float chargeTimer
        {
            get => npc.ai[3];
            set => npc.ai[3] = value;
        }


        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(flags);
            writer.Write((short)clapTimer);
            writer.Write(blastTimer);
            writer.Write((short)deathTimer);
            writer.Write((short)triangleTimer);
            writer.Write(topFrame);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            flags = reader.ReadByte();
            clapTimer = (int) reader.ReadInt16();
            blastTimer = reader.ReadByte();
            deathTimer = (int)reader.ReadInt16();
            triangleTimer = reader.ReadInt16();
            topFrame = reader.ReadByte();
        }

        public override void NPCLoot()
        {
            if (!HighlanderWorld.downedEnlightenmentIdol)
            {
                HighlanderWorld.downedEnlightenmentIdol = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }

            if (Main.rand.NextBool(10)) // Boss Trophy
            {
                //Item.NewItem(npc.getRect(), ItemType<HauntedHatterTrophy>());
            }

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.NextBool(2))
                {
                    //Item.NewItem(npc.getRect(), ItemType<SpiritShears>());
                }
                else
                {
                    //Item.NewItem(npc.getRect(), ItemType<AncientStoneBlaster>());
                }
                if (Main.rand.NextBool(7)) // Boss Vanity
                {
                    //Item.NewItem(npc.getRect(), ItemType<GhostlyGibus>());
                }
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Idol of Enlightenment";
            potionType = ItemID.HealingPotion;
        }

    }
}
