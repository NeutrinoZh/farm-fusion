using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Game
{
    public class ShopUI
    {
        public event Action OnBuyIncreaseGridSize;
        
        private Screens _screens;
        private VisualElement _shopRoot;
        private VisualElement _root;
        
        private ShopProductsList _shopProductsList;

        public ShopUI(Screens screens, VisualElement shopGroup, VisualElement root, VisualTreeAsset template, List<ShopProductData> shopProductsList)
        {
            _screens = screens;
            _shopRoot = shopGroup;
            _root = root;

            _shopProductsList = new ShopProductsList();
            _shopProductsList.InitializeList(_shopRoot, template, shopProductsList);
        }
    }
}