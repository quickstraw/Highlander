using Highlander.Buffs;
using Highlander.Items.SeaDog;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Accessories
{
	public class SeaChampionBarrier : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sea Champion's Barrier");
			Tooltip.SetDefault("Gain a defense bonus for your first 150 health\nEnemies are more likely to target you");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.defense = 7;
			item.accessory = true;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Pink;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(player.statLifeMax - player.statLife <= 150) {
				player.AddBuff(ModContent.BuffType<SeaChampionBarrierBuff>(), 10);
			}
			player.aggro += 400;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<BarnacleBarrier>(), 1);
			recipe.SetResult(this, 1);
			recipe.AddIngredient(ItemID.FleshKnuckles);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.AddRecipe();
		}
	}
}