using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highlander
{
    public class RollTable
    {
        public List<AbnormalEffect> Table;
        public static RollTable AbnormalRollTable;
        
        private RollTable()
        {
            Table = new List<AbnormalEffect>();
            AddChances();
        }

        private void AddChances()
        {
            //Table.Add(5f); //None
            Table.Add(AbnormalEffect.PurpleEnergy); //Purple Energy
            Table.Add(AbnormalEffect.GreenEnergy); //Green Energy
            Table.Add(AbnormalEffect.BurningFlames); //Burning Flames
            Table.Add(AbnormalEffect.ScorchingFlames); //Scorching Flames
            Table.Add(AbnormalEffect.BlizzardyStorm); //Blizzardy Storm
            Table.Add(AbnormalEffect.StormyStorm); //Stormy Storm
            Table.Add(AbnormalEffect.Cloud9); //Cloud 9
            Table.Add(AbnormalEffect.TheOoze); //The Ooze
            Table.Add(AbnormalEffect.StareFromBeyond); //Stare From Beyond
            Table.Add(AbnormalEffect.Amaranthine); //Amaranthine
        }

        public static void MakeTable()
        {
            AbnormalRollTable = new RollTable();
        }
    }
    
    public enum AbnormalEffect : int
    {
        Unknown = -1,
        None = 0,
        PurpleEnergy = 1,
        GreenEnergy = 2,
        BurningFlames = 3,
        ScorchingFlames = 4,
        BlizzardyStorm = 5,
        StormyStorm = 6,
        Cloud9 = 7,
        TheOoze = 8,
        StareFromBeyond = 9,
        Amaranthine = 10,
        Max
    }
}
