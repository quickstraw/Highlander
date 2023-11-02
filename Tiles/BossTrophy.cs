using Highlander.Items.EnlightenmentIdol;
using Highlander.Items.HauntedHatter;
using Highlander.Items.SeaDog;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;


namespace Highlander.Tiles
{
    class BossTrophy : ModTile
    {

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true; // Necessary since Style3x3Wall uses AnchorWall
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			DustType = 7;
			TileID.Sets.DisableSmartCursor[Type] = true;
			//ModTranslation name = CreateMapEntryName();
			//name.SetDefault("Trophy");
			//AddMapEntry(new Color(120, 85, 60), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			switch (frameX / 54)
			{
				case 0:
					item = ItemType<HauntedHatterTrophy>();
					break;
				case 1:
					item = ItemType<EnlightenmentIdolTrophy>();
					break;
				case 2:
					item = ItemType<SeaDogTrophy>();
					break;
			}
			if (item > 0)
			{
				var source = new EntitySource_TileBreak(i, j);
				Item.NewItem(source, i * 16, j * 16, 48, 48, item);
			}
		}


	}
}
