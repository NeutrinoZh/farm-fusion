using UnityEngine;
using Zenject;

namespace Game
{
    public class GridPointer : MonoBehaviour
    {
        [SerializeField] private Transform _pointer;
        private Vector2Int _position;
        private GameGrid _gameGrid;

        [Inject]
        public void Construct(GameGrid gameGrid)
        {
            _gameGrid = gameGrid;
        }

        private void Awake()
        {
            _gameGrid.OnResize += GridResizeHandler;
            GridResizeHandler();
        }

        private void OnDestroy()
        {
            _gameGrid.OnResize -= GridResizeHandler;
        }

        private void GridResizeHandler()
        {
            Debug.Log(transform.position);
            transform.position = _gameGrid.GridPositionToWorldPosition(_position);
        }
    }
}