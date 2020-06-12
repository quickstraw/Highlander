﻿using Highlander.Items.Armor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
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

            if (isAbnormal)
            {
                int effectChance = Main.rand.Next(0, (int)AbnormalEffect.Max);
                AbnormalEffect effect = (AbnormalEffect)effectChance;
                prefix = "" + effect;
            }

            int chance;
            chance = Main.rand.Next(0, names.Count);
            itemName = names[chance];

            player.QuickSpawnItem(mod.ItemType(prefix + itemName));
        }

    }
}
