using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;

namespace Highlander.Sounds.Custom
{
    public class HighlanderSounds
    {
        public static readonly SoundStyle ThunderSound = new($"{nameof(Highlander)}/Sounds/Custom/Thunder", 1)
        {
            Volume = 0.12f,
            Pitch = 0.2f,
            PitchVariance = 0.1f
        };
    }
}
