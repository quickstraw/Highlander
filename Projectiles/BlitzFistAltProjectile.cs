﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;

namespace Highlander.Projectiles
{
	public class BlitzFistAltProjectile : ModProjectile
	{

		private BitsByte flags;

		public override bool Autoload(ref string name)
		{
			return true;
		}

		// The folder path to the flail chain sprite
		private const string ChainTexturePath = "Highlander/Projectiles/BlitzFistProjectileChain";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blitz Fist"); // Set the projectile name to Blitz Fist
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.penetrate = -1; // Make the flail infinitely penetrate like other flails
			projectile.melee = true;
			//	projectile.aiStyle = 15; // The vanilla flails all use aiStyle 15, but we must not use it since we want to customize the range and behavior.
		}

		// This AI code is adapted from the aiStyle 15. We need to re-implement this to customize the behavior of our flail
		public override void AI()
		{
			if (!init)
			{
				init = true;
				var velocity = projectile.velocity.ToRotation();
				projectile.rotation = velocity + MathHelper.Pi / 2;
				projectile.ai[0] = projectile.rotation;
				if (velocity > MathHelper.PiOver2 && velocity < 3 * MathHelper.PiOver2 || velocity < -MathHelper.PiOver2)
				{
					projectile.spriteDirection = 1;
				}
				else
				{
					projectile.spriteDirection = -1;
				}
			}
			projectile.rotation = projectile.ai[0];
			var temp = projectile.rotation - (MathHelper.Pi / 2);
			if (temp > MathHelper.PiOver2 && temp < 3 * MathHelper.PiOver2 || temp < -MathHelper.PiOver2)
			{
				projectile.spriteDirection = 1;
			}
			else
			{
				projectile.spriteDirection = -1;
			}

			// Spawn some dust visuals
			//var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 172, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 100, default, 1.5f);
			//dust.noGravity = true;
			//dust.velocity /= 2f;

			var player = Main.player[projectile.owner];

			// If owner player dies, remove the flail.
			if (player.dead)
			{
				projectile.Kill();
				return;
			}

			// This prevents the item from being able to be used again prior to this projectile dying
			player.itemAnimation = 10;
			player.itemTime = 10;

			// Here we turn the player and projectile based on the relative positioning of the player and projectile.
			int newDirection = projectile.Center.X > player.Center.X ? 1 : -1;
			player.ChangeDir(newDirection);
			projectile.direction = newDirection;

			var vectorToPlayer = player.MountedCenter - projectile.Center;
			float currentChainLength = vectorToPlayer.Length();

			// Here is what various ai[] values mean in this AI code:
			// ai[0] == 0: Just spawned/being thrown out
			// ai[0] == 1: Flail has hit a tile or has reached maxChainLength, and is now in the swinging mode
			// ai[1] == 1 or !projectile.tileCollide: projectile is being forced to retract

			// ai[0] == 0 means the projectile has neither hit any tiles yet or reached maxChainLength
			if (goingForward)
			{
				// This is how far the chain would go measured in pixels
				float maxChainLength = 100f;
				projectile.tileCollide = true;

				if (currentChainLength > maxChainLength)
				{
					// If we reach maxChainLength, we change behavior.
					goingForward = false;
					retracting = true;
					projectile.netUpdate = true;
				}
			}
			else if (!goingForward)
			{
				// When ai[0] == 1f, the projectile has either hit a tile or has reached maxChainLength, so now we retract the projectile
				float elasticFactorA = 14f / player.meleeSpeed;
				float elasticFactorB = 0.9f / player.meleeSpeed;
				float maxStretchLength = 200f; // This is the furthest the flail can stretch before being forced to retract. Make sure that this is a bit less than maxChainLength so you don't accidentally reach maxStretchLength on the initial throw.

				if (retracting)
					projectile.tileCollide = false;

				// If the user lets go of the use button, or if the projectile is stuck behind some tiles as the player moves away, the projectile goes into a mode where it is forced to retract and no longer collides with tiles.
				if (!player.channel || currentChainLength > maxStretchLength || !projectile.tileCollide)
				{
					retracting = true;

					if (projectile.tileCollide)
						projectile.netUpdate = true;

					projectile.tileCollide = false;

					if (currentChainLength < 20f)
						projectile.Kill();
				}

				if (!projectile.tileCollide)
					elasticFactorB *= 2f;

				if (retracting)
				{
					var unit = vectorToPlayer;
					unit.Normalize();
					projectile.velocity *= 0.8f;
					projectile.velocity += 6 * unit;
				}
			}

			// Here we set the rotation based off of the direction to the player tweaked by the velocity, giving it a little spin as the flail turns around each swing 
			//projectile.rotation = vectorToPlayer.ToRotation() - projectile.velocity.X * 0.1f;

			// Here is where a flail like Flower Pow could spawn additional projectiles or other custom behaviors
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			// This custom OnTileCollide code makes the projectile bounce off tiles at 1/5th the original speed, and plays sound and spawns dust if the projectile was going fast enough.
			bool shouldMakeSound = false;

			if (oldVelocity.X != projectile.velocity.X)
			{
				if (Math.Abs(oldVelocity.X) > 4f)
				{
					shouldMakeSound = true;
				}

				projectile.position.X += projectile.velocity.X;
				projectile.velocity.X = -oldVelocity.X * 0.2f;
			}

			if (oldVelocity.Y != projectile.velocity.Y)
			{
				if (Math.Abs(oldVelocity.Y) > 4f)
				{
					shouldMakeSound = true;
				}

				projectile.position.Y += projectile.velocity.Y;
				projectile.velocity.Y = -oldVelocity.Y * 0.2f;
			}

			// ai[0] == 1 is used in AI to represent that the projectile has hit a tile since spawning
			goingForward = false;

			if (shouldMakeSound)
			{
				// if we should play the sound..
				projectile.netUpdate = true;
				Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
				// Play the sound
				Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
			}

			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var player = Main.player[projectile.owner];

			Vector2 mountedCenter = player.MountedCenter;
			Texture2D chainTexture = ModContent.GetTexture(ChainTexturePath);

			var drawPosition = projectile.Center;
			var remainingVectorToPlayer = mountedCenter - drawPosition;

			float rotation = remainingVectorToPlayer.ToRotation() - MathHelper.PiOver2;

			if (projectile.alpha == 0)
			{
				int direction = -1;

				if (projectile.Center.X < mountedCenter.X)
					direction = 1;

				player.itemRotation = (float)Math.Atan2(remainingVectorToPlayer.Y * direction, remainingVectorToPlayer.X * direction);
			}

			// This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
			while (true)
			{
				float length = remainingVectorToPlayer.Length();

				// Once the remaining length is small enough, we terminate the loop
				if (length < 25f || float.IsNaN(length))
					break;

				// drawPosition is advanced along the vector back to the player by 12 pixels
				// 12 comes from the height of ExampleFlailProjectileChain.png and the spacing that we desired between links
				drawPosition += remainingVectorToPlayer * 12 / length;
				remainingVectorToPlayer = mountedCenter - drawPosition;

				// Finally, we draw the texture at the coordinates using the lighting information of the tile coordinates of the chain section
				Color color = Lighting.GetColor((int)drawPosition.X / 16, (int)(drawPosition.Y / 16f));
				spriteBatch.Draw(chainTexture, drawPosition - Main.screenPosition, null, color, rotation, chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
			}

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.boss && !target.immortal)
			{
				var player = Main.player[projectile.owner];
				var vectorToPlayer = player.MountedCenter - projectile.Center;
				vectorToPlayer.Normalize();
				target.velocity -= vectorToPlayer * 4;
				target.netUpdate = true;
			}
			goingForward = false;
			retracting = true;
			projectile.velocity *= 0.2f;
			projectile.netUpdate = true;
		}

		public override void Kill(int timeLeft)
		{
			/**var target = Main.npc[attached];
			target.velocity *= 0.0f;
			target.netUpdate = true;**/
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(flags);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			flags = reader.ReadByte();
		}

		private bool init
		{
			get => flags[0];
			set => flags[0] = value;
		}

		private bool retracting
		{
			get => flags[1];
			set => flags[1] = value;
		}

		private bool goingForward
		{
			get => !flags[2];
			set => flags[2] = !value;
		}
	}
}