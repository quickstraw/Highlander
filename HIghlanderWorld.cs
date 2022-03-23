using Highlander.Items.SeaDog;
using Highlander.Items.Weapons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Highlander
{
    class HighlanderWorld : ModSystem
    {
		public static bool downedSeaDog;
		public static bool downedHauntedHatter;
		public static bool downedEnlightenmentIdol;

        public override void OnWorldLoad()
        {
			downedSeaDog = false;
			downedHauntedHatter = false;
			downedEnlightenmentIdol = false;
		}

        public override void SaveWorldData(TagCompound tag)
        {
			var downed = new List<string>();
			if (downedSeaDog)
			{
				tag["downedSeaDog"] = true;
			}
			if (downedHauntedHatter)
			{
				tag["downedHauntedHatter"] = true;
			}
			if (downedEnlightenmentIdol)
			{
				tag["downedEnlightenmentIdol"] = true;
			}
		}

        public override void LoadWorldData(TagCompound tag)
        {
			downedSeaDog = tag.ContainsKey("downedSeaDog");
			downedHauntedHatter = tag.ContainsKey("downedHauntedHatter");
			downedEnlightenmentIdol = tag.ContainsKey("downedEnlightenmentIdol");
		}

		public override void NetSend(BinaryWriter writer)
		{
			var flags = new BitsByte();
			flags[0] = downedHauntedHatter;
			flags[1] = downedEnlightenmentIdol;
			flags[2] = downedSeaDog;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			downedHauntedHatter = flags[0];
			downedEnlightenmentIdol = flags[1];
			downedSeaDog = flags[2];
		}

		/**public override void PostSetupContent()
		{
			// Boss Checklist Support
			Mod bossChecklist = ModLoader.GetMod("BossChecklist");
			if (bossChecklist != null)
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
			Mod censusMod = ModLoader.GetMod("Census");
			if (censusMod != null)
			{
				// Here I am using Chat Tags to make my condition even more interesting.
				// If you localize your mod, pass in a localized string instead of just English.
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.HatSalesman.HatSalesman>(),
					$"When the Haunted Hatter has been defeated");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.VeteranExplorer.VeteranExplorer>(),
					$"When Skeletron has been defeated");
			}
		}**/

		// We can use PostWorldGen for world generation tasks that don't need to happen between vanilla world generation steps.
		public override void PostWorldGen()
		{
			// Place some items in Ice Chests
			int[] itemsToPlaceInIceChests = { ItemType<ChariotWhip>(), ItemType<RoninLongYari>() };
			int itemsToPlaceInIceChestsChoice = 0;
			for (int chestIndex = 0; chestIndex < Main.chest.Length; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 12th chest is the Ice Chest. Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding.
				// 0 - Wooden
				// 1 - Golden
				// 2 - Locked Golden
				// 3 - Shadow
				// 4 - Locked Shadow
				// 5 - Barrel
				// 6 - Trash Can
				if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36)
				{
					if (Main.rand.NextBool(8)) {
						for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
						{
							if (chest.item[inventoryIndex].type == ItemID.None)
							{
								chest.item[inventoryIndex].SetDefaults(itemsToPlaceInIceChests[itemsToPlaceInIceChestsChoice]);
								itemsToPlaceInIceChestsChoice = (itemsToPlaceInIceChestsChoice + 1) % itemsToPlaceInIceChests.Length;
								// Alternate approach: Random instead of cyclical: chest.item[inventoryIndex].SetDefaults(Main.rand.Next(itemsToPlaceInIceChests));
								break;
							}
						}
					}
				}
			}
		}



	}
}
