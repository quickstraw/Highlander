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
    class DrunkenAnger : ModBuff
    {

		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 4; //Grant a -4 defense decrease to the player while the buff is active.
			player.GetDamage(DamageClass.Throwing) *= 1.15f;
			player.GetCritChance(DamageClass.Throwing) += 2;
			player.ThrownVelocity *= 1.1f;
		}

	}
}
