using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Highlander
{
    class HighlanderWorld : ModWorld
    {
        public static bool downedHauntedHatter;

		public override void Initialize()
		{
			downedHauntedHatter = false;
		}

		public override TagCompound Save()
		{
			var downed = new List<string>();
			if (downedHauntedHatter)
			{
				downed.Add("hauntedHatter");
			}

			return new TagCompound
			{
				["downed"] = downed,
			};
		}

		public override void Load(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");
			downedHauntedHatter = downed.Contains("hauntedHatter");
		}

		public override void NetSend(BinaryWriter writer)
		{
			var flags = new BitsByte();
			flags[0] = downedHauntedHatter;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedHauntedHatter = flags[0];
		}


	}
}
