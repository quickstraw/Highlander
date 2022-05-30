using Highlander.Buffs;
using Highlander.Common.Players;
using Highlander.Common.Systems;
using Highlander.Items.SeaDog;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Accessories
{
	public class OldFlareDispenser : ModItem
	{
		public override void SetStaticDefaults()
		{
			string tooltip = "";
			try
			{
				var keys = KeybindSystem.ActionKeybind.GetAssignedKeys();
				
				if (keys.Count <= 0)
				{
					tooltip = "Bind the action key in Controls to throw flares!";
				}
				else if (keys.Count > 1)
				{
					string sKeys = "";
					for (int i = 0; i < keys.Count - 1; i++)
					{
						sKeys += keys[i] + ", ";
					}
					sKeys += "or, " + keys[keys.Count - 1];
					tooltip = $"Press {sKeys} to throw a flare";
				}
				else
				{
					tooltip = $"Press {keys[0]} to throw a flare";
				}
			}
            catch (Exception e)
            {
				tooltip = "Bind the action key in Controls to throw flares!";
			}
			Tooltip.SetDefault(tooltip);
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.defense = 1;
			Item.accessory = true;
			Item.value = Item.buyPrice(gold: 5);
			Item.rare = ItemRarityID.Blue;
		}

        public override void UpdateEquip(Player player)
        {
			var ModPlayer = player.GetModPlayer<HighlanderPlayer>();
			ModPlayer.hasFlares = true;
        }

    }
}