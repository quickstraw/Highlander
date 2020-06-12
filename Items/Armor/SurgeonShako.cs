using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class SurgeonShako : AbnormalBase
    {
        public SurgeonShako()
        {
            CurrentEffect = 0;
        }
        public SurgeonShako(AbnormalEffect effect) : base(effect)
        {
        }
        public override string Texture => "Highlander/Items/Armor/SurgeonShako";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Formal hat for a fancy surgeon.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 3;
            item.vanity = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = false;
        }

    }
}