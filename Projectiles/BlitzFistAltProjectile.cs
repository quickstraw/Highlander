
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using Terraria.Audio;

namespace Highlander.Projectiles
{
	public class BlitzFistAltProjectile : ModProjectile
	{

		private BitsByte flags;

		// The folder path to the flail chain sprite
		private const string ChainTexturePath = "Highlander/Projectiles/BlitzFistProjectileChain";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blitz Fist"); // Set the projectile name to Blitz Fist
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.penetrate = -1; // Make the flail infinitely penetrate like other flails
			Projectile.DamageType = DamageClass.Melee;
			//	Projectile.aiStyle = 15; // The vanilla flails all use aiStyle 15, but we must not use it since we want to customize the range and behavior.
		}

		// This AI code is adapted from the aiStyle 15. We need to re-implement this to customize the behavior of our flail
		public override void AI()
		{
			if (!init)
			{
				init = true;
				var velocity = Projectile.velocity.ToRotation();
				Projectile.rotation = velocity + MathHelper.Pi / 2;
				Projectile.ai[0] = Projectile.rotation;
				if (velocity > MathHelper.PiOver2 && velocity < 3 * MathHelper.PiOver2 || velocity < -MathHelper.PiOver2)
				{
					Projectile.spriteDirection = 1;
				}
				else
				{
					Projectile.spriteDirection = -1;
				}
			}
			Projectile.rotation = Projectile.ai[0];
			var temp = Projectile.rotation - (MathHelper.Pi / 2);
			if (temp > MathHelper.PiOver2 && temp < 3 * MathHelper.PiOver2 || temp < -MathHelper.PiOver2)
			{
				Projectile.spriteDirection = 1;
			}
			else
			{
				Projectile.spriteDirection = -1;
			}

			// Spawn some dust visuals
			//var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 172, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 100, default, 1.5f);
			//dust.noGravity = true;
			//dust.velocity /= 2f;

			var player = Main.player[Projectile.owner];

			// If owner player dies, remove the flail.
			if (player.dead)
			{
				Projectile.Kill();
				return;
			}

			// This prevents the item from being able to be used again prior to this projectile dying
			player.itemAnimation = 10;
			player.itemTime = 10;

			// Here we turn the player and projectile based on the relative positioning of the player and Projectile.
			int newDirection = Projectile.Center.X > player.Center.X ? 1 : -1;
			player.ChangeDir(newDirection);
			Projectile.direction = newDirection;

			var vectorToPlayer = player.MountedCenter - Projectile.Center;
			float currentChainLength = vectorToPlayer.Length();

			// Here is what various ai[] values mean in this AI code:
			// ai[0] == 0: Just spawned/being thrown out
			// ai[0] == 1: Flail has hit a tile or has reached maxChainLength, and is now in the swinging mode
			// ai[1] == 1 or !Projectile.tileCollide: projectile is being forced to retract

			// ai[0] == 0 means the projectile has neither hit any tiles yet or reached maxChainLength
			if (goingForward)
			{
				// This is how far the chain would go measured in pixels
				float maxChainLength = 100f;
				Projectile.tileCollide = true;

				if (currentChainLength > maxChainLength)
				{
					// If we reach maxChainLength, we change behavior.
					goingForward = false;
					retracting = true;
					Projectile.netUpdate = true;
				}
			}
			else if (!goingForward)
			{
				// When ai[0] == 1f, the projectile has either hit a tile or has reached maxChainLength, so now we retract the projectile
				float elasticFactorA = 14f / player.GetAttackSpeed(DamageClass.Melee);
				float elasticFactorB = 0.9f / player.GetAttackSpeed(DamageClass.Melee);
				float maxStretchLength = 200f; // This is the furthest the flail can stretch before being forced to retract. Make sure that this is a bit less than maxChainLength so you don't accidentally reach maxStretchLength on the initial throw.

				if (retracting)
					Projectile.tileCollide = false;

				// If the user lets go of the use button, or if the projectile is stuck behind some tiles as the player moves away, the projectile goes into a mode where it is forced to retract and no longer collides with tiles.
				if (!player.channel || currentChainLength > maxStretchLength || !Projectile.tileCollide)
				{
					retracting = true;

					if (Projectile.tileCollide)
						Projectile.netUpdate = true;

					Projectile.tileCollide = false;

					if (currentChainLength < 20f)
						Projectile.Kill();
				}

				if (!Projectile.tileCollide)
					elasticFactorB *= 2f;

				if (retracting)
				{
					var unit = vectorToPlayer;
					unit.Normalize();
					Projectile.velocity *= 0.8f;
					Projectile.velocity += 6 * unit;
				}
			}

			// Here we set the rotation based off of the direction to the player tweaked by the velocity, giving it a little spin as the flail turns around each swing 
			//Projectile.rotation = vectorToPlayer.ToRotation() - Projectile.velocity.X * 0.1f;

			// Here is where a flail like Flower Pow could spawn additional projectiles or other custom behaviors
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			// This custom OnTileCollide code makes the projectile bounce off tiles at 1/5th the original speed, and plays sound and spawns dust if the projectile was going fast enough.
			bool shouldMakeSound = false;

			if (oldVelocity.X != Projectile.velocity.X)
			{
				if (Math.Abs(oldVelocity.X) > 4f)
				{
					shouldMakeSound = true;
				}

				Projectile.position.X += Projectile.velocity.X;
				Projectile.velocity.X = -oldVelocity.X * 0.2f;
			}

			if (oldVelocity.Y != Projectile.velocity.Y)
			{
				if (Math.Abs(oldVelocity.Y) > 4f)
				{
					shouldMakeSound = true;
				}

				Projectile.position.Y += Projectile.velocity.Y;
				Projectile.velocity.Y = -oldVelocity.Y * 0.2f;
			}

			// ai[0] == 1 is used in AI to represent that the projectile has hit a tile since spawning
			goingForward = false;

			if (shouldMakeSound)
			{
				// if we should play the sound..
				Projectile.netUpdate = true;
				Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
				// Play the sound
				SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			}

			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var player = Main.player[Projectile.owner];

			Vector2 mountedCenter = player.MountedCenter;
			Texture2D chainTexture = ModContent.Request<Texture2D>(ChainTexturePath).Value;

			var drawPosition = Projectile.Center;
			var remainingVectorToPlayer = mountedCenter - drawPosition;

			float rotation = remainingVectorToPlayer.ToRotation() - MathHelper.PiOver2;

			if (Projectile.alpha == 0)
			{
				int direction = -1;

				if (Projectile.Center.X < mountedCenter.X)
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
				Main.EntitySpriteDraw(chainTexture, drawPosition - Main.screenPosition, null, color, rotation, chainTexture.Size() * 0.5f, 1f, SpriteEffects.None, 0);
			}

			return true;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.boss && !target.immortal)
            {
                var player = Main.player[Projectile.owner];
                var vectorToPlayer = player.MountedCenter - Projectile.Center;
                vectorToPlayer.Normalize();
                target.velocity -= vectorToPlayer * 4;
                target.netUpdate = true;
            }
            goingForward = false;
            retracting = true;
            Projectile.velocity *= 0.2f;
            Projectile.netUpdate = true;
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