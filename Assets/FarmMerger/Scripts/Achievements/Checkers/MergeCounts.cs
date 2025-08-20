using System.Collections.Generic;

namespace Game.AchievementsCheckers
{
    public class MergeCounts : AchievementChecker
    {
        private readonly StatisticsManager _gameStatistics;
        private readonly GameStatistics.GridObjectType _gridObjectType;
        private readonly int _mergeCount;
        
        public MergeCounts(StatisticsManager gameStatistics, GameStatistics.GridObjectType type, int count)
        {
            _gameStatistics = gameStatistics;
            _gridObjectType = type;
            _mergeCount = count;
        }
        
        public override bool IsAchieved()
        {
            return _gameStatistics.Data.MergeCounts.GetValueOrDefault(_gridObjectType, 0) >= _mergeCount;
        }
    }
}