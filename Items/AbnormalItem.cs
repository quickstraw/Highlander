using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;

namespace Highlander.Items
{
    
    class AbnormalItem : ModItem
    {
        public override string Name => CurrentEffect != 0 ? "Unusual" + GetType().Name : GetType().Name;
        public AbnormalEffect CurrentEffect { get; set; }
        public int counter = 0;
        public List<AbnormalEffect> Table = RollTable.AbnormalRollTable.Table;
        private bool roll = true;

        //Can't use [Autoload(false)] lest deriving types not get added
        public sealed override bool IsLoadingEnabled(Mod mod) => SafeIsLoadingEnabled(mod) ?? false;

        /// <summary>
        /// Allows you to safely request whether this item should be autoloaded
        /// </summary>
        /// <param name="mod">The mod adding this item</param>
        /// <returns><see langword="null"/> for the default behaviour (don't autoload item), <see langword="true"/> to let the item autoload or <see langword="false"/> to prevent the item from autoloading</returns>
        public virtual bool? SafeIsLoadingEnabled(Mod mod) => null;

        public override void SetDefaults()
        {
            Item.accessory = true;
            if (roll) {
                CurrentEffect = ReturnRollAbnormalEffect();
                Item.rare = ItemRarityID.LightPurple;
            }
            //SaveData();
        }

        public override ModItem Clone(Item item)
        {
            var clone = (AbnormalItem)base.Clone(item);
            clone.CurrentEffect = CurrentEffect;
            return clone;
        }

        public AbnormalItem()
        {
            roll = false;
            CurrentEffect = 0;
            // Dummy Constructor
        }
        public AbnormalItem(string internalName)
        {
        }
        public AbnormalItem(AbnormalEffect effect)
        {
            CurrentEffect = effect;
            // Dummy Constructor
        }

        /// <summary>
        /// Rolls a random float and determines what effect the Abnormal item gets.
        /// </summary>
        protected void RollAbnormalEffect()
        {
            int rand = Main.rand.Next(0, Table.Count);
            CurrentEffect = Table[rand];
        }

        /// <summary>
        /// Rolls a random float and determines what effect the Abnormal item gets.
        /// </summary>
        protected AbnormalEffect ReturnRollAbnormalEffect()
        {
            int rand = Main.rand.Next(0, Table.Count);
            return Table[rand];
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (CurrentEffect != 0)
            {
                tooltips[0].overrideColor = Color.MediumPurple;
                string name = "" + CurrentEffect;
                name = Regex.Replace(name, "(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])|(?<=[0-9])(?=[A-Z][a-z])|(?<=[a-zA-Z])(?=[0-9])", " ");
                TooltipLine line = new TooltipLine(Mod, "AbnormalToolTip", "Unusual Effect: " + name);
                line.overrideColor = Color.MediumPurple;
                tooltips.Add(line);
            }
        }

        public override void LoadData(TagCompound tag)
        {
            CurrentEffect = (AbnormalEffect)tag.GetInt("AbnormalEffect");
        }

        public override void SaveData(TagCompound tag)
        {
            tag["AbnormalEffect"] = (int)CurrentEffect;
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write((int)CurrentEffect);
        }

        public override void NetReceive(BinaryReader reader)
        {
            CurrentEffect = (AbnormalEffect)reader.ReadInt32();
        }

        public override void UpdateEquip(Player player)
        {
            PlayAbnormalEffect(player);
        }

        protected void PlayAbnormalEffect(Player player)
        {
            /**Vector2 headPosition;
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
            }**/
        }
    }
}