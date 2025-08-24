using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class AchievementPopup 
    {
        private const string k_iconId = "Icon";
        private const string k_rewardLabelId = "AwardLabel";
        private const string k_collectButtonId = "CollectRewardButton";
        private const string k_closeButtonId = "CloseButton";
        
        private readonly Screens _screens;
        
        private readonly VisualElement _popup;
        private readonly Label _rewardLabel;
        private readonly VisualElement _collectButton;
        private readonly VisualElement _icon;

        public event Action<int> OnCollectReward;

        private int _rewardAmount;
      
        public AchievementPopup(Screens screens, VisualElement popup)
        {
            _screens = screens;
            _popup = popup;
            
            _icon = _popup.Q<VisualElement>(k_iconId);
            _rewardLabel = _popup.Q<Label>(k_rewardLabelId);
            _collectButton = _popup.Q<VisualElement>(k_collectButtonId);
            
            _collectButton.RegisterCallback<PointerDownEvent>(e => HandleClick());

            var closeButton = _popup.Q<VisualElement>(k_closeButtonId);
            closeButton.RegisterCallback<PointerDownEvent>(e => CloseButtonHandle());
        }

        ~AchievementPopup()
        {
            _collectButton.UnregisterCallback<PointerDownEvent>(e => HandleClick());
        }
        
        private void CloseButtonHandle()
        {
            _screens.HidePopups();
        }
        
        private void HandleClick()
        {
            OnCollectReward?.Invoke(_rewardAmount);
            _screens.HidePopups();
        }

        public void SetData(AchievementConfig achievement)
        {
            _rewardAmount = achievement.AwardAmount;
            
            _rewardLabel.text = achievement.AwardAmount.ToString();
            
            var background = _icon.style.backgroundImage.value;
            background.sprite = achievement.Icon;
            _icon.style.backgroundImage = background;
        }
    }
}