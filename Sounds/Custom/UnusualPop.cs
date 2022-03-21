﻿using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace Highlander.Sounds.Custom
{
    public class UnusualPop : ModSound
    {
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan)
		{
			// By creating a new instance, this ModSound allows for overlapping sounds. Non-ModSound behavior is to restart the sound, only permitting 1 instance.
			Sound.Value.CreateInstance();
			soundInstance.Volume = volume * .7f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = -1.0f;
			return soundInstance;
		}

	}
}
