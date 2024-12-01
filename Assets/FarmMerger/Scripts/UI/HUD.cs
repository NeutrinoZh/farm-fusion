using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Game
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;

        private const string k_moneyLabel = "MoneyLabel";
        private const string k_wheatLabel = "WheatLabel";

        private Label _moneyLabel;
        private string _moneyPattern;

        private Label _wheatLabel;
        private string _wheatPattern;

        private ResourceManager _resourceManager;

        [Inject]
        public void Construct(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        private void Awake()
        {
            var root = _uiDocument.rootVisualElement;

            _moneyLabel = root.Query<Label>(k_moneyLabel);
            _moneyPattern = _moneyLabel.text;

            _wheatLabel = root.Query<Label>(k_wheatLabel);
            _wheatPattern = _wheatLabel.text;
        }

        private void Start()
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