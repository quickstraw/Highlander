﻿using System;
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

namespace Highlander.Utilities
{
    class AnimationHelper
    {

        public static List<FauxDust> dust;

        public static readonly PlayerLayer ammoGunCounter = new PlayerLayer("Highlander", "AmmoGunCounter", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {

            if (drawInfo.drawPlayer.whoAmI != Main.myPlayer)
                return;

            Player drawPlayer = drawInfo.drawPlayer;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            Mod mod = Highlander.Instance;

            if (drawInfo.shadow != 0f)
            {
                return;
            }
            float angle = 0;//-1.57;

            int yOffset = -110;

            if (modPlayer.maxAmmo <= 0)
            {
                return;
            }

            string texPath;

            if (modPlayer.maxAmmo == 5)
            {
                switch (modPlayer.currentAmmo)
                {
                    case 0:
                        texPath = "UI/GunAmmo5/GunAmmo05";
                        break;
                    case 1:
                        texPath = "UI/GunAmmo5/GunAmmo15";
                        break;
                    case 2:
                        texPath = "UI/GunAmmo5/GunAmmo25";
                        break;
                    case 3:
                        texPath = "UI/GunAmmo5/GunAmmo35";
                        break;
                    case 4:
                        texPath = "UI/GunAmmo5/GunAmmo45";
                        break;
                    case 5:
                        texPath = "UI/GunAmmo5/GunAmmo55";
                        break;
                    default:
                        texPath = "UI/GunAmmo5/GunAmmo05";
                        break;
                }
            }
            else if (modPlayer.maxAmmo == 6)
            {
                switch (modPlayer.currentAmmo)
                {
                    case 0:
                        texPath = "UI/GunAmmo6/GunAmmo06";
                        break;
                    case 1:
                        texPath = "UI/GunAmmo6/GunAmmo16";
                        break;
                    case 2:
                        texPath = "UI/GunAmmo6/GunAmmo26";
                        break;
                    case 3:
                        texPath = "UI/GunAmmo6/GunAmmo36";
                        break;
                    case 4:
                        texPath = "UI/GunAmmo6/GunAmmo46";
                        break;
                    case 5:
                        texPath = "UI/GunAmmo6/GunAmmo56";
                        break;
                    case 6:
                        texPath = "UI/GunAmmo6/GunAmmo66";
                        break;
                    default:
                        texPath = "UI/GunAmmo6/GunAmmo06";
                        break;
                }
                yOffset -= 6;
            }
            else
            {
                return;
            }

            Main.playerDrawData.Add(AmmoGunDrawData(drawInfo, texPath, yOffset, angle));
        });

        public static DrawData AmmoGunDrawData(PlayerDrawInfo drawInfo, string ammoCounterSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 1f;
            Texture2D texture = mod.GetTexture(ammoCounterSprite);
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0);
        }

        public static readonly PlayerLayer mask = new PlayerLayer("Highlander", "Mask", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
        {



        });

        public static readonly PlayerLayer bigHat = new PlayerLayer("Highlander", "AmmoGunCounter", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            string texPath = "Items/Armor/ToySoldier_Head";
            int yOffset = 0;
            float angle = 0;
            Main.playerDrawData.Add(BigHatDrawData(drawInfo, texPath, yOffset, angle));
        });

        public static DrawData BigHatDrawData(PlayerDrawInfo drawInfo, string bigHatSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 1f;
            Texture2D texture = mod.GetTexture(bigHatSprite);
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0);
        }

        public static readonly PlayerLayer unusual = new PlayerLayer("Highlander", "Unusual", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
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
                default:
                    return;
            }
            int yOffset = -65;
            float angle = 0;

            if (storm)
            {
                Main.playerDrawData.Add(StormDrawData(drawInfo, texPath, yOffset, angle));
                Main.playerDrawData.Add(StormDrawData2(drawInfo, texPath, yOffset, angle));
            }
            if (energy)
            {
                Texture2D tex = mod.GetTexture(texPath);

                if (modPlayer.unusual == AbnormalEffect.PurpleEnergy)
                {
                    dust.Add(new PurpleEnergyFaux(drawInfo, new Vector2(0.5f, 16), tex, 2f));
                }
                else
                {
                    dust.Add(new GreenEnergyFaux(drawInfo, new Vector2(0.5f, 16), tex, 2f));
                }
            } else
            if (flames)
            {
                Texture2D tex = mod.GetTexture(texPath);

                float randX = Main.rand.NextFloat(-drawPlayer.width / 2, drawPlayer.width / 2);
                float randY = -Main.rand.NextFloat(-7, 6);

                if (modPlayer.unusual == AbnormalEffect.BurningFlames)
                {
                    dust.Add(new BurningFlamesFaux(drawInfo, new Vector2(randX, randY), tex, 1f));
                }
                else
                {
                    dust.Add(new ScorchingFlamesFaux(drawInfo, new Vector2(randX, randY), tex, 1f));
                }
            } else
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
                        Main.playerDrawData.Add(d.DrawData(drawInfo));
                    }
                }
            }
        });

        public static DrawData StormDrawData(PlayerDrawInfo drawInfo, string unusualSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 2f;
            Texture2D texture = mod.GetTexture(unusualSprite);
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);

            if (drawPlayer.mount.Active)
            {
                Vector2 pos = new Vector2();
                pos.Y += drawPlayer.mount.PlayerOffset;

                pos += drawInfo.position;
                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset + 70 - Main.screenPosition.Y);
            }

            int numFrames = 3;

            int cX = (int)(drawPlayer.position.X / 16f);
            int cY = (int)((drawPlayer.position.Y) / 16f);
            Color color = Lighting.GetColor(cX, cY, Color.White);

            if(modPlayer.unusualLayerTime > 20)
            {
                int newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                while(newFrame == modPlayer.unusualFrame)
                {
                    newFrame = Main.rand.Next(0, numFrames) * texture.Height / numFrames;
                }
                modPlayer.unusualFrame = newFrame;
                modPlayer.unusualLayerTime = 0;
            }

            modPlayer.unusualLayerTime++;
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, modPlayer.unusualFrame, texture.Width, texture.Height / numFrames), color, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0);
        }
        
        public static DrawData StormDrawData2(PlayerDrawInfo drawInfo, string unusualSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 2f;
            Texture2D texture = mod.GetTexture(unusualSprite);
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);

            if (drawPlayer.mount.Active)
            {
                Vector2 pos = new Vector2();
                pos.Y += drawPlayer.mount.PlayerOffset;

                pos += drawInfo.position;
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

        public static DrawData FauxDustDrawData(PlayerDrawInfo drawInfo, FauxDust d, string unusualSprite, int yOffset, float angleInRadians)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 2f;
            Texture2D texture = mod.GetTexture(unusualSprite);
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + yOffset + drawPlayer.height / 0.6f - Main.screenPosition.Y);

            if (drawPlayer.mount.Active)
            {
                Vector2 pos = new Vector2();
                pos.Y += drawPlayer.mount.PlayerOffset;

                pos += drawInfo.position;
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

        public static readonly PlayerLayer unusualFront = new PlayerLayer("Highlander", "UnusualFront", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            Mod mod = Highlander.Instance;
            Player drawPlayer = drawInfo.drawPlayer;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            string texPath = "";
            string texPath2 = "";
            bool storm = false;
            bool energy = false;
            bool flames = false;

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
                default:
                    return;
            }
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
                else if(modPlayer.unusual == AbnormalEffect.StormyStorm && modPlayer.counter % 4 == 0)
                {
                    newD = new StormyStormParticleFaux(drawInfo, new Vector2(randX, randY), texPath2, 1f);
                    newD.front = true;
                    dust.Add(newD);
                }

                Main.playerDrawData.Add(StormDrawData(drawInfo, texPath, yOffset, angle));
                Main.playerDrawData.Add(StormDrawData2(drawInfo, texPath, yOffset, angle));

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
                        Main.playerDrawData.Add(newD.DrawData(drawInfo));
                        //Main.playerDrawData.Add(FauxDustDrawData(drawInfo, newD, texPath, yOffset, angle));
                    }
                }
            }
            if (flames)
            {
                float randX = Main.rand.NextFloat(-drawPlayer.width / 2, drawPlayer.width / 2);
                float randY = -Main.rand.NextFloat(-7, 6);

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

            for (int i = 0; i < dust.Count; i++)
            {
                FauxDust d = dust[i];
                if (d.Player == drawPlayer && d.front)
                {
                    d.Update();
                    if (!d.active)
                    {
                        dust.Remove(d);
                        i--;
                    }
                    else
                    {
                        Main.playerDrawData.Add(d.DrawData(drawInfo));
                    }
                }
            }

        });

    }

}
