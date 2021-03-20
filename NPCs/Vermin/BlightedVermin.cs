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
        float speed = 1.5f;

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

                Tile tile = Main.tile[(int)(npc.Center.X / 16), (int)((npc.position.Y + npc.height) / 16) + 1];
                Vector2 tileCoords = new Vector2((int)(npc.Center.X / 16), (int)((npc.position.Y + npc.height) / 16) + 1);
                Tile targetTile = Main.tile[(int)(target.Center.X / 16), (int)((target.position.Y + target.height) / 16) + 1];
                Vector2 targetTileCoords = new Vector2((int)(target.Center.X / 16), (int)((target.position.Y + target.height) / 16) + 1);

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

                float yDiff = tileCoords.Y - targetTileCoords.Y;

                WalkOverSlopes();

                AttackRange = new Rectangle((int) npc.position.X - (npc.width * 2 / 3), (int) npc.position.Y, npc.width * 2, npc.height);

                if (!npc.confused && AttackRange.Intersects(target.Hitbox) && attackFrameTimer <= 0 && npc.collideY)
                {
                    Attack();
                }
                else if ((npc.velocity.X == 0 || yDiff >= 2) && jumpTimer <= 0 && Math.Abs(vectorToTarget.X) > npc.width / 2)
                {
                    Jump();
                }
                else if (attackFrameTimer <= 1 && Math.Abs(vectorToTarget.X) > 12)
                {
                    if (!npc.confused)
                    {
                        if (npc.velocity.X != direction * speed)
                        {
                            npc.netUpdate = true;
                        }
                        npc.velocity.X += direction * speed / 3;
                        if (Math.Abs(npc.velocity.X) > speed)
                        {
                            npc.velocity.X = direction * speed;
                        }
                    }
                    else
                    {
                        if (npc.velocity.X != direction * 0.6f * speed)
                        {
                            npc.netUpdate = true;
                        }
                        npc.velocity.X = direction * 0.6f * speed;
                    }
                    if (npc.collideY && tile.type != 0)
                    {
                        bool hole = CheckHole();
                        if (hole)
                        {
                            Jump();
                        }
                    }
                } else if(attackFrameTimer < 19 && attackFrameTimer >= 18)
                {
                    npc.netUpdate = true;
                    if (Main.expertMode)
                    {
                        Projectile projectile = Projectile.NewProjectileDirect(npc.Center + new Vector2(npc.direction * npc.width, 0), new Vector2(), ProjectileType<BlightedVerminSpear>(), npc.damage / 3, 9.5f);
                    }
                    else
                    {
                        Projectile projectile = Projectile.NewProjectileDirect(npc.Center + new Vector2(npc.direction * npc.width, 0), new Vector2(), ProjectileType<BlightedVerminSpear>(), npc.damage / 2, 9.5f);
                    }
                    Main.PlaySound(SoundID.Item1.SoundId, (int)npc.Center.X, (int)npc.Center.Y, SoundID.Item1.Style, 0.90f, +0.5f);
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
            npc.velocity.Y -= 7;
            jumpTimer = 60;
            npc.netUpdate = true;
        }

        private bool CheckHole()
        {
            int x = (int)(npc.Center.X / 16);
            int y = (int)((npc.position.Y + npc.height) / 16);
            bool hole1 = false;
            bool hole1Deep = false;
            bool hole2 = false;

            Tile tile = Main.tile[x + npc.direction, y + 1];
            if (tile.type == 0)
            {
                hole1 = true;
            }
            tile = Main.tile[x + npc.direction, y + 2];
            if (tile.type == 0)
            {
                hole1Deep = true;
            }
            tile = Main.tile[x + npc.direction * 2, y + 1];
            if (tile.type == 0)
            {
                hole2 = true;
            }
            return hole1 && hole1Deep && hole2;
        }

        private void WalkOverSlopes()
        {
            Point tileCoordinates1 = npc.Center.ToTileCoordinates();
            if (WorldGen.InWorld((int)tileCoordinates1.X, (int)tileCoordinates1.Y, 5) && !npc.noGravity)
            {
                Vector2 cPosition;
                int cWidth;
                int cHeight;
                this.GetTileCollisionParameters(out cPosition, out cWidth, out cHeight);
                Vector2 vector2 = npc.position - cPosition;
                Collision.StepUp(ref cPosition, ref npc.velocity, cWidth, cHeight, ref npc.stepSpeed, ref npc.gfxOffY, 1, false, 0);
                npc.position = cPosition + vector2;
                npc.netUpdate = true;
            }
        }

        private void GetTileCollisionParameters(out Vector2 cPosition, out int cWidth, out int cHeight)
        {
            cPosition = npc.position;
            cWidth = npc.width;
            cHeight = npc.height;
        }

        private void Attack()
        {
            npc.velocity.X = 0;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
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
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                string prefix = "BlightedVermin";
                string path = "Gores/Vermin/BlightedVermin/";

                Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 2, mod.GetGoreSlot(path + prefix + "ArmGore"), 1f);
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 1, mod.GetGoreSlot(path + prefix + "BodyGore"), 1f);
                }
                else
                {
                    Gore gore = Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 1, mod.GetGoreSlot(path + prefix + "BodyAltGore"), 1f);
                }
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 2, mod.GetGoreSlot(path + prefix + "HeadGore"), 1f);
                }
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 2, mod.GetGoreSlot(path + prefix + "SpearGore"), 1f);
                }
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(npc.Center, new Vector2(1, 0).RotatedBy(Main.rand.NextFloat(0, MathHelper.TwoPi)) * 2, mod.GetGoreSlot(path + prefix + "ShieldGore"), 1f);
                }
            }
        }
    }
}
