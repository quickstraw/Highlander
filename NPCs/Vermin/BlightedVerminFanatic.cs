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
using Terraria.GameContent.ItemDropRules;

namespace Highlander.NPCs.Vermin
{
    class BlightedVerminFanatic : ModNPC
    {
        float speed = 1.9f;


		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Blighted Vermin Monk");
			Main.npcFrameCount[NPC.type] = 10; // make sure to set this for your modnpcs.
		}

		public override void SetDefaults()
		{
			NPC.width = 34;
			NPC.height = 36;
			NPC.aiStyle = -1; // This NPC has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
			NPC.damage = 32;
			NPC.defense = 9;
			NPC.lifeMax = 64;
			NPC.knockBackResist = 0.2f;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath47;
			NPC.value = 200;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Confused] = false; // NPC default to being immune to the Confused debuff. Allowing confused could be a little more work depending on the AI. NPC.confused is true while the NPC is confused.
			DrawOffsetY = -2;
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float spawnChance = 0;
            // Spawn in Cavern layer
            if(spawnInfo.PlayerFloorY > Main.rockLayer && spawnInfo.PlayerFloorY < (Main.maxTilesY - 200))
            {
                spawnChance = 0.06f;
            }
            return spawnChance;
        }

        public override void AI()
        {
            int oldTarget = NPC.target;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosestUpgraded(false);
                if (NPC.HasValidTarget && (Main.player[NPC.target].Center - NPC.Center).Length() > 1000)
                {
                    NPC.target = 255;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.HasValidTarget)
            {
                Player target = Main.player[NPC.target];
                Vector2 vectorToTarget = target.Center - NPC.Center;
                
                Tile tile = Main.tile[(int)(NPC.Center.X / 16), (int)((NPC.position.Y + NPC.height) / 16) + 1];
                Vector2 tileCoords = new Vector2((int)(NPC.Center.X / 16), (int)((NPC.position.Y + NPC.height) / 16) + 1);
                Tile targetTile = Main.tile[(int)(target.Center.X / 16), (int)((target.position.Y + target.height) / 16) + 1];
                Vector2 targetTileCoords = new Vector2((int)(target.Center.X / 16), (int)((target.position.Y + target.height) / 16) + 1);

                if (NPC.confused)
                {
                    vectorToTarget *= -1;
                }
                int direction = NPC.direction;

                if (attackFrameTimer <= 0)
                {
                    if (vectorToTarget.X > 0)
                    {
                        direction = 1;
                        NPC.spriteDirection = -1;
                    }
                    else
                    {
                        direction = -1;
                        NPC.spriteDirection = 1;
                    }
                }
                if (NPC.velocity.X > 0)
                {
                    NPC.direction = 1;
                }
                else if (NPC.velocity.X < 0)
                {
                    NPC.direction = -1;
                }

                float yDiff = tileCoords.Y - targetTileCoords.Y;

                WalkOverSlopes();


                if (((NPC.velocity.X == 0 && NPC.collideX) || (yDiff >= 4 && Math.Abs(vectorToTarget.X) < NPC.width * 2 && yDiff < 8)) && jumpTimer <= 0)
                {
                    Jump(yDiff);   
                }
                else if((Math.Abs(vectorToTarget.X) > 16 && NPC.velocity.Y == 0) || (jumpTimer > 0 && NPC.collideX && !NPC.collideY))
                {
                    if (!NPC.confused)
                    {
                        if (NPC.velocity.X != direction * speed)
                        {
                            NPC.netUpdate = true;
                        }
                        NPC.velocity.X += direction * speed / 4;
                        bool left = NPC.velocity.X < 0 && direction == -1;
                        bool right = NPC.velocity.X > 0 && direction == 1;

                        if ((left || right) && Math.Abs(NPC.velocity.X) > speed)
                        {
                            NPC.velocity.X = direction * speed;
                        }
                    }
                    else
                    {
                        if (NPC.velocity.X != direction * 0.6f * speed)
                        {
                            NPC.netUpdate = true;
                        }

                        NPC.velocity.X = direction * 0.6f * speed;
                    }
                    if (NPC.collideY && tile.TileType != 0)
                    {
                        bool hole = CheckHole();
                        if (hole)
                        {
                            Jump(0);
                        }
                    }
                }

                if (jumpTimer > 0 && (NPC.velocity.Y == 0))
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
                    NPC.netUpdate = true;
                    if (Main.rand.NextBool())
                    {
                        randWalkDirection = -1;
                    }
                    else
                    {
                        randWalkDirection = 1;
                    }
                }

                if (NPC.velocity.X != randWalkDirection * 0.6f * speed)
                {
                    NPC.netUpdate = true;
                }
                NPC.velocity.X = randWalkDirection * 0.6f * speed;

                if (NPC.velocity.X > 0)
                {
                    NPC.spriteDirection = -1;
                    NPC.direction = 1;
                }
                else if (NPC.velocity.X < 0)
                {
                    NPC.spriteDirection = 1;
                    NPC.direction = -1;
                }

                randWalkTimer--;
            }
        }

        private bool CheckHole()
        {
            int x = (int)(NPC.Center.X / 16);
            int y = (int)((NPC.position.Y + NPC.height) / 16);
            bool hole1 = false;
            bool hole1Deep = false;
            bool hole2 = false;

            Tile tile = Main.tile[x + NPC.direction, y + 1];
            if(tile.TileType == 0)
            {
                hole1 = true;
            }
            tile = Main.tile[x + NPC.direction, y + 2];
            if (tile.TileType == 0)
            {
                hole1Deep = true;
            }
            tile = Main.tile[x + NPC.direction * 2, y + 1];
            if (tile.TileType == 0)
            {
                hole2 = true;
            }
            return hole1 && hole1Deep && hole2;
        }

        private void Jump(float yDiff)
        {
            NPC.velocity.X = NPC.direction * speed;
            jumpTimer = 3;
            NPC.netUpdate = true;

            if(yDiff > 5)
            {
                NPC.velocity.Y -= 9;
            }
            else
            {
                NPC.velocity.Y -= 7;
            }
        }

        private float jumpTimer
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private float randWalkTimer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        private float attackFrameTimer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        private float randWalkDirection
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = 0;
            Tile tile = Main.tile[(int)(NPC.Center.X / 16), (int)((NPC.position.Y + NPC.height) / 16) + 1];

            if (NPC.velocity.Y != 0 && !NPC.collideY && !tile.HasTile)
            {
                NPC.frame.Y = frameHeight * 9;
            }
            else if (NPC.velocity.X != 0)
            {
                if (NPC.frameCounter < 6)
                {
                    NPC.frame.Y = frameHeight * 1;
                }
                else if (NPC.frameCounter < 12)
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
                NPC.frameCounter = (NPC.frameCounter + Math.Abs(NPC.velocity.X / 1.5)) % 48;
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
            Point tileCoordinates1 = NPC.Center.ToTileCoordinates();
            if (WorldGen.InWorld((int)tileCoordinates1.X, (int)tileCoordinates1.Y, 5) && !NPC.noGravity)
            {
                Vector2 cPosition;
                int cWidth;
                int cHeight;
                this.GetTileCollisionParameters(out cPosition, out cWidth, out cHeight);
                Vector2 vector2 = NPC.position - cPosition; 
                Collision.StepUp(ref cPosition, ref NPC.velocity, cWidth, cHeight, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, false, 0);
                NPC.position = cPosition + vector2;
                NPC.netUpdate = true;
            }
        }

        private void GetTileCollisionParameters(out Vector2 cPosition, out int cWidth, out int cHeight)
        {
            cPosition = NPC.position;
            cWidth = NPC.width;
            cHeight = NPC.height;
        }

        public override void OnKill()
        {
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<BellOfPestilence>(), 12));
            npcLoot.Add(ItemDropRule.Common(ItemID.Shackle, 50));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                string prefix = "BlightedVerminFanatic";
                string path = "";//"Gores/Vermin/BlightedVerminFanatic/";

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

                var source = NPC.GetSource_Death();
                Gore.NewGoreDirect(source, NPC.Center, new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, Mod.Find<ModGore>(path + prefix + "ArmGore").Type, 1f);
                Gore.NewGoreDirect(source, NPC.Center, new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, Mod.Find<ModGore>(path + prefix + "BodyGore").Type, 1f);
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(source, NPC.Center - new Vector2(0, NPC.height / 3), new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, Mod.Find<ModGore>(path + prefix + "HeadGore").Type, 1f);
                }
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(source, NPC.Center, new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, Mod.Find<ModGore>(path + prefix + "SwordGore").Type, 1f);
                }
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(source, NPC.Center, new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, Mod.Find<ModGore>(path + prefix + "BackSwordGore").Type, 1f);
                }
            }
        }


    }
}
