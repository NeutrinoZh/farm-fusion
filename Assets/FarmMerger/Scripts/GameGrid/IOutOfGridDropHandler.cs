using UnityEngine;

namespace Game
{
    public interface IOutOfGridDropHandler
    {
        public abstract void OnDrop(GridObject gridObject);
    }
}