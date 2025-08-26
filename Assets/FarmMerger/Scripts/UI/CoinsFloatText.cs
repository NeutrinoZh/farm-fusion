using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game
{
    public class CoinsFloatText : MonoBehaviour
    {
        public class CoinsFloatTextPool : MonoMemoryPool<int, CoinsFloatText>
        {
            protected override void Reinitialize(int coinsCount, CoinsFloatText text)
            {
                text.PlayText(coinsCount);
                text.gameObject.SetActive(true);
            }
        }

        [SerializeField] private float _endAnimatedOffset;
        [SerializeField] private float _animatedDuration;
        
        private Transform _startAnimatedPoint;
        private TextMeshProUGUI _textMesh;
        private CoinsFloatTextPool _pool;
        private float _initialY;
        private string _textPattern;

        [Inject]
        public void Construct(Tradesman tradesman, CoinsFloatTextPool pool)
        {
            _pool = pool;
            _startAnimatedPoint = tradesman.transform.GetChild(0);
        }

        private void PlayText(int coinsCount)
        {
            _textMesh.text = _textPattern.Replace("{}", coinsCount.ToString());
            
            transform.position = _startAnimatedPoint.position;
            transform.DOMoveY(
                _initialY + _endAnimatedOffset,
                _animatedDuration
            ).OnComplete(HandleCompleteAnimation);
        }
        
        private void Awake()
        {
            _textMesh = GetComponentInChildren<TextMeshProUGUI>();
            _textPattern = _textMesh.text;
            
            _initialY = transform.position.y;
        }
        
        private void HandleCompleteAnimation()
        {
            _pool.Despawn(this);
        }
    }
}