using Zenject;

namespace Game
{
    public class RewardController
    {
        private RewardPopup _popup;
        private QuestManager _questManager;
        private ResourceManager _resourceManager;
        
        [Inject]
        public RewardController(RewardPopup popup, ResourceManager resourceManager, QuestManager questManager)
        {
            _popup = popup;
            _questManager = questManager;
            _resourceManager = resourceManager;

            _popup.OnCollectReward += OnCollectReward;
        }

        ~RewardController()
        {
            _popup.OnCollectReward -= OnCollectReward;
        }

        private void OnCollectReward()
        {
            _resourceManager.Money += _questManager.CurrentQuest.Award;
            _questManager.NextQuest();
        }
    }
}