using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class NotEnoughEnergyPopup 
    {
        private const string k_collectButtonId = "ShowAdButton";
        private const string k_closeButtonId = "CloseButton";
        
        private readonly Screens _screens;
        public event Action OnShowAd;
      
        public NotEnoughEnergyPopup(Screens screens, VisualElement popup)
        {
            _screens = screens;

            var showAdButton = popup.Q<VisualElement>(k_collectButtonId);
            showAdButton.RegisterCallback<PointerDownEvent>(e => HandleClick());

            var closeButton = popup.Q<VisualElement>(k_closeButtonId);
            closeButton.RegisterCallback<PointerDownEvent>(e => CloseButtonHandle());
        }
        
        private void CloseButtonHandle()
        {
            _screens.HidePopups();
        }
        
        private void HandleClick()
        {
            OnShowAd?.Invoke();
            _screens.HidePopups();
        }
    }
}