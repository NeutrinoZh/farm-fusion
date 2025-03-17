
using UnityEngine.UIElements;

namespace Game
{
    public class ShopProductController
    {
        private Label _productTitle;

        public void SetVisualElement(VisualElement visualElement)
        {
            _productTitle = visualElement.Q<Label>("ProductTitle");
        }

        public void SetProductData(ShopProductData shopProductData)
        {
            _productTitle.text = shopProductData.ProductTitle;
        }
    }
}
