using System;
using UnityEngine;

namespace Game
{
    public class GridObject : MonoBehaviour
    {
        public Action OnClick;

        private GridObjectData _data;
        private GameGrid _grid;

        public Vector2Int Position
        {
            get => _data.position;
            set
            {
                _data.position = value;
                ResetPosition();
            }
        }

        public void ResetPosition()
        {
            transform.localPosition = _grid.GridPositionToLocalPosition(_data.position);
        }

        public void Construct(GridObjectData data, GameGrid grid)
        {
            _data = data;
            _grid = grid;

            Position = _data.position;
        }
    }
}