using System;
using UnityEngine.UIElements;

namespace Game
{
    public class PurchasePopup
    {
        private const string k_purchaseButtonId = "PurchaseButton";
        private const string k_closeButtonId = "CloseButton";
        private const string k_iconId = "Icon";
        private const string k_titleId = "Title";
        private const string k_descriptionId = "Description";
        private const string k_priceId = "Price";
        
        private Screens _screens;

        private VisualElement _icon;
        private Label _title;
        private Label _description;
        private Label _price;

        private ShopProductsEnum _product;
        
        public event Action<ShopProductsEnum> OnPurchase;
        
        public PurchasePopup(Screens screens, VisualElement popup)
        {
            _screens = screens;
            
            _icon = popup.Q<VisualElement>(k_iconId);
            _title = popup.Q<Label>(k_titleId);
            _description = popup.Q<Label>(k_descriptionId);
            _price = popup.Q<Label>(k_priceId);
            
            var closeButton = popup.Q<VisualElement>(k_closeButtonId);
            closeButton.RegisterCallback<PointerDownEvent>(e => CloseButtonHandle());
            
            var purchaseButton = popup.Q<VisualElement>(k_purchaseButtonId);
            purchaseButton.RegisterCallback<PointerDownEvent>(e => PurchaseButtonHandle());
        }

        private void PurchaseButtonHandle()
        {
            OnPurchase?.Invoke(_product);
            _screens.HideAll();
        }
        
        private void CloseButtonHandle()
        {
            _screens.HidePopups();
        }

        public void SetData(ShopProductData data)
        {
            _product = data.Id;
            
            var background = _icon.style.backgroundImage.value;
            background.sprite = data.Icon;
            _icon.style.backgroundImage = background;
            
            _title.text = data.Title;
            _description.text = data.Description;
            _price.text = data.Price.ToString();
        }
    }
}