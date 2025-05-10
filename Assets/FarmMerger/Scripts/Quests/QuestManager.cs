using System;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class QuestManager
    {
        private readonly Quests _quests;
        private readonly Tradesman _tradesman;

        private int _currentQuestProgress;
        private int _currentQuestIndex = -1;
        
        public Quest CurrentQuest { get; private set; }
        public bool IsQuestComplete { get; private set; }

        public int CurrentQuestIndex
        {
            get => _currentQuestIndex;
            set
            {
                _currentQuestIndex = value - 1;
                NextQuest();
            }
        }
        
        public int CurrentQuestProgress
        {
            get => _currentQuestProgress;
            set
            {
                _currentQuestProgress = value;
            
                if (_currentQuestProgress >= CurrentQuest.Quantity)
                {
                    IsQuestComplete = true;
                    OnQuestCompleted?.Invoke();
                }
                
                OnQuestProgress?.Invoke(CurrentQuest, value);
            }
        }

        public event Action<Quest> OnChangeQuest;
        public event Action<Quest, int> OnQuestProgress;
        public event Action OnQuestCompleted;
        
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
            IsQuestComplete = false;
            CurrentQuest = ++_currentQuestIndex >= _quests.Qs.Count ? GenerateRandomQuest() : _quests.Qs[_currentQuestIndex];
            OnChangeQuest?.Invoke(CurrentQuest);
        }

        private void HandleProgress(GridObject gridObject)
        {
            if (IsQuestComplete)
                return;
            
            if (
                gridObject.Data.type != CurrentQuest.RequestedWareType ||
                gridObject.Data.level != CurrentQuest.RequestedWareLevel
            )
                return;
            
            CurrentQuestProgress += 1;
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