using Highlander.Common.Systems;
using Highlander.Projectiles.Equipment;
using Highlander.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using static Highlander.Highlander;
using static Terraria.ModLoader.ModContent;

namespace Highlander.Common.Players
{
    // ModPlayer classes provide a way to attach data to Players and act on that data. ExamplePlayer has a lot of functionality related to 
    // several effects and items in ExampleMod. See SimpleModPlayer for a very simple example of how ModPlayer classes work.
    public class HighlanderPlayer : ModPlayer
    {

        public bool holdingAmmoGun = false;

        public int maxAmmo = 0;
        public int currentAmmo = 0;

        public Projectile bulletCounter;

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
            maxAmmo = 0;
            currentAmmo = 0;

            //wearingAutonomousOrb = false;
            tallHat = TallHat.None;

            emittingAura = false;
            receivingAura = false;
            bellOfPestilence = false;
            hasFlares = false;
        }

        // In MP, other clients need accurate information about your player or else bugs happen.
        // clientClone, SyncPlayer, and SendClientChanges, ensure that information is correct.
        // We only need to do this for data that is changed by code not executed by all clients, 
        // or data that needs to be shared while joining a world.
        // For example, examplePet doesn't need to be synced because all clients know that the player is wearing the ExamplePet item in an equipment slot. 
        // The examplePet bool is set for that player on every clients computer independently (via the Buff.Update), keeping that data in sync.
        // ExampleLifeFruits, however might be out of sync. For example, when joining a server, we need to share the exampleLifeFruits variable with all other clients.
        // In addition, in ExampleUI we have a button that toggles "Non-Stop Party". We need to sync this whenever it changes.
        public override void CopyClientState(ModPlayer targetCopy)
        {
            HighlanderPlayer clone = targetCopy as HighlanderPlayer;
            // Here we would make a backup clone of values that are only correct on the local players Player instance.
            // Some examples would be RPG stats from a GUI, Hotkey states, and Extra Item Slots
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)HighlanderMessageType.HighlanderPlayerSyncPlayer);
            packet.Write((byte)Player.whoAmI);
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
            packet.Send();
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                /**
                bool hasModItemArmor = Player.armor[0] != null && Player.armor[0].ModItem != null;
                bool hasModItemVanity = Player.armor[10] != null && Player.armor[10].ModItem != null;
                if (hasModItemVanity || hasModItemArmor)
                {
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
                }**/
            }
        }

        public override void PostUpdate()
        {
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (bellOfPestilence && hit.DamageType == DamageClass.Melee)
            {
                if (Main.rand.NextBool(2))
                {
                    target.AddBuff(BuffID.Poisoned, 300);
                    target.netUpdate = true;
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
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
