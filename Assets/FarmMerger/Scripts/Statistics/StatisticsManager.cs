using System;
using Zenject;

namespace Game
{
    public class StatisticsManager : IInitializable, IDisposable
    {
        public GameStatistics Data { get; set; }

        private readonly GameGrid _gameGrid;

        public StatisticsManager(GameGrid gameGrid)
        {
            _gameGrid = gameGrid;
        }
        
        public void Initialize()
        {
            _gameGrid.OnMerge += MergeHandle;
        }
        
        public void Dispose()
        {
            _gameGrid.OnMerge -= MergeHandle;
        }
        
        private void MergeHandle(int type, int level)
        {
            var objectType = new GameStatistics.GridObjectType(type, level);
            if (!Data.MergeCounts.TryAdd(objectType, 1))
                Data.MergeCounts[objectType]++;
        }
    }
}