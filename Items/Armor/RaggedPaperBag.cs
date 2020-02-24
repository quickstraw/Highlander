using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class RaggedPaperBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Worn out paper bag with an eye hole poked out.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 2;
            item.vanity = true;
        }
    }
}
