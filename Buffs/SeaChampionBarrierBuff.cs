using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Buffs
{
    class SeaChampionBarrierBuff : ModBuff
    {

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Sea Champion's Barrier");
			//Description.SetDefault("Defense is increased by 10");
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
