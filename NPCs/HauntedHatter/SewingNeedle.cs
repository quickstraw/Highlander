using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
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
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true; // projectiles with hide but without this will draw in the lighting values of the owner player.
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.penetrate = -1;
            //drawOffsetX = -2;
            DrawOriginOffsetY = -46;
            Projectile.ai = new float[4];
            Projectile.hostile = true;
            Projectile.hide = true;
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
                        float angle = (float)Math.Atan2(target.Center.Y - Projectile.Center.Y, target.Center.X - Projectile.Center.X + offset) + MathHelper.PiOver2;
                        float rotation = Projectile.rotation % MathHelper.TwoPi;
                        if (((rotation + 0.1f >= angle && rotation - 0.1f <= angle) || timer > 75) && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.rotation = angle;
                            Projectile.velocity *= 0;
                            stage++;
                            timer = 0;
                            Projectile.netUpdate = true;
                            break;
                        }
                    }
                    else
                    {
                        Projectile.rotation += 0.1f;
                    }
                    if (timer <= 30)
                    {
                        if(timer == 30 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.netUpdate = true;
                        }
                        Projectile.rotation += 0.1f;
                    }
                    if (timer <= 15)
                    {
                        if (timer == 15 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.netUpdate = true;
                        }
                        Projectile.rotation += 0.2f;
                    }
                    if (timer == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.rotation += 0.15f;
                    Projectile.velocity = npc.velocity;
                    timer += 1;
                    break;
                case 1:
                    float CoolAngle = (float)Math.Atan2(target.Center.Y - Projectile.Center.Y, target.Center.X - Projectile.Center.X + offset) + MathHelper.PiOver2;
                    Projectile.rotation = CoolAngle;
                    DrawOriginOffsetY += 5;
                    Projectile.position += 5 * forward;
                    if (DrawOriginOffsetY >= 0)
                    {
                        int diff = -DrawOriginOffsetY;
                        Projectile.position -= diff * forward;
                        Projectile.velocity = 10 * forward;
                        stage++;
                    }
                    break;
                case 2:
                    if (Projectile.velocity.Length() > 24)
                    {
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 24;
                    }
                    else if(Projectile.velocity.Length() < 20)
                    {
                        Projectile.velocity += forward * 0.3f;
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
                Projectile.velocity *= 0;
            }
            if (timer > 180)
            {
                Projectile.alpha = (Projectile.alpha + 2) % 255;
            }
            if (timer > 300)
            {
                Projectile.Kill();
            }
            timer++;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                //SoundEngine.PlaySound(0, (int)Projectile.position.X, (int)Projectile.position.Y); Idk what sound 0 is. [IMPORTANT]
                //SoundEngine.PlaySound(SoundID.Item10.WithPitchVariance(0.1f).WithVolume(0.9f), Projectile.Center);
                SoundEngine.PlaySound(SoundID.Item10 with { PitchVariance = 0.1f, Volume = 0.9f }, Projectile.Center);
            }
            // Changing origin to fix lighting issues inside tiles.
            DrawOriginOffsetY -= 95;
            Projectile.position -= 95 * forward;
            Projectile.hostile = false;
            stuck = true;

            Projectile.velocity = oldVelocity;

            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        private int stage
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private float offset
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        private Player target
        {
            get => Main.player.ToArray()[(int)Projectile.ai[2]];
        }

        private NPC npc
        {
            get => Main.npc[(int)Projectile.ai[3]];
        }

        private Vector2 forward
        {
            get
            {
                float rotation = Projectile.rotation - MathHelper.PiOver2;
                Vector2 output = new Vector2((float) Math.Cos(rotation), (float) Math.Sin(rotation));
                output.Normalize();
                return output;
            }
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((short)Projectile.ai[2]);
            writer.Write((short)Projectile.ai[3]);
            writer.Write((short)timer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.ai[2] = reader.ReadInt16();
            Projectile.ai[3] = reader.ReadInt16();
            timer = reader.ReadInt16();
        }

    }

}
