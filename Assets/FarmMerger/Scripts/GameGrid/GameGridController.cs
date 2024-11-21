using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game
{
    public class GameGridController : MonoBehaviour, IPointerDownHandler
    {
        private GameGrid _grid;
        private GridPointer _pointer;

        [Inject]
        public void Construct(GameGrid grid, GridPointer pointer)
        {
            _grid = grid;
            _pointer = pointer;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            _pointer.Position = _grid.WorldPositionToGridPosition(worldPosition);
        }
    }
}