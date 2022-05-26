using Highlander.Items;
using Highlander.Items.Weapons;
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

namespace Highlander.NPCs.VeteranExplorer
{
    [AutoloadHead]
    class VeteranExplorer : ModNPC
    {

        public override string Texture => "Highlander/NPCs/VeteranExplorer/VeteranExplorer";

        public override void SetStaticDefaults()
        {
            // DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
            DisplayName.SetDefault("Veteran Explorer");
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;

            NPCID.Sets.AttackType[NPC.type] = 0;

            NPC.Happiness
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Love) // Veteran Explorer loves the jungle.
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Like) // Veteran Explorer likes the desert.
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Dislike) // Veteran Explorer dislikes the underground.
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Hate) // Veteran Explorer hates the Hallow.
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Like) // Likes living near the dryad.
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Love) // Loves living near the guide.
                .SetNPCAffection(NPCID.Merchant, AffectionLevel.Like) // Likes living near the merchant.
                .SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Dislike) // Hates living near the goblin tinkerer.
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
            if (NPC.downedBoss3)  //After Skeletron
            {
                return true;
            }
            return false;
        }

        public override List<string> SetNPCNameList()
        {
            List<string> list = new List<string>();
            list.Add("Hubert");
            list.Add("Alfonso");
            list.Add("Gregory");
            list.Add("Marco");
            return list;
        }

        public override string GetChat()
        {
            int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);

            switch (Main.rand.Next(4))
            {
                case 0:
                    return "Greetings! Tell me of your travels.";
                case 1:
                    return "I can spare you some of my extra supplies for a price.";
                case 2:
                    return "You look like you need some help. I think I've got just what you need.";
                default:
                    return "Have you found any treasure?";
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
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<AggressiveAle>());
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<AdventurerPike>());
            nextSlot++;
            if (Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<GreatPike>());
                nextSlot++;
            }
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
            projType = ProjectileID.JavelinFriendly;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override void DrawTownAttackGun(ref float scale, ref int item, ref int closeness)
        {
            scale = 1f;
            item = ItemID.Musket;
            closeness = 6;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("This veteran is a capable fighter. He even sells a couple of items from his adventures.")
            });
        }

    }
}
