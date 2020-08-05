using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.NPCs.EnlightenmentIdol
{
    class EnlightenmentIdol : ModNPC
    {
        private const int BASE_DEF = 20;
        private const int HAND_DAMAGE = 18;
        private const int SPHERE_RADIUS = 450;

        private BitsByte flags = new BitsByte();
        private int clapTimer = 0;
        private byte fistTimer = 0;
        private byte floatTimer = 0;
        private bool dontDamage;

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
            music = MusicID.Boss1;
            musicPriority = MusicPriority.BossMedium;
            //bossBag = ItemType<HauntedHatterBag>();
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.TargetClosest(false);
            }
            if (npc.HasValidTarget)
            {
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

            if (distance.Length() > 180)
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
            if (clapped)
            {
                clapped = false;
                fistTimer = 60;
                npc.netUpdate = true;
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    FistAttack();
                }
            }
            if (!clapping)
            {
                clapTimer++;
            }
            if(clapTimer >= 300)
            {
                clapTimer = 0;
                Clap();
            }
            if(fistTimer > 0)
            {
                fistTimer--;
                if (fistTimer % 15 == 0)
                {
                    FistAttack();
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
            Player target = Main.player[npc.target];
            int type = ModContent.ProjectileType<ArmProjectile>();

            // Get a random point with negative values and find its direction.
            Vector2 rand = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 0));
            rand.Normalize();

            var projectile = Projectile.NewProjectileDirect(target.position + rand * 200, new Vector2(), type, HAND_DAMAGE, 9.5f, 255);
            projectile.rotation = (float)Math.Atan2(target.Center.Y - projectile.Center.Y, target.Center.X - projectile.Center.X) + MathHelper.Pi;

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
                        Main.PlaySound(SoundID.Item37.SoundId, (int)npc.position.X, (int)npc.position.Y, SoundID.Item37.Style, 0.60f, -0.5f);
                    }
                }
                npc.frameCounter++;
            }
            else
            {
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

            spriteBatch.Draw(mod.GetTexture("NPCs/EnlightenmentIdol/IdolSphere"), npc.Center - Main.screenPosition, null, Color.White * (40f / 255f), 0f, new Vector2(SPHERE_RADIUS, SPHERE_RADIUS), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(border, npc.Center - Main.screenPosition, null, Color.White * (255f / 255f), 0f, new Vector2(border.Width / 2, border.Height / 2), 1f, SpriteEffects.None, 0f);

            return true;
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


        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(flags);
            writer.Write((short)clapTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            flags = reader.ReadByte();
            clapTimer = (int) reader.ReadInt16();
        }

    }
}
