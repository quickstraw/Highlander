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
    class Cloud9 : ModDust
    {

        public override void SetStaticDefaults()
        {
        }

        public override void OnSpawn(Dust dust)
        {
            int zoop = Main.rand.Next(0, 3) * 8;
            dust.frame = new Rectangle(0, zoop, 7, 8);
            dust.alpha = 255;
            dust.velocity *= 0.3f;
            dust.velocity.Y *= 0.1f;
            if (dust.velocity.Y > 0)
            {
                dust.velocity.Y *= -1;
            }
            dust.velocity.Y += -0.3f;
            dust.position.X += 0;
            dust.noGravity = true;
            dust.scale = 2.5f;
        }

        public override bool MidUpdate(Dust dust)
        {
            return true;
        }

        public override bool Update(Dust dust)
        {
            if (dust.customData != null && dust.customData is ModDustCustomData initData && !initData[7] && initData.Player != null)
            {
                initData[7] = true;
                initData.offset = dust.position - initData.Player.position;
            }
            // Here we use the customData field. If customData is the type we expect, Player, we do some special movement.
            if (dust.customData != null && dust.customData is ModDustCustomData data && data.Player != null)
            {
                int timer = data.timer;

                Player player = data.Player;

                float strength = dust.scale * 0.4f;
                if (strength > 1f)
                {
                    strength = 1f;
                }
                if (strength > 0.1f)
                {
                    Lighting.AddLight(dust.position, 0.36f * strength, 0.28f * strength, 0.36f * strength);
                }

                if (!player.wet)
                {
                    dust.position.Y += player.velocity.Y * 1f;
                    dust.position.X += player.velocity.X * 0.95f;
                }
                else
                {
                    dust.position.Y += player.velocity.Y * 0.5f;
                    dust.position.X += player.velocity.X * 0.4f;
                }

                data.offset += dust.velocity;
                dust.position = data.offset + player.position;
                dust.scale *= 1.003f;

                if(timer > 50)
                {
                    data[0] = true;
                }

                if (!data[0])
                {
                    dust.alpha -= 6;
                }
                else
                {
                    dust.alpha += 5;
                }

                if (timer > 80)
                {
                    dust.active = false;
                }

                data.timer++;

            }
            return false;
        }
    }
}
