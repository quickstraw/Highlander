using Highlander.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Projectiles.Bullets
{
	public class TargetBullet : ModProjectile
	{
		private const int PathRadius = 3200;
		private const int InnerRadius = PathRadius / 2;
		private const int LastRadius = PathRadius / 4;
		private bool MovedToTarget = false;
		private Vector2[] NextDestination;
		private byte location;
		private bool init = false;
		private bool hit = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Target Bullet");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;        //The recording mode
		}

		public override void SetDefaults()
		{
			Projectile.width = 4;               //The width of projectile hitbox
			Projectile.height = 4;              //The height of projectile hitbox
			Projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			Projectile.friendly = true;         //Can the projectile deal damage to enemies?
			Projectile.hostile = false;         //Can the projectile deal damage to the player?
			Projectile.DamageType = DamageClass.Ranged;           //Is the projectile shoot by a ranged weapon?
			Projectile.penetrate = 5;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.timeLeft = 400;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			Projectile.light = 0.2f;            //How much light emit around the projectile
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = true;          //Can the projectile collide with tiles?
			Projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
			AIType = ProjectileID.Bullet;           //Act exactly like default Bullet
			NextDestination = new Vector2[6];
		}

		private void Initialize()
        {
			init = true;
			Vector2 next = NextDestination[0];
			Vector2 direction = next - Projectile.position;
			direction.Normalize();
			ChangeDirection(direction * Projectile.velocity.Length());
		}

        public void APostAI()
        {
			if (location > NextDestination.Length) return;
			Vector2 next = NextDestination[location];
			if (!init) return;

			bool moveForward = false;
			float lengthSq = Projectile.velocity.LengthSquared();
			float distanceSq = (Projectile.position - next).LengthSquared();

            if (lengthSq + 1 > distanceSq)
            {
				moveForward = true;
            }

            if (moveForward)
            {
				Projectile.position = next;
				Vector2 direction = next - Projectile.position;
				direction.Normalize();
				ChangeDirection(direction * Projectile.velocity.Length());
				location++;
				Vector2 init = new Vector2();
				if(location >= NextDestination.Length || init == NextDestination[location])
                {
					hit = true;
                }
            }
        }

        public override void PostAI()
        {
			if (hit) return;
            try
            {
				APostAI();
			} catch(Exception e)
            {
				Highlander.Log(e.Message);
            }
        }

        public override void AI()
        {
			if (hit) return;
			if (Projectile.timeLeft > 380) return;

            try
            {
				FindPath();
			} catch(Exception e)
            {
				Highlander.Log(e.Message);
			}
			
		}

		private void FindPath()
        {
			if (Target < 0 || false)
			{
				var npcs = Main.npc;
				float min = float.MaxValue;
				int target = 0;
				for (int i = 0; i < Main.npc.Length; i++)
				{
					if (Main.npc[i] != null && !Main.npc[i].friendly)
					{
						float distance = (Main.npc[i].Center - Projectile.Center).LengthSquared();
						if (distance < min)
						{
							min = distance;
							target = i;
						}
					}
				}
				Target = target;
			}

			if (location == 255 || Target < 0) return;
			if (init) return;
			float dist = (Main.npc[Target].Center - Projectile.Center).LengthSquared();
			if (dist > PathRadius * PathRadius * 2) return;

			if (StraightPathToTarget())
			{
				// Shoot toward target.
				MovedToTarget = true;
				var target = Main.npc[Target];
				Vector2 direction = target.position - Projectile.position;
				direction.Normalize();
				NextDestination[0] = target.Center;
				Initialize();
				return;
			}
			else if (NextDestination[0].X != 0 && NextDestination[0].Y != 0)
			{
				Vector2[] intersections;
				HUtils.Intersect(Projectile.Center, PathRadius, Main.npc[Target].Center, PathRadius, out intersections);
				if (intersections.Length >= 2)
				{
					NPC target = Main.npc[Target];
					Vector2 inter1 = intersections[0];
					Vector2 inter2 = intersections[1];
					float interDistanceSq = (inter1 - inter2).LengthSquared();
					if (interDistanceSq >= 10000)
					{
						bool canHit1 = StraightPathTo(inter1);
						bool canHit2 = StraightPathTo(inter2);
						bool canTarget1 = StraightPathToTarget(inter1);
						bool canTarget2 = StraightPathToTarget(inter2);

						if (canHit1 && canTarget1)
						{
							NextDestination[0] = inter1;
							NextDestination[1] = target.Center;
							Initialize();
							return;
						}
						else if (canHit2 && canTarget2)
						{
							NextDestination[0] = inter2;
							NextDestination[1] = target.Center;
							Initialize();
							return;
						}
						else
						{
							// Projectile Side
							Vector2[] intersections2;
							HUtils.Intersect(Projectile.Center, InnerRadius, inter1, InnerRadius, out intersections2);
							Vector2 interInner11 = intersections2[0];
							Vector2 interInner12 = intersections2[1];
							bool canHitInner11 = StraightPathTo(interInner11);
							bool canHitInner12 = StraightPathTo(interInner12);
							bool canTargetInner11 = StraightPathToTarget(interInner11);
							bool canTargetInner12 = StraightPathToTarget(interInner12);
							bool innerPath11 = canHitInner11 && canTargetInner11;
							bool innerPath12 = canHitInner12 && canTargetInner12;

							// Target Side
							Vector2[] intersections3;
							HUtils.Intersect(target.Center, InnerRadius, inter1, InnerRadius, out intersections3);
							Vector2 interInner13 = intersections3[0];
							Vector2 interInner14 = intersections3[1];
							bool canHitInner13 = StraightPathTo(interInner11);
							bool canHitInner14 = StraightPathTo(interInner12);
							bool canTargetInner13 = StraightPathToTarget(interInner11);
							bool canTargetInner14 = StraightPathToTarget(interInner12);
							bool innerPath13 = canHitInner13 && canTargetInner13;
							bool innerPath14 = canHitInner14 && canTargetInner14;

							// Check for final path.
							if (innerPath11 && innerPath13)
							{
								NextDestination[0] = interInner11;
								NextDestination[1] = inter1;
								NextDestination[2] = interInner13;
								NextDestination[3] = target.Center;
								Initialize();
								return;
							}
							else if (innerPath11 && innerPath14)
							{
								NextDestination[0] = interInner11;
								NextDestination[1] = inter1;
								NextDestination[2] = interInner14;
								NextDestination[3] = target.Center;
								Initialize();
								return;
							}
							else if (innerPath12 && innerPath13)
							{
								NextDestination[0] = interInner12;
								NextDestination[1] = inter1;
								NextDestination[2] = interInner13;
								NextDestination[3] = target.Center;
								Initialize();
								return;
							}
							else if (innerPath12 && innerPath14)
							{
								NextDestination[0] = interInner12;
								NextDestination[1] = inter1;
								NextDestination[2] = interInner14;
								NextDestination[3] = target.Center;
								Initialize();
								return;
							}

							// Projectile Side
							Vector2[] intersections4;
							HUtils.Intersect(Projectile.Center, InnerRadius, inter1, InnerRadius, out intersections4);
							Vector2 interInner21 = intersections4[0];
							Vector2 interInner22 = intersections4[1];
							bool canHitInner21 = StraightPathTo(interInner21);
							bool canHitInner22 = StraightPathTo(interInner22);
							bool canTargetInner21 = StraightPathToTarget(interInner21);
							bool canTargetInner22 = StraightPathToTarget(interInner22);
							bool innerPath21 = canHitInner21 && canTargetInner21;
							bool innerPath22 = canHitInner22 && canTargetInner22;

							// Target Side
							Vector2[] intersections5;
							HUtils.Intersect(target.Center, InnerRadius, inter1, InnerRadius, out intersections5);
							Vector2 interInner23 = intersections5[0];
							Vector2 interInner24 = intersections5[1];
							bool canHitInner23 = StraightPathTo(interInner23);
							bool canHitInner24 = StraightPathTo(interInner24);
							bool canTargetInner23 = StraightPathToTarget(interInner23);
							bool canTargetInner24 = StraightPathToTarget(interInner24);
							bool innerPath23 = canHitInner23 && canTargetInner23;
							bool innerPath24 = canHitInner24 && canTargetInner24;

							// Check for final path.
							if (innerPath21 && innerPath23)
							{
								NextDestination[0] = interInner21;
								NextDestination[1] = inter1;
								NextDestination[2] = interInner23;
								NextDestination[3] = target.Center;
								Initialize();
								return;
							}
							else if (innerPath21 && innerPath24)
							{
								NextDestination[0] = interInner21;
								NextDestination[1] = inter1;
								NextDestination[2] = interInner24;
								NextDestination[3] = target.Center;
								Initialize();
								return;
							}
							else if (innerPath22 && innerPath23)
							{
								NextDestination[0] = interInner22;
								NextDestination[1] = inter2;
								NextDestination[2] = interInner23;
								NextDestination[3] = target.Center;
								Initialize();
								return;
							}
							else if (innerPath22 && innerPath24)
							{
								NextDestination[0] = interInner22;
								NextDestination[1] = inter2;
								NextDestination[2] = interInner24;
								NextDestination[3] = target.Center;
								Initialize();
								return;
							}
						}
					}
				}
			}
			else
			{
				//location = 255;
			}
		}

		private bool StraightPathToTarget()
        {
			NPC target = Main.npc[Target];

			bool straightPath;

			straightPath = Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, target.Center, Projectile.width, Projectile.height);

			return straightPath;
		}

		private bool StraightPathToTarget(Vector2 start)
		{
			NPC target = Main.npc[Target];

			bool straightPath;

			straightPath = Collision.CanHit(start, Projectile.width, Projectile.height, target.Center, Projectile.width, Projectile.height);

			return straightPath;
		}

		private bool StraightPathTo(Vector2 finish)
		{
			bool straightPath;

			straightPath = Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, finish, Projectile.width, Projectile.height);

			return straightPath;
		}

		private bool StraightPathTo(Vector2 start, Vector2 finish)
        {
			bool straightPath;

			straightPath = Collision.CanHit(start, Projectile.width, Projectile.height, finish, Projectile.width, Projectile.height);

			return straightPath;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			hit = true;
			Projectile.netUpdate = true;
			return true;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            this.hit = true;
        }

		public override bool PreDraw(ref Color lightColor)
		{
			bool longMode = true;

			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

			if (longMode)
			{
				int max = Projectile.oldPos.Length * 6 - 5;
				for (int i = 0; i < max; i++)
				{
					int index = i / 6;
					int nextIndex = (i + 6) / 6;

					Vector2 drawPos;
					Vector2 interPos;
					Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - index / 2) / (float)Projectile.oldPos.Length);
					color *= Main.rand.NextFloat(0.5f, 1.0f);
					float scale = (max - i) / max;
					scale += Main.rand.NextFloat(-0.2f, 0.2f);
					scale = MathHelper.Clamp(scale, 0.5f, Projectile.scale);

					switch (i % 6)
					{
						case 0:
							drawPos = (Projectile.oldPos[index] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
							Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, scale, SpriteEffects.None, 0);
							break;
						case 1:
							interPos = (Projectile.oldPos[index] * 5 + Projectile.oldPos[nextIndex]) / 6;
							drawPos = (interPos - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
							Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, scale, SpriteEffects.None, 0);
							break;
						case 2:
							interPos = (Projectile.oldPos[index] * 4 + Projectile.oldPos[nextIndex] * 2) / 6;
							drawPos = (interPos - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
							Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, scale, SpriteEffects.None, 0);
							break;
						case 3:
							interPos = (Projectile.oldPos[index] * 3 + Projectile.oldPos[nextIndex] * 3) / 6;
							drawPos = (interPos - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
							Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, scale, SpriteEffects.None, 0);
							break;
						case 4:
							interPos = (Projectile.oldPos[index] * 2 + Projectile.oldPos[nextIndex] * 4) / 6;
							drawPos = (interPos - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
							Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, scale, SpriteEffects.None, 0);
							break;
						case 5:
							interPos = (Projectile.oldPos[index] + Projectile.oldPos[nextIndex] * 5) / 6;
							drawPos = (interPos - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
							Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, scale, SpriteEffects.None, 0);
							break;
					}
				}
			}
			else
			{
				int max = Projectile.oldPos.Length * 4 - 3;
				for (int i = 0; i < max; i++)
				{
					int index = i / 4;
					int nextIndex = (i + 4) / 4;

					Vector2 drawPos;
					Vector2 interPos;
					Color color;
					float scale = (max - i / 4) / max;
					scale *= Projectile.scale;
					scale = MathHelper.Max(scale, 0.5f);

					switch (i % 4)
					{
						case 0:
							drawPos = (Projectile.oldPos[index] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
							color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - index) / (float)Projectile.oldPos.Length);
							Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, scale, SpriteEffects.None, 0);
							break;
						case 1:
							interPos = (Projectile.oldPos[index] * 3 + Projectile.oldPos[nextIndex]) / 4;
							drawPos = (interPos - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
							color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - index) / (float)Projectile.oldPos.Length);
							Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, scale, SpriteEffects.None, 0);
							break;
						case 2:
							interPos = (Projectile.oldPos[index] + Projectile.oldPos[nextIndex]) / 2;
							drawPos = (interPos - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
							color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - index) / (float)Projectile.oldPos.Length);
							Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, scale, SpriteEffects.None, 0);
							break;
						case 3:
							interPos = (Projectile.oldPos[index] + Projectile.oldPos[nextIndex] * 3) / 4;
							drawPos = (interPos - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
							color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - index) / (float)Projectile.oldPos.Length);
							Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, scale, SpriteEffects.None, 0);
							break;
					}

				}
			}
			return true;
		}

		private int Target
		{
			get => (int)Projectile.ai[0] - 1;
			set => Projectile.ai[0] = value + 1;
		}

		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
		}

		private void ChangeDirection(Vector2 newVelocity)
		{
			Projectile.velocity = newVelocity * 1.1f;
			Vector2 offset1 = -newVelocity * 0.33f;
			Vector2 offset2 = offset1.RotatedByRandom(MathHelper.PiOver4);
			Vector2 offset3 = offset1.RotatedByRandom(MathHelper.PiOver4);
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch, SpeedX: offset1.X, SpeedY: offset1.Y);
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch, SpeedX: offset2.X, SpeedY: offset2.Y);
			Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch, SpeedX: offset3.X, SpeedY: offset3.Y);
		}

	}
}