using UnityEngine;
using Zenject;

namespace Game
{
    public class GridPointer : MonoBehaviour
    {
        [SerializeField] private Transform _pointer;

        private Vector2Int _position;
        private GameGrid _gameGrid;


        public Vector2Int Position
        {
            get => _position;
            set
            {
                _position = value;
                Reposition();
            }
        }

        [Inject]
        public void Construct(GameGrid gameGrid)
        {
            _gameGrid = gameGrid;
        }

        private void Awake()
        {
            _gameGrid.OnResize += Reposition;
            Reposition();
        }

        private void OnDestroy()
        {
            _gameGrid.OnResize -= Reposition;
        }

        private void Reposition()
        {
            _pointer.localPosition = _gameGrid.GridPositionToLocalPosition(_position);
        }
    }
}