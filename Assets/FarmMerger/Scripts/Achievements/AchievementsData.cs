using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public struct AchievementsData
    {
        public List<AchievementType> LockedAchievements;
        public List<AchievementType> UnlockedAchievements;
        
        public static readonly AchievementsData k_defaultData = new()
        {
            LockedAchievements = new()
            {
                AchievementType.Merge10Eggs,
                AchievementType.CreateChicken,
            },
            UnlockedAchievements = new()
        };
    }
}