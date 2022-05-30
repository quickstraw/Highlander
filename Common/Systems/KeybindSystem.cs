using Highlander.Items.SeaDog;
using Highlander.Items.Weapons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Common.Systems
{
    class KeybindSystem : ModSystem
    {

		public static ModKeybind ActionKeybind { get; private set; }

		public override void Load()
		{
			// Registers a new keybind
			ActionKeybind = KeybindLoader.RegisterKeybind(Mod, "Action", "F");
		}

		// Please see ExampleMod.cs' Unload() method for a detailed explanation of the unloading process.
		public override void Unload()
		{
			// Not required if your AssemblyLoadContext is unloading properly, but nulling out static fields can help you figure out what's keeping it loaded.
			ActionKeybind = null;
		}

	}
}
