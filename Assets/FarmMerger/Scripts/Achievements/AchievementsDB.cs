using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "New Achievement DB", menuName = "Game/Achievement DB")]
    public class AchievementsDB : ScriptableObject
    {
        [field: SerializeField] public List<AchievementConfig> Achievements { get; set; }
    }
}