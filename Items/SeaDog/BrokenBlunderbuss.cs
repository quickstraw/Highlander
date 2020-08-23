using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.SeaDog
{
    class BrokenBlunderbuss : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Blunderbuss");
            Tooltip.SetDefault("Can be repaired with a musket or The Undertaker");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.rare = ItemRarityID.White;
            item.value = Item.sellPrice(silver: 20);
        }

	}
}
