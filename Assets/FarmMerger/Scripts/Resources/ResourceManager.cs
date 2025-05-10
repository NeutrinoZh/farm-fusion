using System;

namespace Game
{
    public class ResourceManager
    {
        public Action OnNotEnoughMoney;
        public Action OnNotEnoughWheat;

        public event Action OnMoneyChange;
        public event Action OnWheatChange;

        public ResourceData Data;

        public ResourceManager(ResourceData data)
        {
            Data = data;
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
            get => Data.wheatCount;
            set
            {
                Data.wheatCount = value;
                OnWheatChange?.Invoke();
            }
        }
    }
}