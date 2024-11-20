using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GridObject
    {
        public int prefabIndex;
        public Vector2Int position;
    }

    [Serializable]
    public struct GameGridData
    {
        public Vector2Int size;
        public List<GridObject> objects;
    };

    public class GameGrid
    {
        public static readonly GameGridData k_defaultData = new()
        {
            size = new Vector2Int(3, 4),
            objects = new()
        };


        public Action OnResize;

        public Vector2Int Size => _data.size;

        private const string k_Background = "Background";

        private GameGridData _data;
        private GridPrefabs _prefabs;

        private Transform _transform;
        private SpriteRenderer _renderer;

        public GameGrid(GameGridData data, GridPrefabs prefabs, Transform transform)
        {
            _data = data;
            _prefabs = prefabs;
            _transform = transform;
            _renderer = transform.Find(k_Background).GetComponent<SpriteRenderer>();

            Resize(_data.size);
        }

        public void Resize(Vector2Int size)
        {
            _transform.localScale = new Vector3(
                (float)k_defaultData.size.x / size.x,
                (float)k_defaultData.size.x / size.x,
                1
            );
            _renderer.size = size;
            _data.size = size;

            OnResize?.Invoke();
        }

        public Vector3 GridPositionToWorldPosition(Vector2Int position)
        {
            return _transform.position + new Vector3(
                position.x - Size.x / 2,
                position.y - Size.y / 2,
                0
            );
        }
    };
}