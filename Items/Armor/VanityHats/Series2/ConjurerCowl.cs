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
    class ConjurerCowl : AbnormalItem
    {

        public ConjurerCowl() : base()
        {
        }
        public ConjurerCowl(AbnormalEffect effect) : base(effect)
        {
        }
        public override bool? SafeIsLoadingEnabled(Mod mod) => false;
        public override string Texture => "Highlander/Items/Armor/VanityHats/Series2/ConjurerCowl";

        public override void SetStaticDefaults()
        {
            if (CurrentEffect != 0)
            {
                string name = "" + CurrentEffect;
                //name = Regex.Replace(name, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1");
                name = "Unusual";
                name = name + " Conjurer's Cowl";
                DisplayName.SetDefault(name);
            }
            else
            {
                DisplayName.SetDefault("Conjurer's Cowl");
            }
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false;
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
