using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "New Grid Object", menuName = "Game/GridObjectPrefabs")]
    public class GridObjectPrefabs : ScriptableObject
    {
        [Serializable]
        public struct GridObjectData
        {
            public GridObject prefab;
            public int price;
        };

        public List<GridObjectData> objectsPrefabs;
    }
}