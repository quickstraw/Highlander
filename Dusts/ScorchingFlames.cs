using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Highlander.Dusts
{
    class ScorchingFlames : ModDust
    {

        public override void SetStaticDefaults()
        {
        }

        public override void OnSpawn(Dust dust)
        {
            int zoop = Main.rand.Next(0, 4) * 10;
            dust.frame = new Rectangle(zoop, 0, 10, 10);
            dust.alpha = 128;
            dust.velocity *= 0.3f;
            dust.velocity.Y += -0.3f;
            dust.noGravity = true;
            dust.scale = 1.5f;
        }

        public override bool MidUpdate(Dust dust)
        {
            return false;
        }

        public override bool Update(Dust dust)
        {
            float low = 0.5f;
            float midLow = 0.6f;
            float midHigh = 0.7f;
            float high = 0.9f;

            if (dust.customData != null && dust.customData is ModDustCustomData initData && !initData[7] && initData.Player != null)
            {
                initData[7] = true;
                initData.offset = dust.position - initData.Player.position;
            }
            if (dust.customData != null && dust.customData is ModDustCustomData data)
            {
                Player player = data.Player;

                if (dust.scale < 0.8 && !data[3])
                {
                    // Here we assign position to some offset from the player that was assigned. This offset scales with dust.scale. The scale and rotation cause the spiral movement we desired.
                    //dust.velocity.X += player.Center.X - dust.position.X; // Looks like double helix!
                    dust.velocity.X += (player.Center.X - dust.position.X) / 20;
                    data[3] = true;
                }

                if (!player.wet)
                {
                    dust.position.Y += player.velocity.Y * 0.9f;
                    dust.position.X += player.velocity.X * 0.8f;
                }
                else
                {
                    dust.position.Y += player.velocity.Y * 0.5f;
                    dust.position.X += player.velocity.X * 0.4f;
                }

                if (dust.scale <= low && !data[2])
                {
                    int zoop = Main.rand.Next(0, 4) * 10;
                    dust.frame = new Rectangle(zoop, 30, 10, 10);
                    data[2] = true;
                }
                else if (dust.scale > midLow && dust.scale <= midHigh && !data[1])
                {
                    int zoop = Main.rand.Next(0, 4) * 10;
                    dust.frame = new Rectangle(zoop, 20, 10, 10);
                    data[1] = true;
                }
                else if (dust.scale > midHigh && dust.scale <= high && !data[0])
                {
                    int zoop = Main.rand.Next(0, 4) * 10;
                    dust.frame = new Rectangle(zoop, 10, 10, 10);
                    data[0] = true;
                }

                data.offset += dust.velocity;
                dust.position = data.offset + player.position;
            }

            float strength = dust.scale * 1.4f;
            if (strength > 1f)
            {
                strength = 1f;
            }
            Lighting.AddLight(dust.position, 0.13f * strength, 0.55f * strength, 0.32f * strength);

            if (dust.scale > 1f)
            {
                dust.scale -= 0.1f;
            }
            else
            {
                dust.scale -= 0.02f;
            }
            if (dust.scale < 0.2f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
