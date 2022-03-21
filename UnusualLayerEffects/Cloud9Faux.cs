using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Highlander.UnusualLayerEffects
{
    class Cloud9Faux : FauxDust
    {

        private int timer = 0;
        bool ready;

        public Cloud9Faux(PlayerDrawSet info, Vector2 offset, string texturePath, float scale) : base(info, offset, texturePath, scale)
        {
            OnSpawn();
        }

        public Cloud9Faux(PlayerDrawSet info, Vector2 offset, Texture2D texture, float scale) : base(info, offset, texture, scale)
        {
            OnSpawn();
        }

        private void OnSpawn()
        {
            int zoop = Main.rand.Next(0, 3) * 8;
            frame = new Rectangle(0, zoop, 7, 8);
            alpha = 255;
            velocity *= 0.3f;
            velocity.Y *= 0.1f;
            if (velocity.Y > 0)
            {
                velocity.Y *= -1;
            }
            velocity.Y += -0.3f;
            scale = 2.5f;
            Color = Color.White;
        }

        public override void Update()
        {
            float strength = scale * 0.4f;
            if (strength > 1f)
            {
                strength = 1f;
            }
            if (strength > 0.1f)
            {
                Lighting.AddLight(Player.position + Offset, 0.36f * strength, 0.28f * strength, 0.36f * strength);
            }

            Offset += velocity;
            scale *= 1.003f;

            if (timer > 50)
            {
                ready = true;
            }

            if (!ready)
            {
                alpha -= 6;
            }
            else
            {
                alpha += 5;
            }

            if (timer > 80)
            {
                active = false;
            }

            timer++;
        }

    }
}
