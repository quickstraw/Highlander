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
using static Terraria.Mount;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Utilities
{
    public class UnusualLayer : PlayerDrawLayer
    {
        public static List<FauxDust> dust;

        public override Position GetDefaultPosition() => new Between(PlayerDrawLayers.Head, PlayerDrawLayers.FinchNest);

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
            bool storm = false;
            bool energy = false;
            bool flames = false;
            bool cloud9 = false;
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
                    storm = true;
                    break;
                case AbnormalEffect.StormyStorm:
                    texPath = "UnusualLayerEffects/StormyStorm";
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
                case AbnormalEffect.Cloud9:
                    texPath = "UnusualLayerEffects/Cloud9";
                    texPath2 = "UnusualLayerEffects/Cloud9Trail";
                    cloud9 = true;
                    break;
                case AbnormalEffect.TheOoze:
                    texPath = "UnusualLayerEffects/TheOoze";
                    texPath2 = "UnusualLayerEffects/TheOoze2";
                    ooze = true;
                    break;
                default:
                    return;
            }
            texPath = "Highlander/" + texPath;
            texPath2 = "Highlander/" + texPath2;
            int yOffset = -65;
            float angle = 0;

            if (storm)
            {
                drawInfo.DrawDataCache.Add(StormDrawData(drawInfo, texPath, yOffset, angle));
                drawInfo.DrawDataCache.Add(StormDrawData2(drawInfo, texPath, yOffset, angle));
            }
            if (energy)
            {
                Texture2D tex = Request<Texture2D>(texPath).Value;

                if (modPlayer.unusual == AbnormalEffect.PurpleEnergy)
                {
                    dust.Add(new PurpleEnergyFaux(drawInfo, new Vector2(0.5f, 16), tex, 2f));
                }
                else
                {
                    dust.Add(new GreenEnergyFaux(drawInfo, new Vector2(0.5f, 16), tex, 2f));
                }
            }
            else
            if (flames)
            {
                Texture2D tex = Request<Texture2D>(texPath).Value;

                float randX = Main.rand.NextFloat(-drawPlayer.width / 2, drawPlayer.width / 2);
                float randY = -Main.rand.NextFloat(-9, 4);

                if (modPlayer.unusual == AbnormalEffect.BurningFlames)
                {
                    dust.Add(new BurningFlamesFaux(drawInfo, new Vector2(randX, randY), tex, 1f));
                }
                else
                {
                    dust.Add(new ScorchingFlamesFaux(drawInfo, new Vector2(randX, randY), tex, 1f));
                }
            }
            else
            if (cloud9)
            {
                if (modPlayer.counter % 30 == 0)
                {
                    float randX = Main.rand.NextFloat(-drawPlayer.width / 2, drawPlayer.width / 2);
                    float randY = -Main.rand.NextFloat(0, 8);

                    var currDust = new Cloud9Faux(drawInfo, new Vector2(randX, randY), texPath, 1f);

                    dust.Add(currDust);

                    FauxDust trail;

                    trail = new Cloud9TrailFaux(drawInfo, currDust.Offset - currDust.velocity, texPath2, 1f);
                    trail.velocity = currDust.velocity * 0.5f;
                    trail.scale = 0.8f;
                    dust.Add(trail);
                    trail = new Cloud9TrailFaux(drawInfo, currDust.Offset - currDust.velocity, texPath2, 1f);
                    trail.velocity = currDust.velocity * 0.25f;
                    trail.scale = 0.4f;
                    dust.Add(trail);
                    trail = new Cloud9TrailFaux(drawInfo, currDust.Offset - currDust.velocity, texPath2, 1f);
                    trail.velocity = currDust.velocity * 0.125f;
                    trail.scale = 0.2f;
                    dust.Add(trail);
                }
            }

            for (int i = 0; i < dust.Count; i++)
            {
                FauxDust d = dust[i];
                if (d.Player == drawPlayer && !d.front)
                {
                    d.Update();
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
        public static DrawData StormDrawData(PlayerDrawSet drawInfo, string unusualSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 2f;
            Texture2D texture = Request<Texture2D>(unusualSprite).Value;
            int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.Position.Y + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);

            if (drawPlayer.mount.Active)
            {
                MountData data = drawPlayer.mount._data;

                Vector2 pos = new Vector2();
                pos.Y += data.heightBoost;

                pos += drawInfo.Position;
                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset + 70 - Main.screenPosition.Y);
            }

            int numFrames = 3;

            int cX = (int)(drawPlayer.position.X / 16f);
            int cY = (int)((drawPlayer.position.Y) / 16f);
            Color color = Lighting.GetColor(cX, cY, Color.White);

            if (modPlayer.unusualLayerTime > 20)
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

        public static DrawData StormDrawData2(PlayerDrawSet drawInfo, string unusualSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 2f;
            Texture2D texture = Request<Texture2D>(unusualSprite).Value;
            int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.Position.Y + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);

            if (drawPlayer.mount.Active)
            {
                MountData data = drawPlayer.mount._data;

                Vector2 pos = new Vector2();
                pos.Y += data.heightBoost;

                pos += drawInfo.Position;
                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset + 70 - Main.screenPosition.Y);
            }

            int numFrames = 3;

            int cX = (int)(drawPlayer.position.X / 16f);
            int cY = (int)((drawPlayer.position.Y) / 16f);
            Color color = Lighting.GetColor(cX, cY, Color.White);

            if (modPlayer.unusualLayerTime2 > 40)
            {
                int newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                while (newFrame == modPlayer.unusualFrame2)
                {
                    newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                }
                modPlayer.unusualFrame2 = newFrame;
                modPlayer.unusualLayerTime2 = 0;
            }

            modPlayer.unusualLayerTime2++;
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, modPlayer.unusualFrame2, texture.Width, texture.Height / numFrames), color, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0);
        }

        public static DrawData FauxDustDrawData(PlayerDrawSet drawInfo, FauxDust d, string unusualSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            Texture2D texture = Request<Texture2D>(unusualSprite).Value;
            int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.Position.Y + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);

            if (drawPlayer.mount.Active)
            {
                MountData data = drawPlayer.mount._data;

                Vector2 pos = new Vector2();
                pos.Y += data.heightBoost;

                pos += drawInfo.Position;
                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset + 70 - Main.screenPosition.Y);
            }

            int numFrames = 3;

            int cX = (int)(drawPlayer.position.X / 16f);
            int cY = (int)((drawPlayer.position.Y) / 16f);
            Color color = Lighting.GetColor(cX, cY, Color.White);

            if (modPlayer.unusualLayerTime2 > 40)
            {
                int newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                while (newFrame == modPlayer.unusualFrame2)
                {
                    newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                }
                modPlayer.unusualFrame2 = newFrame;
                modPlayer.unusualLayerTime2 = 0;
            }

            modPlayer.unusualLayerTime2++;
            return new DrawData(d.texture, new Vector2(drawX + d.Offset.X, drawY + d.Offset.Y), d.frame, d.Color, angleInRadians, d.origin, d.scale, SpriteEffects.None, 0);
        }


    } // End of Class
}
