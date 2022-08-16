using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Highlander.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Highlander.Projectiles.Equipment
{
	class FlareProjectile : ModProjectile
	{
		private const int dimTime = 3000;

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.netImportant = true;
			Projectile.aiStyle = 14;
			Projectile.penetrate = -1;
			Projectile.alpha = 75;
			Projectile.timeLeft = 900 + dimTime;
			Projectile.light = 1;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

        public override void PostAI()
        {
			Projectile.ai[0] = 6;
			
			if(Projectile.timeLeft <= dimTime && Projectile.light >= 0.2f)
            {
				Projectile.light -= 1f / 60f;
            }
			if(Projectile.oldRot.Length > 0)
            {
				Projectile.rotation = (Projectile.rotation + Projectile.oldRot[0] * 2) / 3;
			}
			if(Projectile.ai[1] > 0 && Main.netMode != NetmodeID.MultiplayerClient)
            {
				Projectile.netUpdate = true;
				Projectile.ai[1] = 0;
				Projectile.velocity *= 0.95f;
				if (Projectile.velocity.Length() < 1)
				{
					Projectile.velocity *= 0.5f;
				}
			}
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
            {
				Projectile.netUpdate = true;
				Projectile.ai[1] = 1;
			}
			return true;
		}

	}
}
