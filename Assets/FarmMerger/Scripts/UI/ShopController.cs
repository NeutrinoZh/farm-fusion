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
        private Screens _screens;

        [Inject]
        public ShopController(Screens screens, UpgradesData upgrades, GridLevels gridLevels, ResourceManager resourceManager, GameGrid grid, ShopUI shopUI)
        {
            _screens = screens;
            _upgrades = upgrades;
            _gridLevels = gridLevels;
            _resourceManager = resourceManager;
            _grid = grid;
            _ui = shopUI;

            Initialize();
        }

        private void Initialize()
        {
            _ui.OnBuyProduct += ShowPopup;
        }

        private void ShowPopup(ShopProductData product)
        {
            _screens.ShowPurchasePopup();
        }

        private void Product(ShopProductData product)
        {
            if (product.ProductId != 0)
                return;
            
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