using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "New Grid Object", menuName = "Game/GridObjectPrefabs")]
    public class GridObjectPrefabs : ScriptableObject
    {
        public List<GridObject> objectsPrefabs;
    }
}