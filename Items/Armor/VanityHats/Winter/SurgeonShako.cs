using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class SurgeonShako : AbnormalItem
    {
        public SurgeonShako() : base()
        {
        }
        public SurgeonShako(AbnormalEffect effect) : base(effect)
        {
        }
        public override bool? SafeIsLoadingEnabled(Mod mod) => true;
        public override string Texture => "Highlander/Items/Armor/VanityHats/Winter/SurgeonShako";

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