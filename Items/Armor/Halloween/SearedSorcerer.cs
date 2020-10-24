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
    class SearedSorcerer : AbnormalItem
    {
        public SearedSorcerer() : base()
        {
        }
        public SearedSorcerer(AbnormalEffect effect) : base(effect)
        {
        }
        public override string Texture => "Highlander/Items/Armor/Halloween/SearedSorcerer";

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
            drawAltHair = false;
        }

        public override bool DrawHead()
        {
            return false;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            HighlanderPlayer modPlayer = player.GetModPlayer<HighlanderPlayer>();
            modPlayer.tallHat = Utilities.TallHat.SearedSorcerer;
        }

    }
}
