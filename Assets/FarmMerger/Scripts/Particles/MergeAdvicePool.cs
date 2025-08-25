using UnityEngine;
using Zenject;

namespace Game
{
    public class MergeAdvicePool : MonoMemoryPool<Vector3, MergeAdviceParticle>
    {
        protected override void Reinitialize(Vector3 position, MergeAdviceParticle particleSystem)
        {
            particleSystem.gameObject.SetActive(true);
            particleSystem.transform.position = position;
            particleSystem.Play();
        }
    }
}