using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Env : MonoBehaviour
    {
        public const string k_flashParticleId = "FlashParticle";
        
        [field: SerializeField] public SpriteRenderer Background { get; private set; }
        [field: SerializeField] public List<Transform> Scalables { get; private set; }
    }
}