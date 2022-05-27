using Highlander.Common.Players;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Highlander.Items.Weapons
{
    abstract class AmmoGun : ModItem
    {

        private const byte MAX_AMMO = 5;
        public byte ammo;

        public override ModItem Clone(Item item)
        {
            var clone = (AmmoGun)base.Clone(item);
            clone.ammo = ammo;
            return clone;
        }


        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line;
			if (ammo <= 0)
			{
				ammo = 0;
				line = new TooltipLine(Mod, "AbnormalToolTip", "Ammo: " + ammo + "/" + MaxAmmo);
				line.OverrideColor = Color.PaleVioletRed;
			}
			else
			{
				line = new TooltipLine(Mod, "AbnormalToolTip", "Ammo: " + ammo + "/" + MaxAmmo);
				line.OverrideColor = Color.LightGreen;
			}

			tooltips.Add(line);
		}
		public override void HoldItem(Player player)
		{
			HighlanderPlayer modPlayer = player.GetModPlayer<HighlanderPlayer>();
			modPlayer.holdingAmmoGun = true;
			modPlayer.maxAmmo = MaxAmmo;
			modPlayer.currentAmmo = ammo;
			base.HoldItem(player);
		}

        public override void LoadData(TagCompound tag)
		{
			ammo = tag.GetByte("ammoLeft");
		}

        public override void SaveData(TagCompound tag)
        {
			tag["ammoLeft"] = ammo;
        }

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(ammo);
		}

		public override void NetReceive(BinaryReader reader)
		{
			ammo = reader.ReadByte();
		}

		public virtual byte GetMaxAmmo()
		{
			return MAX_AMMO;
		}

		public byte MaxAmmo => GetMaxAmmo();


	}
}
