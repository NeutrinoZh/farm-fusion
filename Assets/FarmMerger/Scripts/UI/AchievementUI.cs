using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Game
{
    public class AchievementUI
    {
        private const string k_newAchievementTabId = "NewTab";
        private const string k_completedAchievementTabId = "CompletedTab";
        
        private const string k_newAchievementListId = "NewList";
        private const string k_completedAchievementListId = "CompletedList";

        private const string k_activeTabClassName = "active";
        
        private readonly VisualElement _page;
        private readonly Screens _screens;
        
        private readonly VisualElement _newAchievementsTab;
        private readonly VisualElement _completedAchievementsTab;
        
        private readonly ScrollView _newAchievementsList;
        private readonly ScrollView _completedAchievementsList;
        
        private readonly AchievementScrollView _newAchievementsController;
        private readonly AchievementScrollView _completedAchievementsController;

        private VisualElement _activeTab;
        
        public AchievementUI(
            DiContainer container,
            Screens screens,
            VisualTreeAsset achievementTemplate,
            VisualElement page)
        {
            _screens = screens;
            _page = page;

            _newAchievementsTab = page.Q(k_newAchievementTabId);
            _completedAchievementsTab = page.Q(k_completedAchievementTabId);
            
            _newAchievementsList = page.Q<ScrollView>(k_newAchievementListId);
            _completedAchievementsList = page.Q<ScrollView>(k_completedAchievementListId);

            _newAchievementsController = container.Instantiate<AchievementScrollView>(new object[] {
                achievementTemplate,
                _newAchievementsList
            });
            
            _newAchievementsTab.RegisterCallback<PointerDownEvent>(SetActiveNewTab);
            _completedAchievementsTab.RegisterCallback<PointerDownEvent>(SetActiveCompletedTab);
        }

        private void SetActiveNewTab(PointerDownEvent evt)
        {
            _newAchievementsTab.AddToClassList(k_activeTabClassName);
            _completedAchievementsTab.RemoveFromClassList(k_activeTabClassName);
            
            _newAchievementsList.style.visibility = Visibility.Visible;
            _completedAchievementsList.style.visibility = Visibility.Hidden;
        }

        private void SetActiveCompletedTab(PointerDownEvent evt)
        {
            _newAchievementsTab.RemoveFromClassList(k_activeTabClassName);
            _completedAchievementsTab.AddToClassList(k_activeTabClassName);
            
            _newAchievementsList.style.visibility = Visibility.Hidden;
            _completedAchievementsList.style.visibility = Visibility.Visible;
        }
    }
}