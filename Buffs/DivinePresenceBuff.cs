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
    class DivinePresenceBuff : ModBuff
    {

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Divine Presence");
			//Description.SetDefault("Defense is increased by 4 and damage by 3%");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 4; //Grant a 4 defense increase to the player while the buff is active.
			player.GetDamage(DamageClass.Generic) *= 1.03f;
		}

	}
}
