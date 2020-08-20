using Highlander.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Highlander.Items.SeaDog
{
	public class BarnacleBarrier : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Gain defense bonus for the first 100 health");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.defense = 2;
			item.accessory = true;
			item.value = Item.sellPrice(silver: 60);
			item.rare = ItemRarityID.Green;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(player.statLifeMax - player.statLife <= 100) {
				player.statDefense += 6;
			}
		}
	}
}