using Highlander.Items;
using Highlander.Items.Armor;
using Highlander.Items.SeaDog;
using Highlander.UnusualLayerEffects;
using Highlander.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Highlander
{
	public class Highlander : Mod
	{

		public Highlander()
		{
			Instance = this;
			RollTable.MakeTable();
			UnusualLayer.dust = new List<FauxDust>();
			FauxDust.mod = this;
		}

		public override void Load()
		{
		}

		public override void Unload()
		{
			RollTable.AbnormalRollTable = null;
			UnusualLayer.dust = null;
			FauxDust.mod = null;
			Instance = null;
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			HighlanderMessageType msgType = (HighlanderMessageType)reader.ReadByte();
			switch (msgType)
			{
				case HighlanderMessageType.HighlanderPlayerSyncPlayer:
					byte playernumber = reader.ReadByte();
					HighlanderPlayer modPlayer = Main.player[playernumber].GetModPlayer<HighlanderPlayer>();
					int unboxed = reader.ReadInt32();
					modPlayer.unboxed = unboxed;
					// SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
					break;
				default:
					Logger.WarnFormat("Highlander: Unknown Message type: {0}", msgType);
					break;
			}
		}

		internal static Highlander Instance { get; private set; }

		internal enum HighlanderMessageType : byte
		{
			HighlanderPlayerSyncPlayer,
		}

	}
}