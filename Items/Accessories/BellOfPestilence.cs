using Highlander.Buffs;
using Highlander.Items.SeaDog;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Accessories
{
	public class BellOfPestilence : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Sea Champion's Barrier");
			Tooltip.SetDefault("Melee attacks have a chance to poison enemies");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.defense = 7;
			item.accessory = true;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Orange;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.AddBuff(BuffType<BellOfPestilenceBuff>(), 10);
		}
	}
}