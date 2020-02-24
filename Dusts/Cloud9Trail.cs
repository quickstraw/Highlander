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
    class Cloud9Trail : ModDust
    {

        public override void SetDefaults()
        {
        }

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 7, 7);
            dust.alpha = 0;
            dust.noGravity = true;
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
                dust.scale -= 0.005f;

                dust.alpha += 2;

                if (timer > 50)
                {
                    data[0] = true;
                }

                if (!data[0])
                {
                    dust.alpha -= 3;
                }
                else
                {
                    dust.alpha += 2;
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
