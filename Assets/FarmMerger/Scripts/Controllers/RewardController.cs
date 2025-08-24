using Zenject;

namespace Game
{
    public class RewardController
    {
        private AchievementPopup _achievementPopup;
        private RewardPopup _questPopup;
        private QuestManager _questManager;
        private ResourceManager _resourceManager;
        
        public RewardController(
            RewardPopup questPopup,
            ResourceManager resourceManager,
            QuestManager questManager,
            AchievementPopup achievementPopup
            )
        {
            _questPopup = questPopup;
            _questManager = questManager;
            _resourceManager = resourceManager;
            _achievementPopup = achievementPopup;

            _questPopup.OnCollectReward += OnCollectQuestReward;
            _achievementPopup.OnCollectReward += OnCollectAchievementReward;
        }

        ~RewardController()
        {
            _achievementPopup.OnCollectReward -= OnCollectAchievementReward;
            _questPopup.OnCollectReward -= OnCollectQuestReward;
        }

        private void OnCollectAchievementReward(int reward)
        {
            _resourceManager.Wheat += reward;
        }
        
        private void OnCollectQuestReward()
        {
            _resourceManager.Money += _questManager.CurrentQuest.Award;
            _questManager.NextQuest();
        }
    }
}