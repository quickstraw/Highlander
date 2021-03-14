using Highlander.Items.Accessories.Shields;
using Highlander.Items.Weapons.Spears;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.Vermin
{
	class BlightedVermin : ModNPC
	{
        Rectangle AttackRange;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[npc.type] = 15; // make sure to set this for your modnpcs.
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 36;
			npc.aiStyle = -1; // This npc has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
			npc.damage = 24;
			npc.defense = 12;
			npc.lifeMax = 45;
            npc.knockBackResist = 0.5f;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath47;
			npc.value = 100;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Confused] = false; // npc default to being immune to the Confused debuff. Allowing confused could be a little more work depending on the AI. npc.confused is true while the npc is confused.
            drawOffsetY = -2;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			// we would like this npc to spawn in the overworld.
			return SpawnCondition.Underground.Chance * 0.1f;
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
                if (npc.confused)
                {
                    vectorToTarget *= -1;
                }
                int direction = npc.direction;

                if (attackFrameTimer <= 0)
                {
                    if (vectorToTarget.X > 0)
                    {
                        direction = 1;
                        npc.spriteDirection = -1;
                        npc.direction = 1;
                    }
                    else
                    {
                        direction = -1;
                        npc.spriteDirection = 1;
                        npc.direction = -1;
                    }
                }

                AttackRange = new Rectangle((int) npc.position.X - (npc.width * 2 / 3), (int) npc.position.Y, npc.width * 2, npc.height);

                if (!npc.confused && AttackRange.Intersects(target.Hitbox) && attackFrameTimer <= 0 && npc.collideY)
                {
                    Attack();
                }
                else if (attackFrameTimer <= 0 && npc.velocity.X == 0 && jumpTimer <= 0 && npc.collideY)
                {
                    Jump();
                }
                else if (attackFrameTimer <= 1 && Math.Abs(vectorToTarget.X) > 12)
                {
                    if (!npc.confused)
                    {
                        if (npc.velocity.X != direction * 1.5f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.velocity.X = direction * 1.5f;
                    }
                    else
                    {
                        if (npc.velocity.X != direction * 0.9f)
                        {
                            npc.netUpdate = true;
                        }
                        npc.velocity.X = direction * 0.9f;
                    }
                } else if(attackFrameTimer < 19 && attackFrameTimer >= 18)
                {
                    npc.netUpdate = true;
                    var projectile = Projectile.NewProjectile(npc.Center + new Vector2(npc.direction * npc.width, 0), new Vector2(), ProjectileType<BlightedVerminSpear>(), npc.damage, 9.5f);
                }

                if (jumpTimer > 0)
                {
                    jumpTimer--;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = 0;

            if (attackFrameTimer > 0)
            {
                if (attackFrameTimer > 33)
                {
                    npc.frame.Y = frameHeight * 10;
                }
                else if (attackFrameTimer > 21)
                {
                    npc.frame.Y = frameHeight * 11;
                }
                else if (attackFrameTimer > 18)
                {
                    npc.frame.Y = frameHeight * 12;
                }
                else if (attackFrameTimer > 6)
                {
                    npc.frame.Y = frameHeight * 13;
                }
                else if (attackFrameTimer > 0)
                {
                    npc.frame.Y = frameHeight * 14;
                }
                attackFrameTimer--;
            }
            else if (npc.velocity.Y != 0 && !npc.collideY)
            {
                npc.frame.Y = frameHeight * 9;
            }
            else if (npc.velocity.X != 0)
            {
                if (npc.frameCounter < 6)
                {
                    npc.frame.Y = frameHeight * 1;
                }
                else if (npc.frameCounter < 12)
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
                npc.frameCounter = (npc.frameCounter + Math.Abs(npc.velocity.X / 1.5)) % 48;
            }
        }

        private void Jump()
        {
            npc.velocity.Y -= 8;
            jumpTimer = 60;
            npc.netUpdate = true;
        }

        private void Attack()
        {
            npc.velocity.X = 0;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                attackTimer = 180;
                attackFrameTimer = 39;
                npc.netUpdate = true;
            }
        }

        private float jumpTimer
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        private float attackTimer
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }

        private float attackFrameTimer
        {
            get => npc.ai[2];
            set => npc.ai[2] = value;
        }

        public override void NPCLoot()
        {
            bool getSpear = Main.rand.NextBool(12);
            bool getShield = Main.rand.NextBool(12);
            if (getSpear)
            {
                Item.NewItem(npc.getRect(), ItemType<VerminSpear>());
            }
            if (getShield)
            {
                Item.NewItem(npc.getRect(), ItemType<VerminShield>());
            }
            if (Main.rand.NextBool(50))
            {
                Item.NewItem(npc.getRect(), ItemID.Shackle);
            }

            string prefix = "BlightedVermin";
            string path = "Gores/Vermin/BlightedVermin/";

            Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 2, mod.GetGoreSlot(path + prefix + "ArmGore"), 1f);
            if (Main.rand.NextBool())
            {
                Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 1, mod.GetGoreSlot(path + prefix + "BodyGore"), 1f);
            }
            else
            {
                Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 1, mod.GetGoreSlot(path + prefix + "BodyAltGore"), 1f);
            }
            if (Main.rand.NextBool())
            {
                Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 2, mod.GetGoreSlot(path + prefix + "HeadGore"), 1f);
            }
            if (!getSpear)
            {
                Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 2, mod.GetGoreSlot(path + prefix + "SpearGore"), 1f);
            }
            if (!getShield)
            {
                Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 2, mod.GetGoreSlot(path + prefix + "ShieldGore"), 1f);
            }
        }
    }
}
