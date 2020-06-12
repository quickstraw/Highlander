using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Highlander.Items.Armor
{
    
    [AutoloadEquip(EquipType.Head)]
    class ProfessionalPithHelmetTest : AbnormalItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Professional Pith Helmet");
            Tooltip.SetDefault("Pith helmet worn by only the most professional of mercenaries.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.width = 18;
            item.height = 18;
            item.rare = 2;
            item.vanity = true;
        }

        public override TagCompound Save()
        {
            return base.Save();
        }

        public override void Load(TagCompound tag)
        {
            base.Load(tag);
        }

    }
}
