using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highlander.Utilities
{
    public static class SlopeID
    {
        /// <summary>
        /// A full tile.
        /// </summary>
        public static byte Solid = 0;
        /// <summary>
        /// A tile with the bottom left solid.
        /// </summary>
        public static byte DownLeft = 1;
        /// <summary>
        /// A tile with the bottom right solid.
        /// </summary>
        public static byte DownRight = 2;
        /// <summary>
        /// A tile with the top left solid.
        /// </summary>
        public static byte UpLeft = 3;
        /// <summary>
        /// A tile with the top right solid.
        /// </summary>
        public static byte UpRight = 4;

    }
}
