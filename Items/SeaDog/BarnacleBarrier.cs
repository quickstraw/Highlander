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
			//Tooltip.SetDefault("Gain a defense bonus for your first 100 health");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.defense = 1;
			Item.accessory = true;
			Item.value = Item.sellPrice(silver: 90);
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(player.statLifeMax - player.statLife <= 100) {
				player.AddBuff(ModContent.BuffType<BarnacleBarrierBuff>(), 10);
			}
		}
	}
}