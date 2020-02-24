/**using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Highlander.Dusts
{
    class AbstractGreenEnergy : ModDust
    {
        public Boolean ChangedVelocity { get; set; }

        public override void SetDefaults()
        {
        }

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 10, 10);
            dust.alpha = 128;
            dust.velocity *= 0.1f;
            dust.velocity.Y += -0.5f;
            dust.noGravity = true;
            dust.scale = 1f;

        }

        public override bool MidUpdate(Dust dust)
        {
            return true;
        }

        public override bool Update(Dust dust)
        {
            float low = 0.5f;
            float midLow = 0.6f;
            float midHigh = 0.7f;
            float high = 0.9f;

            // Here we use the customData field. If customeData is the type we expect, Player, we do some special movement.
            if (dust.customData != null && dust.customData is ModDustCustomData data && data.Player != null)
            {
                Player player = data.Player;
                if (dust.scale < 0.75 && !data[0])
                {
                    // Here we assign position to some offset from the player that was assigned. This offset scales with dust.scale. The scale and rotation cause the spiral movement we desired.
                    //dust.velocity.X += player.Center.X - dust.position.X; // Looks like double helix!
                    dust.velocity.X += (player.Center.X - dust.position.X) / 40;
                    data[0] = true;
                }

                dust.position.Y += player.velocity.Y * 0.5f;
                dust.position.X += player.velocity.X * 0.8f;

                float distance = Math.Abs(player.Center.X - dust.position.X);
                if (distance < 4.5)
                {
                    if (dust.scale <= low)
                    {
                        dust.frame = new Rectangle(0, 40, 10, 10);
                        dust.velocity.X *= 0.8f;
                    }
                    else if (dust.scale > low && dust.scale <= midLow)
                    {
                        dust.frame = new Rectangle(0, 30, 10, 10);
                    }
                    else if (dust.scale > midLow && dust.scale <= midHigh)
                    {
                        dust.frame = new Rectangle(0, 20, 10, 10);
                    }
                    else if (dust.scale > midHigh && dust.scale <= high)
                    {
                        dust.frame = new Rectangle(0, 10, 10, 10);
                    }
                    else if (dust.scale > high)
                    {
                        dust.frame = new Rectangle(0, 0, 10, 10);
                    }
                } else if(!data[1] && distance > player.width / 2 + 2)
                {
                    dust.scale = 0.8f;
                    dust.position.Y -= Main.rand.NextFloat(4f, 6f);
                }
                data[1] = true;
            }
            else
            {
                if (dust.scale <= low)
                {
                    dust.frame = new Rectangle(0, 40, 10, 10);
                    dust.velocity.X *= 0.8f;
                }
                else if (dust.scale > low && dust.scale <= midLow)
                {
                    dust.frame = new Rectangle(0, 30, 10, 10);
                }
                else if (dust.scale > midLow && dust.scale <= midHigh)
                {
                    dust.frame = new Rectangle(0, 20, 10, 10);
                }
                else if (dust.scale > midHigh && dust.scale <= high)
                {
                    dust.frame = new Rectangle(0, 10, 10, 10);
                }
                else if (dust.scale > high)
                {
                    dust.frame = new Rectangle(0, 0, 10, 10);
                }
            }

            float strength = dust.scale * 1.4f;
            if (strength > 1f)
            {
                strength = 1f;
            }
            Lighting.AddLight(dust.position, 0.13f * strength, 0.55f * strength, 0.32f * strength);

            dust.position += dust.velocity;
            dust.scale -= 0.015f;
            if (dust.scale < 0.2f)
            {
                dust.active = false;
            }
            return false;
        }

    }
}
**/