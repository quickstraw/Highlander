﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
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
        public override bool? SafeIsLoadingEnabled(Mod mod) => true;
        public override string Texture => "Highlander/Items/Armor/VanityHats/Halloween/SearedSorcerer";

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = 3;
            Item.vanity = true;
            base.SetDefaults();
        }

        public override void UpdateVanity(Player player)
        {
            HighlanderPlayer modPlayer = player.GetModPlayer<HighlanderPlayer>();
            modPlayer.tallHat = Utilities.TallHat.SearedSorcerer;
        }

    }
}