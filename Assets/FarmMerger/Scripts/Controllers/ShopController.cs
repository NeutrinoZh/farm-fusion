using System;
using System.Numerics;
using UnityEngine;
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
        private PurchasePopup _purchasePopup;

        [Inject]
        public ShopController(Screens screens, UpgradesData upgrades, GridLevels gridLevels, ResourceManager resourceManager, GameGrid grid, ShopUI shopUI, PurchasePopup purchasePopup)
        {
            _screens = screens;
            _upgrades = upgrades;
            _gridLevels = gridLevels;
            _resourceManager = resourceManager;
            _grid = grid;
            _ui = shopUI;
            _purchasePopup = purchasePopup;

            Initialize();
        }

        private void Initialize()
        {
            _ui.OnSelectProduct += ShowPopup;
            _purchasePopup.OnPurchase += PurchaseHandle;
        }

        private void ShowPopup(ShopProductData product)
        {
            _screens.ShowPurchasePopup(product, GetPurchasePopupType(product));
        }

        private PurchasePopup.PopUpType GetPurchasePopupType(ShopProductData product)
        {
            if (_resourceManager.Money < product.Price)
                return PurchasePopup.PopUpType.NotEnoughMoney;

            if (product.IsObject && !_grid.GetRandomPosition(out Vector2Int _))
                return PurchasePopup.PopUpType.NotEnoughSpace;

            return PurchasePopup.PopUpType.Purchase;
        }
        
        private void PurchaseHandle(ShopProductData product)
        {
            _resourceManager.Money -= product.Price;
            
            switch (product.Id)
            {
                case ShopProductsEnum.IncreaseGridSize:
                    IncreaseGridSize();
                    break;
                
                case ShopProductsEnum.Nest:
                    SpawnObject(0);
                    break;
                
                case ShopProductsEnum.Barn:
                    SpawnObject(3);
                    break;
                
                case ShopProductsEnum.Beehive:
                    SpawnObject(6);
                    break;
                
                case ShopProductsEnum.PigBarn:
                    SpawnObject(8);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(product), product, null);
            }
        }

        private void SpawnObject(int type)
        {
            if (_grid.GetRandomPosition(out Vector2Int position))
                _grid.AddObject(new GridObjectData
                {
                    type = type,
                    position = position
                });
        }
        
        private void IncreaseGridSize()
        {
            int nextLevel = _upgrades.gridLevel + 1;
            if (nextLevel >= _gridLevels.levels.Count)
                return;

            var level = _gridLevels.levels[nextLevel];
            
            _upgrades.gridLevel = nextLevel;
            _grid.Resize(level.size);
        }
    }
}