using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.UnusualLayerEffects
{
    class StormyStormParticleFaux : FauxDust
    {

        int timer;

        public StormyStormParticleFaux(PlayerDrawInfo info, Vector2 offset, string texturePath, float scale) : base(info, offset, texturePath, scale)
        {
            OnSpawn();
        }

        public StormyStormParticleFaux(PlayerDrawInfo info, Vector2 offset, Texture2D texture, float scale) : base(info, offset, texture, scale)
        {
            OnSpawn();
        }

        private void OnSpawn()
        {
            int zoop = Main.rand.Next(0, 2) * 5;
            frame = new Rectangle(0, zoop, 5, 5);
            alpha = 160;
            velocity.X *= 0.05f;
            if (velocity.Y < 0)
            {
                velocity.Y *= -1;
            }
            velocity.Y *= 0.5f;
            velocity.Y += 0.4f;
            scale = 1f;
        }

        public override void Update()
        {
            Offset += velocity;
            scale -= 0.01f;

            alpha += 1;

            if (timer % 2 == 0)
            {
                alpha += 1;
            }

            if (timer > 20)
            {
                active = false;
            }

            velocity.Y += 0.06f;
            timer++;
        }

    }
}
