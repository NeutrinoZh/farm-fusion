using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public struct GameStatistics
    {
        [Serializable]
        public struct GridObjectType : IEquatable<GridObjectType>
        {
            public int Type;
            public int Level;

            public GridObjectType(int type, int level)
            {
                Type = type;
                Level = level;
            }

            public bool Equals(GridObjectType other)
            {
                return Type == other.Type && Level == other.Level;
            }

            public override bool Equals(object obj)
            {
                return obj is GridObjectType other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Type, Level);
            }
        }
        
        
        [JsonConverter(typeof(GridObjectTypeDictionaryConverter<int>))]
        public Dictionary<GridObjectType, int> MergeCounts { get; set; }
        
        [JsonConverter(typeof(GridObjectTypeDictionaryConverter<int>))]
        public Dictionary<GridObjectType, int> SoldCounts { get; set; }
        
        public int TotalCoinsEarned { get; set; }
        public int TotalEnergySpent { get; set; }
        public int CompletedOrdersCount { get; set; }
        
        public float TotalPlayTime { get; set; }
        public int SessionCount { get; set; }

        public static readonly GameStatistics k_defaultData = new()
        {
            MergeCounts = new(),
            SoldCounts = new(),
            TotalCoinsEarned = 0,
            TotalEnergySpent = 0,
            CompletedOrdersCount = 0,
            TotalPlayTime = 0,
            SessionCount = 0,
        };
    }
}