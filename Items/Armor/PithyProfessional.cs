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

namespace Highlander.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class PithyProfessional : AbnormalBase
    {
        public PithyProfessional()
        {
            CurrentEffect = 0;
        }
        public PithyProfessional(AbnormalEffect effect) : base(effect)
        {
        }
        public override string Texture => "Highlander/Items/Armor/PithyProfessional";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Pith helmet worn by only the most professional of mercenaries.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 3;
            item.vanity = true;
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

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }


    }
}
