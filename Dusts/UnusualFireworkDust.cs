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
    class UnusualFireworkDust : ModDust
    {
        public override void SetStaticDefaults()
        {

        }

        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 100;
            dust.noGravity = true;
            //dust.noLight = true;
            dust.scale = 1.2f;
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
                Lighting.AddLight(dust.position, 0.51f * strength, 0.23f * strength, 0.26f * strength);
            }

            dust.position += dust.velocity;
            dust.scale -= 0.001f;

            if(dust.velocity.LengthSquared() < 1)
            {
                dust.scale -= 0.009f;
            }

            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }

            dust.velocity *= 0.95f;

            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White;
        }
    }
}
