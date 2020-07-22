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
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 1 * 36)
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
