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
        public override string Texture => "Highlander/Items/Armor/SamurEye";

        public override void SetStaticDefaults()
        {
            if(CurrentEffect != 0)
            {
                string name = "" + CurrentEffect;
                name = Regex.Replace(name, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1");
                name = name + " Samur-Eye";
                DisplayName.SetDefault(name);
            }
            else
            {
                DisplayName.SetDefault("Samur-Eye");
            }
            Tooltip.SetDefault("Favorite of a one-eyed Eastern swordsman.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 3;
            item.vanity = true;
            base.SetDefaults();
        }
    }
}
