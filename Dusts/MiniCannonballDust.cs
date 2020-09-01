using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.Dusts
{
    class MiniCannonballDust : ModDust
    {
        public override void SetDefaults()
        {

        }

        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 20;
            //dust.velocity *= 0.2f;
            //dust.velocity.Y -= 2f;
            dust.noGravity = false;
        }

        public override bool Update(Dust dust)
        {

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
