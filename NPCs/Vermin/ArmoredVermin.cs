using Highlander.Items.Accessories.Shields;
using Highlander.Items.Weapons.Spears;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.Vermin
{
	class ArmoredVermin : ModNPC
	{
        Rectangle AttackRange;
        float speed = 1.6f;

        private double frameCounter;
        private byte frame;
        private Texture2D sprite;

		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 15; // make sure to set this for your modnpcs.
		}

        public override void SetDefaults()
		{
			NPC.width = 34;
            NPC.height = 40;
            NPC.aiStyle = -1; // This npc has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            NPC.damage = 42;
            NPC.defense = 30;
            NPC.lifeMax = 300;
            NPC.knockBackResist = 0.5f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath47;
            NPC.value = 100;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.Confused] = false; // npc default to being immune to the Confused debuff. Allowing confused could be a little more work depending on the AI. npc.confused is true while the npc is confused.
            DrawOffsetY = -2;
            //sprite = GetTexture("Highlander/NPCs/Vermin/ArmoredVerminSprite");
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float spawnChance = 0;
            if (Main.hardMode)
            {
                // Spawn in Underground layer
                if (spawnInfo.SpawnTileY > Main.worldSurface && spawnInfo.SpawnTileY < Main.rockLayer)
                {
                    spawnChance = 0.05f;
                }
                // Spawn in Cavern layer
                if (spawnInfo.SpawnTileY > Main.rockLayer && spawnInfo.SpawnTileY < (Main.maxTilesY - 200))
                {
                    spawnChance = 0.12f;
                }
            }
            return spawnChance;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            NPC.netUpdate = true;
            jumpTimer = 0;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            NPC.netUpdate = true;
            jumpTimer = 0;
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            NPC.netUpdate = true;
            jumpTimer = 0;
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

                AttackRange = new Rectangle((int)NPC.position.X - (NPC.width * 2 / 3), (int)NPC.position.Y, NPC.width * 2, NPC.height);
                if (!NPC.confused && AttackRange.Intersects(target.Hitbox) && attackFrameTimer <= 0 && NPC.collideY)
                {
                    Attack();
                }
                else if (((NPC.velocity.X == 0 && NPC.collideX) || (yDiff >= 4 && Math.Abs(vectorToTarget.X) < NPC.width * 2 && yDiff < 8)) && jumpTimer <= 0 && attackFrameTimer <= 0)
                {
                    Jump();
                }
                else if ((attackFrameTimer <= 1 && Math.Abs(vectorToTarget.X) > 16 && NPC.velocity.Y == 0) || (jumpTimer > 0 && NPC.collideX && !NPC.collideY))
                {
                    if (!NPC.confused)
                    {
                        if (NPC.velocity.X != direction * speed)
                        {
                            NPC.netUpdate = true;
                        }
                        bool left = NPC.velocity.X < 0 && direction == -1;
                        bool right = NPC.velocity.X > 0 && direction == 1;

                        if ((left || right) && Math.Abs(NPC.velocity.X) > speed)
                        {
                            NPC.velocity.X = direction * speed;
                        }
                        else
                        {
                            NPC.velocity.X += direction * speed / 4;
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
                            Jump();
                        }
                    }
                }
                else if (attackFrameTimer < 19 && attackFrameTimer >= 18)
                {
                    var source = NPC.GetSource_FromAI();
                    NPC.netUpdate = true;
                    if (Main.expertMode)
                    {
                        Projectile projectile = Projectile.NewProjectileDirect(source, NPC.Center + new Vector2(NPC.direction * NPC.width, 0), new Vector2(), ProjectileType<BlightedVerminSpear>(), NPC.damage / 3, 9.5f);
                    }
                    else
                    {
                        Projectile projectile = Projectile.NewProjectileDirect(source, NPC.Center + new Vector2(NPC.direction * NPC.width, 0), new Vector2(), ProjectileType<BlightedVerminSpear>(), NPC.damage / 2, 9.5f);
                    }
                    SoundEngine.PlaySound(SoundID.Item1.SoundId, (int)NPC.Center.X, (int)NPC.Center.Y, SoundID.Item1.Style, 0.90f, +0.5f);
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
            //FindPostFrame();
        }

        private void FindPostFrame()
        {
            byte newFrame = 0;
            Tile tile = Main.tile[(int)(NPC.Center.X / 16), (int)((NPC.position.Y + NPC.height) / 16) + 1];

            if (NPC.velocity.X != 0)
            {
                if (frameCounter < 6)
                {
                    newFrame = 1;
                }
                else if (frameCounter < 12)
                {
                    newFrame = 2;
                }
                else if (frameCounter < 18)
                {
                    newFrame = 3;
                }
                else if (frameCounter < 24)
                {
                    newFrame = 4;
                }
                else if (frameCounter < 30)
                {
                    newFrame = 5;
                }
                else if (frameCounter < 36)
                {
                    newFrame = 6;
                }
                else if (frameCounter < 42)
                {
                    newFrame = 7;
                }
                else if (frameCounter < 48)
                {
                    newFrame = 8;
                }
                frameCounter = (frameCounter + Math.Abs(NPC.velocity.X / 1.5)) % 48;
            }
            if (NPC.velocity.Y != 0 && !NPC.collideY && !tile.HasTile)
            {
                newFrame = 9;
            }
            if (attackFrameTimer > 0)
            {
                if (attackFrameTimer > 33)
                {
                    newFrame = 10;
                }
                else if (attackFrameTimer > 21)
                {
                    newFrame = 11;
                }
                else if (attackFrameTimer > 18)
                {
                    newFrame = 12;
                }
                else if (attackFrameTimer > 6)
                {
                    newFrame = 13;
                }
                else if (attackFrameTimer > 0)
                {
                    newFrame = 14;
                }
            }

            if (newFrame != frame)
            {
                NPC.netUpdate = true;
                frame = newFrame;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            /**Vector2 drawOffset = new Vector2(0, -2);
            Vector2 drawPos = NPC.position + drawOffset - Main.screenPosition;

            int frameHeight = sprite.Height / 15;
            Vector2 origin = new Vector2(sprite.Width, frameHeight / 2);
            Vector2 spriteDrawPos = drawPos;// + new Vector2(NPC.width / 2, NPC.height / 2);

            SpriteEffects flip = SpriteEffects.None;
            if(NPC.spriteDirection == 1)
            {
                flip = SpriteEffects.FlipHorizontally;
            }

            spriteBatch.Draw(sprite, spriteDrawPos, new Rectangle(0, frame * frameHeight, sprite.Width, frameHeight), drawColor, NPC.rotation, origin, 1.0f, flip, 0);**/
        }

        public override void FindFrame(int frameHeight)
        {
            int newFrame = 0;
            Tile tile = Main.tile[(int)(NPC.Center.X / 16), (int)((NPC.position.Y + NPC.height) / 16) + 1];

            if (attackFrameTimer > 0)
            {
                if (attackFrameTimer > 33)
                {
                    newFrame = frameHeight * 10;
                }
                else if (attackFrameTimer > 21)
                {
                    newFrame = frameHeight * 11;
                }
                else if (attackFrameTimer > 18)
                {
                    newFrame = frameHeight * 12;
                }
                else if (attackFrameTimer > 6)
                {
                    newFrame = frameHeight * 13;
                }
                else if (attackFrameTimer > 0)
                {
                    newFrame = frameHeight * 14;
                }
                attackFrameTimer--;
                if (attackFrameTimer <= 0)
                {
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.velocity.Y != 0 && !NPC.collideY && !tile.HasTile)
            {
                newFrame = frameHeight * 9;
            }
            else if (NPC.velocity.X != 0)
            {
                if (NPC.frameCounter < 6)
                {
                    newFrame = frameHeight * 1;
                }
                else if (NPC.frameCounter < 12)
                {
                    newFrame = frameHeight * 2;
                }
                else if (NPC.frameCounter < 18)
                {
                    newFrame = frameHeight * 3;
                }
                else if (NPC.frameCounter < 24)
                {
                    newFrame = frameHeight * 4;
                }
                else if (NPC.frameCounter < 30)
                {
                    newFrame = frameHeight * 5;
                }
                else if (NPC.frameCounter < 36)
                {
                    newFrame = frameHeight * 6;
                }
                else if (NPC.frameCounter < 42)
                {
                    newFrame = frameHeight * 7;
                }
                else if (NPC.frameCounter < 48)
                {
                    newFrame = frameHeight * 8;
                }
                NPC.frameCounter = (NPC.frameCounter + Math.Abs(NPC.velocity.X / 1.5)) % 48;
            }

            if(newFrame != NPC.frame.Y)
            {
                NPC.netUpdate = true;
                NPC.frame.Y = newFrame;
            }
        }

        private void Jump()
        {
            NPC.velocity.X = NPC.direction * speed;
            NPC.velocity.Y -= 7;
            jumpTimer = 3;
            NPC.netUpdate = true;
        }

        private bool CheckHole()
        {
            int x = (int)(NPC.Center.X / 16);
            int y = (int)((NPC.position.Y + NPC.height) / 16);
            bool hole1 = false;
            bool hole1Deep = false;
            bool hole2 = false;

            Tile tile = Main.tile[x + NPC.direction, y + 1];
            if (tile.TileType == 0)
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

        private void Attack()
        {
            NPC.velocity.X = 0;
            attackFrameTimer = 39;
            NPC.netUpdate = true;
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

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(frameCounter);
            writer.Write(frame);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            frameCounter = reader.ReadDouble();
            frame = reader.ReadByte();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A hardened veteran rat creature is a dangerous thing. The rats are getting bolder by the day.")
            });
        }

        public override void OnKill()
        {
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<VerminSpear>(), 12));
            npcLoot.Add(ItemDropRule.Common(ItemType<VerminShield>(), 12));
            npcLoot.Add(ItemDropRule.Common(ItemID.Shackle, 50));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                string prefix = "ArmoredVermin";
                string path = "";//"Gores/Vermin/ArmoredVermin/";

                float max = MathHelper.PiOver2;
                float min = -MathHelper.PiOver2;

                if (hitDirection == -1)
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
                    Gore gore = Gore.NewGoreDirect(source, NPC.Center, new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, Mod.Find<ModGore>(path + prefix + "Halberd1Gore").Type, 1f);
                }
                if (Main.rand.NextBool())
                {
                    Gore gore = Gore.NewGoreDirect(source, NPC.Center, new Vector2(hitDirection, 0).RotatedBy(Main.rand.NextFloat(min, max)) * 4, Mod.Find<ModGore>(path + prefix + "Halberd2Gore").Type, 1f);
                }
            }
        }
    }
}
