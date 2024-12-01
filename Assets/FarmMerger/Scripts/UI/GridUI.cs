using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Game
{
    public class GridUI
    {
        private UIDocument _uiDocument;
        private Screens _screens;

        private const string k_shopButton = "ShopButton";
        private const string k_moneyLabel = "MoneyLabel";
        private const string k_wheatLabel = "WheatLabel";

        private Label _moneyLabel;
        private string _moneyPattern;

        private Label _wheatLabel;
        private string _wheatPattern;

        private ResourceManager _resourceManager;

        public GridUI(Screens screens, UIDocument uiDocument, ResourceManager resourceManager)
        {
            _screens = screens;
            _uiDocument = uiDocument;
            _resourceManager = resourceManager;

            QueryElements();
            Subscribes();
        }

        private void QueryElements()
        {
            var root = _uiDocument.rootVisualElement;

            VisualElement shopButton = root.Query<VisualElement>(k_shopButton);
            shopButton.RegisterCallback<PointerDownEvent>(e => _screens.ShowShop(), TrickleDown.TrickleDown);

            _moneyLabel = root.Query<Label>(k_moneyLabel);
            _moneyPattern = _moneyLabel.text;

            _wheatLabel = root.Query<Label>(k_wheatLabel);
            _wheatPattern = _wheatLabel.text;
        }

        private void Subscribes()
        {
            _resourceManager.OnMoneyChange += UpdateMoneyLabel;
            _resourceManager.OnWheatChange += UpdateWheatLabel;

            UpdateMoneyLabel();
            UpdateWheatLabel();
        }

        private void OnDestroy()
        {
            _resourceManager.OnMoneyChange -= UpdateMoneyLabel;
            _resourceManager.OnWheatChange -= UpdateWheatLabel;
        }

        private void UpdateMoneyLabel()
        {
            _moneyLabel.text = _moneyPattern.Replace("{}", _resourceManager.Money.ToString());
        }

        private void UpdateWheatLabel()
        {
            _wheatLabel.text = _wheatPattern.Replace("{}", _resourceManager.Wheat.ToString());
        }
    }
}