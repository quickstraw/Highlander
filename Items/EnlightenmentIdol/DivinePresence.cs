using Highlander.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Highlander.Items.EnlightenmentIdol
{
	public class DivinePresence : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Nearby players on your team receive a defense bonus and a slight attack bonus");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.defense = 3;
			item.accessory = true;
			item.value = Item.sellPrice(silver: 30);
			item.rare = ItemRarityID.Expert;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				player.AddBuff(ModContent.BuffType<DivinePresenceBuff>(), 30);
			}
			else
			{
				foreach (Player p in Main.player)
				{
					if (p.team == player.team)
					{
						float distanceSquared = Vector2.DistanceSquared(player.position, p.position);
						if (distanceSquared <= 202500)
						{
							p.AddBuff(ModContent.BuffType<DivinePresenceBuff>(), 30);
							//NetMessage.SendData(MessageID.AddPlayerBuff, number: player.whoAmI, number2: ModContent.BuffType<DivinePresenceBuff>(), number3: 60);
						}
					}
				}
			}
		}
	}
}