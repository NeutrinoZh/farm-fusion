using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Zenject;

namespace Game
{
    public class GridDragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private GameGrid _grid;

        private GridObject _draggedObject;

        [Inject]
        public void Construct(GameGrid grid)
        {
            _grid = grid;
        }

        private void Start()
        {
            EnhancedTouchSupport.Enable();
        }

        private void Update()
        {
            if (_draggedObject == null)
                return;

            var fingers = UnityEngine.InputSystem.EnhancedTouch.Touch.fingers;
            if (fingers.Count == 0)
                return;

            var worldPosition = Camera.main.ScreenToWorldPoint(fingers[0].screenPosition);
            worldPosition.z = 0;
            _draggedObject.transform.position = worldPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            var position = _grid.WorldPositionToGridPosition(worldPosition);
            _draggedObject = _grid.GetObjectOnPosition(position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            var position = _grid.WorldPositionToGridPosition(worldPosition);

            if (!_grid.IsPositionCorrect(position))
            {
                _draggedObject.ResetPosition();
                _draggedObject = null;
                return;
            }

            if (_draggedObject)
            {
                var obj = _grid.GetObjectOnPosition(position);
                if (obj)
                {
                    if (obj.Compare(_draggedObject) && obj != _draggedObject && _grid.GetMaxLevel(obj.Type) > obj.Level)
                        _grid.Merge(obj, _draggedObject);
                    else
                        obj.Position = _draggedObject.Position;
                }

                _draggedObject.Position = position;
            }

            _draggedObject = null;
        }
    }
}