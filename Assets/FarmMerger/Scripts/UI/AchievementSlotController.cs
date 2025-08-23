using UnityEngine.UIElements;

namespace Game
{
    public class AchievementSlotController
    {
        private const string k_iconId = "Icon";
        private const string k_descriptionId = "Description";
        
        private VisualElement _icon;
        private Label _description;

        public void SetVisualElement(VisualElement visualElement)
        {
            _icon = visualElement.Q<VisualElement>(k_iconId);
            _description = visualElement.Q<Label>(k_descriptionId);
        }

        public void SetAchievementData(AchievementConfig data)
        {
            _description.text = data.Description;
            
            var background = _icon.style.backgroundImage.value;
            background.sprite = data.Icon;
            _icon.style.backgroundImage = background;
        }
    }
}