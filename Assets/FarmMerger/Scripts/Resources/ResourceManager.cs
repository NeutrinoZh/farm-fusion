using System;

namespace Game
{
    public class ResourceManager
    {
        public Action OnNotEnoughMoney;
        public Action OnNotEnoughWheat;

        public event Action OnMoneyChange;
        public event Action OnWheatChange;

        private ResourceData _data;

        public ResourceManager(ResourceData data)
        {
            _data = data;
        }

        public int Money
        {
            get => _data.moneyCount;
            set
            {
                _data.moneyCount = value;
                OnMoneyChange?.Invoke();
            }
        }

        public int Wheat
        {
            get => _data.wheatCount;
            set
            {
                _data.wheatCount = value;
                OnWheatChange?.Invoke();
            }
        }
    }
}