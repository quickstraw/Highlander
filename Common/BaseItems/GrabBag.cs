using Highlander.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.HauntedHatter
{
    abstract class GrabBag : ModItem
    {
        public sealed override bool IsLoadingEnabled(Mod mod) => SafeIsLoadingEnabled(mod) ?? false;

        /// <summary>
        /// Allows you to safely request whether this item should be autoloaded
        /// </summary>
        /// <param name="mod">The mod adding this item</param>
        /// <returns><see langword="null"/> for the default behaviour (don't autoload item), <see langword="true"/> to let the item autoload or <see langword="false"/> to prevent the item from autoloading</returns>
        public virtual bool? SafeIsLoadingEnabled(Mod mod) => GetType() != typeof(GrabBag) ? true : null;

        public virtual bool IsPreHardMode() => false;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Treasure Bag");
            //Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            ItemID.Sets.BossBag[Item.type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Item.type] = IsPreHardMode();
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override abstract void ModifyItemLoot(ItemLoot itemLoot);

    }
}
