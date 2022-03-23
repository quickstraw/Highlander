using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Highlander.Gores
{
    class HauntedHatterTopGore : ModGore
    {

        public override void OnSpawn(Gore gore)
        {
            gore.numFrames = 1;
            gore.timeLeft = Gore.goreTime * 3;
            gore.alpha = 0;
        }

        public override bool Update(Gore gore)
        {
            if(gore.velocity.X > 0)
            {
                //gore.rotation += gore.velocity.Y / gore.velocity.Length();
            } else if(gore.velocity.X < 0)
            {
                //gore.rotation -= gore.velocity.Y / gore.velocity.Length();
            }
            return true;
        }

    }
}
