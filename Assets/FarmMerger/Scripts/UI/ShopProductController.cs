
using UnityEngine.UIElements;

namespace Game
{
    public class ShopProductController
    {
        private const string k_titleId = "Title";
        private const string k_iconId = "Icon";
        private const string k_priceId = "Price";
        
        private Label _title;
        private Label _price;
        private VisualElement _icon;

        public void SetVisualElement(VisualElement visualElement)
        {
            _title = visualElement.Q<Label>(k_titleId);
            _icon = visualElement.Q<VisualElement>(k_iconId);
            _price = visualElement.Q<Label>(k_priceId);
        }

        public void SetProductData(ShopProductData data)
        {
            _title.text = data.Title;
            _price.text = data.Price.ToString();
            
            var background = _icon.style.backgroundImage.value;
            background.sprite = data.Icon;
            _icon.style.backgroundImage = background;
        }
    }
}
