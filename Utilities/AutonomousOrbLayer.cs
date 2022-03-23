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
using static Terraria.Mount;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Utilities
{
    public class AutonomousOrbLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new Between(PlayerDrawLayers.FinchNest, PlayerDrawLayers.Head);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Mod mod = Highlander.Instance;
            Player drawPlayer = drawInfo.drawPlayer;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            string texPath = "Items/Armor/AutonomousOrb_Effects";

            DrawData orbData = AutonomousOrbData(drawInfo, texPath, -69, 0);
            orbData.shader = drawPlayer.dye[0].dye;

            drawInfo.DrawDataCache.Add(orbData);
        }

        public static DrawData AutonomousOrbData(PlayerDrawSet drawInfo, string autonomousOrb, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();

            var dye = drawPlayer.dye[0];

            // Tick up the hat effect timer
            modPlayer.hatEffectTime = (short)((modPlayer.hatEffectTime + 1) % 47);
            short timer = modPlayer.hatEffectTime;

            float scale = 1f;
            Texture2D texture = Request<Texture2D>(autonomousOrb).Value;
            int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.Position.Y + yOffset + 70 - Main.screenPosition.Y);
            int numFrames = 1;
            int currFrame = 0;

            if (drawPlayer.mount.Active)
            {
                MountData data = drawPlayer.mount._data;

                Vector2 pos = new Vector2();
                pos.Y += data.heightBoost;

                pos += drawInfo.Position;
                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset + 70 - Main.screenPosition.Y);
            }

            if (timer <= 14)
            {
                //currFrame = 0;
                drawY -= 0;
            }
            else if (timer <= 23)
            {
                //currFrame = 1;
                drawY -= 1;
            }
            else if (timer <= 38)
            {
                //currFrame = 2;
                drawY -= 2;
            }
            else if (timer <= 47)
            {
                //currFrame = 3;
                drawY -= 1;
            }

            Rectangle frame = new Rectangle(0, currFrame * texture.Height / numFrames, texture.Width, texture.Height / numFrames);

            int cX = (int)(drawPlayer.position.X / 16f);
            int cY = (int)((drawPlayer.position.Y) / 16f);
            Color color = Lighting.GetColor(cX, cY, Color.White);

            return new DrawData(texture, new Vector2(drawX, drawY), frame, color, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0);
        }
    }
}
