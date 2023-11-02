using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class SamurEye : AbnormalItem
    {
        public SamurEye() : base()
        {
        }
        public SamurEye(AbnormalEffect effect) : base(effect)
        {
        }
        public override bool? SafeIsLoadingEnabled(Mod mod) => true;
        public override string Texture => "Highlander/Items/Armor/VanityHats/Series1/SamurEye";

        public override void SetStaticDefaults()
        {
            if(CurrentEffect != 0)
            {
                string name = "" + CurrentEffect;
                //name = Regex.Replace(name, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1");
                name = "Unusual";
                name = name + " Samur-Eye";
                //DisplayName.SetDefault(name);
            }
            else
            {
                //DisplayName.SetDefault("Samur-Eye");
            }

            //Tooltip.SetDefault("Favorite of a one-eyed Eastern swordsman.");
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
