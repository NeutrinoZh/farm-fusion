using System;

namespace Game
{
    public class QuestManager
    {
        private Quests _quests;
        private Quest _currentQuest;
        private int _currentQuestIndex = -1;
        private int _currentQuestProgress;
        
        private Tradesman _tradesman;
        
        public Quest CurrentQuest => _currentQuest;

        public int CurrentQuestProgress
        {
            get => _currentQuestProgress;
            set
            {
                _currentQuestProgress = value;
                OnQuestProgress?.Invoke(_currentQuest, value);
            }
        }

        public event Action<Quest> OnChangeQuest;
        public event Action<Quest, int> OnQuestProgress;

        public QuestManager(Quests quests, Tradesman tradesman)
        {
            _quests = quests;
            _tradesman = tradesman;
            
            _tradesman.OnSellItem += HandleProgress;
        }

        ~QuestManager()
        {
            _tradesman.OnSellItem -= HandleProgress;
        }
        
        public void NextQuest()
        {
            CurrentQuestProgress = 0;
            _currentQuest = ++_currentQuestIndex >= _quests.Qs.Count ? GenerateRandomQuest() : _quests.Qs[_currentQuestIndex];
            OnChangeQuest?.Invoke(_currentQuest);
        }

        private void HandleProgress(GridObject gridObject)
        {
            if (
                gridObject.Data.type != CurrentQuest.RequestedWareType ||
                gridObject.Data.level != CurrentQuest.RequestedWareLevel
            )
                return;
            
            int nextLevel = CurrentQuestProgress + 1;
            
            if (nextLevel >= CurrentQuest.Quantity)
                NextQuest();
            else 
                CurrentQuestProgress = nextLevel;
        }

        private Quest GenerateRandomQuest()
        {
            Quest quest = _quests.Qs[new Random().Next(_quests.Qs.Count)];
            return new Quest(
                quest.Icon,
                quest.RequestedWareType,
                quest.RequestedWareLevel,
                quest.Quantity * new Random().Next(1, 4),
                quest.Award
            );
        }
    }
}