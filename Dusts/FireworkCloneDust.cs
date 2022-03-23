using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace Highlander.Dusts
{
    class FireworkCloneDust : ModDust
    {

        public override void SetStaticDefaults()
        {
            
        }

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
        }

        public override bool Update(Dust dust)
        {
            Dust dust1 = dust;

            if ((double)dust1.fadeIn > 0.0 && (double)dust1.fadeIn < 100.0)
            {
                dust1.scale += 0.03f;
                if ((double)dust1.scale > (double)dust1.fadeIn)
                    dust1.fadeIn = 0.0f;
            }
            else
            {
                dust1.scale -= 0.01f;
            }

            // -- //
            float num2 = dust1.scale;
            if ((double)num2 > 1.0) {
                num2 = 1f;
            }

            Lighting.AddLight((int)(dust1.position.X / 16.0), (int)(dust1.position.Y / 16.0), num2 * 1f, num2 * 0.5f, num2 * 0.4f);

            if (dust1.noGravity)
            {
                Dust dust3 = dust1;
                Vector2 vector2_2 = dust3.velocity * 0.93f;
                dust3.velocity = vector2_2;
                if ((double)dust1.fadeIn == 0.0)
                    dust1.scale += 1f / 400f;
            }
            else
            {
                Dust dust3 = dust1;
                Vector2 vector2_2 = dust3.velocity * 0.95f;
                dust3.velocity = vector2_2;
                dust1.scale -= 1f / 400f;
            }

            return base.Update(dust);
        }

    }
}
