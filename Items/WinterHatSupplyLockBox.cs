using Highlander.Items.Armor;
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
    class WinterHatSupplyLockBox : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Winter Hat Supply Lock Box");
            Tooltip.SetDefault("Right Click to open\nRequires a Winter Hat Supply Key");
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
                if (player.inventory[i].type == ModContent.ItemType<WinterHatSupplyKey>() && player.inventory[i].stack >= 1)
                {
                    hasKeys = true;
                    break;
                }
            }
            return hasKeys;
        }

        public override void RightClick(Player player)
        {
            if (player.HasItem(ModContent.ItemType<WinterHatSupplyKey>()))
            {
                for (int i = 0; i < 58; i++)
                {
                    if (player.inventory[i].type == ModContent.ItemType<WinterHatSupplyKey>() && player.inventory[i].stack >= 1)
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
            names.Add("BrassBucket");
            names.Add("TartanTyrolean");
            names.Add("ColdfrontCommander");
            names.Add("SinnerShade");
            names.Add("MightyMitre");
            names.Add("CondorCap");
            names.Add("SurgeonShako");
            names.Add("ToySoldier");
            names.Add("PatriotPeak");

            if (isAbnormal)
            {
                prefix = "Unusual";
            }

            int chance;
            chance = Main.rand.Next(0, names.Count);
            itemName = names[chance];

            player.QuickSpawnItem(mod.ItemType(prefix + itemName));
        }

    }
}
