using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.NPCs.EnlightenmentIdol
{
    class ArmAttack : ModProjectile
    {

		private byte timer = 0;
		private BitsByte flags;
		private float offset;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 14;
        }

		public override void SetDefaults()
		{
			projectile.width = 64;
			projectile.height = 64;
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;
			projectile.penetrate = -1;
			projectile.scale = 1f;
			projectile.alpha = 0;
			drawOffsetX = -210;
		}

		public override void AI()
		{
			if (!flags[0])
			{
				flags[0] = true;
				projectile.rotation = projectile.ai[0];
				projectile.netUpdate = true;
			}

			projectile.spriteDirection = projectile.direction;

			if (projectile.spriteDirection == 1) {
				//projectile.rotation = projectile.velocity.ToRotation();// + MathHelper.Pi;
			}
			else
			{
				//projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
			}
			GetFrame();
			timer++;
		}

		private void GetFrame()
		{
			int newFrame = 0;

			if(timer < 3) // 1
			{
				newFrame = 0;
				projectile.ai[1] = 0;
			}else if (timer < 6) // 2
			{
				newFrame = 1;
				projectile.ai[1] = 0;
			}
			else if (timer < 9) // 3
			{
				newFrame = 2;
				projectile.ai[1] = 0;
			}
			else if (timer < 12) // 4
			{
				newFrame = 3;
				projectile.ai[1] = 0;
			}
			else if (timer < 28) // 5
			{
				newFrame = 4;
				projectile.ai[1] = 0;
			}
			else if (timer < 31) // 6
			{
				newFrame = 5;
				projectile.ai[1] = 0;
			}
			else if (timer < 34) // 7
			{
				newFrame = 6;
				projectile.ai[1] = 20;
			}
			else if (timer < 37) // 8
			{
				newFrame = 7;
				projectile.ai[1] = 28;
			}
			else if (timer < 40) // 9
			{
				newFrame = 8;
				projectile.ai[1] = 38;
			}
			else if (timer < 43) // 10
			{
				newFrame = 9;
				projectile.ai[1] = 42;
			}
			else if (timer < 46) // 11
			{
				newFrame = 10;
				projectile.ai[1] = 32;
			}
			else if (timer < 52) // 12
			{
				newFrame = 11;
				projectile.ai[1] = 48;
			}
			else if (timer < 58) // 13
			{
				newFrame = 12;
				projectile.ai[1] = 4;
			}
			else if (timer < 61) // 14
			{
				newFrame = 13;
				projectile.ai[1] = -2;
			}
			else if (timer < 64) // 15
			{
				newFrame = 12;
				projectile.ai[1] = 2;
			}
			else if (timer < 70) // 16
			{
				newFrame = 11;
				projectile.ai[1] = -4;
			}
			else if (timer < 73) // 17
			{
				newFrame = 10;
				projectile.ai[1] = -48;
			}
			else if (timer < 75) // 18
			{
				newFrame = 9;
				projectile.ai[1] = -32;
			}
			else if (timer < 77) // 19
			{
				newFrame = 8;
				projectile.ai[1] = -42;
			}
			else if (timer < 79) // 20
			{
				newFrame = 7;
				projectile.ai[1] = -38;
			}
			else if (timer < 81) // 21
			{
				newFrame = 6;
				projectile.ai[1] = -28;
			}
			else if (timer < 83) // 22
			{
				newFrame = 5;
				projectile.ai[1] = -20;
			}
			else if (timer < 91) // 23
			{
				newFrame = 4;
				projectile.ai[1] = 0;
			}
			else if (timer < 94) // 24
			{
				newFrame = 3;
				projectile.ai[1] = 0;
			}
			else if (timer < 97) // 25
			{
				newFrame = 2;
				projectile.ai[1] = 0;
			}
			else if (timer < 101) // 26
			{
				newFrame = 1;
				projectile.ai[1] = 0;
			}
			else if (timer < 105) // 27
			{
				newFrame = 0;
				projectile.ai[1] = 0;
			}
			else
			{
				projectile.Kill();
			}
			if(newFrame != projectile.frame)
			{
				projectile.frame = newFrame;
				//projectile.position += forward * projectile.ai[1];

				float offsetX = projectile.ai[1] * (float) Math.Cos(projectile.rotation);

				//drawOffsetX += (int) (offsetX);
				//offset += offsetX - (int) offsetX;
				if (Math.Abs(offset) >= 1)
				{
					drawOffsetX += (int)offset;
					offset -= (int)offset;
				}

				//drawOriginOffsetY += (int)(projectile.ai[1] * Math.Sin(projectile.rotation));
				projectile.netUpdate = true;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((float)(255 - projectile.alpha) / 255f);
		}

		private Vector2 forward
		{
			get
			{
				float rotation = projectile.rotation - MathHelper.Pi;
				Vector2 output = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
				output.Normalize();
				return output;
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(timer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			timer = reader.ReadByte();
		}

	}
}
