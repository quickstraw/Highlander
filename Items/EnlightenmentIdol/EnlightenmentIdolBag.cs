using Highlander.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Highlander.Items.HauntedHatter;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace Highlander.Items.EnlightenmentIdol
{
    class EnlightenmentIdolBag : GrabBag
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(Language.GetTextValue("Mods.Highlander.ItemName.EnlightenmentIdol." + GetType().Name));
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.OneFromOptions(1, ItemType<BlitzFist>(), ItemType<CommanderBlessing>()));
            itemLoot.Add(ItemDropRule.Common(ItemType<DivinePresence>()));
            itemLoot.Add(ItemDropRule.Common(ItemType<EnlightenedMask>(), 7));
        }

    }
}
