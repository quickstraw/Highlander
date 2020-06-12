using Highlander.Items.Armor;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Highlander
{
	public class Highlander : Mod
	{

		public Highlander()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			Instance = this;
			RollTable.MakeTable();
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
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				PyromancerMask item = new PyromancerMask(effect);
				AddItem(effect + "PyromancerMask", item);
				AddEquipTexture(item, EquipType.Head, "PyromancerMask" + effect + "_Head", "Highlander/Items/Armor/PyromancerMask_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				HongKongCone item = new HongKongCone(effect);
				AddItem(effect + "HongKongCone", item);
				AddEquipTexture(item, EquipType.Head, "HongKongCone" + effect + "_Head", "Highlander/Items/Armor/HongKongCone_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				KillerExclusive item = new KillerExclusive(effect);
				AddItem(effect + "KillerExclusive", item);
				AddEquipTexture(item, EquipType.Head, "KillerExclusive" + effect + "_Head", "Highlander/Items/Armor/KillerExclusive_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				BombBeanie item = new BombBeanie(effect);
				AddItem(effect + "BombBeanie", item);
				AddEquipTexture(item, EquipType.Head, "BombBeanie" + effect + "_Head", "Highlander/Items/Armor/BombBeanie_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				TartanTyrolean item = new TartanTyrolean(effect);
				AddItem(effect + "TartanTyrolean", item);
				AddEquipTexture(item, EquipType.Head, "TartanTyrolean" + effect + "_Head", "Highlander/Items/Armor/TartanTyrolean_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				ColdfrontCommander item = new ColdfrontCommander(effect);
				AddItem(effect + "ColdfrontCommander", item);
				AddEquipTexture(item, EquipType.Head, "ColdfrontCommander" + effect + "_Head", "Highlander/Items/Armor/ColdfrontCommander_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				SinnerShade item = new SinnerShade(effect);
				AddItem(effect + "SinnerShade", item);
				AddEquipTexture(item, EquipType.Head, "SinnerShade" + effect + "_Head", "Highlander/Items/Armor/SinnerShade_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				MightyMitre item = new MightyMitre(effect);
				AddItem(effect + "MightyMitre", item);
				AddEquipTexture(item, EquipType.Head, "MightyMitre" + effect + "_Head", "Highlander/Items/Armor/MightyMitre_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				CondorCap item = new CondorCap(effect);
				AddItem(effect + "CondorCap", item);
				AddEquipTexture(item, EquipType.Head, "CondorCap" + effect + "_Head", "Highlander/Items/Armor/CondorCap_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				SurgeonShako item = new SurgeonShako(effect);
				AddItem(effect + "SurgeonShako", item);
				AddEquipTexture(item, EquipType.Head, "SurgeonShako" + effect + "_Head", "Highlander/Items/Armor/SurgeonShako_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				ToySoldier item = new ToySoldier(effect);
				AddItem(effect + "ToySoldier", item);
				AddEquipTexture(item, EquipType.Head, "ToySoldier" + effect + "_Head", "Highlander/Items/Armor/ToySoldier_Head");
			}
			for (int i = 1; i < (int)AbnormalEffect.Max; i++)
			{
				AbnormalEffect effect = (AbnormalEffect)i;
				PatriotPeak item = new PatriotPeak(effect);
				AddItem(effect + "PatriotPeak", item);
				AddEquipTexture(item, EquipType.Head, "PatriotPeak" + effect + "_Head", "Highlander/Items/Armor/PatriotPeak_Head");
			}

		}

		public override void Unload()
		{
			RollTable.AbnormalRollTable = null;
			Instance = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{

		}

		internal static Highlander Instance { get; private set; }

	}
}