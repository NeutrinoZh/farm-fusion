using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class QuestsUI
    {
        private const string k_iconId = "Icon";
        private const string k_counterId = "Counter";
        private const string k_completedIconId = "CompletedIcon";
        
        private readonly VisualElement _questsPanel;
        private readonly Screens _screens;
        private readonly QuestManager _questManager;

        private readonly VisualElement _icon;
        private readonly VisualElement _completedIcon;
        private readonly Label _counter;

        public QuestsUI(Screens screens, VisualElement questsPanel, QuestManager questManager)
        {
            _screens = screens;
            _questsPanel = questsPanel;
            _questManager = questManager;
            
            _icon = questsPanel.Q<VisualElement>(k_iconId);
            _counter = questsPanel.Q<Label>(k_counterId);
            _completedIcon = questsPanel.Q<VisualElement>(k_completedIconId);

            questsPanel.RegisterCallback<PointerDownEvent>(e => HandleClick());
            
            _questManager.OnChangeQuest += HandleQuest;
            _questManager.OnQuestProgress += HandleProgress;
            _questManager.OnQuestCompleted += HandleCompleteQuest;
            
            HandleQuest(_questManager.CurrentQuest);
            HandleProgress(_questManager.CurrentQuest, _questManager.CurrentQuestProgress);
            
            if (_questManager.IsQuestComplete)
                HandleCompleteQuest();
        }

        ~QuestsUI()
        {
            _questManager.OnChangeQuest -= HandleQuest;
            _questManager.OnQuestProgress -= HandleProgress;
            _questManager.OnQuestCompleted -= HandleCompleteQuest;
            
            _questsPanel.UnregisterCallback<PointerDownEvent>(e => HandleClick());
        }

        private void HandleClick()
        {
            if (_questManager.IsQuestComplete)
                _screens.ShowRewardPopup(_questManager.CurrentQuest.Award);
        }
        
        private void HandleCompleteQuest()
        {
            _counter.style.visibility = Visibility.Hidden;
            _completedIcon.style.visibility = Visibility.Visible;
        }

        private void HandleQuest(Quest quest)
        {
            _counter.style.visibility = Visibility.Visible;
            _completedIcon.style.visibility = Visibility.Hidden;
            
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