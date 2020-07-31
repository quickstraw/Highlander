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
    class ReserveHatSupplyLockBox : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reserve Hat Lock Box");
            Tooltip.SetDefault("Right Click to open\nRequires a Hat Key");
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
            names.Add("GuerrillaRebel");
            names.Add("SkiMask");
            names.Add("ImpregnableHelm");
            names.Add("NinjaHeadband");
            names.Add("AutonomousOrb");
            names.Add("Headless");
            names.Add("OpenMind");
            names.Add("PaperBag");

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
                    var modPlayer = player.GetModPlayer<HighlanderPlayer>();
                    modPlayer.unboxed = mod.ItemType(prefix + itemName);
                    //NetworkText message = NetworkText.FromLiteral(text);
                    //NetMessage.BroadcastChatMessage(message, Color.MediumPurple);
                }

                int type = ModContent.ProjectileType<UnusualFireworkProjectile>();
                var projectile = Projectile.NewProjectile(new Vector2(player.position.X, player.position.Y - 20), new Vector2(), type, 0, 0.0f);
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile);
                }
            }

            player.QuickSpawnItem(mod.ItemType(prefix + itemName));
        }

    }
}
