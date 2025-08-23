using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class AchievementScrollView
    {
        private readonly VisualTreeAsset _achievementTemplate;
        private readonly AchievementManager _achievementManager;
        private readonly AchievementsDB _achievementsDB;
        private readonly ScrollView _scrollView;
        
        private readonly List<AchievementSlotController> _achievementSlots;
        
        public AchievementScrollView(
            AchievementManager achievementManager,
            VisualTreeAsset achievementTemplate,
            AchievementsDB achievementsDB,
            ScrollView scrollView
            )
        {
            _achievementSlots = new();
            _achievementTemplate = achievementTemplate;
            _achievementManager = achievementManager;
            _achievementsDB = achievementsDB;
            _scrollView = scrollView;

            Initialize();
        }

        ~AchievementScrollView()
        {
            _achievementManager.OnAchievementsUpdated -= UpdateUI;
        }
        
        private void Initialize()
        {
            _achievementManager.OnAchievementsUpdated += UpdateUI;
        }

        private void UpdateUI()
        {
            var achievements = _achievementManager.Data.LockedAchievements;
            for (int i = 0; i < achievements.Count; ++i)
            {
                if (i >= _achievementSlots.Count)
                    InstantiateSlot();

                var achievementConfig = _achievementsDB.Achievements.Find(
                    item => item.Type == achievements[i]);
                
                _achievementSlots[i].SetAchievementData(achievementConfig);
            }
        }

        private void InstantiateSlot()
        {
            var newListEntry = _achievementTemplate.Instantiate();

            var newEntryLogic = new AchievementSlotController();
            newListEntry.userData = newEntryLogic;
            
            newEntryLogic.SetVisualElement(newListEntry);
            
            _achievementSlots.Add(newEntryLogic);
            _scrollView.Add(newListEntry);
        }
    }
}