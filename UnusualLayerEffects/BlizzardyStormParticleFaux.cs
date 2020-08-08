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
    class BlizzardyStormParticleFaux : FauxDust
    {

        int timer;

        public BlizzardyStormParticleFaux(PlayerDrawInfo info, Vector2 offset, string texturePath, float scale) : base(info, offset, texturePath, scale)
        {
            OnSpawn();
        }

        public BlizzardyStormParticleFaux(PlayerDrawInfo info, Vector2 offset, Texture2D texture, float scale) : base(info, offset, texture, scale)
        {
            OnSpawn();
        }

        private void OnSpawn()
        {
            int zoop = Main.rand.Next(0, 2) * 5;
            frame = new Rectangle(0, zoop, 5, 5);
            alpha = 128;
            velocity.X *= 0.3f;
            if (velocity.Y < 0)
            {
                velocity.Y *= -1;
            }
            scale = 1f;
        }

        public override void Update()
        {
            Offset += velocity;
            scale -= 0.01f;

            alpha += 1;

            if (timer > 30)
            {
                active = false;
            }

            timer++;
        }

    }
}
