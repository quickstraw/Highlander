using Highlander.Buffs;
using Highlander.Common.Players;
using Highlander.Common.Systems;
using Highlander.Items.SeaDog;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.Accessories
{
	public class OldFlareDispenser : ModItem
	{
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string key;
            try
            {
                var keys = KeybindSystem.ActionKeybind.GetAssignedKeys();

                if (keys.Count <= 0)
                {
                    key = "<Unbound>";
                }
                else if (keys.Count > 1)
                {
                    string sKeys = "";
                    for (int i = 0; i < keys.Count - 1; i++)
                    {
                        sKeys += keys[i] + ", ";
                    }
                    sKeys += "or, " + keys[keys.Count - 1];
                    key = sKeys;
                }
                else
                {
                    key = keys[0];
                }
            }
            catch (Exception e)
            {
                key = "<Unbound>";
            }
            foreach(TooltipLine line in tooltips)
            {
                if(line.Text.Length > 15)
                {
                    line.Text = line.Text.FormatWith(key);
                }
            }
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