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
        private VisualElement _root;

        public ShopUI(Screens screens, VisualElement shopGroup)
        {
            _screens = screens;
            _root = shopGroup;

            Initialize();
        }

        private void Initialize()
        {
            VisualElement increaseGridSizeButton = _root.Query<VisualElement>(k_increaseGridSizeButtonId);
            increaseGridSizeButton.RegisterCallback<PointerDownEvent>(e => OnBuyIncreaseGridSize?.Invoke(), TrickleDown.TrickleDown);

            VisualElement exitButton = _root.Query<VisualElement>(k_exitButtonId);
            exitButton.RegisterCallback<PointerDownEvent>(e => _screens.HideAll(), TrickleDown.TrickleDown);
        }
    }
}