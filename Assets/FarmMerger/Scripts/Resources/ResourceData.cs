using System;

namespace Game
{
    [Serializable]
    public struct ResourceData
    {
        public int moneyCount;
        public int wheatCount;

        public static readonly ResourceData k_defaultData = new()
        {
            moneyCount = 0,
            wheatCount = 1000
        };
    };
}