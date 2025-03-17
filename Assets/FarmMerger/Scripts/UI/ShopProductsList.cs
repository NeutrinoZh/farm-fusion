using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class ShopProductsList
    {
        private VisualTreeAsset _listEntryTemplate;
        
        private ScrollView _listView;
        private List<ShopProductData> _shopProductsData;
        
        public void InitializeList(VisualElement root, VisualTreeAsset listEntryTemplate, List<ShopProductData> shopProductsData)
        {
            _shopProductsData = shopProductsData;
            
            _listEntryTemplate = listEntryTemplate;
            _listView = root.Q<ScrollView>("Body");
            
            foreach (ShopProductData shopProductData in _shopProductsData)
            {
                var newListEntry = listEntryTemplate.Instantiate();

                var newListEntryLogic = new ShopProductController();
                newListEntry.userData = newListEntryLogic;

                newListEntryLogic.SetVisualElement(newListEntry);
                
                _listView.Add(newListEntry);
            }
        }
    }
}