using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Highlander.UnusualLayerEffects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Mount;

namespace Highlander.Utilities
{
    class HatEffects
    {

        public static readonly PlayerLayer AutonomousOrb = new PlayerLayer("Highlander", "AutonomousOrb", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            Mod mod = Highlander.Instance;
            Player drawPlayer = drawInfo.drawPlayer;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            string texPath = "Items/Armor/AutonomousOrb_Effects";

            DrawData orbData = AutonomousOrbData(drawInfo, texPath, -69, 0);
            orbData.shader = drawPlayer.dye[0].dye;

            Main.playerDrawData.Add(orbData);
        });

        public static DrawData AutonomousOrbData(PlayerDrawInfo drawInfo, string autonomousOrb, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();

            var dye = drawPlayer.dye[0];

            // Tick up the hat effect timer
            modPlayer.hatEffectTime = (short)((modPlayer.hatEffectTime + 1) % 47);
            short timer = modPlayer.hatEffectTime;

            float scale = 1f;
            Texture2D texture = mod.GetTexture(autonomousOrb);
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + yOffset + 70 - Main.screenPosition.Y);
            int numFrames = 1;
            int currFrame = 0;

            if (drawPlayer.mount.Active)
            {
                MountData data = drawPlayer.mount._data;

                Vector2 pos = new Vector2();
                pos.Y += data.heightBoost;

                pos += drawInfo.position;
                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset + 70 - Main.screenPosition.Y);
            }

            if (timer <= 14)
            {
                //currFrame = 0;
                drawY -= 0;
            } else if(timer <= 23)
            {
                //currFrame = 1;
                drawY -= 1;
            } else if (timer <= 38)
            {
                //currFrame = 2;
                drawY -= 2;
            } else if(timer <= 47)
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

        public static readonly PlayerLayer TallHatLayer = new PlayerLayer("Highlander", "TallHat", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            Mod mod = Highlander.Instance;
            Player drawPlayer = drawInfo.drawPlayer;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            string texPath = "";

            switch (modPlayer.tallHat)
            {
                case TallHat.ToySoldier:
                    texPath = "Utilities/TallHatTextures/ToySoldier";
                    break;
                case TallHat.CroneDome:
                    texPath = "Utilities/TallHatTextures/CroneDome";
                    break;
                case TallHat.SearedSorcerer:
                    texPath = "Utilities/TallHatTextures/SearedSorcerer";
                    break;
                case TallHat.SirPumpkinton:
                    texPath = "Utilities/TallHatTextures/SirPumpkinton";
                    break;
                default:
                    return;
            }

            DrawData tallHatData = TallHatData(drawInfo, texPath, 4, 0);
            tallHatData.shader = drawPlayer.dye[0].dye;

            Main.playerDrawData.Add(tallHatData);
        });

        public static DrawData TallHatData(PlayerDrawInfo drawInfo, string hat, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();

            var dye = drawPlayer.dye[0];

            float scale = 1f;
            Texture2D texture = mod.GetTexture(hat);
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + yOffset + 0 - Main.screenPosition.Y);

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

                pos += drawInfo.position;
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
