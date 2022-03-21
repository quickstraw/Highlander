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

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class Anger : AbnormalItem
    {
        public Anger() : base()
        {
        }
        public Anger(AbnormalEffect effect) : base(effect)
        {
        }
        public override bool? SafeIsLoadingEnabled(Mod mod) => true;
        public override string Texture => "Highlander/Items/Armor/VanityHats/Series1/Anger";

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = 3;
            Item.vanity = true;
            base.SetDefaults();
        }

    }
}
