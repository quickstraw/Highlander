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
using Highlander.Common.Players;

namespace Highlander.Utilities
{
    public class AmmoLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new Between(PlayerDrawLayers.EyebrellaCloud, PlayerDrawLayers.BeetleBuff);
        }

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            HighlanderPlayer player = drawInfo.drawPlayer.GetModPlayer<HighlanderPlayer>();
            // The layer will be visible only if the player is holding an ExampleItem in their hands. Or if another modder forces this layer to be visible.
            return player.holdingAmmoGun;

            // If you'd like to reference another PlayerDrawLayer's visibility,
            // you can do so by getting its instance via ModContent.GetInstance<OtherDrawLayer>(), and calling GetDefaultVisiblity on it
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.whoAmI != Main.myPlayer)
                return;

            Player drawPlayer = drawInfo.drawPlayer;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            Mod mod = Highlander.Instance;
            bool altTextures = ModContent.GetInstance<HighlanderClientConfig>().AlternateGunUI;

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
                        texPath = "Highlander/UI/GunAmmo5/GunAmmo05";
                        break;
                    case 1:
                        texPath = "Highlander/UI/GunAmmo5/GunAmmo15";
                        break;
                    case 2:
                        texPath = "Highlander/UI/GunAmmo5/GunAmmo25";
                        break;
                    case 3:
                        texPath = "Highlander/UI/GunAmmo5/GunAmmo35";
                        break;
                    case 4:
                        texPath = "Highlander/UI/GunAmmo5/GunAmmo45";
                        break;
                    case 5:
                        texPath = "Highlander/UI/GunAmmo5/GunAmmo55";
                        break;
                    default:
                        texPath = "Highlander/UI/GunAmmo5/GunAmmo05";
                        break;
                }
            }
            else if (modPlayer.maxAmmo == 6)
            {
                switch (modPlayer.currentAmmo)
                {
                    case 0:
                        if (altTextures)
                        {
                            texPath = "Highlander/UI/GunAmmo6/GunAmmo06";
                        }
                        else
                        {
                            texPath = "Highlander/UI/GunAmmo6/Compact06";
                        }
                        break;
                    case 1:
                        if (altTextures)
                        {
                            texPath = "Highlander/UI/GunAmmo6/GunAmmo16";
                        }
                        else
                        {
                            texPath = "Highlander/UI/GunAmmo6/Compact16";
                        }
                        break;
                    case 2:
                        if (altTextures)
                        {
                            texPath = "Highlander/UI/GunAmmo6/GunAmmo26";
                        }
                        else
                        {
                            texPath = "Highlander/UI/GunAmmo6/Compact26";
                        }
                        break;
                    case 3:
                        if (altTextures)
                        {
                            texPath = "Highlander/UI/GunAmmo6/GunAmmo36";
                        }
                        else
                        {
                            texPath = "Highlander/UI/GunAmmo6/Compact36";
                        }
                        break;
                    case 4:
                        if (altTextures)
                        {
                            texPath = "Highlander/UI/GunAmmo6/GunAmmo46";
                        }
                        else
                        {
                            texPath = "Highlander/UI/GunAmmo6/Compact46";
                        }
                        break;
                    case 5:
                        if (altTextures)
                        {
                            texPath = "Highlander/UI/GunAmmo6/GunAmmo56";
                        }
                        else
                        {
                            texPath = "Highlander/UI/GunAmmo6/Compact56";
                        }
                        break;
                    case 6:
                        if (altTextures)
                        {
                            texPath = "Highlander/UI/GunAmmo6/GunAmmo66";
                        }
                        else
                        {
                            texPath = "Highlander/UI/GunAmmo6/Compact66";
                        }
                        break;
                    default:
                        if (altTextures)
                        {
                            texPath = "Highlander/UI/GunAmmo6/GunAmmo06";
                        }
                        else
                        {
                            texPath = "Highlander/UI/GunAmmo6/Compact06";
                        }
                        break;
                }
                yOffset -= 6;
            }
            else
            {
                return;
            }

            drawInfo.DrawDataCache.Add(AmmoGunDrawData(drawInfo, texPath, yOffset, angle));
        }

        public static DrawData AmmoGunDrawData(PlayerDrawSet drawInfo, string ammoCounterSprite, int yOffset, float angleInRadians)
        {
            bool altTextures = ModContent.GetInstance<HighlanderClientConfig>().AlternateGunUI;
            int ammoPosY = 80;
            if (altTextures)
            {
                ammoPosY = 60;
            }
            
            float opacity = 0.9f;

            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = Highlander.Instance;
            HighlanderPlayer modPlayer = drawPlayer.GetModPlayer<HighlanderPlayer>();
            float scale = 1f;
            Texture2D texture = Request<Texture2D>(ammoCounterSprite).Value;
            int drawX = (int)(drawInfo.Position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.Position.Y + yOffset + ammoPosY - Main.screenPosition.Y);

            if (drawPlayer.mount.Active)
            {
                MountData data = drawPlayer.mount._data;

                Vector2 pos = new Vector2();
                pos.Y += data.heightBoost;

                pos += drawInfo.Position;

                int smoothOffset = (int)((drawInfo.Position - Main.screenPosition) - (drawPlayer.position - Main.screenPosition)).Y + data.heightBoost;

                drawX = (int)(pos.X + drawPlayer.width / 2f - Main.screenPosition.X);
                drawY = (int)(pos.Y + yOffset - smoothOffset + ammoPosY - Main.screenPosition.Y);
            }
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, 0, texture.Width, texture.Height), Color.White * opacity, angleInRadians, new Vector2(texture.Width / 2f, texture.Height / 2f), scale, SpriteEffects.None, 0);
        }
    }
}
