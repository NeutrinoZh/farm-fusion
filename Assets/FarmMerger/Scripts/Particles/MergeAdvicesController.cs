using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class MergeAdvicesController
    {
        private readonly MergeAdvicePool _mergeAdvicePool;
        private readonly GridDragDrop _gridDragDrop;
        private readonly GameGrid _gameGrid;
        
        private readonly List<MergeAdviceParticle> _advices;
        
        
        public MergeAdvicesController(GameGrid grid, GridDragDrop dragAndDrop, MergeAdvicePool mergeAdvicePool)
        {
            _advices = new();
            _gameGrid = grid;
            _gridDragDrop = dragAndDrop;
            _mergeAdvicePool = mergeAdvicePool;

            _gridDragDrop.OnStartDragging += HandleDragging;
            _gridDragDrop.OnEndDragging += HandleDrop;
        }

        ~MergeAdvicesController()
        {
            _gridDragDrop.OnStartDragging -= HandleDragging;
            _gridDragDrop.OnEndDragging -= HandleDrop;
        }

        private void HandleDragging(GridObject gridObject)
        {
            foreach (var obj in _gameGrid.Objects)
                if (
                    obj != gridObject &&
                    obj.Type == gridObject.Type &&
                    obj.Level == gridObject.Level)
                {
                    var advice = _mergeAdvicePool.Spawn(obj.transform.position);

                    float scale = _gameGrid.Scale * 0.1f;
                    advice.transform.localScale = new Vector3(scale, scale, 1);
                    
                    _advices.Add(advice);
                } 
        }

        private void HandleDrop(GridObject gridObject)
        {
            foreach (var advice in _advices)
                _mergeAdvicePool.Despawn(advice);
            _advices.Clear();
        }
    }
}