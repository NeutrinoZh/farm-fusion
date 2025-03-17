using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Game
{
    public class Screens : MonoBehaviour
    {
        public Action OnHideAll;
        public Action OnShowShop;
        public Action OnShowAchievements;

        [SerializeField] private UIDocument _uiDocument;

        [SerializeField] private VisualTreeAsset _shopProductTemplate;
        [SerializeField] private List<ShopProductData> _shopProductsList;
        
        private const string k_hudId = "HUD";
        private const string k_shopId = "Shop";
        private const string k_achievementsId = "Achievements";

        private const int k_bottomOffsetHide = -450;
        private const int k_bottomOffsetShow = 0;

        private VisualElement _hudGroup;
        private VisualElement _shopGroup;
        private VisualElement _achievementGroup;

        private GridUI _gridUI;
        private TabBarUI _tabBarUI;
        private ShopUI _shopUI;

        private DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void Awake()
        {
            var root = _uiDocument.rootVisualElement;

            _hudGroup = root.Q<VisualElement>(k_hudId);
            _shopGroup = root.Q<VisualElement>(k_shopId);
            _achievementGroup = root.Q<VisualElement>(k_achievementsId);

            _gridUI = new GridUI(this, _uiDocument, _diContainer.Resolve<ResourceManager>());
            _diContainer.Bind<GridUI>().FromInstance(_gridUI);

            _tabBarUI = new TabBarUI(this, root);
            _diContainer.Bind<TabBarUI>().FromInstance(_tabBarUI);

            _shopUI = new ShopUI(this, _shopGroup, root, _shopProductTemplate, _shopProductsList);
            _diContainer.Bind<ShopUI>().FromInstance(_shopUI);

            _diContainer.Instantiate<ShopController>();

            HideAll();
        }

        public void HideAll()
        {
            _shopGroup.style.bottom = k_bottomOffsetHide;
            _achievementGroup.style.bottom = k_bottomOffsetHide;
            
            OnHideAll?.Invoke();
        }

        public void ShowShop()
        {
            _shopGroup.style.bottom = k_bottomOffsetShow;
            OnShowShop?.Invoke();
        }

        public void ShowAchievements()
        {
            _achievementGroup.style.bottom = k_bottomOffsetShow;
            OnShowAchievements?.Invoke();
        }
    }
}