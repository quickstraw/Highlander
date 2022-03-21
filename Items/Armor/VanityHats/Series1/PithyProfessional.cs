using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class PithyProfessional : AbnormalItem
    {
        public PithyProfessional() : base()
        {
        }
        public PithyProfessional(AbnormalEffect effect) : base(effect)
        {
        }
        public override bool? SafeIsLoadingEnabled(Mod mod) => true;
        public override string Texture => "Highlander/Items/Armor/VanityHats/Series1/PithyProfessional";

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = 3;
            Item.vanity = true;
            base.SetDefaults();
        }

        /**public override void UpdateVanity(Player player, EquipType type)
        {
            base.UpdateVanity(player, type);
            Vector2 headPosition = player.Center;
            float headHeight = 2 * (player.height / 5);
            headPosition.Y -= headHeight;
            headPosition.X -= player.width / 2 + 5;
            Dust currDust = Dust.NewDustDirect(headPosition, player.width + 10, player.height / 8, mod.DustType("AbstractPurpleEnergy"));
            ModDustCustomData data = new ModDustCustomData(player);
            currDust.customData = data;
        }**/


    }
}
