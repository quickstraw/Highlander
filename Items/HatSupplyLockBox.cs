using Highlander.Items.Armor;
using Highlander.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items
{
    class HatSupplyLockBox : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hat Supply Lock Box");
            Tooltip.SetDefault("Right Click to open\nRequires a Hat Supply Key");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 22;
            item.maxStack = 99;
            item.rare = 3;
            item.value = Item.buyPrice(0, 1, 0, 0);
        }

        public override bool CanRightClick()
        {
            Player player = Main.player[Main.myPlayer];

            bool hasKeys = false;

            for (int i = 0; i < 58; i++)
            {
                if (player.inventory[i].type == ModContent.ItemType<HatSupplyKey>() && player.inventory[i].stack >= 1)
                {
                    hasKeys = true;
                    break;
                }
            }
            return hasKeys;
        }

        public override void RightClick(Player player)
        {
            if (player.HasItem(ModContent.ItemType<HatSupplyKey>()))
            {
                for (int i = 0; i < 58; i++)
                {
                    if (player.inventory[i].type == ModContent.ItemType<HatSupplyKey>() && player.inventory[i].stack >= 1)
                    {
                        player.inventory[i].stack -= 1;
                        break;
                    }
                }
            }
            else
            {
                return;
            }


            bool isAbnormal = Main.rand.NextBool(50);

            String prefix = "";
            String itemName;

            List<String> names = new List<string>();
            names.Add("Hotrod");
            names.Add("LegendaryLid");
            names.Add("OlSnaggletooth");
            names.Add("PithyProfessional");
            names.Add("PyromancerMask");
            names.Add("SamurEye");
            names.Add("StainlessPot");
            names.Add("StoutShako");
            names.Add("TeamCaptain");
            names.Add("KillerExclusive");
            names.Add("HongKongCone");

            int chance;
            chance = Main.rand.Next(0, names.Count);
            itemName = names[chance];

            if (isAbnormal)
            {
                prefix = "Unusual";
                string itemRead = Regex.Replace(itemName, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1");
                string text = player.name + " unboxed an Unusual " + itemRead + "!";
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText(text, Color.MediumPurple);
                }
                else
                {
                    NetworkText message = NetworkText.FromLiteral(text);
                    NetMessage.BroadcastChatMessage(message, Color.MediumPurple);
                }

                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int type = ModContent.ProjectileType<UnusualFireworkProjectile>();
                    var projectile = Projectile.NewProjectileDirect(new Vector2(player.position.X, player.position.Y - 20), new Vector2(), type, 0, 0.0f);
                }
            }

            player.QuickSpawnItem(mod.ItemType(prefix + itemName));
        }

    }
}
