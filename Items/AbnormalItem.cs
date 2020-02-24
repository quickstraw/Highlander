using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Highlander.Items
{
    class AbnormalItem : ModItem
    {

        public AbnormalEffect CurrentEffect { get; set; }
        public int counter = 0;
        public List<float> Table = RollTable.AbnormalRollTable.Table;

        public override void SetDefaults()
        {
            item.accessory = true;
            CurrentEffect = ReturnRollAbnormalEffect();
            Save();
        }
        public override ModItem Clone()
        {
            var clone = (AbnormalItem)base.Clone();
            clone.CurrentEffect = CurrentEffect;
            return clone;
        }

        /// <summary>
        /// Rolls a random float and determines what effect the Abnormal item gets.
        /// </summary>
        protected void RollAbnormalEffect()
        {
            float totalChance = 0;
            foreach(float f in Table)
            {
                totalChance += f;
            }

            float randFloat = Main.rand.NextFloat(0, totalChance);

            for(int i = 0; i < Table.Count; i++)
            {
                randFloat -= Table[i];
                if(randFloat <= 0)
                {
                    CurrentEffect = (AbnormalEffect)i;
                    Save();
                    break;
                }
            }
        }

        /// <summary>
        /// Rolls a random float and determines what effect the Abnormal item gets.
        /// </summary>
        protected AbnormalEffect ReturnRollAbnormalEffect()
        {
            float totalChance = 0;
            foreach (float f in Table)
            {
                totalChance += f;
            }

            float randFloat = Main.rand.NextFloat(0, totalChance);

            for (int i = 0; i < Table.Count; i++)
            {
                randFloat -= Table[i];
                if (randFloat <= 0)
                {
                    return (AbnormalEffect)i;
                }
            }
            return AbnormalEffect.Unknown;
        }

        public override bool CloneNewInstances => true;

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Main.NewText(CurrentEffect);
            if (CurrentEffect != 0)
            {
                TooltipLine line = new TooltipLine(mod, "AbnormalToolTip", "Abnormal effect is: " + CurrentEffect);
                line.overrideColor = Color.PaleVioletRed;
                tooltips.Add(line);
            }
        }

        public override void Load(TagCompound tag)
        {
            CurrentEffect = (AbnormalEffect)tag.GetInt("AbnormalEffect");
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "AbnormalEffect", (int)CurrentEffect }
            };
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write((int)CurrentEffect);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            CurrentEffect = (AbnormalEffect)reader.ReadInt32();
        }

        public override void UpdateEquip(Player player)
        {
            PlayAbnormalEffect(player);
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            Main.NewText(CurrentEffect);
            PlayAbnormalEffect(player);
        }

        protected void PlayAbnormalEffect(Player player)
        {
            Vector2 headPosition;
            float headHeight;
            Dust currDust;
            ModDustCustomData data;

            switch (CurrentEffect)
            {
                case AbnormalEffect.Unknown:
                    break;
                case AbnormalEffect.None:
                    break;
                case AbnormalEffect.PurpleEnergy:
                    headPosition = player.Center;
                    headHeight = 2 * (player.height / 5);
                    headPosition.Y -= headHeight - 14;
                    headPosition.X -= 6 - 3;

                    currDust = Dust.NewDustPerfect(headPosition, mod.DustType("PurpleEnergy"));
                    data = new ModDustCustomData(player);
                    currDust.customData = data;
                    break;
                case AbnormalEffect.GreenEnergy:
                    headPosition = player.Center;
                    headHeight = 2 * (player.height / 5);
                    headPosition.Y -= headHeight - 14;
                    headPosition.X -= 6 - 3;

                    currDust = Dust.NewDustPerfect(headPosition, mod.DustType("GreenEnergy"));
                    data = new ModDustCustomData(player);
                    currDust.customData = data;
                    break;
                case AbnormalEffect.BurningFlames:
                    headPosition = player.Center;
                    headHeight = 2 * (player.height / 5);
                    headPosition.Y -= headHeight + 12;
                    headPosition.X -= player.width / 2 + 2;

                    currDust = Dust.NewDustDirect(headPosition, player.width, player.height / 8 + 10, mod.DustType("BurningFlames"));
                    data = new ModDustCustomData(player);
                    currDust.customData = data;
                    break;
                case AbnormalEffect.ScorchingFlames:
                    headPosition = player.Center;
                    headHeight = 2 * (player.height / 5);
                    headPosition.Y -= headHeight + 12;
                    headPosition.X -= player.width / 2 + 2;

                    currDust = Dust.NewDustDirect(headPosition, player.width, player.height / 8 + 10, mod.DustType("ScorchingFlames"));
                    data = new ModDustCustomData(player);
                    currDust.customData = data;
                    break;
                case AbnormalEffect.BlizzardyStorm:
                    headPosition = player.Center;
                    headHeight = 2 * (player.height / 5);
                    headPosition.Y -= headHeight + 28;
                    headPosition.X -= 2 * player.width / 3 - 4;

                    if (counter % 5 == 0)
                    {
                        currDust = Dust.NewDustDirect(headPosition, player.width / 3, player.height / 8, mod.DustType("BlizzardyStorm"));
                        data = new ModDustCustomData(player);
                        currDust.customData = data;
                    }
                    else if (counter % 4 == 0)
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

                    if (counter % 5 == 0)
                    {
                        currDust = Dust.NewDustDirect(headPosition, player.width / 3, player.height / 8, mod.DustType("StormyStorm"));
                        data = new ModDustCustomData(player);
                        currDust.customData = data;
                    }
                    else if (counter % 4 == 0)
                    {
                        headPosition.X += 0;
                        headPosition.Y += 12;
                        currDust = Dust.NewDustDirect(headPosition, player.width, player.height / 8, mod.DustType("StormyStormParticle"));
                        data = new ModDustCustomData(player);
                        currDust.customData = data;
                    }

                    counter = (counter + 1) % 60;
                    break;
                default:
                    break;
            }
        }
    }
}