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
    class PurpleEnergyFaux : FauxDust
    {

        public PurpleEnergyFaux(PlayerDrawSet info, Vector2 offset, string texturePath, float scale) : base(info, offset, texturePath, scale)
        {
            OnSpawn();
        }

        public PurpleEnergyFaux(PlayerDrawSet info, Vector2 offset, Texture2D texture, float scale) : base(info, offset, texture, scale)
        {
            OnSpawn();
        }

        private void OnSpawn()
        {
            frame = new Rectangle(0, 0, 20, 8);
            alpha = 0;
            velocity *= 0.03f;
            velocity.Y += -0.6f;
            scale = 0.9f;
            Color = Color.Transparent;
            Color.R = 12;
            Color.B = 12;
            Color.G = 12;
            Color.A = (byte) alpha;
        }

        public override void Update()
        {
            int low = 40;
            int midLow = 30;
            int midHigh = 20;
            int high = 10;

            if (Player != null)
            {

                if (timer >= low)
                {
                    frame = new Rectangle(0, 32, 20, 8);
                    scale -= 0.015f;
                    Offset.X += 0.05563f * 0.2f;
                }
                else if (timer < low && timer >= midLow)
                {
                    frame = new Rectangle(0, 24, 20, 8);
                    scale -= 0.04f;
                    Offset.X += 0.2094f * 0.2f;
                }
                else if (timer < midLow && timer >= midHigh)
                {
                    frame = new Rectangle(0, 16, 20, 8);
                    scale -= 0.01f;
                    Offset.X += 0.05563f * 0.2f;
                }
                else if (timer < midHigh && timer >= high)
                {
                    frame = new Rectangle(0, 8, 20, 8);
                    scale += 0.01f;
                    Offset.X -= 0.05563f * 0.2f;
                }
                else if (timer < high)
                {
                    frame = new Rectangle(0, 0, 20, 8);
                    scale += 0.04f;
                    Offset.X -= 0.2094f * 0.2f;
                }

                float strength = scale * 1.4f;
                if (strength > 1f)
                {
                    strength = 1f;
                }
                Vector2 lightPos = new Vector2(Player.position.X + Player.width / 2 + Offset.X, Player.position.Y + Offset.Y);
                Lighting.AddLight(lightPos, 0.42f * strength, 0.24f * strength, 0.34f * strength);

                Offset += velocity;

                if (timer > 50)
                {
                    active = false;
                }

                timer++;

            }
        }

    }
}
