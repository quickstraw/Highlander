using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;


namespace Highlander.Items.HauntedHatter
{
    [AutoloadEquip(EquipType.HandsOff)]
    class HauntedHatterMask : ModItem
    {

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Mask worn by a familiar hatter.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 3;
            item.vanity = true;
            item.accessory = true;
        }

    }
}
