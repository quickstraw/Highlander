using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.Buffs
{
    class DrunkenAnger : ModBuff
    {

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tipsy");
			Description.SetDefault("Increased throwing abilities, lowered defense");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 4; //Grant a +4 defense boost to the player while the buff is active.
			player.thrownCrit += 2;
			player.thrownDamageMult = 1.1f;
			player.thrownVelocity *= 1.1f;
		}

	}
}
