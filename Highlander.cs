using Highlander.Items;
using Highlander.Items.Armor;
using Highlander.UnusualLayerEffects;
using Highlander.Utilities;
using Microsoft.Xna.Framework;
using System;
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
			AnimationHelper.dust = new List<FauxDust>();
		}

		public override void Load()
		{
			AbnormalEffect effect = 0;

			AbnormalItem item = new PithyProfessional(effect);
			AddItem("UnusualPithyProfessional", item);
			AddEquipTexture(item, EquipType.Head, "UnusualPithyProfessional_Head", "Highlander/Items/Armor/PithyProfessional_Head");

			item = new LegendaryLid(effect);
			AddItem("UnusualLegendaryLid", item);
			AddEquipTexture(item, EquipType.Head, "UnusualLegendaryLid_Head", "Highlander/Items/Armor/LegendaryLid_Head");

			item = new BrassBucket(effect);
			AddItem("UnusualBrassBucket", item);
			AddEquipTexture(item, EquipType.Head, "UnusualBrassBucket_Head", "Highlander/Items/Armor/BrassBucket_Head");

			item = new TeamCaptain(effect);
			AddItem("UnusualTeamCaptain", item);
			AddEquipTexture(item, EquipType.Head, "UnusualTeamCaptain_Head", "Highlander/Items/Armor/TeamCaptain_Head");

			item = new StainlessPot(effect);
			AddItem("UnusualStainlessPot", item);
			AddEquipTexture(item, EquipType.Head, "UnusualStainlessPot_Head", "Highlander/Items/Armor/StainlessPot_Head");

			item = new Hotrod(effect);
			AddItem("UnusualHotrod", item);
			AddEquipTexture(item, EquipType.Head, "UnusualHotrod_Head", "Highlander/Items/Armor/Hotrod_Head");

			item = new StoutShako(effect);
			AddItem("UnusualStoutShako", item);
			AddEquipTexture(item, EquipType.Head, "UnusualStoutShako_Head", "Highlander/Items/Armor/StoutShako_Head");

			item = new SamurEye(effect);
			AddItem("UnusualSamurEye", item);
			AddEquipTexture(item, EquipType.Head, "UnusualSamurEye_Head", "Highlander/Items/Armor/SamurEye_Head");

			item = new OlSnaggletooth(effect);
			AddItem("UnusualOlSnaggletooth", item);
			AddEquipTexture(item, EquipType.Head, "UnusualOlSnaggletooth_Head", "Highlander/Items/Armor/OlSnaggletooth_Head");

			item = new PyromancerMask(effect);
			AddItem("UnusualPyromancerMask", item);
			AddEquipTexture(item, EquipType.Head, "UnusualPyromancerMask_Head", "Highlander/Items/Armor/PyromancerMask_Head");

			item = new HongKongCone(effect);
			AddItem("UnusualHongKongCone", item);
			AddEquipTexture(item, EquipType.Head, "UnusualHongKongCone_Head", "Highlander/Items/Armor/HongKongCone_Head");

			item = new KillerExclusive(effect);
			AddItem("UnusualKillerExclusive", item);
			AddEquipTexture(item, EquipType.Head, "UnusualKillerExclusive_Head", "Highlander/Items/Armor/KillerExclusive_Head");

			item = new TartanTyrolean(effect);
			AddItem("UnusualTartanTyrolean", item);
			AddEquipTexture(item, EquipType.Head, "UnusualTartanTyrolean_Head", "Highlander/Items/Armor/TartanTyrolean_Head");

			item = new ColdfrontCommander(effect);
			AddItem("UnusualColdfrontCommander", item);
			AddEquipTexture(item, EquipType.Head, "UnusualColdfrontCommander_Head", "Highlander/Items/Armor/ColdfrontCommander_Head");

			item = new SinnerShade(effect);
			AddItem("UnusualSinnerShade", item);
			AddEquipTexture(item, EquipType.Head, "UnusualSinnerShade_Head", "Highlander/Items/Armor/SinnerShade_Head");

			item = new MightyMitre(effect);
			AddItem("UnusualMightyMitre", item);
			AddEquipTexture(item, EquipType.Head, "UnusualMightyMitre_Head", "Highlander/Items/Armor/MightyMitre_Head");

			item = new CondorCap(effect);
			AddItem("UnusualCondorCap", item);
			AddEquipTexture(item, EquipType.Head, "UnusualCondorCap_Head", "Highlander/Items/Armor/CondorCap_Head");

			item = new SurgeonShako(effect);
			AddItem("UnusualSurgeonShako", item);
			AddEquipTexture(item, EquipType.Head, "UnusualSurgeonShako_Head", "Highlander/Items/Armor/SurgeonShako_Head");

			item = new ToySoldier(effect);
			AddItem("UnusualToySoldier", item);
			AddEquipTexture(item, EquipType.Head, "UnusualToySoldier_Head", "Highlander/Items/Armor/ToySoldier_Head");

			item = new PatriotPeak(effect);
			AddItem("UnusualPatriotPeak", item);
			AddEquipTexture(item, EquipType.Head, "UnusualPatriotPeak_Head", "Highlander/Items/Armor/PatriotPeak_Head");

			item = new Globetrotter(effect);
			AddItem("UnusualGlobetrotter", item);
			AddEquipTexture(item, EquipType.Head, "UnusualGlobetrotter_Head", "Highlander/Items/Armor/Globetrotter_Head");

			item = new Anger(effect);
			AddItem("UnusualAnger", item);
			AddEquipTexture(item, EquipType.Head, "UnusualAnger_Head", "Highlander/Items/Armor/Anger_Head");

			// OG Hats

			item = new PaperBag(effect);
			AddItem("UnusualPaperBag", item);
			AddEquipTexture(item, EquipType.Head, "UnusualPaperBag_Head", "Highlander/Items/Armor/PaperBag_Head");

			/**for (int i = 1; i < (int)AbnormalEffect.Max; i++)
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
			}**/

		}

		public override void Unload()
		{
			RollTable.AbnormalRollTable = null;
			AnimationHelper.dust = null;
			Instance = null;
		}

		public override void UpdateUI(GameTime gameTime)
		{
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{

		}

		public override void PostSetupContent()
		{
			// Boss Checklist Support
			Mod bossChecklist = ModLoader.GetMod("BossChecklist");
			if (bossChecklist != null)
			{
				bossChecklist.Call(
					"AddBoss",
					4.99f,
					ModContent.NPCType<NPCs.HauntedHatter.HauntedHatter>(),
					this, // Mod
					"Haunted Hatter",
					(Func<bool>)(() => HighlanderWorld.downedHauntedHatter),
					ModContent.ItemType<Items.HauntedHatter.SpookyHeadwear>(),
					new List<int> { ModContent.ItemType<Items.HauntedHatter.HauntedHatterTrophy>(), ModContent.ItemType<Items.HauntedHatter.GhostlyGibus>() },
					new List<int> { ModContent.ItemType<Items.HauntedHatter.HauntedHatterBag>(), ModContent.ItemType<Items.HauntedHatter.EnchantedNeedleHook>()
					, ModContent.ItemType<Items.HauntedHatter.SpiritShears>(), ModContent.ItemType<Items.Weapons.AncientStoneBlaster>()},
					$"Spawns after using [i:{ModContent.ItemType<Items.HauntedHatter.SpookyHeadwear>()}] which can be crafted or dropped once after defeating the Eater of Worlds or the Brain of Cthulu.",
					"Haunted Hatter spirited away the challengers");
				// Additional bosses here
			}

			// Census Support
			Mod censusMod = ModLoader.GetMod("Census");
			if (censusMod != null)
			{
				// Here I am using Chat Tags to make my condition even more interesting.
				// If you localize your mod, pass in a localized string instead of just English.
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.HatSalesman.HatSalesman>(),
					$"When the Haunted Hatter has been defeated");
				censusMod.Call("TownNPCCondition", ModContent.NPCType<NPCs.VeteranExplorer.VeteranExplorer>(),
					$"When Skeletron has been defeated");
			}
		}

		internal static Highlander Instance { get; private set; }

	}
}