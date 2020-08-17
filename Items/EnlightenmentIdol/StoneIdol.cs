using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.EnlightenmentIdol
{
    class StoneIdol : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Idol");
            Tooltip.SetDefault("Summons the Idol of Enlightenment");
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
            bool spawned = NPC.AnyNPCs(mod.NPCType("EnlightenmentIdol"));
            return !spawned;
        }

        public override bool UseItem(Player player)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("EnlightenmentIdol"));
            }

            return true;
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.SetResult(this, 1);
            recipe.AddIngredient(ItemID.SoulofLight, 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddRecipe();
        }


	}
}
