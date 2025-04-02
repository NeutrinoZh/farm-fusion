using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GridObject : MonoBehaviour
    {
        public Action OnClick;

        private int _price;

        private GridObjectData _data;
        private GameGrid _grid;

        private SpriteRenderer _sprite;
        public SpriteRenderer Sprite => _sprite;

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
        public int Price => _price;

        private void Awake()
        {
            _sprite = transform.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _grid.OnResize += ResetPosition;
        }

        private void OnDestroy()
        {
            _grid.OnResize -= ResetPosition;
        }
        
        public void ResetPosition()
        {
            transform.localPosition = _grid.GridPositionToLocalPosition(_data.position);
        }

        public void Construct(GridObjectData data, GameGrid grid, int price)
        {
            _data = data;
            _grid = grid;
            _price = price;

            Position = _data.position;
        }

        public bool Compare(GridObject rhs)
        {
            return rhs.Type == Type && rhs.Level == Level;
        }
    }
}