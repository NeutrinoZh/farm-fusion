using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        [Serializable]
        public struct SpawnableItem
        {
            public int typeId;
            public float chanceToDrop;
        };


        [SerializeField] private int _wheatCost;
        [SerializeField] public List<SpawnableItem> items;

        private ResourceManager _resourceManager;
        private GridObject _gridObject;
        private GameGrid _grid;

        [Inject]
        public void Construct(GameGrid grid, ResourceManager resourceManager)
        {
            _grid = grid;
            _resourceManager = resourceManager;
        }

        private void Awake()
        {
            _gridObject = GetComponent<GridObject>();
        }

        private void Start()
        {
            _gridObject.OnClick += HandleClick;
        }

        private void OnDestroy()
        {
            _gridObject.OnClick -= HandleClick;
        }

        private void HandleClick()
        {
            if (_resourceManager.Wheat < _wheatCost)
            {
                _resourceManager.OnNotEnoughWheat?.Invoke();
                return;
            }

            if (!_grid.GetRandomPosition(out Vector2Int position))
                return;

            if (items.Count == 0)
                return;

            float sumOfChances = 0;
            float random = UnityEngine.Random.Range(0.0f, 1.0f);

            SpawnableItem item = items[0];

            for (int i = 0; i < items.Count; ++i)
            {
                sumOfChances += items[i].chanceToDrop;
                if (sumOfChances > random)
                {
                    item = items[i];
                    break;
                }
            }

            _grid.AddObject(new GridObjectData
            {
                type = item.typeId,
                position = position
            });

            _resourceManager.Wheat -= _wheatCost;
        }
    }
}