using UnityEngine.UIElements;

namespace Game
{
    public class TabBarUI
    {
        private Screens _screens;
        private VisualElement _root;

        private const string k_shopButton = "ShopButton";
        private const string k_achievementsButton = "AchievementButton";
        private const string k_exitButton = "ExitButton";

        private VisualElement _shopButton;
        private VisualElement _achievementsButton;
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
            _shopButton = _root.Q<VisualElement>(k_shopButton);
            _shopButton.RegisterCallback<PointerDownEvent>(e => _screens.ShowShop(), TrickleDown.TrickleDown);

            _achievementsButton = _root.Q<VisualElement>(k_achievementsButton);
            _achievementsButton.RegisterCallback<PointerDownEvent>(e => _screens.ShowAchievements(), TrickleDown.TrickleDown);

            _exitButton = _root.Q<VisualElement>(k_exitButton);
            _exitButton.RegisterCallback<PointerDownEvent>(e => _screens.HideAll(), TrickleDown.TrickleDown);
        }

        private void Subscribes()
        {
            _screens.OnShowShop += () => ShowExitButtonForSection(_shopButton);
            _screens.OnShowAchievements += () => ShowExitButtonForSection(_achievementsButton);
            _screens.OnHideAll += HideExitButton;
        }

        private void HideExitButton()
        {
            _exitButton.style.display = DisplayStyle.None;
            _shopButton.style.display = DisplayStyle.Flex;
            _achievementsButton.style.display = DisplayStyle.Flex;
        }

        private void ShowExitButtonForSection(VisualElement section)
        {
            _exitButton.style.display = DisplayStyle.Flex;
            section.style.display = DisplayStyle.None;
            _exitButton.PlaceBehind(section);
        }
    }
}