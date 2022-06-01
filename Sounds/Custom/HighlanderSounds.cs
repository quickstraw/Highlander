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
        public static readonly SoundStyle ThunderSound = new($"{nameof(Highlander)}/Sounds/Custom/Thunder", 3)
        {
            Volume = 0.12f,
            Pitch = 0.2f,
            PitchVariance = 0.1f
        };

        public static readonly SoundStyle UnusualOpenSound = new($"{nameof(Highlander)}/Sounds/Custom/UnusualOpen", 3)
        {
            Volume = 1.0f,
            Pitch = -1.0f,
            PitchVariance = 0.1f
        };

        public static readonly SoundStyle UnusualPop = new($"{nameof(Highlander)}/Sounds/Custom/UnusualPop", 3)
        {
            Volume = 0.7f,
            Pitch = -1.0f,
            PitchVariance = 0.1f
        };

        public static readonly SoundStyle UnusualPopVanilla = SoundID.Item14 with
        {
            Volume = 0.7f,
            Pitch = 1.0f,
            PitchVariance = 0.1f
        };
    }
}
