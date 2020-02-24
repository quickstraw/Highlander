using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Highlander
{
    class ModDustCustomData
    {
        public Player Player { get; set; }
        private BitsByte bools = new BitsByte();
        public Vector2 offset;
        public int timer { get; set; }
        public Object customData { get; set; }

        public ModDustCustomData(Player player)
        {
            Player = player;
            timer = 0;
        }

        public bool this[int index]
        {
            get
            {
                return bools[index];
            }

            set{
                bools[index] = value;
            }
        }







    }
}
