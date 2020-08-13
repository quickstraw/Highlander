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
    class BackwardsBallcap : AbnormalItem
    {

        public override bool Autoload(ref string name)
        {
            return false;
        }

        public BackwardsBallcap() : base()
        {
        }
        public BackwardsBallcap(AbnormalEffect effect) : base(effect)
        {
        }
        public override string Texture => "Highlander/Items/Armor/Series2/BackwardsBallcap";

        public override void SetStaticDefaults()
        {
            //Tooltip.SetDefault("Eastern designed helmet that looks eerily like a lid.");
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
            drawAltHair = false;
        }

    }
}
