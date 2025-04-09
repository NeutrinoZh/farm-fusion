using UnityEngine.UIElements;

namespace Game
{
    public class PurchasePopup
    {
        private const string k_closeButtonId = "CloseButton";
        
        private Screens _screens;
        
        public PurchasePopup(Screens screens, VisualElement popup)
        {
            _screens = screens;
            
            var closeButton = popup.Q<VisualElement>(k_closeButtonId);
            closeButton.RegisterCallback<PointerDownEvent>(e => Handle());
        }

        private void Handle()
        {
            _screens.HidePopups();
        }
    }
}