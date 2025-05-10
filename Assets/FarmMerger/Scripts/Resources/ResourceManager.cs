using System;
using UnityEngine;

namespace Game
{
    public class ResourceManager
    {
        public Action OnNotEnoughMoney;
        public Action OnNotEnoughWheat;

        public event Action OnMoneyChange;
        public event Action OnWheatChange;

        public ResourceData Data;
        
        private readonly LifeController _lifeController;
        private DateTime _pauseTime;
        
        private const float k_wheatPerSecondOffline = 0.2f;
        private const float k_wheatPerSecondOnline = 0.4f;

        public ResourceManager(LifeController lifeController, ResourceData data)
        {
            Data = data;
            _lifeController = lifeController;

            _lifeController.OnResume += HandleResume;
            _lifeController.OnPause += HandlePause;
            _lifeController.OnEverySecond += HandleSeconds;
        }

        ~ResourceManager()
        {
            _lifeController.OnResume -= HandleResume;
            _lifeController.OnPause -= HandlePause;
            _lifeController.OnEverySecond -= HandleSeconds;
        }
        
        public int Money
        {
            get => Data.moneyCount;
            set
            {
                Data.moneyCount = value;
                OnMoneyChange?.Invoke();
            }
        }

        public int Wheat
        {
            get => (int)Data.wheatCount;
            set
            {
                Data.wheatCount = value;
                OnWheatChange?.Invoke();
            }
        }

        public void HandleLaunch(DateTime closeTime)
        {
            _pauseTime = closeTime;
            HandleResume();
        }
        
        private void HandlePause()
        {
            _pauseTime = DateTime.Now;
        }

        private void HandleResume()
        {
            var seconds = (DateTime.Now - _pauseTime).Seconds;
            Wheat += (int)(seconds * k_wheatPerSecondOffline);
        }

        private void HandleSeconds()
        {
            Data.wheatCount += k_wheatPerSecondOnline;
            OnWheatChange?.Invoke();
        }
    }
}