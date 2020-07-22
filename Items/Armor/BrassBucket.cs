﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class BrassBucket : AbnormalItem
    {
        public BrassBucket() : base()
        {
        }

        public BrassBucket(AbnormalEffect effect) : base(effect)
        {
        }

        public override string Texture => "Highlander/Items/Armor/BrassBucket";

        public override void SetStaticDefaults()
        {
            //Tooltip.SetDefault("For when you need to purge the corruption.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 3;
            item.vanity = true;
            base.SetDefaults();
        }

        public override bool DrawHead()
        {
            return false;
        }

    }
}
