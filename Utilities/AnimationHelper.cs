using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
