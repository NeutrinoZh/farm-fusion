using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

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
        public static readonly GameGridData defaultData = new()
        {
            size = new Vector2Int(3, 4),
            objects = new()
        };

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
                (float)defaultData.size.x / size.x,
                (float)defaultData.size.x / size.x,
                1
            );
            _renderer.size = size;
        }
    };
}