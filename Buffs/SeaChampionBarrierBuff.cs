using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Buffs
{
    class SeaChampionBarrierBuff : ModBuff
    {

		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 10; //Grant a 10 defense increase to the player while the buff is active.
		}

	}
}
