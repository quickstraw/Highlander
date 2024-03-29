﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Highlander.Items.LockBoxes
{
    class WinterHatSupplyKey : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Winter Hat Key");
            Tooltip.SetDefault("Opens one Winter Hat Lock Box");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 14;
            Item.height = 20;
            Item.value = 25000;
            Item.rare = 0;
        }

    }
}
