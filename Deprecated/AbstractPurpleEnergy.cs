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
    class AbstractPurpleEnergy : ModDust
    {
        public Boolean ChangedVelocity { get; set; }

        public override void SetDefaults()
        {
        }

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 10, 10);
            dust.alpha = 128;
            dust.velocity *= 0.2f;
            dust.velocity.Y += -0.5f;
            dust.noGravity = true;
            dust.scale = 1.1f;
        }

        public override bool MidUpdate(Dust dust)
        {
            return false;
        }

        public override bool Update(Dust dust)
        {
            float low = 0.5f;
            float midLow = 0.65f;
            float midHigh = 0.8f;
            float high = 0.925f;

            // Here we use the customData field. If customeData is the type we expect, Player, we do some special movement.
            if (dust.customData != null && dust.customData is ModDustCustomData data && data.Player != null)
            {
                Player player = data.Player;

                if (data[0])
                {
                    dust.velocity.X *= 0.97f;
                }

                if (dust.scale < midHigh && !data[0])
                {
                    // Here we assign position to some offset from the player that was assigned. This offset scales with dust.scale. The scale and rotation cause the spiral movement we desired.
                    //dust.velocity.X += player.Center.X - dust.position.X; // Looks like double helix!
                    dust.velocity.X += (player.Center.X - dust.position.X) / 20;
                    dust.velocity.Y *= 0.7f;
                    data[0] = true;
                }

                dust.position.Y += player.velocity.Y * 1f;
                dust.position.X += player.velocity.X * 0.95f;

                float distance = Math.Abs(player.Center.X - dust.position.X);
                if (distance < 5)
                {
                    if (dust.scale <= low)
                    {
                        dust.frame = new Rectangle(0, 40, 10, 10);
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
                if(!data[1]) //&& distance > player.width / 2 + 2)
                {
                    if(distance / 8 > 1)
                    {
                        dust.scale /= (distance / 8);
                    }
                    dust.position.Y -= distance * distance / 15;
                }
                data[1] = true;
                if(dust.scale <= low)
                {
                    //dust.velocity.X *= 0.8f;
                    dust.velocity.Y *= 0.8f;
                }
                else if(dust.scale < midLow)
                {
                    dust.velocity.Y *= 0.9f;
                }
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
            Lighting.AddLight(dust.position, 0.42f * strength, 0.24f * strength, 0.34f * strength);

            dust.position += dust.velocity;

            if(dust.scale > 1)
            {
                dust.scale -= 0.1f;
            }
            else
            {
                dust.scale -= 0.015f;
            }
            
            if (dust.scale < 0.3f)
            {
                dust.active = false;
            }
            return false;
        }

    }
}
**/