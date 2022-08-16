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
            Item.width = 18;
            Item.height = 18;
            Item.rare = 3;
            Item.vanity = true;
            Item.accessory = true;
        }

    }
}
