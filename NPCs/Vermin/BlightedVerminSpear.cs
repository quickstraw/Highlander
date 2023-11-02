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
			//DisplayName.SetDefault("Blighted Vermin's Spear");
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 18;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 20;
			Projectile.penetrate = -1;
			Projectile.scale = 0.8f;
			Projectile.alpha = 255;
		}

	}
}
