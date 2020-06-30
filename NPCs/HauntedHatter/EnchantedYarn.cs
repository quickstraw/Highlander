using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.NPCs.HauntedHatter
{
    class EnchantedYarn : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true; // projectiles with hide but without this will draw in the lighting values of the owner player.
        }


        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.scale = 0.4f;
            drawOriginOffsetY = -6;
            drawOffsetX = -6;
            projectile.hide = true;
        }

        public override void AI()
        {
            if(projectile.scale < 1.0f)
            {
                projectile.scale += 0.01f;
            }

            projectile.velocity.Y += 0.6f;
            //projectile.rotation += projectile.velocity.X / 20;
            if(projectile.velocity.X > 0)
            {
                projectile.rotation += AngularVelocity / 10;
            }
            else if(projectile.velocity.X < 0)
            {
                projectile.rotation -= AngularVelocity / 10;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bool kill = false;
            if (projectile.ai[0] < 3)
            {
                projectile.velocity = oldVelocity;
                projectile.velocity.Y *= -0.9f;
                projectile.velocity.Y += 1.5f;
                if (projectile.velocity.Y > 0 || Math.Abs(projectile.velocity.Y) < 2)
                {
                    projectile.velocity.Y = 0;
                }
                projectile.ai[0]++;
                projectile.netUpdate = true;
                Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
            }
            else
            {
                projectile.velocity *= 0;
                kill = true;
                Main.PlaySound(SoundID.Item10, (int)projectile.position.X, (int)projectile.position.Y);
            }
            return kill;
        }

        public override void Kill(int timeLeft)
        {
            var gore = Gore.NewGoreDirect(projectile.position, projectile.velocity * 0.4f, mod.GetGoreSlot("Gores/EnchantedYarn"), 1f);
            gore.rotation = projectile.rotation;
        }

        private float AngularVelocity
        {
            get => MathHelper.TwoPi * projectile.velocity.Length() / 60;
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindNPCs.Add(index);
        }

    }
}
