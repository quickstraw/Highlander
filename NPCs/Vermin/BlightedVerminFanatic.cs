using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Highlander.Utilities;
using Highlander.Items.Accessories;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.Vermin
{
    class BlightedVerminFanatic : ModNPC
    {
        float speed = 1.9f;


		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Blighted Vermin Monk");
			Main.npcFrameCount[npc.type] = 10; // make sure to set this for your modnpcs.
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 36;
			npc.aiStyle = -1; // This npc has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
			npc.damage = 32;
			npc.defense = 9;
			npc.lifeMax = 64;
			npc.knockBackResist = 0.2f;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath47;
			npc.value = 200;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Confused] = false; // npc default to being immune to the Confused debuff. Allowing confused could be a little more work depending on the AI. npc.confused is true while the npc is confused.
			drawOffsetY = -2;
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float spawnChance = 0;
            // Spawn in Cavern layer
            if(spawnInfo.playerFloorY > Main.rockLayer && spawnInfo.playerFloorY < (Main.maxTilesY - 200))
            {
                spawnChance = 0.06f;
            }
            return spawnChance;
        }

        public override void AI()
        {
            int oldTarget = npc.target;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.TargetClosestUpgraded(false);
                if (npc.HasValidTarget && (Main.player[npc.target].Center - npc.Center).Length() > 1000)
                {
                    npc.target = 255;
                    npc.netUpdate = true;
                }
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
                    }
                    else
                    {
                        direction = -1;
                    }
                }
                if (npc.velocity.X > 0)
                {
                    npc.spriteDirection = -1;
                    npc.direction = 1;
                }
                else if (npc.velocity.X < 0)
                {
                    npc.spriteDirection = 1;
                    npc.direction = -1;
                }

                float yDiff = tileCoords.Y - targetTileCoords.Y;

                WalkOverSlopes();


                if (((npc.velocity.X == 0 && npc.collideX) || (yDiff >= 4 && Math.Abs(vectorToTarget.X) < npc.width * 2 && yDiff < 8)) && jumpTimer <= 0)
                {
                    Jump(yDiff);   
                }
                else if((Math.Abs(vectorToTarget.X) > 16 && npc.velocity.Y == 0) || (jumpTimer > 0 && npc.collideX && !npc.collideY))
                {
                    if (!npc.confused)
                    {
                        if (npc.velocity.X != direction * speed)
                        {
                            npc.netUpdate = true;
                        }
                        npc.velocity.X += direction * speed / 4;
                        bool left = npc.velocity.X < 0 && direction == -1;
                        bool right = npc.velocity.X > 0 && direction == 1;

                        if ((left || right) && Math.Abs(npc.velocity.X) > speed)
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
                            Jump(0);
                        }
                    }
                }

                if (jumpTimer > 0 && (npc.velocity.Y == 0))
                {
                    jumpTimer--;
                }
            }
            else
            {
                WalkOverSlopes();
                if (randWalkTimer <= 0)
                {
                    randWalkTimer = Main.rand.Next(240, 360);
                    npc.netUpdate = true;
                    if (Main.rand.NextBool())
                    {
                        randWalkDirection = -1;
                    }
                    else
                    {
                        randWalkDirection = 1;
                    }
                }

                if (npc.velocity.X != randWalkDirection * 0.6f * speed)
                {
                    npc.netUpdate = true;
                }
                npc.velocity.X = randWalkDirection * 0.6f * speed;

                if (npc.velocity.X > 0)
                {
                    npc.spriteDirection = -1;
                    npc.direction = 1;
                }
                else if (npc.velocity.X < 0)
                {
                    npc.spriteDirection = 1;
                    npc.direction = -1;
                }

                randWalkTimer--;
            }
        }

        private bool CheckHole()
        {
            int x = (int)(npc.Center.X / 16);
            int y = (int)((npc.position.Y + npc.height) / 16);
            bool hole1 = false;
            bool hole1Deep = false;
            bool hole2 = false;

            Tile tile = Main.tile[x + npc.direction, y + 1];
            if(tile.type == 0)
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

        private void Jump(float yDiff)
        {
            npc.velocity.X = npc.direction * speed;
            jumpTimer = 3;
            npc.netUpdate = true;

            if(yDiff > 5)
            {
                npc.velocity.Y -= 9;
            }
            else
            {
                npc.velocity.Y -= 7;
            }
        }

        private float jumpTimer
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        private float randWalkTimer
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }

        private float attackFrameTimer
        {
            get => npc.ai[2];
            set => npc.ai[2] = value;
        }

        private float randWalkDirection
        {
            get => npc.ai[3];
            set => npc.ai[3] = value;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = 0;
            Tile tile = Main.tile[(int)(npc.Center.X / 16), (int)((npc.position.Y + npc.height) / 16) + 1];

            if (npc.velocity.Y != 0 && !npc.collideY && !tile.active())
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 300);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Poisoned, 300);
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

        public override void NPCLoot()
        {
            bool getBell = Main.rand.NextBool(12);
            if (Main.rand.NextBool(50))
            {
                Item.NewItem(npc.getRect(), ItemID.Shackle);
            }
            if (getBell)
            {
                Item.NewItem(npc.getRect(), ItemType<BellOfPestilence>());
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                string prefix = "BlightedVerminFanatic";
                string path = "Gores/Vermin/BlightedVerminFanatic/";

                float max = MathHelper.PiOver2;
                float min = -MathHelper.PiOver2;

                if(hitDirection == -1)
                {
                    min = 0;
                }
                else
                {
                    max = 0;
                }

                Gore.NewGoreDirect(npc.Center, new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, mod.GetGoreSlot(path + prefix + "ArmGore"), 1f);
                Gore.NewGoreDirect(npc.Center, new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, mod.GetGoreSlot(path + prefix + "BodyGore"), 1f);
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(npc.Center - new Vector2(0, npc.height / 3), new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, mod.GetGoreSlot(path + prefix + "HeadGore"), 1f);
                }
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(npc.Center, new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, mod.GetGoreSlot(path + prefix + "SwordGore"), 1f);
                }
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(npc.Center, new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, mod.GetGoreSlot(path + prefix + "BackSwordGore"), 1f);
                }
            }
        }


    }
}
