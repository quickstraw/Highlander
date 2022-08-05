using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Highlander.Projectiles.Bullets
{
	public class RicochetingBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ricocheting Bullet");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;        //The recording mode
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;               //The width of projectile hitbox
			Projectile.height = 8;              //The height of projectile hitbox
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
			Projectile.netUpdate = true;
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = Projectile.damage;
				Projectile.netUpdate = true;
			}
			if (Projectile.damage > Projectile.ai[0] * 0.5f)
			{
				Projectile.damage = (int)(Projectile.damage * 0.80f + 1);
				Projectile.netUpdate = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int i = 0; i < Projectile.oldPos.Length * 2 - 1; i++)
			{
				int index = i / 2;
				int nextIndex = (i + 2) / 2;
				if (i % 2 == 0)
                {
					Vector2 drawPos = (Projectile.oldPos[index] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - index / 2) / (float)Projectile.oldPos.Length);
					Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
				}
                else
                {
					Vector2 interPos = (Projectile.oldPos[index] + Projectile.oldPos[nextIndex]) / 2;
					Vector2 drawPos = (interPos - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - index / 2) / (float)Projectile.oldPos.Length);
					Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[index], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
				}
			}

			return true;
		}

		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}

	}
}