
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Items.HauntedHatter
{
	internal class EnchantedNeedleHook : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Needle Hook");
		}

		public override void SetDefaults()
		{
			/*
				this.noUseGraphic = true;
				this.damage = 0;
				this.knockBack = 7f;
				this.useStyle = 5;
				this.name = "Amethyst Hook";
				this.shootSpeed = 10f;
				this.shoot = 230;
				this.width = 18;
				this.height = 28;
				this.useSound = 1;
				this.useAnimation = 20;
				this.useTime = 20;
				this.rare = 1;
				this.noMelee = true;
				this.value = 20000;
			*/
			// Instead of copying these values, we can clone and modify the ones we want to copy
			Item.CloneDefaults(ItemID.DiamondHook);
			Item.shootSpeed = 13.5f; // how quickly the hook is shot.
			Item.shoot = ProjectileType<EnchantedNeedleProjectile>();
			Item.rare = ItemRarityID.Expert;
			Item.value = Item.sellPrice(silver: 80);
			Item.expert = true;
		}
	}

	internal class EnchantedNeedleProjectile : ModProjectile
	{
		private static Asset<Texture2D> chainTexture;

		public override void Load()
		{
			chainTexture = Request<Texture2D>("Highlander/Items/HauntedHatter/EnchantedNeedleHookChain");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("${ProjectileName.GemHookDiamond}");
			ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true; // projectiles with hide but without this will draw in the lighting values of the owner player.
		}

		public override void SetDefaults()
		{
			/*	this.netImportant = true;
				this.name = "Gem Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			*/
			Projectile.CloneDefaults(ProjectileID.GemHookDiamond);
			Projectile.scale = 1f;
			Projectile.hide = true;
		}

		// Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
		public override bool? CanUseGrapple(Player player)
		{
			int hooksOut = 0;
			for (int l = 0; l < 1000; l++)
			{
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == Projectile.type)
				{
					hooksOut++;
				}
			}
			if (hooksOut > 3) // This hook can have 3 hooks out.
			{
				return false;
			}
			return true;
		}

		// Return true if it is like: Hook, CandyCaneHook, BatHook, GemHooks
		//public override bool? SingleGrappleHook(Player player)
		//{
		//	return true;
		//}

		// Use this to kill oldest hook. For hooks that kill the oldest when shot, not when the newest latches on: Like SkeletronHand
		// You can also change the projectile like: Dual Hook, Lunar Hook
		//public override void UseGrapple(Player player, ref int type)
		//{
		//	int hooksOut = 0;
		//	int oldestHookIndex = -1;
		//	int oldestHookTimeLeft = 100000;
		//	for (int i = 0; i < 1000; i++)
		//	{
		//		if (Main.projectile[i].active && Main.projectile[i].owner == projectile.whoAmI && Main.projectile[i].type == projectile.type)
		//		{
		//			hooksOut++;
		//			if (Main.projectile[i].timeLeft < oldestHookTimeLeft)
		//			{
		//				oldestHookIndex = i;
		//				oldestHookTimeLeft = Main.projectile[i].timeLeft;
		//			}
		//		}
		//	}
		//	if (hooksOut > 1)
		//	{
		//		Main.projectile[oldestHookIndex].Kill();
		//	}
		//}

		// Amethyst Hook is 300, Static Hook is 600
		public override float GrappleRange()
		{
			return 352f; // 22 blocks
		}

		public override void NumGrappleHooks(Player player, ref int numHooks)
		{
			numHooks = 3;
		}

		// default is 11, Lunar is 24
		public override void GrappleRetreatSpeed(Player player, ref float speed)
		{
			speed = 18f;
		}

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			speed = 11;
		}

        public override void PostDraw(Color lightColor)
		{
			float strength = 0.5f;
			Lighting.AddLight(Projectile.position - forward * 20 + Projectile.velocity, 0.36f * strength, 0.24f * strength, 0.40f * strength);
		}

		public override bool PreDrawExtras()
		{
			Vector2 playerCenter = Main.player[Projectile.owner].MountedCenter;
			Vector2 center = Projectile.Center;
			Vector2 distToProj = playerCenter - Projectile.Center;
			float projRotation = distToProj.ToRotation() - 1.57f;
			float distance = distToProj.Length();

			while (distance > 30f && !float.IsNaN(distance))
			{
				distToProj.Normalize();                 //get unit vector
				distToProj *= 24f;                      //speed = 24
				center += distToProj;                   //update draw position
				distToProj = playerCenter - center;    //update distance
				distance = distToProj.Length();
				Color drawColor = Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16));

				//Draw chain
				Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
					chainTexture.Value.Bounds, drawColor, projRotation,
					chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
			}
			return false;
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCsAndTiles.Add(index);
        }

        private Vector2 forward
		{
			get
			{
				float rotation = Projectile.rotation - MathHelper.PiOver2;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

	}
}
