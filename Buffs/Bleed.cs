using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.Buffs
{
    class Bleed : ModBuff
    {

		private int timer;

		public override void SetDefaults()
		{
			timer = 0;
			DisplayName.SetDefault("Bleeding");
			Description.SetDefault("Damage over time");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
			canBeCleared = false;
		}

		/**public override void Update(Player player, ref int buffIndex)
		{
			if (timer % 12 == 0)
			{
				player.
			}
			timer = (timer + 1) % 12;
		}**/

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (timer % 12 == 0)
			{
				npc.StrikeNPCNoInteraction(1, 0, 0);
				if (npc.velocity.Y < 0)
				{
					if (!npc.boss)
					{
						npc.velocity.Y *= 0.6f;
						if(npc.velocity.Y >= -1)
						{
							if(npc.velocity.Y < 0)
							{
								npc.velocity.Y = 0;
							}
							else
							{
								npc.velocity.Y++;
							}
						}
					}
				}
			}
			timer = (timer + 1) % 12;
		}

	}
}
