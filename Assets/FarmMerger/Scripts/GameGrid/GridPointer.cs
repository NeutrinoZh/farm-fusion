using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game
{
    public class GridPointer : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform _pointer;
        private SpriteRenderer _renderer;

        private Vector2Int _position;
        private GameGrid _grid;

        private GridObject _selectedObject;

        public Vector2Int Position
        {
            get => _position;
            set
            {
                _position = value;
                _renderer.enabled = _grid.HasObjectOnPosition(_position);
                Reposition();
            }
        }

        [Inject]
        public void Construct(GameGrid gameGrid)
        {
            _grid = gameGrid;
        }

        private void Awake()
        {
            _renderer = _pointer.GetComponent<SpriteRenderer>();
            _renderer.enabled = false;

            _grid.OnResize += Reposition;
        }

        private void OnDestroy()
        {
            _grid.OnResize -= Reposition;
        }

        private void Reposition()
        {
            _pointer.localPosition = _grid.GridPositionToLocalPosition(_position);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            var position = _grid.WorldPositionToGridPosition(worldPosition);

            if (_selectedObject && position == _position)
                _selectedObject.OnClick?.Invoke();
            else
            {
                Position = position;
                _selectedObject = _grid.GetObjectOnPosition(_position);
            }
        }
    }
}