using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "New Shop Product", menuName = "FarmMerger/Shop Product")]
    public class ShopProductData : ScriptableObject
    {
        [field: SerializeField] public ShopProductsEnum Id { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }    
        [field: SerializeField] public string Title { get; private set; }        
        [field: SerializeField] public string Description { get; private set; }  
        [field: SerializeField] public int Price { get; private set; }   
        [field: SerializeField] public bool IsObject { get; private set; }
    }
}