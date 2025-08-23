using UnityEngine;

namespace Game
{
    
    [CreateAssetMenu(fileName = "New Achievement", menuName = "Game/Achievement")]
    public class AchievementConfig : ScriptableObject
    {
        [field: SerializeField] public AchievementType Type { get; private set; }
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}