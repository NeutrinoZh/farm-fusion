using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Game
{
    public class ShopUI
    {
        private const string k_bodyId = "Body";
        private const string k_buyButtonId = "BuyButton";
        
        public event Action<ShopProductData> OnSelectProduct;
        
        public ShopUI(VisualElement root, VisualTreeAsset listEntryTemplate, List<ShopProductData> shopProductsList)
        {
            var listView = root.Q<ScrollView>(k_bodyId);
            
            foreach (ShopProductData shopProductData in shopProductsList)
            {
                var newListEntry = listEntryTemplate.Instantiate();

                var newListEntryLogic = new ShopProductController();
                newListEntry.userData = newListEntryLogic;

                newListEntryLogic.SetVisualElement(newListEntry);
                newListEntryLogic.SetProductData(shopProductData);
                
                var buyButton = newListEntry.Q<VisualElement>(k_buyButtonId);
                buyButton.RegisterCallback<PointerDownEvent>(e => Handle(shopProductData), TrickleDown.TrickleDown);
                
                listView.Add(newListEntry);
            }
        }

        private void Handle(ShopProductData productData)
        {
            OnSelectProduct?.Invoke(productData);
        }
    }
}