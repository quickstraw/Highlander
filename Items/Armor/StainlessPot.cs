using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class StainlessPot : AbnormalItem
    {
        
        public StainlessPot() : base()
        {
        }
        public StainlessPot(AbnormalEffect effect) : base(effect)
        {
        }
        public override string Texture => "Highlander/Items/Armor/StainlessPot";

        public override void SetStaticDefaults()
        {
            //Tooltip.SetDefault("Your head will smell delicious.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 3;
            item.vanity = true;
            base.SetDefaults();
        }
    }
}
