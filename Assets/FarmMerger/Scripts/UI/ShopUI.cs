using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Game
{
    public class ShopUI
    {
        public event Action<ShopProductData> OnBuyProduct;
        
        public ShopUI(VisualElement root, VisualTreeAsset listEntryTemplate, List<ShopProductData> shopProductsList)
        {
            var listView = root.Q<ScrollView>("Body");
            
            foreach (ShopProductData shopProductData in shopProductsList)
            {
                var newListEntry = listEntryTemplate.Instantiate();

                var newListEntryLogic = new ShopProductController();
                newListEntry.userData = newListEntryLogic;

                newListEntryLogic.SetVisualElement(newListEntry);
                newListEntryLogic.SetProductData(shopProductData);
                
                var buyButton = newListEntry.Q<VisualElement>("BuyButton");
                buyButton.RegisterCallback<PointerDownEvent>(e => Handle(shopProductData), TrickleDown.TrickleDown);
                
                listView.Add(newListEntry);
            }
        }

        private void Handle(ShopProductData productData)
        {
            OnBuyProduct?.Invoke(productData);
        }
    }
}