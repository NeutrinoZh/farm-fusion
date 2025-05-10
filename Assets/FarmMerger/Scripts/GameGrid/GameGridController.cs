using System.IO;
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
    }
}