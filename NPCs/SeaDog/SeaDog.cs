using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Highlander.Items.SeaDog;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.SeaDog
{
    [AutoloadBossHead]
    class SeaDog : ModNPC
    {

        private BitsByte flags;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 14; // make sure to set this for your modnpcs.
            DisplayName.SetDefault("Sea Dog");
        }

        public override void SetDefaults()
        {
            //npc.frame = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);
            drawOffsetY = 0;// -52;
            npc.aiStyle = -1;
            npc.lifeMax = 2600;
            npc.damage = 14;
            npc.defense = 9;
            npc.knockBackResist = 0f;
            npc.width = 50;
            npc.height = 70;
            npc.npcSlots = 50f;
            npc.boss = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath18;
            npc.value = 30000;
            npc.alpha = 0;
            music = MusicID.Boss2;
            musicPriority = MusicPriority.BossMedium;
            bossBag = ItemType<SeaDogBag>();
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.65f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.9f);
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
                Vector2 vectorToTarget = target.Center - npc.Center;
                int direction;
                if(vectorToTarget.X > 0)
                {
                    direction = 1;
                    npc.spriteDirection = 1;
                    npc.direction = 1;
                }
                else
                {
                    direction = -1;
                    npc.spriteDirection = -1;
                    npc.direction = -1;
                }

                if (throwTimer <= 0)
                {
                    Throw(vectorToTarget);
                }
                else if (throwFrameTimer <= 0 && npc.velocity.X == 0 && jumpTimer <= 0)
                {
                    Jump();
                }
                else if (throwFrameTimer <= 1 && Math.Abs(vectorToTarget.X) > 12)
                {
                    if(npc.velocity.X != direction * 3)
                    {
                        npc.netUpdate = true;
                    }
                    npc.velocity.X = direction * 3;
                }

                if(jumpTimer > 0)
                {
                    jumpTimer--;
                }
                if (throwTimer > 0)
                {
                    throwTimer--;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = 0;

            if(throwFrameTimer > 0)
            {
                if(throwFrameTimer > 18)
                {
                    npc.frame.Y = frameHeight * 10;
                }
                else if (throwFrameTimer > 12)
                {
                    npc.frame.Y = frameHeight * 11;
                }
                else if (throwFrameTimer > 6)
                {
                    npc.frame.Y = frameHeight * 12;
                }
                else if (throwFrameTimer > 0)
                {
                    npc.frame.Y = frameHeight * 13;
                }
                throwFrameTimer--;
            }
            else if(npc.velocity.Y != 0)
            {
                npc.frame.Y = frameHeight * 9;
            }
            else if (npc.velocity.X != 0)
            {
                if(npc.frameCounter < 6)
                {
                    npc.frame.Y = frameHeight * 1;
                }
                else if(npc.frameCounter < 12)
                {
                    npc.frame.Y = frameHeight * 2;
                }
                else if (npc.frameCounter < 18)
                {
                    npc.frame.Y = frameHeight * 3;
                }
                else if (npc.frameCounter < 24)
                {
                    npc.frame.Y = frameHeight * 4;
                }
                else if (npc.frameCounter < 30)
                {
                    npc.frame.Y = frameHeight * 5;
                }
                else if (npc.frameCounter < 36)
                {
                    npc.frame.Y = frameHeight * 6;
                }
                else if (npc.frameCounter < 42)
                {
                    npc.frame.Y = frameHeight * 7;
                }
                else if (npc.frameCounter < 48)
                {
                    npc.frame.Y = frameHeight * 8;
                }
                npc.frameCounter = (npc.frameCounter + Math.Abs(npc.velocity.X / 3)) % 48;
            }
        }

        private void Jump()
        {
            npc.velocity.Y -= 8;
            jumpTimer = 60;
            npc.netUpdate = true;
        }

        private void Throw(Vector2 vectorToPlayer)
        {
            npc.velocity.X = 0;

            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 velocity = vectorToPlayer;
                velocity.Normalize();
                velocity *= 8;
                velocity.Y -= 6;

                throwTimer = 180;
                throwFrameTimer = 24;
                npc.netUpdate = true;
                var projectile = Projectile.NewProjectile(npc.Center, velocity, ProjectileType<SeaDogProjectile>(), 13, 9.5f);
            }
        }

        private float jumpTimer
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        private float throwTimer
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }

        private float throwFrameTimer
        {
            get => npc.ai[2];
            set => npc.ai[2] = value;
        }

        public override void NPCLoot()
        {
            if (!HighlanderWorld.downedSeaDog)
            {
                HighlanderWorld.downedSeaDog = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }

            if (Main.rand.NextBool(10))
            {
                Item.NewItem(npc.getRect(), ItemType<SeaDogTrophy>());
            }

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.NextBool(2))
                {
                    Item.NewItem(npc.getRect(), ItemType<FeralFrenzy>());
                }
                else
                {
                    Item.NewItem(npc.getRect(), ItemType<BrokenBlunderbuss>());
                }
                if (Main.rand.NextBool(7))
                {
                    Item.NewItem(npc.getRect(), ItemType<SeaDogMask>());
                }
                if (Main.rand.NextBool())
                {
                    Item.NewItem(npc.getRect(), ItemID.SpelunkerPotion, 2);
                }
                else
                {
                    Item.NewItem(npc.getRect(), ItemID.GillsPotion, 2);
                }

                int rand = Main.rand.Next(30, 50);
                Item.NewItem(npc.getRect(), ItemID.GoldOre, rand);
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Sea Dog";
            potionType = ItemID.LesserHealingPotion;
        }

    }
}
