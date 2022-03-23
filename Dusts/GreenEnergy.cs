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
    class GreenEnergy : ModDust
    {
        public Boolean ChangedVelocity { get; set; }

        public override void SetStaticDefaults()
        {
        }

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 20, 8);
            dust.alpha = 240;
            dust.velocity *= 0.03f;
            dust.velocity.Y += -0.6f;
            dust.noGravity = true;
            dust.scale = 0.6f;
        }

        public override bool MidUpdate(Dust dust)
        {
            return true;
        }

        public override bool Update(Dust dust)
        {

            int low = 40;
            int midLow = 30;
            int midHigh = 20;
            int high = 10;

            if (dust.customData != null && dust.customData is ModDustCustomData initData && !initData[7] && initData.Player != null)
            {
                initData[7] = true;
                initData.offset = dust.position - initData.Player.position;
            }
            // Here we use the customData field. If customeData is the type we expect, Player, we do some special movement.
            if (dust.customData != null && dust.customData is ModDustCustomData data && data.Player != null)
            {
                int timer = data.timer;

                Player player = data.Player;

                if (timer >= low)
                {
                    dust.frame = new Rectangle(0, 32, 20, 8);
                    dust.scale -= 0.01f;
                    data.offset.X += 0.05563f;
                }
                else if (timer < low && timer >= midLow)
                {
                    dust.frame = new Rectangle(0, 24, 20, 8);
                    dust.scale -= 0.04f;
                    data.offset.X += 0.2094f;
                }
                else if (timer < midLow && timer >= midHigh)
                {
                    dust.frame = new Rectangle(0, 16, 20, 8);
                    dust.scale -= 0.01f;
                    data.offset.X += 0.05563f;
                }
                else if (timer < midHigh && timer >= high)
                {
                    dust.frame = new Rectangle(0, 8, 20, 8);
                    dust.scale += 0.01f;
                    data.offset.X -= 0.05563f;
                }
                else if (timer < high)
                {
                    dust.frame = new Rectangle(0, 0, 20, 8);
                    dust.scale += 0.04f;
                    data.offset.X -= 0.2094f;
                }

                float strength = dust.scale * 1.4f;
                if (strength > 1f)
                {
                    strength = 1f;
                }
                Lighting.AddLight(dust.position, 0.13f * strength, 0.55f * strength, 0.32f * strength);

                data.offset += dust.velocity;
                dust.position = data.offset + player.position;

                if (timer > 50)
                {
                    dust.active = false;
                }

                data.timer++;

            }
            return false;
        }

    }
}
