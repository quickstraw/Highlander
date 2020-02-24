using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class OlSnaggletooth : AbnormalBase
    {
        public OlSnaggletooth()
        {
            CurrentEffect = 0;
        }
        public OlSnaggletooth(AbnormalEffect effect) : base(effect)
        {
        }
        public override string Texture => "Highlander/Items/Armor/OlSnaggletooth";

        public override void SetStaticDefaults()
        {
            if (CurrentEffect != 0)
            {
                string name = "" + CurrentEffect;
                name = Regex.Replace(name, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1");
                name = name + " Ol' Snaggletooth";
                DisplayName.SetDefault(name);
            }
            else
            {
                DisplayName.SetDefault("Ol' Snaggletooth");
            }
            Tooltip.SetDefault("Made from a genuine crocodile.");
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
