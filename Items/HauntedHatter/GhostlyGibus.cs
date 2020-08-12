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

namespace Highlander.Items.HauntedHatter
{
    [AutoloadEquip(EquipType.Head)]
    class GhostlyGibus : ModItem
    {
        public override string Texture => "Highlander/Items/HauntedHatter/GhostlyGibus";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Haunted Hat");
            //Tooltip.SetDefault("Only those deemed worthy don the Ghostly Gibus.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = ItemRarityID.Blue;
            item.vanity = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }

    }
}
