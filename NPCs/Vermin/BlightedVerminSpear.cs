using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.NPCs.Vermin
{
    class BlightedVerminSpear : ModProjectile
    {

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Cleansing Wave");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 18;
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 39;
			projectile.penetrate = 1;
			projectile.scale = 0.8f;
			projectile.alpha = 255;
		}

	}
}
