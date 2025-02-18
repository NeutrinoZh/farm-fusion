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

        [SerializeField] private UIDocument _uiDocument;

        private const string k_hudId = "HUD";
        private const string k_shopId = "Shop";

        private const int k_bottomOffsetHide = -450;
        private const int k_bottomOffsetShow = 0;

        private VisualElement _hudGroup;
        private VisualElement _shopGroup;

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

            _hudGroup = root.Query<VisualElement>(k_hudId);
            _shopGroup = root.Query<VisualElement>(k_shopId);

            _gridUI = new GridUI(this, _uiDocument, _diContainer.Resolve<ResourceManager>());
            _diContainer.Bind<GridUI>().FromInstance(_gridUI);

            _tabBarUI = new TabBarUI(this, root);
            _diContainer.Bind<TabBarUI>().FromInstance(_tabBarUI);

            _shopUI = new ShopUI(this, _shopGroup, root);
            _diContainer.Bind<ShopUI>().FromInstance(_shopUI);

            _diContainer.Instantiate<ShopController>();

            HideAll();
        }

        public void HideAll()
        {
            _shopGroup.style.bottom = k_bottomOffsetHide;
            OnHideAll?.Invoke();
        }

        public void ShowShop()
        {
            _shopGroup.style.bottom = k_bottomOffsetShow;
            OnShowShop?.Invoke();
        }
    }
}