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
using Terraria.ID;

namespace Highlander.Items.Armor.VanityHats.Series2
{
    [AutoloadEquip(EquipType.Head)]
    class BackwardsBallcap : AbnormalItem
    {

        public BackwardsBallcap() : base()
        {
        }
        public BackwardsBallcap(AbnormalEffect effect) : base(effect)
        {
        }
        public override bool? SafeIsLoadingEnabled(Mod mod) => false;
        public override string Texture => "Highlander/Items/Armor/VanityHats/Series2/BackwardsBallcap";

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
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
