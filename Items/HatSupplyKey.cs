using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Highlander.Items
{
    class HatSupplyKey : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hat Supply Key");
            Tooltip.SetDefault("Opens one Hat Supply Lock Box");
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 14;
            item.height = 20;
            item.value = 25000;
            item.rare = 0;
        }

    }
}
