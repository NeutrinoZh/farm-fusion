using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class PurchasePopup
    {
        public enum PopUpType
        {
            Purchase,
            NotEnoughMoney,
            NotEnoughSpace
        }
        
        private const string k_purchaseButtonId = "PurchaseButton";
        private const string k_purchaseLabelId = "PurchaseLabel";
        private const string k_closeButtonId = "CloseButton";
        private const string k_iconId = "Icon";
        private const string k_titleId = "Title";
        private const string k_descriptionId = "Description";
        private const string k_priceId = "Price";
        
        private const string k_purchaseText = "Purchase";
        private const string k_notEnoughMoneyText = "Not enough money";
        private const string k_notEnoughSpaceText = "Not enough space";
        
        private readonly Color k_normalButtonColor = Color.white; 
        private readonly Color k_disabledButtonColor = new(0.4f, 0.4f, 0.4f); 
        
        private readonly Color k_normalTextColor = new(0.314f, 0.212f, 0.6f);
        private readonly Color k_disabledTextColor = new(0.8f, 0.6f, 0.6f);
        
        private readonly Screens _screens;

        private readonly VisualElement _icon;
        private readonly Label _title;
        private readonly Label _description;
        private readonly Label _price;
        private readonly Label _purchaseLabel;
        private readonly VisualElement _purchaseButton;

        private ShopProductData _product;
        private PopUpType _type;
        
        public event Action<ShopProductData> OnPurchase;
        
        public PurchasePopup(Screens screens, VisualElement popup)
        {
            _screens = screens;
            
            _icon = popup.Q<VisualElement>(k_iconId);
            _title = popup.Q<Label>(k_titleId);
            _description = popup.Q<Label>(k_descriptionId);
            _price = popup.Q<Label>(k_priceId);
            _purchaseLabel = popup.Q<Label>(k_purchaseLabelId);
            
            var closeButton = popup.Q<VisualElement>(k_closeButtonId);
            closeButton.RegisterCallback<PointerDownEvent>(e => CloseButtonHandle());
            
            _purchaseButton = popup.Q<VisualElement>(k_purchaseButtonId);
            _purchaseButton.RegisterCallback<PointerDownEvent>(e => PurchaseButtonHandle());
        }

        private void PurchaseButtonHandle()
        {
            if (_type != PopUpType.Purchase)
                return;
            
            OnPurchase?.Invoke(_product);
            _screens.HideAll();
        }
        
        private void CloseButtonHandle()
        {
            _screens.HidePopups();
        }

        public void SetData(ShopProductData data, PopUpType type)
        {
            _type = type;
            _product = data;
            
            var background = _icon.style.backgroundImage.value;
            background.sprite = data.Icon;
            _icon.style.backgroundImage = background;
            
            _title.text = data.Title;
            _description.text = data.Description;
            _price.text = data.Price.ToString();

            switch (type)
            {
                case PopUpType.NotEnoughMoney:
                    _purchaseLabel.text = k_notEnoughMoneyText;
                    _purchaseLabel.style.color = k_disabledTextColor;
                    _purchaseButton.style.unityBackgroundImageTintColor = k_disabledButtonColor;
                    break;
                case PopUpType.NotEnoughSpace:
                    _purchaseLabel.text = k_notEnoughSpaceText;
                    _purchaseLabel.style.color = k_disabledTextColor;
                    _purchaseButton.style.unityBackgroundImageTintColor = k_disabledButtonColor;
                    break;
                case PopUpType.Purchase:
                    _purchaseLabel.text = k_purchaseText;
                    _purchaseLabel.style.color = k_normalTextColor;
                    _purchaseButton.style.unityBackgroundImageTintColor = k_normalButtonColor;
                    break;
            }
        }
    }
}