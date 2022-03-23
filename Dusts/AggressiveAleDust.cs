using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.Dusts
{
    class AggressiveAleDust : ModDust
    {
        public override void SetStaticDefaults()
        {

        }

        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 80;
            dust.velocity *= 0.2f;
            dust.velocity.Y = -1.2f;
            dust.noGravity = false;
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
                Lighting.AddLight(dust.position, 0.23f * strength, 0.097f * strength, 0.003f * strength);
            }

            dust.position += dust.velocity;
            dust.scale -= 0.015f;
            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }

            return false;
        }
    }
}
