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

        public int Type => _data.type;
        public int Level => _data.level;
        public int Index => _data.index;
        public GridObjectData Data => _data;

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

        public bool Compare(GridObject rhs)
        {
            return rhs.Type == Type && rhs.Level == Level;
        }
    }
}