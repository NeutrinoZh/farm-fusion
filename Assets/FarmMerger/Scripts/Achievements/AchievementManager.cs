using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game
{
    public class AchievementManager : ITickable
    {
        private Dictionary<AchievementType, AchievementChecker>  _achievementCheckers;
        
        public AchievementsData Data { get; set; }
        
        public void SetAchievements(Dictionary<AchievementType, AchievementChecker> achievementCheckers)
        {
            _achievementCheckers = achievementCheckers; 
        }
        
        public bool IsUnlocked(AchievementType achievement)
        {
            return Data.UnlockedAchievements.Contains(achievement);
        }
        
        private void UnlockAchievement(AchievementType achievement)
        {
            Debug.Log($"UnlockAchievement: {achievement}");
            
            if (!Data.LockedAchievements.Contains(achievement))
                return;
            
            Data.LockedAchievements.Remove(achievement);
            Data.UnlockedAchievements.Add(achievement);
        }

        public void Tick()
        {
            var toUnlock = (
                from achievement in Data.LockedAchievements
                let checker = _achievementCheckers[achievement]
                where checker.IsAchieved()
                select achievement).ToList();

            foreach (var achievement in toUnlock)
                UnlockAchievement(achievement);
        }
    }
}