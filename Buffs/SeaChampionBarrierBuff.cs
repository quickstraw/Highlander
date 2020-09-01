using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.Buffs
{
    class SeaChampionBarrierBuff : ModBuff
    {

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sea Champion's Barrier");
			Description.SetDefault("Defense is increased by 10");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 10; //Grant a 6 defense increase to the player while the buff is active.
		}

	}
}
