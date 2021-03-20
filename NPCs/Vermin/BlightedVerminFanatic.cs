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

namespace Highlander.NPCs.Vermin
{
    class BlightedVerminFanatic : ModNPC
    {
        float speed = 1.9f;


		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[npc.type] = 10; // make sure to set this for your modnpcs.
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 36;
			npc.aiStyle = -1; // This npc has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
			npc.damage = 32;
			npc.defense = 10;
			npc.lifeMax = 58;
			npc.knockBackResist = 0.2f;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath47;
			npc.value = 200;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Confused] = false; // npc default to being immune to the Confused debuff. Allowing confused could be a little more work depending on the AI. npc.confused is true while the npc is confused.
			drawOffsetY = -2;
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

                float yDiff = tileCoords.Y - targetTileCoords.Y;

                WalkOverSlopes();


                if ((npc.velocity.X == 0 || yDiff >= 2) && jumpTimer <= 0 && Math.Abs(vectorToTarget.X) > npc.width / 2)
                {
                    Jump(yDiff);   
                }
                else if(Math.Abs(vectorToTarget.X) > npc.width / 2)
                {
                    if (!npc.confused)
                    {
                        if (npc.velocity.X != direction * speed)
                        {
                            npc.netUpdate = true;
                        }
                        npc.velocity.X += direction * speed / 3;
                        if(Math.Abs(npc.velocity.X) > speed)
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

                if(jumpTimer > 0 && (npc.collideY || npc.collideX))
                {
                    jumpTimer--;
                }
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
            jumpTimer = 30;
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


    }
}
