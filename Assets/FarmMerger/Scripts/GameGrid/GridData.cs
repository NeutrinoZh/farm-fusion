

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GridObjectData
    {
        public int index;
        public int type;
        public int level;

        public Vector2Int position;
    }

    [Serializable]
    public struct GridData
    {
        public Vector2Int size;
        public List<GridObjectData> objects;

        public static readonly GridData k_defaultData = new()
        {
            size = new Vector2Int(3, 4),
            objects = new()
        };
    };
}