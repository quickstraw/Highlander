using Highlander.Buffs;
using Highlander.Items.SeaDog;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Accessories
{
	public class BellOfPestilence : ModItem
	{
        public override LocalizedText DisplayName => base.DisplayName;
        public override LocalizedText Tooltip => base.Tooltip;

        public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("Melee attacks have a chance to poison enemies");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.defense = 7;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Orange;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.AddBuff(BuffType<BellOfPestilenceBuff>(), 10);
		}
	}
}