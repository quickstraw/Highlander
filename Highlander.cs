using Highlander.Common.Players;
using Highlander.Common.Systems;
using Highlander.Items;
using Highlander.Items.Armor;
using Highlander.Items.Armor.Halloween;
using Highlander.Items.Armor.VanityHats;
using Highlander.Items.Armor.VanityHats.Series2;
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
			Instance = this;
			RollTable.MakeTable();
			UnusualLayer.dust = new List<FauxDust>();
			FauxDust.mod = this;
		}

		public override void Load()
		{
			AbnormalEffect effect = AbnormalEffect.Unknown;

			// Series 1
			AbnormalItem item = new Anger(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/Anger_Head");

			item = new HongKongCone(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/HongKongCone_Head");

			item = new Hotrod(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/Hotrod_Head");

			item = new KillerExclusive(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/KillerExclusive_Head");

			item = new LegendaryLid(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/LegendaryLid_Head");

			item = new OlSnaggletooth(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/OlSnaggletooth_Head");

			item = new PithyProfessional(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/PithyProfessional_Head");

			item = new PyromancerMask(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/PyromancerMask_Head");

			item = new SamurEye(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/SamurEye_Head");

			item = new StainlessPot(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/StainlessPot_Head");

			item = new StoutShako(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/StoutShako_Head");

			item = new TeamCaptain(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series1/TeamCaptain_Head");

			// Series 2
			item = new DeadCone(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series2/DeadCone_Head");

			item = new FruitShoot(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series2/FruitShoot_Head");

			item = new FuriousFukaamigasa(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series2/FuriousFukaamigasa_Head");

			item = new HeroHachimaki(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series2/HeroHachimaki_Head");

			item = new HotDogger(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series2/HotDogger_Head");

			item = new JanissaryKetche(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series2/JanissaryKetche_Head");

			item = new Law(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series2/Law_Head");

			item = new MediMask(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series2/MediMask_Head");

			item = new SurgeonStahlhelm(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series2/SurgeonStahlhelm_Head");

			item = new ToughStuffMuffs(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Series2/ToughStuffMuffs_Head");

			// Winter
			item = new BrassBucket(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Winter/BrassBucket_Head");

			item = new ColdfrontCommander(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Winter/ColdfrontCommander_Head");

			item = new CondorCap(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Winter/CondorCap_Head");

			item = new Globetrotter(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Winter/Globetrotter_Head");

			item = new MightyMitre(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Winter/MightyMitre_Head");

			item = new PatriotPeak(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Winter/PatriotPeak_Head");

			item = new SinnerShade(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Winter/SinnerShade_Head");

			item = new SurgeonShako(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Winter/SurgeonShako_Head");

			item = new TartanTyrolean(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Winter/TartanTyrolean_Head");

			item = new ToySoldier(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Winter/ToySoldier_Head");

			// Halloween
			item = new CroneDome(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Halloween/CroneDome_Head");

			item = new Executioner(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Halloween/Executioner_Head");

			item = new Hellmet(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Halloween/Hellmet_Head");

			item = new InfernalImpaler(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Halloween/InfernalImpaler_Head");

			item = new MacabreMask(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Halloween/MacabreMask_Head");

			item = new OneWayTicket(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Halloween/OneWayTicket_Head");

			item = new SearedSorcerer(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Halloween/SearedSorcerer_Head");

			item = new SirPumpkinton(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Halloween/SirPumpkinton_Head");

			// Orignal Hats
			item = new AutonomousOrb(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/AutonomousOrb_Head");

			item = new BloodWarriorMask(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/BloodWarriorMask_Head");

			item = new GuerrillaRebel(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/GuerrillaRebel_Head");

			item = new Headless(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/Headless_Head");

			item = new ImpregnableHelm(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/ImpregnableHelm_Head");

			item = new MedicalMask(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/MedicalMask_Head");

			item = new NinjaHeadband(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/NinjaHeadband_Head");

			item = new OpenMind(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/OpenMind_Head");

			item = new PaperBag(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/PaperBag_Head");

			item = new SkiMask(effect);
			AddContent(item);
			AddEquipTexture(item, EquipType.Head, "Highlander/Items/Armor/VanityHats/SkiMask_Head");

		}

		public override void Unload()
		{
			RollTable.AbnormalRollTable = null;
			UnusualLayer.dust = null;
			FauxDust.mod = null;
			Instance = null;
		}

		private void AddEquipTexture(ModItem item, EquipType type, string texpath)
        {
			EquipLoader.AddEquipTexture(Instance, texpath, type, item);
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

		public override void PostSetupContent()
		{
			// Boss Checklist Support
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
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
			if (ModLoader.TryGetMod("Census", out Mod censusMod))
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

		internal enum HighlanderMessageType : byte
		{
			HighlanderPlayerSyncPlayer,
		}

	}
}