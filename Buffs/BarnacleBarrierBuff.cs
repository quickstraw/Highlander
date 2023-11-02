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
    class BarnacleBarrierBuff : ModBuff
    {

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Barnacle Barrier");
			//Description.SetDefault("Defense is increased by 6");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 6; //Grant a 6 defense increase to the player while the buff is active.
		}

	}
}
