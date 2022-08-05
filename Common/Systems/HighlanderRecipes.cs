using Highlander.Items.Accessories;
using Highlander.Items.EnlightenmentIdol;
using Highlander.Items.HauntedHatter;
using Highlander.Items.SeaDog;
using Highlander.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Common.Systems
{
    public class HighlanderRecipes : ModSystem
    {

        public override void AddRecipes()
        {
			var resultItem = GetInstance<BuccaneerBlaster>();

			resultItem.CreateRecipe()
				.AddIngredient(ItemType<TrustyBlunderbuss>(), 1)
				.AddIngredient(ItemID.Cannon)
				.AddTile(TileID.WorkBenches)
				.Register();

			var resultItem2 = GetInstance<GlowingTreasure>();

			resultItem2.CreateRecipe()
                .AddIngredient(ItemID.Torch, 1)
				.AddIngredient(ItemID.GoldOre, 4)
				.AddTile(TileID.WorkBenches)
				.Register();

			resultItem2.CreateRecipe()
				.AddIngredient(ItemID.Torch, 1)
				.AddIngredient(ItemID.PlatinumOre, 4)
				.AddTile(TileID.WorkBenches)
				.Register();

			var resultItem3 = GetInstance<TrustyBlunderbuss>();

			resultItem3.CreateRecipe()
				.AddIngredient(ItemType<BrokenBlunderbuss>(), 1)
				.AddIngredient(ItemID.Musket, 1)
				.AddTile(TileID.WorkBenches)
				.Register();

			resultItem3.CreateRecipe()
				.AddIngredient(ItemType<BrokenBlunderbuss>(), 1)
				.AddIngredient(ItemID.TheUndertaker, 1)
				.AddTile(TileID.WorkBenches)
				.Register();

			var resultItem4 = GetInstance<SpookyHeadwear>();

			resultItem4.CreateRecipe()
				.AddIngredient(ItemID.Silk, 10)
				.AddIngredient(ItemID.ShadowScale, 4)
				.AddTile(TileID.DemonAltar)
				.Register();

			resultItem4.CreateRecipe()
				.AddIngredient(ItemID.ShadowScale, 10)
				.AddIngredient(ItemID.TissueSample, 4)
				.AddTile(TileID.DemonAltar)
				.Register();

			var resultItem5 = GetInstance<SeaChampionBarrier>();

			resultItem5.CreateRecipe()
				.AddIngredient(ItemType<BarnacleBarrier>(), 1)
				.AddIngredient(ItemID.FleshKnuckles, 1)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();

			var resultItem6 = GetInstance<StoneIdol>();

			resultItem6.CreateRecipe()
				.AddIngredient(ItemID.StoneBlock, 10)
				.AddIngredient(ItemID.SoulofLight, 4)
				.AddTile(TileID.MythrilAnvil)
				.Register();

		}

	}
}
