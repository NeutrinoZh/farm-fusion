using System;
using System.Numerics;
using Zenject;

namespace Game
{
    public class ShopController
    {
        private UpgradesData _upgrades;
        private ResourceManager _resourceManager;
        private GridLevels _gridLevels;
        private GameGrid _grid;
        private ShopUI _ui;

        [Inject]
        public ShopController(UpgradesData upgrades, GridLevels gridLevels, ResourceManager resourceManager, GameGrid grid, ShopUI shopUI)
        {
            _upgrades = upgrades;
            _gridLevels = gridLevels;
            _resourceManager = resourceManager;
            _grid = grid;
            _ui = shopUI;

            Initialize();
        }

        private void Initialize()
        {
            _ui.OnBuyIncreaseGridSize += IncreaseGridSize;
        }

        public void IncreaseGridSize()
        {
            int nextLevel = _upgrades.gridLevel + 1;
            if (nextLevel >= _gridLevels.levels.Count)
                return;

            var level = _gridLevels.levels[nextLevel];
            if (_resourceManager.Money < level.price)
                return;

            _resourceManager.Money -= level.price;
            _upgrades.gridLevel = nextLevel;
            _grid.Resize(level.size);
        }
    }
}