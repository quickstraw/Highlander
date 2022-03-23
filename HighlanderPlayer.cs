using Highlander.Items;
using Highlander.Items.Armor;
using Highlander.Items.Armor.VanityHats;
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
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Highlander.Highlander;
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
		public int unusualFrame3 = 0;

		public int maxAmmo = 0;
		public int currentAmmo = 0;

		public Projectile bulletCounter;

		public int unboxed = -1;

		public short hatEffectTime = 0;
		public bool wearingAutonomousOrb = false;
		public TallHat tallHat = TallHat.None;

		public int counter = 0;

		public bool emittingAura = false;
		public bool receivingAura = false;
		public bool bellOfPestilence = false;

		public byte clock = 0;

		public override void ResetEffects() {
			holdingAmmoGun = false;
			unusual = 0; 
			maxAmmo = 0;
			currentAmmo = 0;

			wearingAutonomousOrb = false;
			tallHat = TallHat.None;

			emittingAura = false;
			receivingAura = false;
			bellOfPestilence = false;
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
			clone.unboxed = unboxed;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)HighlanderMessageType.HighlanderPlayerSyncPlayer);
			packet.Write((byte)Player.whoAmI);
			packet.Write(unboxed);
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			// Here we would sync something like an RPG stat whenever the player changes it.
			HighlanderPlayer clone = clientPlayer as HighlanderPlayer;
			// Send a Mod Packet with the changes.
			var packet = Mod.GetPacket();
			packet.Write((byte)HighlanderMessageType.HighlanderPlayerSyncPlayer);
			packet.Write((byte)Player.whoAmI);
			packet.Write(unboxed);
			packet.Send();
		}

		public override void UpdateDead() {
		}

        public override void SaveData(TagCompound tag)
        {
            
        }

        public override void LoadData(TagCompound tag)
        {
            
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

        public override void UpdateVisibleVanityAccessories()
        {
            
        }

        public override void UpdateEquips()
        {
            
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

        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            
        }

        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {
            
        }

        public override void GetDyeTraderReward(List<int> dyeItemIDsPool) {
		}

		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
			if (Main.netMode != NetmodeID.Server)
			{
				bool hasModItemArmor = Player.armor[0] != null && Player.armor[0].ModItem != null;
				bool hasModItemVanity = Player.armor[10] != null && Player.armor[10].ModItem != null;
				if (hasModItemVanity || hasModItemArmor)
				{
					if(hasModItemVanity && Player.armor[10].ModItem.GetType().BaseType != null || hasModItemArmor && Player.armor[0].ModItem.GetType().BaseType != null)
					{
						if (hasModItemVanity && Player.armor[10].ModItem.GetType().BaseType == typeof(AbnormalItem)
							|| hasModItemArmor && Player.armor[0].ModItem.GetType().BaseType == typeof(AbnormalItem)) {
							AbnormalItem item;

							if (hasModItemVanity && Player.armor[10].ModItem.GetType().BaseType == typeof(AbnormalItem))
							{
								item = (AbnormalItem)Player.armor[10].ModItem;
							}
							else if(hasModItemArmor && Player.armor[0].ModItem.GetType().BaseType == typeof(AbnormalItem))
							{
								item = (AbnormalItem)Player.armor[0].ModItem;
							}
							else
							{
								return;
							}
							

							unusual = item.CurrentEffect;

							Vector2 headPosition = Player.position;
							headPosition.X += Player.width / 2;

							if (Player.mount.Active)
							{
								headPosition.Y += (Player.height - Player.mount.PlayerOffset) / 2;
								headPosition.Y += Player.mount.PlayerOffset;
							}
							else
							{
								headPosition.Y += Player.height / 2;
							}

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
									/**headPosition = Player.Center;
									headHeight = 2 * (Player.height / 5);
									headPosition.Y -= headHeight - 14;
									headPosition.X -= 6 - 3;

									currDust = Dust.NewDustPerfect(headPosition, mod.DustType("PurpleEnergy"));
									data = new ModDustCustomData(player);
									currDust.customData = data;**/
									break;
								case AbnormalEffect.GreenEnergy:
									/**headPosition = Player.Center;
									headHeight = 2 * (Player.height / 5);
									headPosition.Y -= headHeight - 14;
									headPosition.X -= 6 - 3;

									currDust = Dust.NewDustPerfect(headPosition, mod.DustType("GreenEnergy"));
									data = new ModDustCustomData(player);
									currDust.customData = data;**/
									break;
								case AbnormalEffect.BurningFlames:
									/**headHeight = 2 * (42 / 5);
									headPosition.Y -= headHeight + 16;
									headPosition.X -= Player.width / 2 + 2;

									currDust = Dust.NewDustDirect(headPosition, Player.width, 42 / 8 + 4, mod.DustType("BurningFlames"));
									data = new ModDustCustomData(player);
									currDust.customData = data;**/
									Lighting.AddLight(headPosition, 0.58f, 0.41f, 0.01f);
									break;
								case AbnormalEffect.ScorchingFlames:
									/**headHeight = 2 * (42 / 5);
									headPosition.Y -= headHeight + 16;
									headPosition.X -= Player.width / 2 + 2;

									currDust = Dust.NewDustDirect(headPosition, Player.width, 42 / 8 + 4, mod.DustType("ScorchingFlames"));
									data = new ModDustCustomData(player);
									currDust.customData = data;**/
									Lighting.AddLight(headPosition, 0.13f, 0.55f, 0.32f);
									break;
								case AbnormalEffect.BlizzardyStorm:
									/**headHeight = 2 * (42 / 5);
									headPosition.Y -= headHeight + 28;
									headPosition.X -= 2 * Player.width / 3 - 4;

									if (counter % 4 == 0)
									{
										headPosition.X += 0;
										headPosition.Y += 12;
										currDust = Dust.NewDustDirect(headPosition, Player.width, 42 / 8, mod.DustType("BlizzardyStormParticle"));
										data = new ModDustCustomData(player);
										currDust.customData = data;
									}
									**/
									counter = (counter + 1) % 60;
									break;
								case AbnormalEffect.StormyStorm:
									/**
									headHeight = 2 * (42 / 5);
									headPosition.Y -= headHeight + 28;
									headPosition.X -= 2 * Player.width / 3 - 4;

									if (counter % 4 == 0)
									{
										headPosition.X += 0;
										headPosition.Y += 12;
										currDust = Dust.NewDustDirect(headPosition, Player.width, 42 / 8, mod.DustType("StormyStormParticle"));
										data = new ModDustCustomData(player);
										currDust.customData = data;
									}
									**/
									counter = (counter + 1) % 60;
									break;
								case AbnormalEffect.Cloud9:
									/**
									headHeight = 2 * (42 / 5);
									headPosition.Y -= headHeight + 12;
									headPosition.X -= Player.width / 2 + 8;

									if (counter % 30 == 0)
									{
										currDust = Dust.NewDustDirect(headPosition, Player.width + 16, 42 / 8 + 10, mod.DustType("Cloud9"));
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
									}**/
									counter = (counter + 1) % 60;
									break;
								default:
									break;
							}



						}
					}
					if (hasModItemVanity && Player.armor[10].ModItem.GetType() == typeof(AutonomousOrb) || hasModItemArmor && !hasModItemVanity && Player.armor[0].ModItem.GetType() == typeof(AutonomousOrb))
					{
						wearingAutonomousOrb = true;
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

        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            base.HideDrawLayers(drawInfo);
        }

        /**
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int count = layers.Count;

			if (!Player.dead)
			{
				if (wearingAutonomousOrb)
				{
					for (int i = 0; i < count; i++)
					{
						PlayerLayer layer = layers[i];
						if (layer.Name == "Head")
						{
							if (i != layers.Count - 1)
							{
								layers.Insert(i + 1, HatEffects.AutonomousOrb);
							}
							else
							{
								layers.Add(HatEffects.AutonomousOrb);
							}
							break;
						}
					}
				}
				if (tallHat != TallHat.None)
				{
					for (int i = 0; i < count; i++)
					{
						PlayerLayer layer = layers[i];
						if (layer.Name == "Head")
						{
							if (i != layers.Count - 1)
							{
								layers.Insert(i + 1, HatEffects.TallHatLayer);
							}
							else
							{
								layers.Add(HatEffects.TallHatLayer);
							}
							break;
						}
					}
				}
				if (unusual != 0)
				{
					AnimationHelper.unusual.visible = true;
					layers.Insert(0, AnimationHelper.unusual);
					layers.Add(AnimationHelper.unusualFront);
				}
				if (holdingAmmoGun)
				{
					AnimationHelper.ammoGunCounter.visible = true;
					layers.Add(AnimationHelper.ammoGunCounter);
				}
			}

		}
				**/

		public override void PostUpdate()
		{
			if (unboxed != -1)
			{
				string text = Player.name + " unboxed an " + ModContent.GetModItem(unboxed).Item.Name + "!";

				if (Main.netMode == NetmodeID.Server)
				{
					NetworkText message = NetworkText.FromLiteral(text);
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(message, Color.MediumPurple);
				}

				unboxed = -1;
			}

			if (receivingAura)
			{
				Player.statDefense += 8;
			}
			//Main.NewText(Player.bodyFrame.Y / Player.bodyFrame.Height);

			clock = (byte) ((clock + 1) % 60);
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (bellOfPestilence && item.DamageType == DamageClass.Melee)
			{
				if (Main.rand.NextBool(2))
				{
					target.AddBuff(BuffID.Poisoned, 300);
					target.netUpdate = true;
				}
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if (bellOfPestilence && proj.DamageType == DamageClass.Melee)
			{
				if (Main.rand.NextBool(2))
				{
					target.AddBuff(BuffID.Poisoned, 300);
					target.netUpdate = true;
				}
			}
		}

	}
}
