using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Buffs
{
    class PiercedArmor : ModBuff
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
			player.statDefense -= 4;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.defense -= 4;
		}

	}
}
