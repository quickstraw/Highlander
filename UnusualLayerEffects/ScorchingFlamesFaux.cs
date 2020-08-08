﻿using Microsoft.Xna.Framework;
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
    class ScorchingFlamesFaux : FauxDust
    {

        BitsByte flags;

        public ScorchingFlamesFaux(PlayerDrawInfo info, Vector2 offset, string texturePath, float scale) : base(info, offset, texturePath, scale)
        {
            OnSpawn();
        }

        public ScorchingFlamesFaux(PlayerDrawInfo info, Vector2 offset, Texture2D texture, float scale) : base(info, offset, texture, scale)
        {
            OnSpawn();
        }

        private void OnSpawn()
        {
            int zoop = Main.rand.Next(0, 4) * 10;
            frame = new Rectangle(zoop, 0, 10, 10);
            alpha = 128;
            velocity *= 0.3f;
            velocity.Y += -0.3f;
            scale = 1.5f;
        }

        public override void Update()
        {
            float low = 0.5f;
            float midLow = 0.6f;
            float midHigh = 0.7f;
            float high = 0.9f;

            if (scale < 0.8 && !flags[3])
            {
                // Here we assign position to some offset from the player that was assigned. This offset scales with scale. The scale and rotation cause the spiral movement we desired.
                //velocity.X += player.Center.X - position.X; // Looks like double helix!
                velocity.X += (Player.Center.X - Offset.X) / 20;
                flags[3] = true;
            }

            if (scale <= low && !flags[2])
            {
                int zoop = Main.rand.Next(0, 4) * 10;
                frame = new Rectangle(zoop, 30, 10, 10);
                flags[2] = true;
            }
            else if (scale > midLow && scale <= midHigh && !flags[1])
            {
                int zoop = Main.rand.Next(0, 4) * 10;
                frame = new Rectangle(zoop, 20, 10, 10);
                flags[1] = true;
            }
            else if (scale > midHigh && scale <= high && !flags[0])
            {
                int zoop = Main.rand.Next(0, 4) * 10;
                frame = new Rectangle(zoop, 10, 10, 10);
                flags[0] = true;
            }

            Offset += velocity;

            float strength = scale * 1.4f;
            if (strength > 1f)
            {
                strength = 1f;
            }

            if (scale > 1f)
            {
                scale -= 0.1f;
            }
            else
            {
                scale -= 0.02f;
            }
            if (scale < 0.2f)
            {
                active = false;
            }
        }

    }
}
