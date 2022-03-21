using Microsoft.Xna.Framework;
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

namespace Highlander.Items.Armor.VanityHats
{
    [AutoloadEquip(EquipType.Head)]
    class PaperBag : VanityItem
    {
        public override bool? SafeIsLoadingEnabled(Mod mod) => true;
        public override string Texture => "Highlander/Items/Armor/VanityHats/PaperBag";

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ItemRarityID.Orange;
            Item.vanity = true;
            base.SetDefaults();
        }
    }
}
