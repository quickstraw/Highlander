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
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(silver: 20);
        }

	}
}
