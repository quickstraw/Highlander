using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.NPCs.EnlightenmentIdol
{
    class ChargeDummyProjectile : ModProjectile
    {

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Cleansing Wave");
		}

		public override void SetDefaults()
		{
			Projectile.width = 54;
			Projectile.height = 54;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 3;
			Projectile.penetrate = 1;
			Projectile.scale = 0.8f;
			Projectile.alpha = 255;
		}

	}
}
