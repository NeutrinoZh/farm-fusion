using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class CoinsFloatTextController
    {
        private readonly CoinsFloatText.CoinsFloatTextPool _pool;
        private readonly Tradesman _tradesman;
        
        public CoinsFloatTextController(
            CoinsFloatText.CoinsFloatTextPool pool,
            Tradesman tradesman)
        {
            _tradesman = tradesman;
            _pool = pool;
            
            _tradesman.OnSellItem += HandleSellItem;
        }

        ~CoinsFloatTextController()
        {
            _tradesman.OnSellItem -= HandleSellItem;
        }
        
        private void HandleSellItem(GridObject gridObject)
        {
            _pool.Spawn(gridObject.Price);
        }
    }
}