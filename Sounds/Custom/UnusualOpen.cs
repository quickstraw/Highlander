using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace Highlander.Sounds.Custom
{
    public class UnusualOpen : ModSound
    {
		public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan)
		{
			// By creating a new instance, this ModSound allows for overlapping sounds. Non-ModSound behavior is to restart the sound, only permitting 1 instance.
			soundInstance = Sound.Value.CreateInstance();
			soundInstance.Volume = volume * 1.0f;
			soundInstance.Pan = pan;
			soundInstance.Pitch = -1.0f;
			return soundInstance;
		}

	}
}
