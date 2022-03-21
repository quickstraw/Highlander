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
    class Cloud9TrailFaux : FauxDust
    {

        private int timer = 0;
        bool ready;

        public Cloud9TrailFaux(PlayerDrawSet info, Vector2 offset, string texturePath, float scale) : base(info, offset, texturePath, scale)
        {
            OnSpawn();
        }

        public Cloud9TrailFaux(PlayerDrawSet info, Vector2 offset, Texture2D texture, float scale) : base(info, offset, texture, scale)
        {
            OnSpawn();
        }

        private void OnSpawn()
        {
            frame = new Rectangle(0, 0, 7, 7);
            alpha = 0;
            Color = Color.White;
        }

        public override void Update()
        {
            Offset += velocity;
            scale -= 0.005f;

            alpha += 2;

            if (timer > 50)
            {
                ready = true;
            }

            if (!ready)
            {
                alpha -= 3;
            }
            else
            {
                alpha += 2;
            }

            if (timer > 80)
            {
                active = false;
            }

            timer++;
        }

    }
}
