using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GridLevels", menuName = "Game/GridLevels", order = 0)]
    public class GridLevels : ScriptableObject
    {
        [Serializable]
        public struct GridLevel
        {
            public Vector2Int size;
            public int price;
        }

        public List<GridLevel> levels;
    }
}