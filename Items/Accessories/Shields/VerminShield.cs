using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;

namespace Highlander.Items.Accessories.Shields
{
	[AutoloadEquip(EquipType.Shield)]
	public class VerminShield : ModItem
	{

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.defense = 2;
			Item.value = Item.sellPrice(gold: 0, silver: 25);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		}

	}
}