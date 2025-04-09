using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "New Shop Product", menuName = "FarmMerger/Shop Product")]
    public class ShopProductData : ScriptableObject
    {
        [field: SerializeField] public int ProductId { get; private set; }
        [field: SerializeField] public string ProductTitle { get; private set; }        
    }
}