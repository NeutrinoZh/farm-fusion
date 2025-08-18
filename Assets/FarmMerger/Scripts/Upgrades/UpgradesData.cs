
using System;

namespace Game
{
    [Serializable]
    public struct UpgradesData
    {
        public int gridLevel;
        
        public static readonly UpgradesData k_defaultData = new()
        {
            gridLevel = 0,
        };
    }
}