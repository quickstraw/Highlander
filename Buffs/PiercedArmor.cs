﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.Buffs
{
    class PiercedArmor : ModBuff
    {

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Pierced Armor");
			Description.SetDefault("Defense is lowered by 4");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 4;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense -= 4;
		}

	}
}
