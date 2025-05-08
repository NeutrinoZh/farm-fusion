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
        
        public override void InstallBindings()
        {
            Container.Bind<QuestManager>().FromInstance(new QuestManager(_quests, _tradesman));
            Container.Bind<Env>().FromInstance(_env);
            Container.Bind<UpgradesData>().FromInstance(new UpgradesData()
            {
                gridLevel = 0
            }).NonLazy();

            Container.Bind<GridPrefabs>().FromInstance(_gridPrefabs);

            Container.Bind<GameGrid>().AsSingle().NonLazy();
            Container.Bind<Transform>().FromInstance(_gameGrid).WhenInjectedInto<GameGrid>();
            Container.Bind<GridData>().FromInstance(GridData.k_defaultData).WhenInjectedInto<GameGrid>();
            Container.Bind<GridLevels>().FromInstance(_gridLevels);
            Container.Bind<GridPointer>().FromInstance(_gridPointer);

            Container.Bind<ResourceManager>().AsSingle().NonLazy();
            Container.Bind<ResourceData>().FromInstance(ResourceData.k_defaultData).WhenInjectedInto<ResourceManager>();
            
            Container.Bind<Tradesman>().FromInstance(_tradesman).AsSingle();
            
        }
    }
}