using Highlander.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.EnlightenmentIdol
{
    [AutoloadEquip(EquipType.Back)]
	public class DivinePresence : ModItem
	{

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.defense = 4;
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 4);
			Item.rare = ItemRarityID.Expert;
			Item.expert = true;
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