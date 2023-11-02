using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Buffs
{
    class Bleed : ModBuff
    {

		private int timer;

		public override void SetStaticDefaults()
		{
			timer = 0;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			Main.buffNoTimeDisplay[Type] = false;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (timer % 12 == 0)
			{
				npc.SimpleStrikeNPC(1, 0);
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
