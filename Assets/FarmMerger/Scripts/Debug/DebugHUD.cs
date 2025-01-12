using System;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Game
{
    public class DebugHUD : MonoBehaviour
    {
        public event Action FreeCoin;

        [SerializeField] private UIDocument _debugHUD;

        private const string k_freeCoinButtonId = "FreeCoin";
        private VisualElement _freeCoinButton;

        private DebugCommands _debugCommands;

        private DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void Awake()
        {
            var root = _debugHUD.rootVisualElement;

            _freeCoinButton = root.Query<VisualElement>(k_freeCoinButtonId);
            _freeCoinButton.RegisterCallback<PointerDownEvent>(e => FreeCoin?.Invoke());
        }

        private void Start()
        {
            _diContainer.Bind<DebugHUD>().FromInstance(this).WhenInjectedInto<DebugCommands>();
            _debugCommands = _diContainer.Instantiate<DebugCommands>();
        }
    }
}