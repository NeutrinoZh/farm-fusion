namespace Game
{
    public class DebugCommands
    {
        private const int k_freeCoinCount = 100;

        private DebugHUD _debugHUD;
        private ResourceManager _resourceManager;

        public DebugCommands(DebugHUD debugHUD, ResourceManager resourceManager)
        {
            _debugHUD = debugHUD;
            _resourceManager = resourceManager;

            _debugHUD.FreeCoin += HandleFreeCoin;
        }

        ~DebugCommands()
        {
            _debugHUD.FreeCoin -= HandleFreeCoin;
        }

        private void HandleFreeCoin()
        {
            _resourceManager.Money += k_freeCoinCount;
        }
    }
}