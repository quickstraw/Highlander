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
    public class TallHatLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new AfterParent(PlayerDrawLayers.Head);//new Between(PlayerDrawLayers.Head, PlayerDrawLayers.FinchNest);
        }

        // Returning true in this property makes this layer appear on the minimap player head icon.
        public override bool IsHeadLayer => true;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            HighlanderPlayer player = drawInfo.drawPlayer.GetModPlayer<HighlanderPlayer>();
            // The layer will be visible only if the player is holding an ExampleItem in their hands. Or if another modder forces this layer to be visible.
            return player.tallHat != 0;

            // If you'd like to reference another PlayerDrawLayer's visibility,
            // you can do so by getting its instance via ModContent.GetInstance<OtherDrawLayer>(), and calling GetDefaultVisiblity on it
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Mod mod = Highlander.Instance;
            Player drawPlayer = drawInfo.drawPlayer;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            string texPath = "";

            switch (modPlayer.tallHat)
            {
                case TallHat.ToySoldier:
                    texPath = "Highlander/Utilities/TallHatTextures/ToySoldier";
                    break;
                case TallHat.CroneDome:
                    texPath = "Highlander/Utilities/TallHatTextures/CroneDome";
                    break;
                case TallHat.SearedSorcerer:
                    texPath = "Highlander/Utilities/TallHatTextures/SearedSorcerer";
                    break;
                case TallHat.SirPumpkinton:
                    texPath = "Highlander/Utilities/TallHatTextures/SirPumpkinton";
                    break;
                default:
                    return;
            }

            DrawData tallHatData = TallHatData(drawInfo, texPath, 4, 0);
            tallHatData.shader = drawPlayer.dye[0].dye;

            drawInfo.DrawDataCache.Add(tallHatData);
        }

        public static DrawData TallHatData(PlayerDrawSet drawInfo, string hat, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();

            var dye = drawPlayer.dye[0];

            float scale = 1f;
            Texture2D texture = Request<Texture2D>(hat).Value;
            int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.Position.Y + yOffset + 0 - Main.screenPosition.Y);

            int playerFrame = drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height;
            if (playerFrame == 7 || playerFrame == 8 || playerFrame == 9 || playerFrame == 14 || playerFrame == 15 || playerFrame == 16)
            {
                drawY -= 2;
            }

            if (drawPlayer.mount.Active)
            {
                MountData data = drawPlayer.mount._data;

                Vector2 pos = new Vector2();
                pos.Y += data.heightBoost;

                pos += drawInfo.Position;
                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset - Main.screenPosition.Y);
            }

            Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height);

            int cX = (int)(drawPlayer.position.X / 16f);
            int cY = (int)((drawPlayer.position.Y) / 16f);
            Color color = Lighting.GetColor(cX, cY, Color.White);

            SpriteEffects effect = drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            return new DrawData(texture, new Vector2(drawX, drawY), frame, color, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, effect, 0);
        }
    }

    public enum TallHat : int
    {
        None = 0,
        ToySoldier = 1,
        CroneDome = 2,
        SearedSorcerer = 3,
        SirPumpkinton = 4,
        Max
    }
}
