using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Projectiles
{
    class RoninLongYariProjectile : ModProjectile
    {

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ronin Long Yari");
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.aiStyle = 19;
			Projectile.penetrate = -1;
			Projectile.scale = 1.4f;
			Projectile.alpha = 0;

			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}

		// In here the AI uses this example, to make the code more organized and readable
		// Also showcased in ExampleJavelinProjectile.cs
		public float movementFactor // Change this value to alter how fast the spear moves
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.boss)
			{
				int netDamage = (damage - (target.defense)) / 2;
				int extraDamage = damage - netDamage;
				target.StrikeNPC(extraDamage, knockback, 0, crit);
				Projectile.netUpdate = true;
			}
			else
			{
			}
		}

		// It appears that for this AI, only the ai0 field is used!
		public override void AI()
		{
			//Projectile.spriteDirection = Projectile.direction; // Flips the projectile horizontally based on what direction it is facing.
			//Projectile.spriteDirection = 1;

			// Since we access the owner player instance so much, it's useful to create a helper local variable for this
			// Sadly, Projectile/ModProjectile does not have its own
			Player projOwner = Main.player[Projectile.owner];
			// Here we set some of the projectile's owner properties, such as held item and itemtime, along with projectile direction and position based on the player
			Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
			Projectile.direction = projOwner.direction;
			projOwner.heldProj = Projectile.whoAmI;
			projOwner.itemTime = projOwner.itemAnimation;
			Projectile.position.X = ownerMountedCenter.X - (float)(Projectile.width  / 2);
			Projectile.position.Y = ownerMountedCenter.Y - (float)(Projectile.height / 2);
			Projectile.position += forward * 106;
			// As long as the player isn't frozen, the spear can move
			if (!projOwner.frozen)
			{
				if (movementFactor == 0f) // When initially thrown out, the ai0 will be 0f
				{
					movementFactor = 3f; // Make sure the spear moves forward when initially thrown out
					Projectile.netUpdate = true; // Make sure to netUpdate this spear
				}
				if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3) // Somewhere along the item animation, make sure the spear moves back
				{
					movementFactor -= 1.8f;
				}
				else // Otherwise, increase the movement factor
				{
					movementFactor += 1.0f;
				}
			}
			// Change the spear position based off of the velocity and the movementFactor
			Projectile.position += Projectile.velocity * movementFactor;

			// When we reach the end of the animation, we can kill the spear projectile
			if (projOwner.itemAnimation == 0)
			{
				Projectile.Kill();
			}
			// Apply proper rotation, with an offset of 135 degrees due to the sprite's rotation, notice the usage of MathHelper, use this class!
			// MathHelper.ToRadians(xx degrees here)
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);

			// Offset by 90 degrees here
			/**if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation -= MathHelper.ToRadians(90f);
				float rotation = Projectile.rotation - MathHelper.PiOver4;
				Vector2 forward = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				forward.Normalize();

				Projectile.position += forward * 24;
			}
			else
			{
				float rotation = Projectile.rotation - (MathHelper.PiOver2 + MathHelper.PiOver4);
				Vector2 forward = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				forward.Normalize();

				Projectile.position += forward * 24;
			}**/

			// These dusts are added later, for the 'ExampleMod' effect
			if (Main.rand.NextBool(3))
			{
				//Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustType<Sparkle>(),
				//	Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 200, Scale: 1.2f);
				//dust.velocity += Projectile.velocity * 0.3f;
				//dust.velocity *= 0.2f;
			}
			if (Main.rand.NextBool(4))
			{
				//Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustType<Sparkle>(),
				//	0, 0, 254, Scale: 0.3f);
				//dust.velocity += Projectile.velocity * 0.5f;
				//dust.velocity *= 0.5f;
			}
		}

		private Vector2 forward
		{
			get
			{
				float rotation = Projectile.rotation;
				if(Projectile.spriteDirection == 1)
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

	}
}
