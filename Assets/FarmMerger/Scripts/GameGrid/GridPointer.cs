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
            _pointer.localPosition = _gameGrid.GridPositionToWorldPosition(_position);
            Debug.Log(_pointer.position);
        }
    }
}