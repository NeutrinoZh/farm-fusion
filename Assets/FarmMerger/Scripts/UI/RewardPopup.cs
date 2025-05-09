using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class RewardPopup 
    {
        private const string k_rewardLabelId = "AwardLabel";
        private const string k_collectButtonId = "CollectRewardButton";
        private const string k_closeButtonId = "CloseButton";
        
        private Screens _screens;
        
        private readonly VisualElement _popup;
        private readonly Label _rewardLabel;
        private readonly VisualElement _collectButton;

        public event Action OnCollectReward;
      
        public RewardPopup(Screens screens, VisualElement popup)
        {
            _screens = screens;
            _popup = popup;
            
            _rewardLabel = _popup.Q<Label>(k_rewardLabelId);
            _collectButton = _popup.Q<VisualElement>(k_collectButtonId);
            
            _collectButton.RegisterCallback<PointerDownEvent>(e => HandleClick());

            var closeButton = _popup.Q<VisualElement>(k_closeButtonId);
            closeButton.RegisterCallback<PointerDownEvent>(e => CloseButtonHandle());

        }

        ~RewardPopup()
        {
            _collectButton.UnregisterCallback<PointerDownEvent>(e => HandleClick());
        }
        
        private void CloseButtonHandle()
        {
            _screens.HidePopups();
        }
        
        private void HandleClick()
        {
            OnCollectReward?.Invoke();
            _screens.HidePopups();
        }

        public void SetData(int reward)
        {
            _rewardLabel.text = reward.ToString();
        }
    }
}