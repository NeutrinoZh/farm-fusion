using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class MergeAdviceParticle : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        private void Start()
        {
            transform
                .DORotate(new Vector3(0, 0, 360), 6f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);       
        }
        
        public void Play()
        {
            _particleSystem.Play();
        }
    }
}