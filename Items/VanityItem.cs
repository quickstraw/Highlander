using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;

namespace Highlander.Items
{

    class VanityItem : ModItem
    {
        //Can't use [Autoload(false)] lest deriving types not get added
        public sealed override bool IsLoadingEnabled(Mod mod) => SafeIsLoadingEnabled(mod) ?? false;

        /// <summary>
        /// Allows you to safely request whether this item should be autoloaded
        /// </summary>
        /// <param name="mod">The mod adding this item</param>
        /// <returns><see langword="null"/> for the default behaviour (don't autoload item), <see langword="true"/> to let the item autoload or <see langword="false"/> to prevent the item from autoloading</returns>
        public virtual bool? SafeIsLoadingEnabled(Mod mod) => null;

        public override void SetDefaults()
        {
            Item.accessory = true;
        }
    }
}