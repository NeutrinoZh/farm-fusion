using UnityEngine;
using Zenject;

namespace Game
{
    public class GameGridController : MonoBehaviour
    {
        private GameGrid _grid;
        private GridPointer _pointer;

        [Inject]
        public void Construct(GameGrid grid, GridPointer pointer)
        {
            _grid = grid;
            _pointer = pointer;
        }

        private void Awake()
        {
            _grid.AddObject(new GridObjectData
            {
                prefabIndex = 1,
                position = new Vector2Int()
                {
                    x = Random.Range(0, _grid.Size.x),
                    y = Random.Range(0, _grid.Size.y),
                }
            });
        }
    }
}