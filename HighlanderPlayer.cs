using Highlander.UI;
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
		public int maxAmmo = 0;
		public int currentAmmo = 0;

		public Projectile bulletCounter;

		public override void ResetEffects() {
			holdingAmmoGun = false;
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
		}

	}
}
