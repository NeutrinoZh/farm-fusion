using System;
using UnityEngine.UIElements;

namespace Game
{
    public class ShopUI
    {
        public event Action OnBuyIncreaseGridSize;

        private const string k_increaseGridSizeButtonId = "GridSizeButton";
        private const string k_exitButtonId = "ExitButton";

        private Screens _screens;
        private VisualElement _shopRoot;
        private VisualElement _root;

        public ShopUI(Screens screens, VisualElement shopGroup, VisualElement root)
        {
            _screens = screens;
            _shopRoot = shopGroup;
            _root = root;

            Initialize();
        }

        private void Initialize()
        {
            VisualElement increaseGridSizeButton = _shopRoot.Query<VisualElement>(k_increaseGridSizeButtonId);
            increaseGridSizeButton.RegisterCallback<PointerDownEvent>(e => OnBuyIncreaseGridSize?.Invoke(), TrickleDown.TrickleDown);

            VisualElement exitButton = _root.Query<VisualElement>(k_exitButtonId);
            exitButton.RegisterCallback<PointerDownEvent>(e => _screens.HideAll(), TrickleDown.TrickleDown);
        }
    }
}