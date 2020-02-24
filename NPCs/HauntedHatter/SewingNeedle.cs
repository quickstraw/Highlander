using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.NPCs.HauntedHatter
{
    class SewingNeedle : ModProjectile
    {

        private bool stuck;
        private int timer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true; // projectiles with hide but without this will draw in the lighting values of the owner player.
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.penetrate = -1;
            //drawOffsetX = -2;
            drawOriginOffsetY = -46;
            projectile.ai = new float[4];
            projectile.hostile = true;
            projectile.hide = true;
        }

        public override void AI()
        {
            if (!stuck)
            {
                NormalAI();
            }
            else
            {
                StuckAI();
            }
        }

        private void NormalAI()
        {
            switch (stage)
            {
                case 0:
                    if (timer > 45)
                    {
                        float angle = (float)Math.Atan2(target.Center.Y - projectile.Center.Y, target.Center.X - projectile.Center.X + offset) + MathHelper.PiOver2;
                        float rotation = projectile.rotation % MathHelper.TwoPi;
                        if (((rotation + 0.1f >= angle && rotation - 0.1f <= angle) || timer > 75) && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            projectile.rotation = angle;
                            projectile.velocity *= 0;
                            stage++;
                            timer = 0;
                            projectile.netUpdate = true;
                            break;
                        }
                    }
                    else
                    {
                        projectile.rotation += 0.1f;
                    }
                    if (timer <= 30)
                    {
                        if(timer == 30 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.rotation += 0.1f;
                    }
                    if (timer <= 15)
                    {
                        if (timer == 15 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.rotation += 0.2f;
                    }
                    if (timer == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        projectile.netUpdate = true;
                    }
                    projectile.rotation += 0.15f;
                    projectile.velocity = npc.velocity;
                    timer += 1;
                    break;
                case 1:
                    float CoolAngle = (float)Math.Atan2(target.Center.Y - projectile.Center.Y, target.Center.X - projectile.Center.X + offset) + MathHelper.PiOver2;
                    projectile.rotation = CoolAngle;
                    drawOriginOffsetY += 5;
                    projectile.position += 5 * forward;
                    if (drawOriginOffsetY >= 0)
                    {
                        int diff = -drawOriginOffsetY;
                        projectile.position -= diff * forward;
                        projectile.velocity = 10 * forward;
                        stage++;
                    }
                    break;
                case 2:
                    if (projectile.velocity.Length() > 24)
                    {
                        projectile.velocity.Normalize();
                        projectile.velocity *= 24;
                    }
                    else if(projectile.velocity.Length() < 20)
                    {
                        projectile.velocity += forward * 0.3f;
                    }
                    break;
                default:
                    break;
            }
        }

        private void StuckAI()
        {
            if (timer > 2)
            {
                projectile.velocity *= 0;
            }
            if (timer > 180)
            {
                projectile.alpha = (projectile.alpha + 2) % 255;
            }
            if (timer > 300)
            {
                projectile.Kill();
            }
            timer++;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
                Main.PlaySound(SoundID.Item10.WithPitchVariance(0.1f).WithVolume(0.9f), projectile.Center);
            }
            // Changing origin to fix lighting issues inside tiles.
            drawOriginOffsetY -= 95;
            projectile.position -= 95 * forward;
            projectile.hostile = false;
            stuck = true;

            projectile.velocity = oldVelocity;

            return false;
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }

        private int stage
        {
            get => (int)projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        private float offset
        {
            get => (int)projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        private Player target
        {
            get => Main.player.ToArray()[(int)projectile.ai[2]];
        }

        private NPC npc
        {
            get => Main.npc[(int)projectile.ai[3]];
        }

        private Vector2 forward
        {
            get
            {
                float rotation = projectile.rotation - MathHelper.PiOver2;
                Vector2 output = new Vector2((float) Math.Cos(rotation), (float) Math.Sin(rotation));
                output.Normalize();
                return output;
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((short)projectile.ai[2]);
            writer.Write((short)projectile.ai[3]);
            writer.Write((short)timer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.ai[2] = reader.ReadInt16();
            projectile.ai[3] = reader.ReadInt16();
            timer = reader.ReadInt16();
        }

    }

}
