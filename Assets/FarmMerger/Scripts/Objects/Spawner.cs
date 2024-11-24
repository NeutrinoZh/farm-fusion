using UnityEngine;
using Zenject;

namespace Game
{
    public class Spawner : MonoBehaviour
    {
        private GridObject _gridObject;
        private GameGrid _grid;

        [Inject]
        public void Construct(GameGrid grid)
        {
            _grid = grid;
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
            if (_grid.GetRandomPosition(out Vector2Int position))
                _grid.AddObject(new GridObjectData
                {
                    prefabIndex = 0,
                    position = position
                });
        }
    }
}