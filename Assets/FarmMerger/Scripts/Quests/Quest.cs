using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct Quest
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int RequestedWareType { get; private set; }
        [field: SerializeField] public int RequestedWareLevel { get; private set; }
        [field: SerializeField] public int Quantity { get; private set; }
        [field: SerializeField] public int Award { get; private set; }
        
        public Quest(Sprite icon, int requestedWareType, int requestedWareLevel, int quantity, int award)
        {
            Icon = icon;
            RequestedWareType = requestedWareType;
            RequestedWareLevel = requestedWareLevel;
            Quantity = quantity;
            Award = award;
        }
    }
}