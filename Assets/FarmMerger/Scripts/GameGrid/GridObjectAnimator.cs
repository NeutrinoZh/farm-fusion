using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class GridObjectAnimator : MonoBehaviour
    {
        private Vector3 _initScale;
        
        private void Start()
        {
            _initScale = transform.localScale;
            IdleAnimation();
        }

        private void IdleAnimation()
        {
            transform.localScale = _initScale * 0.95f;
            DOTween.To(
                () => transform.localScale, 
                value => transform.localScale = value,
                _initScale * 1.05f,
                1
            )
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);  
            
            transform.rotation = Quaternion.Euler(0, 0, 8);
            DOTween.To(
                () => transform.rotation.eulerAngles.z, 
                value => transform.rotation = Quaternion.Euler(0, 0, value),
                -8,
                1
            )
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);;  
        }
        
    }
}