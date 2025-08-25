using Zenject;
using UnityEngine;

namespace Game
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private Transform _gameGrid;
        [SerializeField] private GridPrefabs _gridPrefabs;
        [SerializeField] private GridPointer _gridPointer;
        [SerializeField] private GridLevels _gridLevels;
        [SerializeField] private Env _env;
        [SerializeField] private Quests _quests;
        [SerializeField] private Tradesman _tradesman;
        [SerializeField] private LifeController _lifeController;
        [SerializeField] private AchievementsDB _achievementsDB;
        [SerializeField] private ParticleSystem _flashRoundParticle;
        
        private SaveController _saveController;
        
        public override void InstallBindings()
        {
            Container.Bind<LifeController>().FromInstance(_lifeController).AsSingle();
            Container.Bind<QuestManager>().FromInstance(new QuestManager(_quests, _tradesman));
            Container.Bind<Env>().FromInstance(_env);
            Container.Bind<UpgradesManager>().FromInstance(new UpgradesManager(UpgradesData.k_defaultData));

            Container.Bind<GridPrefabs>().FromInstance(_gridPrefabs);

            Container.Bind<GameGrid>().AsSingle().NonLazy();
            Container.Bind<Transform>().FromInstance(_gameGrid).WhenInjectedInto<GameGrid>();
            Container.Bind<GridData>().FromInstance(GridData.k_defaultData).WhenInjectedInto<GameGrid>();
            Container.Bind<GridLevels>().FromInstance(_gridLevels);
            Container.Bind<GridPointer>().FromInstance(_gridPointer);

            Container.Bind<ResourceManager>().AsSingle().NonLazy();
            Container.Bind<ResourceData>().FromInstance(ResourceData.k_defaultData).WhenInjectedInto<ResourceManager>();

            Container.BindInterfacesAndSelfTo<StatisticsManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<AchievementManager>().AsSingle().NonLazy();
            Container.Bind<AchievementsDB>().FromInstance(_achievementsDB);
            
            Container.Bind<Tradesman>().FromInstance(_tradesman).AsSingle();

            Container.Bind<ParticleSystem>().WithId(Env.k_flashParticleId).FromInstance(_flashRoundParticle);
            
            _saveController = Container.Instantiate<SaveController>();
        }

        public void OnEnable()
        {
            Container
                .Resolve<AchievementManager>()
                .SetAchievements(new() {
                    { 
                        AchievementType.Merge10Eggs,
                        Container.Instantiate<AchievementsCheckers.MergeCounts>(new object[]{
                            new GameStatistics.GridObjectType(1, 0),
                            10
                        }) 
                    },
                    {
                        AchievementType.CreateChicken,
                        Container.Instantiate<AchievementsCheckers.CreateObject>(new object[]
                        {
                            new GameStatistics.GridObjectType(1, 2),
                        })
                    },
                    {
                        AchievementType.Merge100Eggs,
                        Container.Instantiate<AchievementsCheckers.MergeCounts>(new object[]
                        {
                            new GameStatistics.GridObjectType(1, 0),
                            100
                        })
                    }
            });
        }       
    }
}