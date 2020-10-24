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

namespace Highlander.Items.Armor.Halloween
{
    [AutoloadEquip(EquipType.Head)]
    class OneWayTicket : AbnormalItem
    {
        public OneWayTicket() : base()
        {
        }
        public OneWayTicket(AbnormalEffect effect) : base(effect)
        {
        }
        public override string Texture => "Highlander/Items/Armor/Halloween/OneWayTicket";

        public override void SetStaticDefaults()
        {
            if (CurrentEffect != 0)
            {
                string name = "" + CurrentEffect;
                //name = Regex.Replace(name, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1");
                name = "Unusual";
                name = name + " One-Way Ticket";
                DisplayName.SetDefault(name);
            }
            else
            {
                DisplayName.SetDefault("One-Way Ticket");
            }
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
