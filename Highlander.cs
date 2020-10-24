using Highlander.Items;
using Highlander.Items.Armor;
using Highlander.Items.Armor.Halloween;
using Highlander.Items.Armor.Series2;
using Highlander.Items.SeaDog;
using Highlander.UnusualLayerEffects;
using Highlander.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
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
			FauxDust.mod = this;
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

			// Series 2

			item = new DeadCone(effect);
			AddItem("UnusualDeadCone", item);
			AddEquipTexture(item, EquipType.Head, "UnusualDeadCone_Head", "Highlander/Items/Armor/Series2/DeadCone_Head");

			item = new FruitShoot(effect);
			AddItem("UnusualFruitShoot", item);
			AddEquipTexture(item, EquipType.Head, "UnusualFruitShoot_Head", "Highlander/Items/Armor/Series2/FruitShoot_Head");

			item = new FuriousFukaamigasa(effect);
			AddItem("UnusualFuriousFukaamigasa", item);
			AddEquipTexture(item, EquipType.Head, "UnusualFuriousFukaamigasa_Head", "Highlander/Items/Armor/Series2/FuriousFukaamigasa_Head");

			item = new HeroHachimaki(effect);
			AddItem("UnusualHeroHachimaki", item);
			AddEquipTexture(item, EquipType.Head, "UnusualHeroHachimaki_Head", "Highlander/Items/Armor/Series2/HeroHachimaki_Head");

			item = new HotDogger(effect);
			AddItem("UnusualHotDogger", item);
			AddEquipTexture(item, EquipType.Head, "UnusualHotDogger_Head", "Highlander/Items/Armor/Series2/HotDogger_Head");

			item = new JanissaryKetche(effect);
			AddItem("UnusualJanissaryKetche", item);
			AddEquipTexture(item, EquipType.Head, "UnusualJanissaryKetche_Head", "Highlander/Items/Armor/Series2/JanissaryKetche_Head");

			item = new Law(effect);
			AddItem("UnusualLaw", item);
			AddEquipTexture(item, EquipType.Head, "UnusualLaw_Head", "Highlander/Items/Armor/Series2/Law_Head");

			item = new MediMask(effect);
			AddItem("UnusualMediMask", item);
			AddEquipTexture(item, EquipType.Head, "UnusualMediMask_Head", "Highlander/Items/Armor/Series2/MediMask_Head");

			item = new SurgeonStahlhelm(effect);
			AddItem("UnusualSurgeonStahlhelm", item);
			AddEquipTexture(item, EquipType.Head, "UnusualSurgeonStahlhelm_Head", "Highlander/Items/Armor/Series2/SurgeonStahlhelm_Head");

			item = new ToughStuffMuffs(effect);
			AddItem("UnusualToughStuffMuffs", item);
			AddEquipTexture(item, EquipType.Head, "UnusualToughStuffMuffs_Head", "Highlander/Items/Armor/Series2/ToughStuffMuffs_Head");

			// Halloween Hats

			item = new CroneDome(effect);
			AddItem("UnusualCroneDome", item);
			AddEquipTexture(item, EquipType.Head, "UnusualCroneDome_Head", "Highlander/Items/Armor/Halloween/CroneDome_Head");

			item = new Executioner(effect);
			AddItem("UnusualExecutioner", item);
			AddEquipTexture(item, EquipType.Head, "UnusualExecutioner_Head", "Highlander/Items/Armor/Halloween/Executioner_Head");

			item = new Hellmet(effect);
			AddItem("UnusualHellmet", item);
			AddEquipTexture(item, EquipType.Head, "UnusualHellmet_Head", "Highlander/Items/Armor/Halloween/Hellmet_Head");

			item = new InfernalImpaler(effect);
			AddItem("UnusualInfernalImpaler", item);
			AddEquipTexture(item, EquipType.Head, "UnusualInfernalImpaler_Head", "Highlander/Items/Armor/Halloween/InfernalImpaler_Head");

			item = new MacabreMask(effect);
			AddItem("UnusualMacabreMask", item);
			AddEquipTexture(item, EquipType.Head, "UnusualMacabreMask_Head", "Highlander/Items/Armor/Halloween/MacabreMask_Head");

			item = new OneWayTicket(effect);
			AddItem("UnusualOneWayTicket", item);
			AddEquipTexture(item, EquipType.Head, "UnusualOneWayTicket_Head", "Highlander/Items/Armor/Halloween/OneWayTicket_Head");

			item = new SearedSorcerer(effect);
			AddItem("UnusualSearedSorcerer", item);
			AddEquipTexture(item, EquipType.Head, "UnusualSearedSorcerer_Head", "Highlander/Items/Armor/Halloween/SearedSorcerer_Head");

			item = new SirPumpkinton(effect);
			AddItem("UnusualSirPumpkinton", item);
			AddEquipTexture(item, EquipType.Head, "UnusualSirPumpkinton_Head", "Highlander/Items/Armor/Halloween/SirPumpkinton_Head");

			// OG Hats

			item = new PaperBag(effect);
			AddItem("UnusualPaperBag", item);
			AddEquipTexture(item, EquipType.Head, "UnusualPaperBag_Head", "Highlander/Items/Armor/PaperBag_Head");

			item = new OpenMind(effect);
			AddItem("UnusualOpenMind", item);
			AddEquipTexture(item, EquipType.Head, "UnusualOpenMind_Head", "Highlander/Items/Armor/OpenMind_Head");

			item = new Headless(effect);
			AddItem("UnusualHeadless", item);
			AddEquipTexture(item, EquipType.Head, "UnusualHeadless_Head", "Highlander/Items/Armor/Headless_Head");

			item = new GuerrillaRebel(effect);
			AddItem("UnusualGuerrillaRebel", item);
			AddEquipTexture(item, EquipType.Head, "UnusualGuerrillaRebel_Head", "Highlander/Items/Armor/GuerrillaRebel_Head");

			item = new SkiMask(effect);
			AddItem("UnusualSkiMask", item);
			AddEquipTexture(item, EquipType.Head, "UnusualSkiMask_Head", "Highlander/Items/Armor/SkiMask_Head");

			item = new ImpregnableHelm(effect);
			AddItem("UnusualImpregnableHelm", item);
			AddEquipTexture(item, EquipType.Head, "UnusualImpregnableHelm_Head", "Highlander/Items/Armor/ImpregnableHelm_Head");

			item = new NinjaHeadband(effect);
			AddItem("UnusualNinjaHeadband", item);
			AddEquipTexture(item, EquipType.Head, "UnusualNinjaHeadband_Head", "Highlander/Items/Armor/NinjaHeadband_Head");

			item = new AutonomousOrb(effect);
			AddItem("UnusualAutonomousOrb", item);
			AddEquipTexture(item, EquipType.Head, "UnusualAutonomousOrb_Head", "Highlander/Items/Armor/AutonomousOrb_Head");

			item = new MedicalMask(effect);
			AddItem("UnusualMedicalMask", item);
			AddEquipTexture(item, EquipType.Head, "UnusualMedicalMask_Head", "Highlander/Items/Armor/MedicalMask_Head");

			item = new BloodWarriorMask(effect);
			AddItem("UnusualBloodWarriorMask", item);
			AddEquipTexture(item, EquipType.Head, "UnusualBloodWarriorMask_Head", "Highlander/Items/Armor/BloodWarriorMask_Head");

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
			FauxDust.mod = null;
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
					$"Use [i:{ModContent.ItemType<Items.HauntedHatter.SpookyHeadwear>()}] which can be crafted or dropped once after defeating the Eater of Worlds or the Brain of Cthulu.",
					"Haunted Hatter spirited away the challengers");
				// Additional bosses here
				bossChecklist.Call(
					"AddBoss",
					9.1f,
					ModContent.NPCType<NPCs.EnlightenmentIdol.EnlightenmentIdol>(),
					this, // Mod
					"Idol of Enlightenment",
					(Func<bool>)(() => HighlanderWorld.downedEnlightenmentIdol),
					ModContent.ItemType<Items.EnlightenmentIdol.StoneIdol>(),
					new List<int> { ModContent.ItemType<Items.EnlightenmentIdol.EnlightenmentIdolTrophy>(), ModContent.ItemType<Items.EnlightenmentIdol.EnlightenedMask>() },
					new List<int> { ModContent.ItemType<Items.EnlightenmentIdol.EnlightenmentIdolBag>(), ModContent.ItemType<Items.EnlightenmentIdol.DivinePresence>()
					, ModContent.ItemType<Items.EnlightenmentIdol.BlitzFist>(), ModContent.ItemType<Items.EnlightenmentIdol.CommanderBlessing>()},
					$"Use [i:{ModContent.ItemType<Items.EnlightenmentIdol.StoneIdol>()}] which can be crafted with [i:{ItemID.SoulofLight}] and [i:{ItemID.StoneBlock}].",
					"Idol of Enlightenment helped the challengers pass on",
					"Highlander/NPCs/EnlightenmentIdol/EnlightenmentIdolBossChecklist");

				bossChecklist.Call(
					"AddBoss",
					0.8f,
					ModContent.NPCType<NPCs.SeaDog.SeaDog>(),
					this, // Mod
					"Sea Dog",
					(Func<bool>)(() => HighlanderWorld.downedSeaDog),
					ModContent.ItemType<Items.SeaDog.GlowingTreasure>(),
					new List<int> { ModContent.ItemType<Items.SeaDog.SeaDogTrophy>(), ModContent.ItemType<Items.SeaDog.SeaDogMask>() },
					new List<int> { ModContent.ItemType<Items.SeaDog.SeaDogBag>(), ModContent.ItemType<BarnacleBarrier>(),
						ModContent.ItemType<FeralFrenzy>(), ModContent.ItemType<BrokenBlunderbuss>(), ItemID.GoldOre, ItemID.SpelunkerPotion, ItemID.GillsPotion},
					$"Use [i:{ModContent.ItemType<Items.SeaDog.GlowingTreasure>()}] at a beach.",
					"Sea Dog devoured the challengers");
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

		public override void PostUpdateEverything()
		{
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			HighlanderMessageType msgType = (HighlanderMessageType)reader.ReadByte();
			switch (msgType)
			{
				case HighlanderMessageType.HighlanderPlayerSyncPlayer:
					byte playernumber = reader.ReadByte();
					HighlanderPlayer modPlayer = Main.player[playernumber].GetModPlayer<HighlanderPlayer>();
					int unboxed = reader.ReadInt32();
					modPlayer.unboxed = unboxed;
					// SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
					break;
				default:
					Logger.WarnFormat("Highlander: Unknown Message type: {0}", msgType);
					break;
			}
		}

		internal static Highlander Instance { get; private set; }

		internal enum HighlanderMessageType : byte
		{
			HighlanderPlayerSyncPlayer,
		}

	}
}