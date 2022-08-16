using Highlander.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class ToySoldier : AbnormalItem
    {
        public ToySoldier() : base()
        {
        }
        public ToySoldier(AbnormalEffect effect) : base(effect)
        {
        }
        public override bool? SafeIsLoadingEnabled(Mod mod) => true;
        public override string Texture => "Highlander/Items/Armor/VanityHats/Winter/ToySoldier";

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

        public override void UpdateVanity(Player player)
        {
            HighlanderPlayer modPlayer = player.GetModPlayer<HighlanderPlayer>();
            modPlayer.tallHat = Utilities.TallHat.ToySoldier;
        }

    }
}