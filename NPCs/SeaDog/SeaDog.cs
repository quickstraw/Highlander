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
using Terraria.GameContent.ItemDropRules;

namespace Highlander.NPCs.SeaDog
{
    [AutoloadBossHead]
    class SeaDog : ModNPC
    {

        private BitsByte flags;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14; // make sure to set this for your modnpcs.
            DisplayName.SetDefault("Sea Dog");
        }

        public override void SetDefaults()
        {
            //NPC.frame = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);
            DrawOffsetY = 0;// -52;
            NPC.aiStyle = -1;
            NPC.lifeMax = 2600;
            NPC.damage = 14;
            NPC.defense = 9;
            NPC.knockBackResist = 0f;
            NPC.width = 50;
            NPC.height = 70;
            NPC.npcSlots = 50f;
            NPC.boss = true;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath18;
            NPC.value = 30000;
            NPC.alpha = 0;
            Music = MusicID.Boss2;
            //musicPriority = MusicPriority.BossMedium;
            //bossBag = ItemType<SeaDogBag>();
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.65f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.9f);
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest(false);
            }
            if (NPC.HasValidTarget)
            {
                Player target = Main.player[NPC.target];
                Vector2 vectorToTarget = target.Center - NPC.Center;
                int direction;
                if(vectorToTarget.X > 0)
                {
                    direction = 1;
                    NPC.spriteDirection = 1;
                    NPC.direction = 1;
                }
                else
                {
                    direction = -1;
                    NPC.spriteDirection = -1;
                    NPC.direction = -1;
                }

                if (throwTimer <= 0)
                {
                    Throw(vectorToTarget);
                }
                else if (throwFrameTimer <= 0 && NPC.velocity.X == 0 && jumpTimer <= 0)
                {
                    Jump();
                }
                else if (throwFrameTimer <= 1 && Math.Abs(vectorToTarget.X) > 12)
                {
                    if(NPC.velocity.X != direction * 3)
                    {
                        NPC.netUpdate = true;
                    }
                    NPC.velocity.X = direction * 3;
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
            NPC.frame.Y = 0;

            if(throwFrameTimer > 0)
            {
                if(throwFrameTimer > 18)
                {
                    NPC.frame.Y = frameHeight * 10;
                }
                else if (throwFrameTimer > 12)
                {
                    NPC.frame.Y = frameHeight * 11;
                }
                else if (throwFrameTimer > 6)
                {
                    NPC.frame.Y = frameHeight * 12;
                }
                else if (throwFrameTimer > 0)
                {
                    NPC.frame.Y = frameHeight * 13;
                }
                throwFrameTimer--;
            }
            else if(NPC.velocity.Y != 0)
            {
                NPC.frame.Y = frameHeight * 9;
            }
            else if (NPC.velocity.X != 0)
            {
                if(NPC.frameCounter < 6)
                {
                    NPC.frame.Y = frameHeight * 1;
                }
                else if(NPC.frameCounter < 12)
                {
                    NPC.frame.Y = frameHeight * 2;
                }
                else if (NPC.frameCounter < 18)
                {
                    NPC.frame.Y = frameHeight * 3;
                }
                else if (NPC.frameCounter < 24)
                {
                    NPC.frame.Y = frameHeight * 4;
                }
                else if (NPC.frameCounter < 30)
                {
                    NPC.frame.Y = frameHeight * 5;
                }
                else if (NPC.frameCounter < 36)
                {
                    NPC.frame.Y = frameHeight * 6;
                }
                else if (NPC.frameCounter < 42)
                {
                    NPC.frame.Y = frameHeight * 7;
                }
                else if (NPC.frameCounter < 48)
                {
                    NPC.frame.Y = frameHeight * 8;
                }
                NPC.frameCounter = (NPC.frameCounter + Math.Abs(NPC.velocity.X / 3)) % 48;
            }
        }

        private void Jump()
        {
            NPC.velocity.Y -= 8;
            jumpTimer = 60;
            NPC.netUpdate = true;
        }

        private void Throw(Vector2 vectorToPlayer)
        {
            NPC.velocity.X = 0;

            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                var source = NPC.GetSource_FromAI();

                Vector2 velocity = vectorToPlayer;
                velocity.Normalize();
                velocity *= 8;
                velocity.Y -= 6;

                throwTimer = 180;
                throwFrameTimer = 24;
                NPC.netUpdate = true;
                var projectile = Projectile.NewProjectile(source, NPC.Center, velocity, ProjectileType<SeaDogProjectile>(), 13, 9.5f);
            }
        }

        private float jumpTimer
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private float throwTimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        private float throwFrameTimer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        public override void OnKill()
        {
            if (!HighlanderWorld.downedSeaDog)
            {
                HighlanderWorld.downedSeaDog = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ItemType<SeaDogBag>()));

            npcLoot.Add(ItemDropRule.Common(ItemType<SeaDogTrophy>(), 10));

            // All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            // Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            // Boss masks are spawned with 1/7 chance
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SeaDogMask>(), 7));
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ItemType<FeralFrenzy>(), ItemType<BrokenBlunderbuss>()));
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, ItemID.SpelunkerPotion, ItemID.GillsPotion));
            notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.GoldOre, 1, 30, 50));

            npcLoot.Add(notExpertRule);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Sea Dog";
            potionType = ItemID.LesserHealingPotion;
        }

    }
}
