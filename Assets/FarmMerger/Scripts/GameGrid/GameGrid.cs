using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace Game
{
    public class GameGrid
    {
        public event Action OnResize;
        public event Action<int, int> OnMerge;
        public event Action<int, int> OnAdd;

        public Vector2Int Size => _gridSize;
        public GridData Data => _data;
        
        private const string k_background = "Background";
        private const string k_envBackground = "Background";

        private readonly GridData _data;
        private readonly GridPrefabs _prefabs;
        private readonly GridLevels _gridLevels;
        private UpgradesManager _upgradesManager;

        private readonly Dictionary<int, GridObject> _objectInstances;
        private int _nextIndex;

        private readonly Transform _transform;
        private readonly SpriteRenderer _renderer;
        private readonly BoxCollider2D _collider;
        private readonly DiContainer _diContainer;
        private readonly ParticleSystem _mergeParticle;
        private readonly Env _env;

        private float _oddYOffset;
        private float _oddXOffset;
        private float _scale;
        private Vector2Int _gridSize;
        
        public List<GridObject> Objects => _objectInstances.Values.ToList();
        public float Scale => _scale;
        
        public GameGrid(
            Env env,
            GridData data,
            GridLevels gridLevels,
            UpgradesManager upgradesManager,
            GridPrefabs prefabs,
            Transform transform, 
            DiContainer container,
            [Inject(Id=Env.k_flashParticleId)] ParticleSystem mergeParticle
            )
        {
            _env = env;
            _data = data;
            _prefabs = prefabs;
            _gridLevels = gridLevels;
            _diContainer = container;
            _mergeParticle = mergeParticle;
            _upgradesManager = upgradesManager;
            
            _transform = transform;
            _renderer = transform.Find(k_background).GetComponent<SpriteRenderer>();
            _collider = transform.GetComponent<BoxCollider2D>();
            _objectInstances = new();
            _nextIndex = 0;
        }
        
        public GridObject AddObject(GridObjectData objectData, Vector3 position)
        {
            objectData.index = _nextIndex++;

            var data = _prefabs.objectsPrefabs[objectData.type].objectsPrefabs[objectData.level];
            var instance = _diContainer.InstantiatePrefab(data.prefab).GetComponent<GridObject>();
            var scale = instance.transform.localScale;

            instance.transform.parent = _transform;
            instance.transform.localScale = scale;
            instance.Construct(objectData, this, data.price, position);

            _data.objects.Add(objectData);
            _objectInstances.Add(objectData.index, instance);

            OnAdd?.Invoke(objectData.type, objectData.level);
            return instance;
        }

        public void Resize(Vector2Int size, Vector3 backgroundPosition)
        {
            _scale = (float)_gridLevels.levels[0].size.x / size.x;
            Vector3 scale3 = new Vector3(_scale, _scale, 1);

            _transform.localScale = scale3;

            _gridSize = size;
            _renderer.size = size;
            _collider.size = size;

            bool isXOdd = size.x % 2 == 0;
            bool isYOdd = size.y % 2 == 0;

            _oddXOffset = isXOdd ? 0f : 0.5f;
            _oddYOffset = isYOdd ? 0f : 0.5f;

            _env.Background.size = new Vector2(
                size.x * 2,
                size.y * 3f
            );
            _env.Background.transform.localScale = scale3;
            _env.Background.transform.localPosition = backgroundPosition;
            
            foreach (var bush in _env.Scalables)
                bush.transform.localScale = scale3;

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

        public Vector3 GridPositionToWorldPosition(Vector2Int position)
        {
            return _transform.TransformPoint(GridPositionToLocalPosition(position));
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
            if (
                !_objectInstances.ContainsKey(lhs.Index) ||
                !_objectInstances.ContainsKey(rhs.Index))
                return;

            if (lhs.Type != rhs.Type)
                return;

            int type = lhs.Type;
            int level = lhs.Level;

            var worldPosition = GridPositionToWorldPosition(lhs.Position);
            
            AddObject(new GridObjectData()
            {
                type = type,
                level = level + 1,
                position = lhs.Position
            }, worldPosition);
            
            RemoveObject(lhs);
            RemoveObject(rhs);
            
            _mergeParticle.transform.position = worldPosition;
            _mergeParticle.Play();
            
            OnMerge?.Invoke(type, level);
        }
    };
}