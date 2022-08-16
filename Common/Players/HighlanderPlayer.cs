using Highlander.Common.Systems;
using Highlander.Items;
using Highlander.Items.Armor;
using Highlander.Items.Armor.Halloween;
using Highlander.Items.Armor.VanityHats;
using Highlander.Projectiles.Equipment;
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

namespace Highlander.Common.Players
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
        public bool hasFlares = false;

        public const float maxFlares = 3;
        public float flares = maxFlares;

        public byte clock = 0;

        public override void ResetEffects()
        {
            holdingAmmoGun = false;
            unusual = 0;
            maxAmmo = 0;
            currentAmmo = 0;

            wearingAutonomousOrb = false;
            tallHat = TallHat.None;

            emittingAura = false;
            receivingAura = false;
            bellOfPestilence = false;
            hasFlares = false;
        }

        public override void OnEnterWorld(Player player)
        {
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
        public override void clientClone(ModPlayer clientClone)
        {
            HighlanderPlayer clone = clientClone as HighlanderPlayer;
            // Here we would make a backup clone of values that are only correct on the local players Player instance.
            // Some examples would be RPG stats from a GUI, Hotkey states, and Extra Item Slots
            clone.unboxed = unboxed;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)HighlanderMessageType.HighlanderPlayerSyncPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write(unboxed);
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            // Here we would sync something like an RPG stat whenever the player changes it.
            HighlanderPlayer clone = clientPlayer as HighlanderPlayer;
            // Send a Mod Packet with the changes.
            var packet = Mod.GetPacket();
            packet.Write((byte)HighlanderMessageType.HighlanderPlayerSyncPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write(unboxed);
            packet.Send();
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                bool hasModItemArmor = Player.armor[0] != null && Player.armor[0].ModItem != null;
                bool hasModItemVanity = Player.armor[10] != null && Player.armor[10].ModItem != null;
                if (hasModItemVanity || hasModItemArmor)
                {
                    if (hasModItemVanity && Player.armor[10].ModItem.GetType().BaseType != null || hasModItemArmor && Player.armor[0].ModItem.GetType().BaseType != null)
                    {
                        if (hasModItemVanity && Player.armor[10].ModItem.GetType().BaseType == typeof(AbnormalItem)
                            || hasModItemArmor && Player.armor[0].ModItem.GetType().BaseType == typeof(AbnormalItem))
                        {
                            AbnormalItem item;

                            if (hasModItemVanity && Player.armor[10].ModItem.GetType().BaseType == typeof(AbnormalItem))
                            {
                                item = (AbnormalItem)Player.armor[10].ModItem;
                            }
                            else if (hasModItemArmor && Player.armor[0].ModItem.GetType().BaseType == typeof(AbnormalItem))
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
                                    break;
                                case AbnormalEffect.GreenEnergy:
                                    break;
                                case AbnormalEffect.BurningFlames:
                                    Lighting.AddLight(headPosition, 0.58f, 0.41f, 0.01f);
                                    break;
                                case AbnormalEffect.ScorchingFlames:
                                    Lighting.AddLight(headPosition, 0.13f, 0.55f, 0.32f);
                                    break;
                                case AbnormalEffect.BlizzardyStorm:
                                    counter = (counter + 1) % 60;
                                    break;
                                case AbnormalEffect.StormyStorm:
                                    counter = (counter + 1) % 60;
                                    break;
                                case AbnormalEffect.Cloud9:
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
                    if (hasModItemVanity && Player.armor[10].ModItem.GetType() == typeof(ToySoldier) || hasModItemArmor && !hasModItemVanity && Player.armor[0].ModItem.GetType() == typeof(ToySoldier))
                    {
                        tallHat = TallHat.ToySoldier;
                    }
                    if (hasModItemVanity && Player.armor[10].ModItem.GetType() == typeof(CroneDome) || hasModItemArmor && !hasModItemVanity && Player.armor[0].ModItem.GetType() == typeof(CroneDome))
                    {
                        tallHat = TallHat.CroneDome;
                    }
                    if (hasModItemVanity && Player.armor[10].ModItem.GetType() == typeof(SearedSorcerer) || hasModItemArmor && !hasModItemVanity && Player.armor[0].ModItem.GetType() == typeof(SearedSorcerer))
                    {
                        tallHat = TallHat.SearedSorcerer;
                    }
                    if (hasModItemVanity && Player.armor[10].ModItem.GetType() == typeof(SirPumpkinton) || hasModItemArmor && !hasModItemVanity && Player.armor[0].ModItem.GetType() == typeof(SirPumpkinton))
                    {
                        tallHat = TallHat.SirPumpkinton;
                    }
                }
                else
                {
                }
            }
        }

        public override void PostUpdate()
        {
            if (unboxed != -1)
            {
                string text = Player.name + " unboxed an " + GetModItem(unboxed).Item.Name + "!";

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

            if (hasFlares)
            {
                if(clock == 0 && flares < maxFlares)
                {
                    flares += 0.10f;
                }
            }

            clock = (byte)((clock + 1) % 60);
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

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.ActionKeybind.JustPressed)
            {
                if (hasFlares && flares >= 1)
                {
                    Vector2 mouse = Main.MouseWorld;
                    Vector2 vectorToMouse = mouse - Player.Center;
                    var source = this.Player.GetSource_Accessory(Find<ModItem>("Highlander/OldFlareDispenser").Item);//Player.GetSource_Accessory(Find<ModItem>("OldFlareDispenser").Item);
                    Vector2 velocity = vectorToMouse;
                    velocity.Normalize();
                    velocity *= 8f;
                    var projectile = Projectile.NewProjectile(source, Player.MountedCenter, velocity, ProjectileType<FlareProjectile>(), 0, 0f);
                    NetMessage.SendData(MessageID.SyncProjectile, number: projectile);
                    flares--;
                }
            }
        }

    }
}
