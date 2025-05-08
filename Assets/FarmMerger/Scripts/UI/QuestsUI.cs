using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class QuestsUI
    {
        private const string k_iconId = "Icon";
        private const string k_counterId = "Counter";
        
        private VisualElement _questsPanel;
        private Screens _screens;
        private QuestManager _questManager;

        private VisualElement _icon;
        private Label _counter;

        public QuestsUI(Screens screens, VisualElement questsPanel, QuestManager questManager)
        {
            _screens = screens;
            _questsPanel = questsPanel;
            _questManager = questManager;
            
            _icon = questsPanel.Q<VisualElement>("Icon");
            _counter = questsPanel.Q<Label>("Counter");
            
            _questManager.OnChangeQuest += HandleQuest;
            _questManager.OnQuestProgress += HandleProgress;
            _questManager.NextQuest();
        }

        private void HandleQuest(Quest quest)
        {
            var background = _icon.style.backgroundImage.value;
            background.sprite = quest.Icon;
            _icon.style.backgroundImage = background;

            HandleProgress(quest, 0);
        }

        private void HandleProgress(Quest quest, int progress)
        {
            _counter.text = $"{progress}/{quest.Quantity}";
        }
    }
}