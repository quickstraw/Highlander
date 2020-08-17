using Highlander.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Items.EnlightenmentIdol
{
	public class CommanderBlessing : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Commander's Blessing");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = Item.sellPrice(gold: 3, silver: 60);
			item.rare = ItemRarityID.Pink;
			item.noMelee = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 30;
			item.useTime = 30;
			item.knockBack = 8.3f;
			item.damage = 107;
			item.noUseGraphic = false;
			item.shoot = ModContent.ProjectileType<ArmProjectile>();
			item.shootSpeed = 0f;
			item.UseSound = SoundID.Item1;
			item.summon = true;
			item.autoReuse = true;
			item.mana = 11;
		}
	}
}