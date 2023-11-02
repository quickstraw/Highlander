using Highlander.Common.Players;
using Highlander.Common.Systems;
using Highlander.Items.SeaDog;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander
{
    public class Highlander : Mod
	{

		private static int LogCounter = 0;

		public Highlander()
		{
			Instance = this;
		}

		public override void Load()
		{
			
		}

		public override void Unload()
		{
			Instance = null;
		}

		private void AddEquipTexture(ModItem item, EquipType type, string texpath)
        {
			EquipLoader.AddEquipTexture(Instance, texpath, type, item);
        }

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			HighlanderMessageType msgType = (HighlanderMessageType)reader.ReadByte();
			switch (msgType)
			{
				case HighlanderMessageType.HighlanderPlayerSyncPlayer:
					byte playernumber = reader.ReadByte();
					HighlanderPlayer modPlayer = Main.player[playernumber].GetModPlayer<HighlanderPlayer>();
					// SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
					break;
				default:
					Logger.WarnFormat("Highlander: Unknown Message type: {0}", msgType);
					break;
			}
		}

		public override void PostSetupContent()
		{
			// Boss Checklist Support
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
			{
				bossChecklist.Call(
					"AddBoss",
					4.99f,
					ModContent.NPCType<NPCs.HauntedHatter.HauntedHatter>(),
					this, // Mod
					"Haunted Hatter",
					(Func<bool>)(() => HighlanderWorld.downedHauntedHatter),
					ModContent.ItemType<Items.HauntedHatter.SpookyHeadwear>(),
					new List<int> { ModContent.ItemType<Items.HauntedHatter.HauntedHatterTrophy>(), ModContent.ItemType<Items.HauntedHatter.GhostlyGibus>() },
					new List<int> { ModContent.ItemType<Items.HauntedHatter.HauntedHatterBag>(), ModContent.ItemType<Items.HauntedHatter.EnchantedNeedleHook>()
					, ModContent.ItemType<Items.HauntedHatter.SpiritShears>(), ModContent.ItemType<Items.Weapons.AncientStoneBlaster>()},
					$"Use [i:{ModContent.ItemType<Items.HauntedHatter.SpookyHeadwear>()}] which can be crafted or dropped once after defeating the Eater of Worlds or the Brain of Cthulu.",
					"Haunted Hatter spirited away the challengers");
				// Additional bosses here
				bossChecklist.Call(
					"AddBoss",
					9.1f,
					ModContent.NPCType<NPCs.EnlightenmentIdol.EnlightenmentIdol>(),
					this, // Mod
					"Idol of Enlightenment",
					(Func<bool>)(() => HighlanderWorld.downedEnlightenmentIdol),
					ModContent.ItemType<Items.EnlightenmentIdol.StoneIdol>(),
					new List<int> { ModContent.ItemType<Items.EnlightenmentIdol.EnlightenmentIdolTrophy>(), ModContent.ItemType<Items.EnlightenmentIdol.EnlightenedMask>() },
					new List<int> { ModContent.ItemType<Items.EnlightenmentIdol.EnlightenmentIdolBag>(), ModContent.ItemType<Items.EnlightenmentIdol.DivinePresence>()
					, ModContent.ItemType<Items.EnlightenmentIdol.BlitzFist>(), ModContent.ItemType<Items.EnlightenmentIdol.CommanderBlessing>()},
					$"Use [i:{ModContent.ItemType<Items.EnlightenmentIdol.StoneIdol>()}] which can be crafted with [i:{ItemID.SoulofLight}] and [i:{ItemID.StoneBlock}].",
					"Idol of Enlightenment helped the challengers pass on",
					"Highlander/NPCs/EnlightenmentIdol/EnlightenmentIdolBossChecklist");

				bossChecklist.Call(
					"AddBoss",
					0.8f,
					ModContent.NPCType<NPCs.SeaDog.SeaDog>(),
					this, // Mod
					"Sea Dog",
					(Func<bool>)(() => HighlanderWorld.downedSeaDog),
					ModContent.ItemType<Items.SeaDog.GlowingTreasure>(),
					new List<int> { ModContent.ItemType<Items.SeaDog.SeaDogTrophy>(), ModContent.ItemType<Items.SeaDog.SeaDogMask>() },
					new List<int> { ModContent.ItemType<Items.SeaDog.SeaDogBag>(), ModContent.ItemType<BarnacleBarrier>(),
						ModContent.ItemType<FeralFrenzy>(), ModContent.ItemType<BrokenBlunderbuss>(), ItemID.GoldOre, ItemID.SpelunkerPotion, ItemID.GillsPotion},
					$"Use [i:{ModContent.ItemType<Items.SeaDog.GlowingTreasure>()}] at a beach.",
					"Sea Dog devoured the challengers");
			}

			// Census Support
			if (ModLoader.TryGetMod("Census", out Mod censusMod))
			{
				// Here I am using Chat Tags to make my condition even more interesting.
				// If you localize your mod, pass in a localized string instead of just English.
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.VeteranExplorer.VeteranExplorer>(),
					$"When Skeletron has been defeated");
			}
		}

		internal static Highlander Instance { get; private set; }

		public static void Log(string text)
        {
			Instance.Logger.Debug(LogCounter + ": " + text);
			LogCounter++;
        }

		internal enum HighlanderMessageType : byte
		{
			HighlanderPlayerSyncPlayer,
		}

	}
}