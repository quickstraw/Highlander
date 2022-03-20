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
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool spawned = NPC.AnyNPCs(Mod.Find<ModNPC>("EnlightenmentIdol").Type);
            return !spawned;
        }

        public override bool? UseItem(Player player)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, Mod.Find<ModNPC>("EnlightenmentIdol").Type);
            }

            return true;
        }

        public override void AddRecipes()
		{
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 10)
                .AddIngredient(ItemID.SoulofLight, 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }


	}
}
