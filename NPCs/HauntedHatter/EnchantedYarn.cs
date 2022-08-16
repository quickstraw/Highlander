using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.NPCs.HauntedHatter
{
    class EnchantedYarn : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true; // projectiles with hide but without this will draw in the lighting values of the owner player.
        }


        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.scale = 0.4f;
            DrawOriginOffsetY = -6;
            DrawOffsetX = -6;
            Projectile.hide = true;
        }

        public override void AI()
        {
            if(Projectile.scale < 1.0f)
            {
                Projectile.scale += 0.01f;
            }

            Projectile.velocity.Y += 0.6f;
            //projectile.rotation += projectile.velocity.X / 20;
            if(Projectile.velocity.X > 0)
            {
                Projectile.rotation += AngularVelocity / 10;
            }
            else if(Projectile.velocity.X < 0)
            {
                Projectile.rotation -= AngularVelocity / 10;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool kill = false;
            if (Projectile.ai[0] < 3)
            {
                Projectile.velocity = oldVelocity;
                Projectile.velocity.Y *= -0.9f;
                Projectile.velocity.Y += 1.5f;
                if (Projectile.velocity.Y > 0 || Math.Abs(Projectile.velocity.Y) < 2)
                {
                    Projectile.velocity.Y = 0;
                }
                Projectile.ai[0]++;
                Projectile.netUpdate = true;
                //SoundEngine.PlaySound(SoundID.Dig, (int)Projectile.position.X, (int)Projectile.position.Y);
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }
            else
            {
                Projectile.velocity *= 0;
                kill = true;
                //SoundEngine.PlaySound(SoundID.Item10, (int)Projectile.position.X, (int)Projectile.position.Y);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            }
            return kill;
        }

        public override void Kill(int timeLeft)
        {
            var gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position, Projectile.velocity * 0.4f, Mod.Find<ModGore>("EnchantedYarn").Type, 1f);
            gore.rotation = Projectile.rotation;
        }

        private float AngularVelocity
        {
            get => MathHelper.TwoPi * Projectile.velocity.Length() / 60;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }

    }
}
