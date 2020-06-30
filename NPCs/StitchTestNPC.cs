using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.NPCs
{
    class StitchTestNPC : ModNPC
    {

        public override void SetDefaults()
        {
            //npc.frame = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);
            drawOffsetY = 0;// -52;
            npc.aiStyle = -1;
            npc.lifeMax = 200;
            npc.damage = 0;
            npc.defense = 1;
            npc.knockBackResist = 0f;
            npc.width = 80;
            npc.height = 120;
            npc.npcSlots = 50f;
            npc.boss = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 100000;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        private void Init()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC((int) npc.position.X, (int) npc.position.Y, ModContent.NPCType<StitchTestNPCBody>(), 0, 0, 0, npc.whoAmI);
                npc.netUpdate = true;
            }
        }

        public override void AI()
        {
            if(npc.ai[0] != 1)
            {
                npc.ai[0] = 1;
                Init();
            }
            npc.position.X += 1;
            npc.rotation += 0.01f;
        }

    }

    internal class StitchTestNPCBody : ModNPC
    {
        public override string Texture => "Highlander/NPCs/StitchTestNPCBody";

        public NPC Master
        {
            get
            {
                return Main.npc[(int)npc.ai[2]];
            }
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6; // make sure to set this for your modnpcs.
        }

        public override void SetDefaults()
        {
            //npc.frame = new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT);
            drawOffsetY = 0;// -52;

            npc.aiStyle = -1;
            npc.lifeMax = 3800;
            npc.damage = 0;
            npc.width = 80;
            npc.height = 120;
            npc.boss = true;

            npc.noGravity = true;
            npc.noTileCollide = true;

            npc.dontCountMe = true;
            npc.dontTakeDamage = true;
        }

        public override void AI()
        {
            npc.position = Master.position;
            npc.rotation = Master.rotation;
            npc.realLife = Master.whoAmI;

            if(Master.life <= 0)
            {
                npc.life = 0;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = (int) (frameHeight * (int) (npc.frameCounter / 10));
            npc.frameCounter = (npc.frameCounter + 1.75) % 60;
        }

    }

}
