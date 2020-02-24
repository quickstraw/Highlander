using Microsoft.Xna.Framework;
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
    class LegendaryLid : AbnormalBase
    {
        public LegendaryLid()
        {
            CurrentEffect = 0;
        }
        public LegendaryLid(AbnormalEffect effect) : base(effect)
        {
        }
        public override string Texture => "Highlander/Items/Armor/LegendaryLid";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Eastern designed helmet that looks eerily like a lid.");
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
            drawAltHair = true;
        }

    }
}
