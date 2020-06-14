using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class Hotrod : AbnormalItem
    {
        public Hotrod() : base()
        {
        }
        public Hotrod(AbnormalEffect effect) : base(effect)
        {
        }
        public override string Texture => "Highlander/Items/Armor/Hotrod";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used to weld fine pieces of work.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 3;
            item.vanity = true;
            base.SetDefaults();
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }

    }
}
