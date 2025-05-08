using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Tradesman : MonoBehaviour, IOutOfGridDropHandler
    {
        private GameGrid _grid;
        private ResourceManager _resourceManager;

        public event Action<GridObject> OnSellItem; 
        
        [Inject]
        public void Construct(GameGrid grid, ResourceManager resourceManager)
        {
            _grid = grid;
            _resourceManager = resourceManager;
        }

        public void OnDrop(GridObject gridObject)
        {
            OnSellItem?.Invoke(gridObject);
            
            _resourceManager.Money += gridObject.Price;
            _grid.RemoveObject(gridObject);
            Destroy(gridObject.gameObject);
        }
    }
}