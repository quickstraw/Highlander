using Highlander.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Highlander.Items.EnlightenmentIdol
{
	public class CommanderBlessing : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(Language.GetTextValue("Mods.Highlander.ItemName.EnlightenmentIdol." + GetType().Name));
			Tooltip.SetDefault(Language.GetTextValue("Mods.Highlander.ItemTooltip.EnlightenmentIdol." + GetType().Name));
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(gold: 3, silver: 60);
			Item.rare = ItemRarityID.Pink;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.knockBack = 8.3f;
			Item.damage = 107;
			Item.noUseGraphic = false;
			Item.shoot = ModContent.ProjectileType<ArmProjectile>();
			Item.shootSpeed = 0f;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Summon;
			Item.autoReuse = true;
			Item.mana = 13;
		}
	}
}