using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GridPrefabs", menuName = "Game/GridPrefabs")]
    public class GridPrefabs : ScriptableObject
    {
        public List<GridObjectPrefabs> objectsPrefabs;
    }
}