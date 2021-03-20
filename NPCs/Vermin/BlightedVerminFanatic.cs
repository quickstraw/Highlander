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
        float speed = 1.8f;


		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[npc.type] = 10; // make sure to set this for your modnpcs.
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 36;
			npc.aiStyle = -1; // This npc has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
			npc.damage = 24;
			npc.defense = 12;
			npc.lifeMax = 45;
			npc.knockBackResist = 0.2f;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath47;
			npc.value = 100;
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
                int branch = 0;

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

                Tile frontTile = Main.tile[(int)((npc.Center.X) / 16) + direction, (int)((npc.position.Y + npc.height) / 16 - 1)];

                float yDiff = tileCoords.Y - targetTileCoords.Y;

                bool upSlope = false;

                /**if (npc.collideX)
                {
                    //Main.NewText(frontTile);
                    if(frontTile.active() && Main.tileSolid[frontTile.type] && frontTile.halfBrick())//frontTile.slope() != SlopeID.Solid)
                    {
                        npc.position.Y -= 8;
                        upSlope = true;
                    }
                    else if(frontTile.slope() == SlopeID.Solid)
                    {
                        Tile frontTopTile = Main.tile[(int)((npc.Center.X + (npc.width / 2) * direction) / 16) + direction, (int)((npc.position.Y + npc.height) / 16 - 2)];
                        if(!frontTopTile.active() || !Main.tileSolid[frontTopTile.type])
                        {
                            Tile frontTopTopTile = Main.tile[(int)((npc.Center.X) / 16) + direction, (int)((npc.position.Y + npc.height) / 16 - 3)];
                            Tile frontTopTopTopTile = Main.tile[(int)((npc.Center.X) / 16) + direction, (int)((npc.position.Y + npc.height) / 16 - 4)];
                            //Main.NewText(frontTopTopTile + " | " + frontTopTopTopTile);
                            if ((!frontTopTopTile.active() || !Main.tileSolid[frontTopTopTile.type]) && (!frontTopTopTopTile.active() || !Main.tileSolid[frontTopTopTopTile.type]))
                            {
                                npc.position.Y -= 4;
                            }
                        }
                        else
                        {
                            Main.NewText(frontTopTile);
                        }
                    }
                    
                }**/

                /**if(npc.velocity.Y >= 0.0)
                {
                    Main.NewText("workds");

                    Vector2 position = npc.position;

                    int index1 = (int)((position.X + (double)(npc.width / 2) + (double)((npc.width / 2 + 1) * direction)) / 16.0);
                    int index2 = (int)((position.Y + (double)npc.height - 1.0) / 16.0);


                    if (Main.tile[index1, index2] == null)
                        Main.tile[index1, index2] = new Tile();
                    if (Main.tile[index1, index2 - 1] == null)
                        Main.tile[index1, index2 - 1] = new Tile();
                    if (Main.tile[index1, index2 - 2] == null)
                        Main.tile[index1, index2 - 2] = new Tile();
                    if (Main.tile[index1, index2 - 3] == null)
                        Main.tile[index1, index2 - 3] = new Tile();
                    if (Main.tile[index1, index2 + 1] == null)
                        Main.tile[index1, index2 + 1] = new Tile();
                    if (Main.tile[index1 - direction, index2 - 3] == null)
                        Main.tile[index1 - direction, index2 - 3] = new Tile();
                    /**if ((double)(index1 * 16) < position.X + (double)npc.width && (double)(index1 * 16 + 16) > position.X &&
                        (Main.tile[index1, index2].nactive() && !Main.tile[index1, index2].topSlope() && (!Main.tile[index1, index2 - 1].topSlope() &&
                        Main.tileSolid[(int)Main.tile[index1, index2].type]) && !Main.tileSolidTop[(int)Main.tile[index1, index2].type] ||
                        Main.tile[index1, index2 - 1].halfBrick() && Main.tile[index1, index2 - 1].nactive()) && ((!Main.tile[index1, index2 - 1].nactive() ||
                        !Main.tileSolid[(int)Main.tile[index1, index2 - 1].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 1].type] ||
                        Main.tile[index1, index2 - 1].halfBrick() && (!Main.tile[index1, index2 - 4].nactive() ||
                        !Main.tileSolid[(int)Main.tile[index1, index2 - 4].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 4].type])) &&
                        ((!Main.tile[index1, index2 - 2].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 2].type] ||
                        Main.tileSolidTop[(int)Main.tile[index1, index2 - 2].type]) && (!Main.tile[index1, index2 - 3].nactive() ||
                        !Main.tileSolid[(int)Main.tile[index1, index2 - 3].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 3].type]) &&
                        (!Main.tile[index1 - direction, index2 - 3].nactive() || !Main.tileSolid[(int)Main.tile[index1 - direction, index2 - 3].type]))) || npc.collideX)**/
                /**if(frontTile.active() && Main.tileSolid[frontTile.type])
                {
                    Tile frontTopTile = Main.tile[(int)((npc.Center.X + (npc.width / 2) * direction) / 16) + direction, (int)((npc.position.Y + npc.height) / 16 - 2)];
                    if (!frontTopTile.active() || !Main.tileSolid[frontTopTile.type])
                    {
                        Tile frontTopTopTile = Main.tile[(int)((npc.Center.X) / 16) + direction, (int)((npc.position.Y + npc.height) / 16 - 3)];
                        Tile frontTopTopTopTile = Main.tile[(int)((npc.Center.X) / 16) + direction, (int)((npc.position.Y + npc.height) / 16 - 4)];
                        //Main.NewText(frontTopTopTile + " | " + frontTopTopTopTile);
                        if ((!frontTopTopTile.active() || !Main.tileSolid[frontTopTopTile.type]) && (!frontTopTopTopTile.active() || !Main.tileSolid[frontTopTopTopTile.type]))
                        {
                            float posY = (float)(index2 * 16);
                            if (Main.tile[index1, index2].halfBrick())
                                posY += 8f;
                            if (Main.tile[index1, index2 - 1].halfBrick())
                                posY -= 8f;
                            if ((double)posY < position.Y + (double)npc.height)
                            {
                                float yOffset = (float)position.Y + (float)npc.height - posY;
                                float num5 = 16.1f;
                                if ((double)yOffset <= (double)num5)
                                {
                                    npc.gfxOffY = npc.gfxOffY + ((float)npc.position.Y + (float)npc.height - posY);
                                    npc.position.Y = posY - npc.height;
                                }
                            }
                        }
                    }

                }
            }**/

                WalkOverSlopesNew();


                if ((npc.velocity.X == 0 || yDiff >= 2) && !upSlope && jumpTimer <= 0)
                {
                    branch = 1;
                    //Main.NewText(branch);
                    Jump(yDiff);   
                }
                else if(Math.Abs(vectorToTarget.X) > npc.width / 2)
                {
                    branch = 2;
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
                        //npc.velocity.X = direction * speed;
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
                            Main.NewText(hole);
                            Jump(0);
                        }
                    }
                }

                

                //tile.slope(SlopeID.Solid);
                //Main.NewText(tile.type);
                //Main.NewText(branch + " " + npc.velocity + " " + jumpTimer);
                //Main.NewText(yDiff);

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
            //Main.NewText(hole1 + " " + hole1Deep + "" + hole2);
            //Main.NewText(hole1 && hole1Deep && hole2);
            return hole1 && hole1Deep && hole2;
        }

        private void Jump(float yDiff)
        {
            //Main.NewText(yDiff);
            jumpTimer = 30;
            npc.netUpdate = true;

            if(yDiff > 5)
            {
                //npc.velocity.Y -= 9;
            }
            else
            {
                //npc.velocity.Y -= 7;
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
            if (npc.velocity.Y >= 0.0)
            {
                int num3 = 0;
                if (npc.velocity.X < 0.0)
                    num3 = -1;
                if (npc.velocity.X > 0.0)
                    num3 = 1;
                Vector2 position = npc.position;

                int index1 = (int)((position.X + (double)(npc.width / 2) + (double)((npc.width / 2 + 1) * num3)) / 16.0);
                int index2 = (int)((position.Y + (double)npc.height - 1.0) / 16.0);
                if (Main.tile[index1, index2] == null)
                    Main.tile[index1, index2] = new Tile();
                if (Main.tile[index1, index2 - 1] == null)
                    Main.tile[index1, index2 - 1] = new Tile();
                if (Main.tile[index1, index2 - 2] == null)
                    Main.tile[index1, index2 - 2] = new Tile();
                if (Main.tile[index1, index2 - 3] == null)
                    Main.tile[index1, index2 - 3] = new Tile();
                if (Main.tile[index1, index2 + 1] == null)
                    Main.tile[index1, index2 + 1] = new Tile();
                if ((double)(index1 * 16) < position.X + (double)npc.width && (double)(index1 * 16 + 16) > position.X && (Main.tile[index1, index2].nactive() &&
                    !Main.tile[index1, index2].topSlope() && (!Main.tile[index1, index2 - 1].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2].type]) &&
                    !Main.tileSolidTop[(int)Main.tile[index1, index2].type] || Main.tile[index1, index2 - 1].halfBrick() && Main.tile[index1, index2 - 1].nactive()) &&
                    ((!Main.tile[index1, index2 - 1].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 1].type] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 1].type] ||
                    Main.tile[index1, index2 - 1].halfBrick() && (!Main.tile[index1, index2 - 4].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 4].type] ||
                    Main.tileSolidTop[(int)Main.tile[index1, index2 - 4].type])) && ((!Main.tile[index1, index2 - 2].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 2].type] ||
                    Main.tileSolidTop[(int)Main.tile[index1, index2 - 2].type]) && (!Main.tile[index1, index2 - 3].nactive() || !Main.tileSolid[(int)Main.tile[index1, index2 - 3].type] ||
                    Main.tileSolidTop[(int)Main.tile[index1, index2 - 3].type]) && (!Main.tile[index1 - num3, index2 - 3].nactive() || !Main.tileSolid[(int)Main.tile[index1 - num3, index2 - 3].type]))))
                {
                    float num8 = (float)(index2 * 16);
                    if (Main.tile[index1, index2].halfBrick())
                        num8 += 8f;
                    if (Main.tile[index1, index2 - 1].halfBrick())
                        num8 -= 8f;
                    if ((double)num8 < position.Y + (double)npc.height)
                    {
                        float num9 = (float)position.Y + (float)npc.height - num8;
                        if ((double)num9 <= 16.1)
                        {
                            npc.gfxOffY = npc.gfxOffY + ((float)npc.position.Y + (float)npc.height - num8);
                            npc.position.Y = (float)((double)num8 - (double)npc.height);
                            npc.stepSpeed = (double)num9 >= 9.0 ? 2f : 1f;
                        }
                    }
                }
            }
            if (npc.velocity.Y == 0.0)
            {
                int index1 = (int)((npc.position.X + (double)(npc.width / 2) + (double)((npc.width / 2 + 2) * npc.direction) + npc.velocity.X * 5.0) / 16.0);
                int index2 = (int)((npc.position.Y + (double)npc.height - 15.0) / 16.0);
                if (Main.tile[index1, index2] == null)
                    Main.tile[index1, index2] = new Tile();
                if (Main.tile[index1, index2 - 1] == null)
                    Main.tile[index1, index2 - 1] = new Tile();
                if (Main.tile[index1, index2 - 2] == null)
                    Main.tile[index1, index2 - 2] = new Tile();
                if (Main.tile[index1, index2 - 3] == null)
                    Main.tile[index1, index2 - 3] = new Tile();
                if (Main.tile[index1, index2 + 1] == null)
                    Main.tile[index1, index2 + 1] = new Tile();
                if (Main.tile[index1 + npc.direction, index2 - 1] == null)
                    Main.tile[index1 + npc.direction, index2 - 1] = new Tile();
                if (Main.tile[index1 + npc.direction, index2 + 1] == null)
                    Main.tile[index1 + npc.direction, index2 + 1] = new Tile();
                if (Main.tile[index1 - npc.direction, index2 + 1] == null)
                    Main.tile[index1 - npc.direction, index2 + 1] = new Tile();
                int spriteDirection = npc.spriteDirection * -1;
                if (npc.velocity.X < 0.0 && spriteDirection == -1 || npc.velocity.X > 0.0 && spriteDirection == 1)
                {
                    bool flag4 = false;
                    float num3 = 3f;
                    if (Main.tile[index1, index2 - 2].nactive() && Main.tileSolid[(int)Main.tile[index1, index2 - 2].type])
                    {
                        if (Main.tile[index1, index2 - 3].nactive() && Main.tileSolid[(int)Main.tile[index1, index2 - 3].type])
                        {
                            npc.velocity.Y = -8.5f;
                            npc.netUpdate = true;
                        }
                        else
                        {
                            npc.velocity.Y = -7.5f;
                            npc.netUpdate = true;
                        }
                    }
                    else if (Main.tile[index1, index2 - 1].nactive() && !Main.tile[index1, index2 - 1].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2 - 1].type])
                    {
                        npc.velocity.Y = -7.0f;
                        npc.netUpdate = true;
                    }
                    else if (npc.position.Y + (double)npc.height - (double)(index2 * 16) > 20.0 && Main.tile[index1, index2].nactive() && (!Main.tile[index1, index2].topSlope() && Main.tileSolid[(int)Main.tile[index1, index2].type]))
                    {
                        npc.velocity.Y = -6.0f;
                        npc.netUpdate = true;
                    }
                    else if ((npc.directionY < 0 || (double)Math.Abs((float)npc.velocity.X) > (double)num3) && (!flag4 || !Main.tile[index1, index2 + 1].nactive() ||
                        !Main.tileSolid[(int)Main.tile[index1, index2 + 1].type]) && ((!Main.tile[index1, index2 + 2].nactive() ||
                        !Main.tileSolid[(int)Main.tile[index1, index2 + 2].type]) && (!Main.tile[index1 + npc.direction, index2 + 3].nactive() ||
                        !Main.tileSolid[(int)Main.tile[index1 + npc.direction, index2 + 3].type])))
                    {
                        npc.velocity.Y = -8.0f;
                        npc.netUpdate = true;
                    }
                }
            }
        }

        private void WalkOverSlopesNew()
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
            }
        }

        private void GetTileCollisionParameters(out Vector2 cPosition, out int cWidth, out int cHeight)
        {
            cPosition = npc.position;
            cWidth = npc.width;
            cHeight = npc.height;
            if (cHeight == npc.height)
                return;
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            float local1 = @cPosition.Y;
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            double num1 = (double)local1 + (double)(npc.height - cHeight);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            local1 = (float)num1;
        }


    }
}
