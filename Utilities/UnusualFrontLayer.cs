using Highlander.UnusualLayerEffects;
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
using static Highlander.Utilities.UnusualLayer;
using static Terraria.Mount;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Utilities
{
    public class UnusualFrontLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new Between(PlayerDrawLayers.FaceAcc, PlayerDrawLayers.MountFront);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            HighlanderPlayer player = drawInfo.drawPlayer.GetModPlayer<HighlanderPlayer>();
            // The layer will be visible only if the player is holding an ExampleItem in their hands. Or if another modder forces this layer to be visible.
            return player.unusual != 0;

            // If you'd like to reference another PlayerDrawLayer's visibility,
            // you can do so by getting its instance via ModContent.GetInstance<OtherDrawLayer>(), and calling GetDefaultVisiblity on it
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Mod mod = Highlander.Instance;
            Player drawPlayer = drawInfo.drawPlayer;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            string texPath = "";
            string texPath2 = "";
            string texPath3 = "";
            bool storm = false;
            bool energy = false;
            bool flames = false;
            bool ooze = false;

            switch (modPlayer.unusual)
            {
                case AbnormalEffect.PurpleEnergy:
                    texPath = "UnusualLayerEffects/PurpleEnergy";
                    energy = true;
                    break;
                case AbnormalEffect.GreenEnergy:
                    texPath = "UnusualLayerEffects/GreenEnergy";
                    energy = true;
                    break;
                case AbnormalEffect.BlizzardyStorm:
                    texPath = "UnusualLayerEffects/BlizzardyStorm";
                    texPath2 = "UnusualLayerEffects/BlizzardyStormParticle";
                    storm = true;
                    break;
                case AbnormalEffect.StormyStorm:
                    texPath = "UnusualLayerEffects/StormyStorm";
                    texPath2 = "UnusualLayerEffects/StormyStormParticle";
                    storm = true;
                    break;
                case AbnormalEffect.BurningFlames:
                    texPath = "UnusualLayerEffects/BurningFlames";
                    flames = true;
                    break;
                case AbnormalEffect.ScorchingFlames:
                    texPath = "UnusualLayerEffects/ScorchingFlames";
                    flames = true;
                    break;
                case AbnormalEffect.TheOoze:
                    texPath = "UnusualLayerEffects/TheOoze";
                    texPath2 = "UnusualLayerEffects/TheOoze2";
                    texPath3 = "UnusualLayerEffects/TheOoze3";
                    ooze = true;
                    break;
                case AbnormalEffect.StareFromBeyond:
                    texPath = "UnusualLayerEffects/StareFromBeyond";
                    texPath2 = "UnusualLayerEffects/StareFromBeyond2";
                    texPath3 = "UnusualLayerEffects/StareFromBeyond3";
                    ooze = true;
                    break;
                case AbnormalEffect.Amaranthine:
                    texPath = "UnusualLayerEffects/Amaranthine";
                    texPath2 = "UnusualLayerEffects/Amaranthine2";
                    texPath3 = "UnusualLayerEffects/Amaranthine3";
                    ooze = true;
                    break;
                default:
                    return;
            }
            texPath = "Highlander/" + texPath;
            texPath2 = "Highlander/" + texPath2;
            texPath3 = "Highlander/" + texPath3;
            int yOffset = -65;
            float angle = 0;

            if (storm)
            {

                float randX = Main.rand.NextFloat(-drawPlayer.width / 2, drawPlayer.width / 2);
                float randY = -Main.rand.NextFloat(8, 10);

                FauxDust newD;

                if (modPlayer.unusual == AbnormalEffect.BlizzardyStorm && modPlayer.counter % 4 == 0)
                {
                    newD = new BlizzardyStormParticleFaux(drawInfo, new Vector2(randX, randY), texPath2, 1f);
                    newD.front = true;
                    dust.Add(newD);
                }
                else if (modPlayer.unusual == AbnormalEffect.StormyStorm && modPlayer.counter % 4 == 0)
                {
                    newD = new StormyStormParticleFaux(drawInfo, new Vector2(randX, randY), texPath2, 1f);
                    newD.front = true;
                    dust.Add(newD);
                }

                drawInfo.DrawDataCache.Add(StormDrawData(drawInfo, texPath, yOffset, angle));
                drawInfo.DrawDataCache.Add(StormDrawData2(drawInfo, texPath, yOffset, angle));

            }
            if (energy)
            {
                for (int i = 0; i < dust.Count; i++)
                {

                    FauxDust d = dust[i];
                    if (d.Player == drawPlayer)
                    {
                        FauxDust newD = new PurpleEnergyFaux(d.drawInfo, d.Offset, d.texture, d.scale);
                        newD.Color.R = 6;
                        newD.Color.B = 6;
                        newD.Color.G = 6;
                        drawInfo.DrawDataCache.Add(newD.DrawData(drawInfo));
                        //Main.playerDrawData.Add(FauxDustDrawData(drawInfo, newD, texPath, yOffset, angle));
                    }
                }
            }
            if (flames)
            {
                float randX = Main.rand.NextFloat(-drawPlayer.width / 2, drawPlayer.width / 2);
                float randY = -Main.rand.NextFloat(-9, 4);

                FauxDust newD;

                if (modPlayer.unusual == AbnormalEffect.BurningFlames)
                {
                    newD = new BurningFlamesFaux(drawInfo, new Vector2(randX, randY), texPath, 1f);
                    newD.front = true;
                    newD.alpha = 200;
                    dust.Add(newD);
                }
                else
                {
                    newD = new ScorchingFlamesFaux(drawInfo, new Vector2(randX, randY), texPath, 1f);
                    newD.front = true;
                    newD.alpha = 200;
                    dust.Add(newD);
                }
            }
            if (ooze)
            {
                drawInfo.DrawDataCache.Add(OozeDrawData(drawInfo, texPath, yOffset, angle));
                drawInfo.DrawDataCache.Add(OozeDrawData2(drawInfo, texPath2, yOffset, angle));
                drawInfo.DrawDataCache.Add(OozeDrawData3(drawInfo, texPath3, yOffset, angle));
            }

            for (int i = 0; i < dust.Count; i++)
            {
                FauxDust d = dust[i];
                if (d.Player == drawPlayer && d.front)
                {
                    d.SafeUpdate();
                    if (!d.active)
                    {
                        dust.Remove(d);
                        i--;
                    }
                    else
                    {
                        drawInfo.DrawDataCache.Add(d.DrawData(drawInfo));
                    }
                }
            }
        }

        public static DrawData OozeDrawData(PlayerDrawSet drawInfo, string unusualSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 1f;
            Texture2D texture = Request<Texture2D>(unusualSprite).Value;
            int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.Position.Y + yOffset + 75 + drawPlayer.height / 0.6f - Main.screenPosition.Y);

            if (drawPlayer.mount.Active)
            {
                MountData data = drawPlayer.mount._data;

                Vector2 pos = new Vector2();
                pos.Y += data.heightBoost;

                pos += drawInfo.Position;
                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset + 70 + 75 - Main.screenPosition.Y);
            }

            int numFrames = 8;

            int cX = (int)(drawPlayer.position.X / 16f);
            int cY = (int)((drawPlayer.position.Y) / 16f);
            Color color = Lighting.GetColor(cX, cY, Color.White);
            Color other = Color.White;
            color = new Color((color.R * 2 + other.R) / 3, (color.G * 2 + other.G) / 3, (color.B * 2 + other.B) / 3, (color.A * 2 + other.A) / 3) * 0.9f;// Color.White * 0.8f;

            if (modPlayer.unusualLayerTime > 10)
            {
                int newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                while (newFrame == modPlayer.unusualFrame)
                {
                    newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                }
                modPlayer.unusualFrame = newFrame;
                modPlayer.unusualLayerTime = 0;
            }

            modPlayer.unusualLayerTime++;
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, modPlayer.unusualFrame, texture.Width, texture.Height / numFrames), color, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0);
        }

        public static DrawData OozeDrawData2(PlayerDrawSet drawInfo, string unusualSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 1f;
            Texture2D texture = Request<Texture2D>(unusualSprite).Value;
            int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.Position.Y + yOffset + 75 + drawPlayer.height / 0.6f - Main.screenPosition.Y);

            if (drawPlayer.mount.Active)
            {
                MountData data = drawPlayer.mount._data;

                Vector2 pos = new Vector2();
                pos.Y += data.heightBoost;

                pos += drawInfo.Position;
                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset + 70 + 75 - Main.screenPosition.Y);
            }

            int numFrames = 8;

            int cX = (int)(drawPlayer.position.X / 16f);
            int cY = (int)((drawPlayer.position.Y) / 16f);
            Color color = Lighting.GetColor(cX, cY, Color.White);
            Color other = Color.White;
            color = new Color((color.R * 2 + other.R) / 3, (color.G * 2 + other.G) / 3, (color.B * 2 + other.B) / 3, (color.A * 2 + other.A) / 3) * 0.9f;// Color.White * 0.8f;

            if (modPlayer.unusualLayerTime == 5)
            {
                int newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                while (newFrame == modPlayer.unusualFrame)
                {
                    newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                }
                modPlayer.unusualFrame2 = newFrame;
            }

            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, modPlayer.unusualFrame2, texture.Width, texture.Height / numFrames), color, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0);
        }

        public static DrawData OozeDrawData3(PlayerDrawSet drawInfo, string unusualSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 1f;
            Texture2D texture = Request<Texture2D>(unusualSprite).Value;
            int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.Position.Y + yOffset + 75 + drawPlayer.height / 0.6f - Main.screenPosition.Y);

            if (drawPlayer.mount.Active)
            {
                MountData data = drawPlayer.mount._data;

                Vector2 pos = new Vector2();
                pos.Y += data.heightBoost;

                pos += drawInfo.Position;
                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset + 70 + 75 - Main.screenPosition.Y);
            }

            int numFrames = 8;

            int cX = (int)(drawPlayer.position.X / 16f);
            int cY = (int)((drawPlayer.position.Y) / 16f);
            Color color = Color.White;

            if (modPlayer.unusualLayerTime2 > 79)
            {
                int newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                while (newFrame == modPlayer.unusualFrame)
                {
                    newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                }
                modPlayer.unusualFrame3 = newFrame;
                modPlayer.unusualLayerTime2 = 0;
            }
            modPlayer.unusualLayerTime2++;

            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, modPlayer.unusualFrame3, texture.Width, texture.Height / numFrames), color, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0);
        }

    } // End of class
}
