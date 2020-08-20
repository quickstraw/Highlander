using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.SeaDog
{
    class GlowingTreasure : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glowing Treasure");
            Tooltip.SetDefault("Summons the Sea Dog");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 20;
            item.rare = ItemRarityID.Orange;
            item.useAnimation = 45;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool spawned = NPC.AnyNPCs(mod.NPCType("SeaDog"));
            bool beach = player.ZoneBeach;
            return !spawned && beach;
        }

        public override bool UseItem(Player player)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SeaDog"));
            }

            return true;
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Torch, 1);
			recipe.SetResult(this, 1);
            recipe.AddIngredient(ItemID.GoldOre, 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.SetResult(this, 1);
            recipe.AddIngredient(ItemID.PlatinumOre, 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.AddRecipe();
        }


	}
}
