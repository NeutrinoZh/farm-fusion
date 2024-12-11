using UnityEngine;

namespace Game
{
    public class Env : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer Background { get; private set; }
    }
}