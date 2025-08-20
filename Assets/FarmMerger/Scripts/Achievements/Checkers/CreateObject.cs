using System;
using UnityEngine;
using Zenject;

namespace Game.AchievementsCheckers
{
    public class CreateObject : AchievementChecker
    {
        private readonly GameGrid _grid;
        private readonly GameStatistics.GridObjectType _gridObjectType;
        private bool _achived;
        
        public CreateObject(GameGrid grid, GameStatistics.GridObjectType type)
        {
            _grid = grid;
            _gridObjectType = type;
            _achived = false;
            
            _grid.OnAdd += HandleCreateNewOne;
        }

        ~CreateObject()
        {
            _grid.OnAdd -= HandleCreateNewOne;   
        }
        
        public override bool IsAchieved()
        {
            return _achived;
        }

        private void HandleCreateNewOne(int type, int level)
        {  
            if (_achived)
                return;
            
            var objectType = new GameStatistics.GridObjectType(type, level);
            _achived = objectType.Equals(_gridObjectType);
        }
    }
}