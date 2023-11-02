using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.SeaDog
{
    class GlowingTreasure : ModItem
    {

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
            bool spawned = NPC.AnyNPCs(Mod.Find<ModNPC>("SeaDog").Type);
            bool beach = player.ZoneBeach;
            return !spawned && beach;
        }

        public override bool? UseItem(Player player)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, Mod.Find<ModNPC>("SeaDog").Type);
            }

            return true;
        }
	}
}
