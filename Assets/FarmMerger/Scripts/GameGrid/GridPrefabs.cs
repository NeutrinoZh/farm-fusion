using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GridPrefabs", menuName = "Game", order = 0)]
    public class GridPrefabs : ScriptableObject
    {
        public Transform cellPrefab;
        public List<Transform> objectsPrefabs;
    }
}