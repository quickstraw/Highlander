using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace Highlander.Items.Accessories.Shields
{
	[AutoloadEquip(EquipType.Shield)]
	public class VerminShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
			item.defense = 2;
			item.value = Item.sellPrice(gold: 0, silver: 25);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		}

	}
}