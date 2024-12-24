using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace Game
{
    public class GameGrid
    {
        public Action OnResize;

        public Vector2Int Size => _data.size;

        private const string k_Background = "Background";
        private const string k_EnvBackground = "Background";

        private GridData _data;
        private GridPrefabs _prefabs;

        private Dictionary<int, GridObject> _objectInstances;
        private int _nextIndex;

        private Transform _transform;
        private SpriteRenderer _renderer;
        private BoxCollider2D _collider;
        private DiContainer _diContainer;
        private Env _env;

        private float _oddYOffset;
        private float _oddXOffset;

        public GameGrid(Env env, GridData data, GridPrefabs prefabs, Transform transform, DiContainer container)
        {
            _env = env;
            _data = data;
            _prefabs = prefabs;
            _transform = transform;
            _renderer = transform.Find(k_Background).GetComponent<SpriteRenderer>();
            _collider = transform.GetComponent<BoxCollider2D>();
            _objectInstances = new();
            _nextIndex = 0;
            _diContainer = container;

            Resize(_data.size);
        }

        public GridObject AddObject(GridObjectData gridObject)
        {
            gridObject.index = _nextIndex++;

            var data = _prefabs.objectsPrefabs[gridObject.type].objectsPrefabs[gridObject.level];
            var instance = _diContainer.InstantiatePrefab(data.prefab).GetComponent<GridObject>();
            var scale = instance.transform.localScale;

            instance.transform.parent = _transform;
            instance.transform.localScale = scale;
            instance.Construct(gridObject, this, data.price);

            _data.objects.Add(gridObject);
            _objectInstances.Add(gridObject.index, instance);

            return instance;
        }

        public void Resize(Vector2Int size)
        {
            _transform.localScale = new Vector3(
                (float)GridData.k_defaultData.size.x / size.x,
                (float)GridData.k_defaultData.size.x / size.x,
                1
            );


            _renderer.size = size;
            _collider.size = size;
            _data.size = size;

            bool isXOdd = size.x % 2 == 0;
            bool isYOdd = size.y % 2 == 0;

            _oddXOffset = isXOdd ? 0.5f : 0f;
            _oddYOffset = isYOdd ? 0.5f : 0f;

            _env.Background.size = size * 3;
            _env.Background.transform.position = new Vector3(
                isXOdd ? 0.15f : 0.1f,
                isYOdd ? 0f : 0f,
                0
            );

            OnResize?.Invoke();
        }

        public Vector3 GridPositionToLocalPosition(Vector2Int position)
        {
            return new Vector3(
               -Size.x / 2f + position.x + 0.5f,
               -Size.y / 2f + position.y + 0.5f,
                0
            );
        }

        public bool IsPositionCorrect(Vector2Int position)
        {
            return !(position.x < 0 || position.x >= Size.x || position.y < 0 || position.y >= Size.y);
        }

        public Vector2Int WorldPositionToGridPosition(Vector3 position)
        {
            var localPosition = _transform.InverseTransformPoint(position);
            return new Vector2Int(
                (int)(localPosition.x + Mathf.Ceil(Size.x / 2f) - _oddXOffset),
                (int)(localPosition.y + Mathf.Ceil(Size.y / 2f) - _oddYOffset)
            );
        }

        public bool HasObjectOnPosition(Vector2Int position)
        {
            foreach (var obj in _data.objects)
                if (obj.position == position)
                    return true;
            return false;
        }

        public GridObject GetObjectOnPosition(Vector2Int position)
        {
            foreach (var obj in _data.objects)
                if (obj.position == position)
                    return _objectInstances[obj.index];
            return null;
        }

        public bool GetRandomPosition(out Vector2Int position)
        {
            position = new Vector2Int();

            int countFreeCells = Size.x * Size.y - _objectInstances.Count;

            if (countFreeCells == 0)
                return false;

            for (int x = 0; x < Size.x; ++x)
                for (int y = 0; y < Size.y; ++y)
                {
                    position.x = x;
                    position.y = y;

                    if (HasObjectOnPosition(position))
                        continue;

                    if (countFreeCells == 1 || UnityEngine.Random.Range(0, 4) == 0)
                        return true;

                    countFreeCells -= 1;
                }

            return GetRandomPosition(out position);
        }

        public void RemoveObject(GridObject obj)
        {
            if (!_objectInstances.ContainsKey(obj.Index))
                return;

            _objectInstances.Remove(obj.Index);
            _data.objects.Remove(obj.Data);

            UnityEngine.Object.Destroy(obj.gameObject);
        }

        public int GetMaxLevel(int type)
        {
            return _prefabs.objectsPrefabs[type].objectsPrefabs.Count - 1;
        }

        public void Merge(GridObject lhs, GridObject rhs)
        {
            if (!_objectInstances.ContainsKey(lhs.Index) || !_objectInstances.ContainsKey(rhs.Index))
                return;

            if (lhs.Type != rhs.Type)
                return;

            AddObject(new GridObjectData()
            {
                type = lhs.Type,
                level = lhs.Level + 1,
                position = lhs.Position
            });

            RemoveObject(lhs);
            RemoveObject(rhs);
        }
    };
}