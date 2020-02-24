using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;


namespace Highlander.Dusts
{
    class AggressiveAleDustExplosive : ModDust
    {
        public override void SetDefaults()
        {

        }

        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 80;
            dust.velocity *= 2f;
            dust.noGravity = false;
        }

        public override bool Update(Dust dust)
        {
            float strength = dust.scale * dust.scale * dust.scale * dust.scale;
            if (strength > 1f)
            {
                strength = 1f;
            }
            if (strength > 0.33f)
            {
                Lighting.AddLight(dust.position, 0.23f * strength, 0.097f * strength, 0.003f * strength);
            }

            /**dust.position += dust.velocity;
            dust.scale -= 0.015f;
            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }**/

            return true;
        }
    }
}
