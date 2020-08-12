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
using Terraria.ID;

namespace Highlander.Items.Armor.Series2
{
    [AutoloadEquip(EquipType.Head)]
    class HeroHachimaki : AbnormalItem
    {
        public HeroHachimaki() : base()
        {
        }
        public HeroHachimaki(AbnormalEffect effect) : base(effect)
        {
        }
        public override string Texture => "Highlander/Items/Armor/Series2/HeroHachimaki";

        public override void SetStaticDefaults()
        {
            if (CurrentEffect != 0)
            {
                string name = "" + CurrentEffect;
                //name = Regex.Replace(name, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1");
                name = "Unusual";
                name = name + " Hero's Hachimaki";
                DisplayName.SetDefault(name);
            }
            else
            {
                DisplayName.SetDefault("Hero's Hachimaki");
            }
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = ItemRarityID.Orange;
            item.vanity = true;
            base.SetDefaults();
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }

    }
}
