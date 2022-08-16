using Highlander.Common.Systems;
using Highlander.Items;
using Highlander.Items.LockBoxes;
using Highlander.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Highlander.NPCs.HatSalesman
{
    [AutoloadHead]
    class HatSalesman : ModNPC
    {

        public override string Texture => "Highlander/NPCs/HatSalesman/HatSalesman";

        /**public override bool Autoload(ref string name)
        {
            name = "Hat Salesman";
            return mod.Properties.Autoload;
        }**/

        public override void SetStaticDefaults()
        {
            // DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
            DisplayName.SetDefault("Hat Salesman");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = -14;

            NPC.Happiness
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Love) // Hat Salesman loves the desert.
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Like) // Hat Salesman likes the snow.
                .SetBiomeAffection<MushroomBiome>(AffectionLevel.Dislike) // Hat Salesman dislikes the underground.
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Hate) // Hat Salesman hates the Hallow.
                .SetNPCAffection(NPCID.Merchant, AffectionLevel.Like) // Likes living near the merchant.
                .SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Like) // Likes living near the dye trader.
                .SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Like) // Likes living near the arms dealer.
                .SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Love) // Loves living near the goblin tinkerer.
                .SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Truffle, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Hate)
            ;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7; // 7 is passive ai

            NPC.damage = 10;
            NPC.defense = 25;
            NPC.lifeMax = 250;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money) //Whether or not the conditions have been met for this town NPC to be able to move into town.
        {

            if (HighlanderWorld.downedHauntedHatter)  //so after the EoC is killed
            {
                return true;
            }
            return false;
        }

        public override List<string> SetNPCNameList()
        {
            List<string> list = new List<string>();
            list.Add("Clank");
            list.Add("HatBot-5000");
            list.Add("Beepulon");
            list.Add("SalesBot");
            list.Add("BootBot");
            return list;
        }

        public override string GetChat()
        {
            int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
            if (partyGirl >= 0 && Main.rand.NextBool(4))
            {
                return "" + Main.npc[partyGirl].GivenName.ToUpper() + " can no longer use my hats for parties!";
            }
            switch (Main.rand.Next(6))
            {
                case 0:
                    return "*BEEP* Welcome to my shop!";
                case 1:
                    return "Have you found anymore Hat Crates?";
                case 2:
                    return "Would you like to purchase Hat Keys?";
                case 3:
                    return "Be careful trying to collect Reserve Hat Crates! It's dangerous down in the Underworld.";
                case 4:
                    return "Remember to wear a coat while looking for Winter Hat Crates. You don't want to freeze to death.";
                default:
                    return "Do my shiny keys catch your attention, human?";
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<HatSupplyKey>());
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<WinterHatSupplyKey>());
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<SpookyHatKey>());
            nextSlot++;
        }


        // Make this Town NPC teleport to the King and/or Queen statue when triggered.
        public override bool CanGoToStatue(bool toKingStatue)
        {
            return true;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<WrenchProjectile>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A hat salesman? He only sells keys!")
            });
        }

    }
}
