using Highlander.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Projectiles.Spears
{
    class VerminSpearProjectile : ModProjectile
    {

		private Rectangle shortBox;
		private Rectangle midBox;
		private Rectangle longBox;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Vermin Spear");
		}

		Texture2D texture = ModContent.Request<Texture2D>("Highlander/Projectiles/Spears/VerminSpearProjectile").Value;

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = 19;
			Projectile.penetrate = -1;
			Projectile.scale = 1.0f;
			Projectile.alpha = 0;

			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			
		}

		public float movementFactor // Change this value to alter how fast the spear moves
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.HasBuff(BuffID.Poisoned) || target.HasBuff(BuffID.Venom))
            {
				modifiers.FinalDamage *= 1.2f;
				modifiers.SetCrit();
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
			if (!modifiers.PvP) return;
            if (target.HasBuff(BuffID.Poisoned) || target.HasBuff(BuffID.Venom))
            {
                modifiers.FinalDamage *= 1.2f;
            }
        }

		// It appears that for this AI, only the ai0 field is used!
		public override void AI()
		{
			// Since we access the owner player instance so much, it's useful to create a helper local variable for this
			// Sadly, Projectile/ModProjectile does not have its own
			Player projOwner = Main.player[Projectile.owner];

			Projectile.spriteDirection = Projectile.direction;
			Projectile.damage = 16;

			// Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
			// MathHelper.ToRadians(xx degrees here)
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
			// Offset by 90 degrees here
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation -= MathHelper.ToRadians(90f);
			}

			// Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
			Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
			Projectile.direction = projOwner.direction;
			projOwner.heldProj = Projectile.whoAmI;
			projOwner.itemTime = projOwner.itemAnimation;
			Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width / 2);
			Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
			Projectile.position += forward * 54;

			Vector2 shortPos = new Vector2(Projectile.position.X - 3, Projectile.position.Y - 3);
			shortPos -= forward * 24;
			shortBox = new Rectangle((int) shortPos.X - 3, (int) shortPos.Y - 3, 6, 6);
			Vector2 midPos = new Vector2(Projectile.position.X - 3, Projectile.position.Y - 3);
			midPos -= forward * 16;
			midBox = new Rectangle((int)midPos.X - 3, (int)midPos.Y - 3, 6, 6);
			Vector2 longPos = new Vector2(Projectile.position.X - 3, Projectile.position.Y - 3);
			longPos -= forward * 8;
			longBox = new Rectangle((int)longPos.X - 3, (int)longPos.Y - 3, 6, 6);

			foreach (NPC npc in Main.npc)
			{
				if(shortBox.Intersects(npc.Hitbox) || midBox.Intersects(npc.Hitbox) || longBox.Intersects(npc.Hitbox))
				{
					if (npc.immune[projOwner.whoAmI] == 0 && !npc.friendly)
					{
						var hitInfo = new NPC.HitInfo()
						{
							Damage = Projectile.damage,
							Knockback = Projectile.knockBack,
							HitDirection = Projectile.direction
                        };
						var dealtDamage = npc.StrikeNPC(hitInfo);
						NetMessage.SendStrikeNPC(npc, hitInfo);
						OnHitNPC(npc, hitInfo, dealtDamage);
						npc.immune[projOwner.whoAmI] = 10;
					}
					
				}
			}

			// As long as the player isn't frozen, the spear can move
			if (!projOwner.frozen)
			{
				if (movementFactor == 0f) // When initially thrown out, the ai0 will be 0f
				{
					movementFactor = 0.1f; // Make sure the spear moves forward when initially thrown out
					Projectile.netUpdate = true; // Make sure to netUpdate this spear
				}
				if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
				{
					movementFactor -= 0.1f;
				}
				else // Otherwise, increase the movement factor
				{
					movementFactor += 0.05f;
				}
			}
			// Change the spear position based off of the velocity and the movementFactor
			Projectile.position += Projectile.velocity * movementFactor;
			// When we reach the end of the animation, we can kill the spear projectile
			if (projOwner.itemAnimation == 0)
			{
				Projectile.Kill();
			}
		}

		private Vector2 forward
		{
			get
			{
				float rotation = Projectile.rotation;
				//rotation -= 3 * MathHelper.PiOver4;
				if (Projectile.spriteDirection == 1)
				{
					rotation -= 3 * MathHelper.PiOver4;
				}
				else
				{
					rotation -= 1 * MathHelper.PiOver4;
				}
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			//spriteBatch.Draw(texture, Projectile.position, lightColor);
			//spriteBatch.Draw(texture, Projectile.position + forward * 12 - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height),
			//	lightColor, Projectile.rotation, new Vector2(0 * texture.Width / 2, 0 * texture.Height / 2), 1f, SpriteEffects.None, 0);
			return true;
		}

	}
}
