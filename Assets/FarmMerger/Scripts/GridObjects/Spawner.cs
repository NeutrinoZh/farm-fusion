using UnityEngine;
using Zenject;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private int _wheatCost;

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

            _grid.AddObject(new GridObjectData
            {
                type = 1,
                position = position
            });

            _resourceManager.Wheat -= _wheatCost;
        }
    }
}