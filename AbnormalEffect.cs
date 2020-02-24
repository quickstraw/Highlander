using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highlander
{
    public class RollTable
    {
        public List<float> Table;
        public static RollTable AbnormalRollTable = new RollTable();
        
        private RollTable()
        {
            Table = new List<float>();
            AddChances();
        }

        private void AddChances()
        {
            Table.Add(5f); //None
            Table.Add(5f); //Purple Energy
            Table.Add(5f); //Green Energy
            Table.Add(5f); //Burning Flames
            Table.Add(5f); //Scorching Flames
            Table.Add(5f); //Blizzardy Storm
            Table.Add(5f); //Stormy Storm
            Table.Add(5f); //Cloud 9
        }
    }
    
    enum AbnormalEffect : int
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
        Max
    }
}
