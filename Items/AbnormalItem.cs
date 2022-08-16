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
        public AbnormalEffect CurrentEffect = AbnormalEffect.Unknown;
        public int counter = 0;
        public List<AbnormalEffect> Table = RollTable.AbnormalRollTable.Table;
        private bool roll = true;

        //Can't use [Autoload(false)] lest deriving types not get added
        public sealed override bool IsLoadingEnabled(Mod mod) => SafeIsLoadingEnabled(mod) ?? false;

        protected override bool CloneNewInstances => true;

        /// <summary>
        /// Allows you to safely request whether this item should be autoloaded
        /// </summary>
        /// <param name="mod">The mod adding this item</param>
        /// <returns><see langword="null"/> for the default behaviour (don't autoload item), <see langword="true"/> to let the item autoload or <see langword="false"/> to prevent the item from autoloading</returns>
        public virtual bool? SafeIsLoadingEnabled(Mod mod) => null;

        public override void SetDefaults()
        {
            Item.accessory = true;
            if (roll)
            {
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
                tooltips[0].OverrideColor = Color.MediumPurple;
                string name = "" + CurrentEffect;
                name = Regex.Replace(name, "(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])|(?<=[0-9])(?=[A-Z][a-z])|(?<=[a-zA-Z])(?=[0-9])", " ");
                TooltipLine line = new TooltipLine(Mod, "AbnormalToolTip", "Unusual Effect: " + name);
                line.OverrideColor = Color.MediumPurple;
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
        }
    }
}