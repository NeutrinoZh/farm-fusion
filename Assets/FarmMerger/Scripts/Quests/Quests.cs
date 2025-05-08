using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    
    [CreateAssetMenu(fileName = "New Quests List", menuName = "Game/Quest List")]
    public class Quests : ScriptableObject
    {
        [field: SerializeField] public List<Quest> Qs { get; private set; }
    }
}