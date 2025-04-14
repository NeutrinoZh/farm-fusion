using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct Quest
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int RequestedWareType { get; private set; }
        [field: SerializeField] public string RequestedWareLevel { get; private set; }
        [field: SerializeField] public int Quantity { get; private set; }
        [field: SerializeField] public int Award { get; private set; }
    }
}