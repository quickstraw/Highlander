using Highlander.Items.Armor;
using Highlander.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Highlander
{
	public class Highlander : Mod
	{
		private UserInterface _exampleResourceBarUserInterface;
		internal ExampleResourceBar ExampleResourceBar;

		public Highlander()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			Instance = this;
		}

		public override void Load()
		{
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				PithyProfessional item = new PithyProfessional(effect);
				AddItem(effect + "PithyProfessional", item);
				AddEquipTexture(item, EquipType.Head, "PithyProfessional" + effect + "_Head", "Highlander/Items/Armor/PithyProfessional_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				LegendaryLid item = new LegendaryLid(effect);
				AddItem(effect + "LegendaryLid", item);
				AddEquipTexture(item, EquipType.Head, "LegendaryLid" + effect + "_Head", "Highlander/Items/Armor/LegendaryLid_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				BrassBucket item = new BrassBucket(effect);
				AddItem(effect + "BrassBucket", item);
				AddEquipTexture(item, EquipType.Head, "BrassBucket" + effect + "_Head", "Highlander/Items/Armor/BrassBucket_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				TeamCaptain item = new TeamCaptain(effect);
				AddItem(effect + "TeamCaptain", item);
				AddEquipTexture(item, EquipType.Head, "TeamCaptain" + effect + "_Head", "Highlander/Items/Armor/TeamCaptain_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				StainlessPot item = new StainlessPot(effect);
				AddItem(effect + "StainlessPot", item);
				AddEquipTexture(item, EquipType.Head, "StainlessPot" + effect + "_Head", "Highlander/Items/Armor/StainlessPot_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				Hotrod item = new Hotrod(effect);
				AddItem(effect + "Hotrod", item);
				AddEquipTexture(item, EquipType.Head, "Hotrod" + effect + "_Head", "Highlander/Items/Armor/Hotrod_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				StoutShako item = new StoutShako(effect);
				AddItem(effect + "StoutShako", item);
				AddEquipTexture(item, EquipType.Head, "StoutShako" + effect + "_Head", "Highlander/Items/Armor/StoutShako_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				SamurEye item = new SamurEye(effect);
				AddItem(effect + "SamurEye", item);
				AddEquipTexture(item, EquipType.Head, "SamurEye" + effect + "_Head", "Highlander/Items/Armor/SamurEye_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				OlSnaggletooth item = new OlSnaggletooth(effect);
				AddItem(effect + "OlSnaggletooth", item);
				AddEquipTexture(item, EquipType.Head, "OlSnaggletooth" + effect + "_Head", "Highlander/Items/Armor/OlSnaggletooth_Head");
			}

			if (!Main.dedServ)
			{
				ExampleResourceBar = new ExampleResourceBar();
				_exampleResourceBarUserInterface = new UserInterface();
				_exampleResourceBarUserInterface.SetState(ExampleResourceBar);
			}
		}

		public override void Unload()
		{

		}

		public override void UpdateUI(GameTime gameTime)
		{
			_exampleResourceBarUserInterface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{

			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1)
			{
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"Highlander: Ammo Counter",
					delegate {
						_exampleResourceBarUserInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}

		}

		internal static Highlander Instance { get; private set; }

	}
}