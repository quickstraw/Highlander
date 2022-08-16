using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.Dusts
{
    class EnlightenmentIdolDust : ModDust
    {
        public override void SetStaticDefaults()
        {

        }

        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 80;
            dust.velocity *= 0.2f;
            dust.velocity.Y -= 0.1f;
            dust.noGravity = true;
            dust.scale = 0.8f;
        }

        public override bool Update(Dust dust)
        {

            float strength = dust.scale * 1.4f;
            if (strength > 1f)
            {
                strength = 1f;
            }
            if (strength > 0.1f)
            {
                Lighting.AddLight(dust.position, 0.42f * strength, 0.37f * strength, 0.21f * strength);
            }

            dust.position += dust.velocity;
            dust.scale -= 0.015f;
            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }

            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White * (255f - dust.alpha);
        }
    }
}
