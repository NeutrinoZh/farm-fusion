namespace Game
{
    public class NotEnoughEnergyPopupController
    {
        private readonly ResourceManager _resourceManager;
        private Screens _screens;
        
        public NotEnoughEnergyPopupController(
            Screens screens,
            ResourceManager resourceManager
            )
        {
            _screens = screens;
            _resourceManager = resourceManager;

            _resourceManager.OnNotEnoughWheat += HandleNotEnoughWheat;
        }

        ~NotEnoughEnergyPopupController()
        {
            _resourceManager.OnNotEnoughWheat -= HandleNotEnoughWheat;
        }

        private void HandleNotEnoughWheat()
        {
            _screens.ShowNotEnoughEnergy();
        }
    }
}