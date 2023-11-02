using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.HauntedHatter
{
    class SpookyHeadwear : ModItem
    {
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
