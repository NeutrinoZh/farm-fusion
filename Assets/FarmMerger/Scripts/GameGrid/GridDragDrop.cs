using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Zenject;

namespace Game
{
    public class GridDragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        private const string k_layerOfInteractable = "Interactable";
        private const float k_dragSensitive = 0.5f * 0.5f;
        private const int k_draggedSpriteOrder = 100;

        private LayerMask _layerMask;
        private GameGrid _grid;

        private GridObject _draggedObject;
        private int _defaultSpriteOrder;

        private bool _dragging;
        private Vector3 _downPosition;

        [Inject]
        public void Construct(GameGrid grid)
        {
            _grid = grid;
        }

        private void Start()
        {
#if UNITY_EDITOR
            UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
#endif

            EnhancedTouchSupport.Enable();

            _layerMask = LayerMask.GetMask(k_layerOfInteractable);
        }

        private void Update()
        {
            if (!_dragging)
                return;

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
            _downPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            var position = _grid.WorldPositionToGridPosition(_downPosition);

            _draggedObject = _grid.GetObjectOnPosition(position);
            if (_draggedObject)
            {
                _defaultSpriteOrder = _draggedObject.Sprite.sortingOrder;
                _draggedObject.Sprite.sortingOrder = k_draggedSpriteOrder;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_dragging)
            {
                if (_draggedObject)
                    _draggedObject.Sprite.sortingOrder = _defaultSpriteOrder;
                _draggedObject = null;
                return;
            }

            _dragging = false;

            if (!_draggedObject)
                return;

            var worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            var position = _grid.WorldPositionToGridPosition(worldPosition);

            if (_grid.IsPositionCorrect(position))
                CorrectDrop();
            else
                IncorrectDrop();

            _draggedObject.Sprite.sortingOrder = _defaultSpriteOrder;
            _draggedObject = null;

            void CorrectDrop()
            {
                var obj = _grid.GetObjectOnPosition(position);
                if (obj)
                {
                    if (obj.Compare(_draggedObject) && obj != _draggedObject && _grid.GetMaxLevel(obj.Type) > obj.Level)
                        _grid.Merge(obj, _draggedObject);
                    else
                    {
                        obj.Position = _draggedObject.Position;
                    }
                }

                _draggedObject.Position = position;
            }

            void IncorrectDrop()
            {
                _draggedObject.ResetPosition();

                var hit = Physics2D.Raycast(worldPosition - Vector3.forward, Vector3.forward, _layerMask);
                if (hit && hit.transform.TryGetComponent(out IOutOfGridDropHandler outOfGridDropHandler))
                    outOfGridDropHandler.OnDrop(_draggedObject);
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            float sqrDistance = (_downPosition - Camera.main.ScreenToWorldPoint(eventData.position)).sqrMagnitude;
            if (sqrDistance > k_dragSensitive)
                _dragging = true;
        }
    }
}