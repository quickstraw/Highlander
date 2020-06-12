using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.HauntedHatter
{
    class SpookyHeadwear : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spooky Headwear");
            Tooltip.SetDefault("Summons the Haunted Hatter");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 20;
            item.rare = ItemRarityID.White;
            item.useAnimation = 45;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool spawned = NPC.AnyNPCs(mod.NPCType("HauntedHatter"));
            return !spawned;
        }

        public override bool UseItem(Player player)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("HauntedHatter"));
            }

            return true;
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.SetResult(this, 1);
            recipe.AddIngredient(ItemID.ShadowScale);
            recipe.AddTile(TileID.DemonAltar);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.SetResult(this, 1);
            recipe.AddIngredient(ItemID.TissueSample);
            recipe.AddTile(TileID.DemonAltar);
            recipe.AddRecipe();
        }


	}
}
