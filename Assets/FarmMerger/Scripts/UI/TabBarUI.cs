using UnityEngine.UIElements;

namespace Game
{
    public class TabBarUI
    {
        private Screens _screens;
        private VisualElement _root;

        private const string k_shopButton = "ShopButton";
        private const string k_exitButton = "ExitButton";

        private VisualElement _shopButton;
        private VisualElement _exitButton;

        public TabBarUI(Screens screens, VisualElement root)
        {
            _screens = screens;
            _root = root;

            QueryElements();
            Subscribes();
        }

        private void QueryElements()
        {
            _shopButton = _root.Query<VisualElement>(k_shopButton);
            _shopButton.RegisterCallback<PointerDownEvent>(e => _screens.ShowShop(), TrickleDown.TrickleDown);

            _exitButton = _root.Query<VisualElement>(k_exitButton);
            _exitButton.RegisterCallback<PointerDownEvent>(e => _screens.HideAll(), TrickleDown.TrickleDown);

        }

        private void Subscribes()
        {
            _screens.OnShowShop += () => ShowExitButtonForSection(_shopButton);
            _screens.OnHideAll += HideExitButton;
        }

        private void HideExitButton()
        {
            _exitButton.style.display = DisplayStyle.None;
            _shopButton.style.display = DisplayStyle.Flex;
        }

        private void ShowExitButtonForSection(VisualElement section)
        {
            _exitButton.style.display = DisplayStyle.Flex;
            section.style.display = DisplayStyle.None;
            _exitButton.PlaceBehind(section);
        }
    }
}