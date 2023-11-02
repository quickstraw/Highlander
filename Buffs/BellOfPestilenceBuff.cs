using Highlander.Common.Players;
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
    class BellOfPestilenceBuff : ModBuff
    {

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Bell of Pestilence");
			//Description.SetDefault("Melee attacks have a chance to poison enemies");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<HighlanderPlayer>().bellOfPestilence = true;
		}

	}
}
