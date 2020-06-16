using Highlander.Items;
using Highlander.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;

namespace Highlander
{
	// ModPlayer classes provide a way to attach data to Players and act on that data. ExamplePlayer has a lot of functionality related to 
	// several effects and items in ExampleMod. See SimpleModPlayer for a very simple example of how ModPlayer classes work.
	public class HighlanderPlayer : ModPlayer
	{

		public bool holdingAmmoGun = false;
		public AbnormalEffect unusual = 0; 
		public short unusualLayerTime = 0;
		public short unusualLayerTime2 = 0;
		public int unusualFrame = 0;
		public int unusualFrame2 = 0;

		public int maxAmmo = 0;
		public int currentAmmo = 0;

		public Projectile bulletCounter;

		private int counter = 0;

		public override void ResetEffects() {
			holdingAmmoGun = false;
			unusual = 0; 
			maxAmmo = 0;
			currentAmmo = 0;
		}

		public override void OnEnterWorld(Player player) {
			// We can refresh UI using OnEnterWorld. OnEnterWorld happens after Load, so nonStopParty is the correct value.
			//GetInstance<ExampleMod>().ExampleUI.ExampleButton.HoverText = "SendClientChanges Example: Non-Stop Party " + (nonStopParty ? "On" : "Off");
		}

		// In MP, other clients need accurate information about your player or else bugs happen.
		// clientClone, SyncPlayer, and SendClientChanges, ensure that information is correct.
		// We only need to do this for data that is changed by code not executed by all clients, 
		// or data that needs to be shared while joining a world.
		// For example, examplePet doesn't need to be synced because all clients know that the player is wearing the ExamplePet item in an equipment slot. 
		// The examplePet bool is set for that player on every clients computer independently (via the Buff.Update), keeping that data in sync.
		// ExampleLifeFruits, however might be out of sync. For example, when joining a server, we need to share the exampleLifeFruits variable with all other clients.
		// In addition, in ExampleUI we have a button that toggles "Non-Stop Party". We need to sync this whenever it changes.
		public override void clientClone(ModPlayer clientClone) {
			HighlanderPlayer clone = clientClone as HighlanderPlayer;
			// Here we would make a backup clone of values that are only correct on the local players Player instance.
			// Some examples would be RPG stats from a GUI, Hotkey states, and Extra Item Slots
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = mod.GetPacket();
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			// Here we would sync something like an RPG stat whenever the player changes it.
			HighlanderPlayer clone = clientPlayer as HighlanderPlayer;
			// Send a Mod Packet with the changes.
			var packet = mod.GetPacket();
			packet.Send();
		}

		public override void UpdateDead() {
		}

		public override TagCompound Save() {
			// Read https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound to better understand Saving and Loading data.
			return new TagCompound {
				// {"somethingelse", somethingelse}, // To save more data, add additional lines
			};
			//note that C# 6.0 supports indexer initializers
			//return new TagCompound {
			//	["score"] = score
			//};
		}

		public override void Load(TagCompound tag) {
		}

		public override void LoadLegacy(BinaryReader reader) {
		}

		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath) {
		}

		public override void UpdateBiomes() {
		}

		public override bool CustomBiomesMatch(Player other) {
			return base.CustomBiomesMatch(other);
		}

		public override void CopyCustomBiomesTo(Player other) {
		}

		public override void SendCustomBiomes(BinaryWriter writer) {
		}

		public override void ReceiveCustomBiomes(BinaryReader reader) {
		}

		public override void UpdateBiomeVisuals() {
			//bool usePurity = NPC.AnyNPCs(NPCType<PuritySpirit>());
			//player.ManageSpecialBiomeVisuals("ExampleMod:PuritySpirit", usePurity);
			//bool useVoidMonolith = voidMonolith && !usePurity && !NPC.AnyNPCs(NPCID.MoonLordCore);
			//player.ManageSpecialBiomeVisuals("ExampleMod:MonolithVoid", useVoidMonolith, player.Center);
		}

		public override Texture2D GetMapBackgroundImage() {
			return null;
		}

		public override void UpdateBadLifeRegen() {
		}

		public override void ProcessTriggers(TriggersSet triggersSet) {
		}

		public override void PreUpdateBuffs() {
		}


		public override void PostUpdateBuffs() {
		}

		public override void UpdateVanityAccessories() {
		}

		public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff) {
		}

		public override void PostUpdateEquips() {
		}

		public override void PostUpdateMiscEffects() {
		}

		public override void FrameEffects() {
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit,
			ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
		}

		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit) {
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
			return true;
		}

		public override float UseTimeMultiplier(Item item) {
			return 1f;
		}

		public override void OnConsumeMana(Item item, int manaConsumed) {
		}

		public override void AnglerQuestReward(float quality, List<Item> rewardItems) {
		}

		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk) {
		}

		public override void GetFishingLevel(Item fishingRod, Item bait, ref int fishingLevel) {
		}

		public override void GetDyeTraderReward(List<int> dyeItemIDsPool) {
		}

		public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo) {
		}

		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
			if (Main.netMode != NetmodeID.Server)
			{
				if(player.armor[10] != null && player.armor[10].modItem != null)
				{
					if(player.armor[10].modItem.GetType().BaseType != null)
					{
						if (player.armor[10].modItem.GetType().BaseType == typeof(AbnormalItem)) {
							AbnormalItem item = (AbnormalItem)player.armor[10].modItem;

							unusual = item.CurrentEffect;

							Vector2 headPosition;
							float headHeight;
							Dust currDust;
							ModDustCustomData data;

							switch (item.CurrentEffect)
							{
								case AbnormalEffect.Unknown:
									break;
								case AbnormalEffect.None:
									break;
								case AbnormalEffect.PurpleEnergy:
									/**headPosition = player.Center;
									headHeight = 2 * (player.height / 5);
									headPosition.Y -= headHeight - 14;
									headPosition.X -= 6 - 3;

									currDust = Dust.NewDustPerfect(headPosition, mod.DustType("PurpleEnergy"));
									data = new ModDustCustomData(player);
									currDust.customData = data;**/
									break;
								case AbnormalEffect.GreenEnergy:
									/**headPosition = player.Center;
									headHeight = 2 * (player.height / 5);
									headPosition.Y -= headHeight - 14;
									headPosition.X -= 6 - 3;

									currDust = Dust.NewDustPerfect(headPosition, mod.DustType("GreenEnergy"));
									data = new ModDustCustomData(player);
									currDust.customData = data;**/
									break;
								case AbnormalEffect.BurningFlames:
									headPosition = player.Center;
									headHeight = 2 * (player.height / 5);
									headPosition.Y -= headHeight + 16;
									headPosition.X -= player.width / 2 + 2;

									currDust = Dust.NewDustDirect(headPosition, player.width, player.height / 8 + 4, mod.DustType("BurningFlames"));
									data = new ModDustCustomData(player);
									currDust.customData = data;
									break;
								case AbnormalEffect.ScorchingFlames:
									headPosition = player.Center;
									headHeight = 2 * (player.height / 5);
									headPosition.Y -= headHeight + 16;
									headPosition.X -= player.width / 2 + 2;

									currDust = Dust.NewDustDirect(headPosition, player.width, player.height / 8 + 4, mod.DustType("ScorchingFlames"));
									data = new ModDustCustomData(player);
									currDust.customData = data;
									break;
								case AbnormalEffect.BlizzardyStorm:
									headPosition = player.Center;
									headHeight = 2 * (player.height / 5);
									headPosition.Y -= headHeight + 28;
									headPosition.X -= 2 * player.width / 3 - 4;

									if (counter % 4 == 0)
									{
										headPosition.X += 0;
										headPosition.Y += 12;
										currDust = Dust.NewDustDirect(headPosition, player.width, player.height / 8, mod.DustType("BlizzardyStormParticle"));
										data = new ModDustCustomData(player);
										currDust.customData = data;
									}

									counter = (counter + 1) % 60;
									break;
								case AbnormalEffect.StormyStorm:
									headPosition = player.Center;
									headHeight = 2 * (player.height / 5);
									headPosition.Y -= headHeight + 28;
									headPosition.X -= 2 * player.width / 3 - 4;

									if (counter % 4 == 0)
									{
										headPosition.X += 0;
										headPosition.Y += 12;
										currDust = Dust.NewDustDirect(headPosition, player.width, player.height / 8, mod.DustType("StormyStormParticle"));
										data = new ModDustCustomData(player);
										currDust.customData = data;
									}

									counter = (counter + 1) % 60;
									break;
								case AbnormalEffect.Cloud9:
									headPosition = player.Center;
									headHeight = 2 * (player.height / 5);
									headPosition.Y -= headHeight + 12;
									headPosition.X -= player.width / 2 + 8;

									if (counter % 30 == 0)
									{
										currDust = Dust.NewDustDirect(headPosition, player.width + 16, player.height / 8 + 10, mod.DustType("Cloud9"));
										data = new ModDustCustomData(player);
										currDust.customData = data;
										var trailDust = Dust.NewDustPerfect(currDust.position - currDust.velocity, mod.DustType("Cloud9Trail"), currDust.velocity * 0.5f);
										data = new ModDustCustomData(player);
										trailDust.customData = data;
										trailDust.scale = 0.8f;
										trailDust = Dust.NewDustPerfect(currDust.position - currDust.velocity, mod.DustType("Cloud9Trail"), currDust.velocity * 0.25f);
										data = new ModDustCustomData(player);
										trailDust.customData = data;
										trailDust.scale = 0.4f;
										trailDust = Dust.NewDustPerfect(currDust.position - currDust.velocity, mod.DustType("Cloud9Trail"), currDust.velocity * 0.125f);
										data = new ModDustCustomData(player);
										trailDust.customData = data;
										trailDust.scale = 0.2f;
									}
									counter = (counter + 1) % 60;
									break;
								default:
									break;
							}



						}
					}
				}
				else
				{
				}
			}
		}

		public override bool ModifyNurseHeal(NPC nurse, ref int health, ref bool removeDebuffs, ref string chatText)
		{
			return base.ModifyNurseHeal(nurse, ref health, ref removeDebuffs, ref chatText);
		}

		public override void PostBuyItem(NPC vendor, Item[] shop, Item item)
		{
		}

		public override void PostSellItem(NPC vendor, Item[] shopInventory, Item item)
		{
		}

		public override void PlayerConnect(Player player)
		{
		}

		public override void OnRespawn(Player player)
		{
		}

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			if (holdingAmmoGun)
			{
				AnimationHelper.ammoGunCounter.visible = true;
				layers.Add(AnimationHelper.ammoGunCounter);
			}
			if (unusual != 0)
			{
				AnimationHelper.unusual.visible = true;
				layers.Insert(0, AnimationHelper.unusual);
				layers.Add(AnimationHelper.unusualFront);
			}

			int count = layers.Count;

			

			/**for(int i = 0; i < count; i++)
			{
				PlayerLayer layer = layers[i];
				Main.NewText(layer.Name);
				if (layer.Name == "Head")
				{
					//Main.NewText(layer.Name);
					if (i != layers.Count - 1)
					{
						layers.Insert(i, AnimationHelper.unusualFront);
					}
					else
					{
						layers.Add(AnimationHelper.unusualFront);
					}
					break;
				}
			}**/

		}

	}
}
