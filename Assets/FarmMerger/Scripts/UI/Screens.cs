using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Game
{
    public class Screens : MonoBehaviour
    {
        public event Action OnHideAll;
        public event Action OnShowShop;
        public event Action OnShowAchievements;
        public event Action OnShowMap;
        public event Action OnShowAdvice;

        [SerializeField] private UIDocument _uiDocument;

        [SerializeField] private VisualTreeAsset _shopProductTemplate;
        [SerializeField] private List<ShopProductData> _shopProductsList;
        
        private const string k_hudId = "HUD";
        private const string k_shopId = "Shop";
        private const string k_achievementsId = "Achievements";
        private const string k_mapId = "Map";
        private const string k_adviceId = "Advice";
        private const string k_foggingId = "Fogging";
        private const string k_purchasePopupId = "PurchasePopup";
        private const string k_questsId = "SidePanel";
        private const string k_rewardPopupId = "AwardPopup";

        private const int k_bottomOffsetHide = -480;
        private const int k_bottomOffsetShow = 0;
        
        private const float k_opacityHide = 0;
        private const float k_opacityShow = 0.8f;
        private const float k_delayBeforeDisableFogging = 0.5f;

        private VisualElement _hudGroup;
        private VisualElement _shopGroup;
        private VisualElement _achievementGroup;
        private VisualElement _mapGroup;
        private VisualElement _adviceGroup;
        private VisualElement _foggingGroup;
        private VisualElement _purchasePopup;
        private VisualElement _questsGroup;
        private VisualElement _rewardPopup;

        private GridUI _gridUI;
        private QuestsUI _questsUI;
        private TabBarUI _tabBarUI;
        private ShopUI _shopUI;
        private AchievementUI _achievementUI;
        private PurchasePopup _purchasePopupUI;
        private RewardPopup _rewardPopupUI;
        
        private DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void Awake()
        {
            var root = _uiDocument.rootVisualElement;

            _hudGroup = root.Q<VisualElement>(k_hudId);
            _shopGroup = root.Q<VisualElement>(k_shopId);
            _achievementGroup = root.Q<VisualElement>(k_achievementsId);
            _mapGroup = root.Q<VisualElement>(k_mapId);
            _adviceGroup = root.Q<VisualElement>(k_adviceId);
            _foggingGroup = root.Q<VisualElement>(k_foggingId);
            _purchasePopup = root.Q<VisualElement>(k_purchasePopupId);
            _questsGroup = root.Q<VisualElement>(k_questsId);
            _rewardPopup = root.Q<VisualElement>(k_rewardPopupId);
            
            _gridUI = new GridUI(this, _uiDocument, _diContainer.Resolve<ResourceManager>());
            _diContainer.Bind<GridUI>().FromInstance(_gridUI);

            _tabBarUI = new TabBarUI(this, root);
            _diContainer.Bind<TabBarUI>().FromInstance(_tabBarUI);

            _shopUI = new ShopUI(root, _shopProductTemplate, _shopProductsList);
            _diContainer.Bind<ShopUI>().FromInstance(_shopUI);

            _purchasePopupUI = new PurchasePopup(this, _purchasePopup);
            _diContainer.Bind<PurchasePopup>().FromInstance(_purchasePopupUI);

            _questsUI = new QuestsUI(this, _questsGroup, _diContainer.Resolve<QuestManager>());
            _diContainer.Bind<QuestsUI>().FromInstance(_questsUI);

            _rewardPopupUI = new RewardPopup(this, _rewardPopup);
            _diContainer.Bind<RewardPopup>().FromInstance(_rewardPopupUI);

            _achievementUI = new AchievementUI(this, _achievementGroup);
            
            _diContainer.Bind<Screens>().FromInstance(this);
            _diContainer.Instantiate<ShopController>();
            _diContainer.Instantiate<RewardController>();

            HideAll();
        }
        
        public void HideAll()
        {
            _shopGroup.style.bottom = k_bottomOffsetHide;
            _achievementGroup.style.bottom = k_bottomOffsetHide;
            _mapGroup.style.bottom = k_bottomOffsetHide;
             _adviceGroup.style.opacity = k_opacityHide;

            HidePopups();
            
            OnHideAll?.Invoke();
        }

        public void ShowShop()
        {
            HideAll();
            _shopGroup.style.bottom = k_bottomOffsetShow;
            OnShowShop?.Invoke();
        }

        public void ShowAchievements()
        {
            HideAll();
            _achievementGroup.style.bottom = k_bottomOffsetShow;
            OnShowAchievements?.Invoke();
        }

        public void ShowMap()
        {
            HideAll();
            _mapGroup.style.bottom = k_bottomOffsetShow;
            OnShowMap?.Invoke();
        }

        public void ShowAdvice()
        {
            HideAll();
            _adviceGroup.style.opacity = k_opacityShow;
            OnShowAdvice?.Invoke();
        }

        public void ShowPurchasePopup(ShopProductData data, PurchasePopup.PopUpType type)
        {
            _purchasePopupUI.SetData(data, type);
            
            _purchasePopup.style.bottom = k_bottomOffsetShow;
            _foggingGroup.style.display = DisplayStyle.Flex;
            _foggingGroup.style.opacity = k_opacityShow;
        }

        public void ShowRewardPopup(int reward)
        {
            _rewardPopupUI.SetData(reward);
            
            _rewardPopup.style.bottom = k_bottomOffsetShow;
            _foggingGroup.style.display = DisplayStyle.Flex;
            _foggingGroup.style.opacity = k_opacityShow;
        }
        
        public void HidePopups()
        {
            _rewardPopup.style.bottom = k_bottomOffsetHide;
            _purchasePopup.style.bottom = k_bottomOffsetHide;
            _foggingGroup.style.opacity = k_opacityHide;
            
            StartCoroutine(SwitcherDisplayPopupCoroutine());
        }

        private IEnumerator SwitcherDisplayPopupCoroutine()
        {
            yield return new WaitForSeconds(k_delayBeforeDisableFogging);
            _foggingGroup.style.display = DisplayStyle.None;
        }
    }
}