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
            //DisplayName.SetDefault("Spooky Headwear");
            //Tooltip.SetDefault("Summons the Haunted Hatter");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool spawned = NPC.AnyNPCs(Mod.Find<ModNPC>("HauntedHatter").Type);
            return !spawned;
        }

        public override bool? UseItem(Player player)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, Mod.Find<ModNPC>("HauntedHatter").Type);
            }

            return true;
        }
	}
}
