using Zenject;
using UnityEngine;

namespace Game
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private Transform _gameGrid;
        [SerializeField] private GridPrefabs _gridPrefabs;

        public override void InstallBindings()
        {
            Container.Bind<GridPrefabs>().FromInstance(_gridPrefabs);

            Container.Bind<GameGrid>().AsSingle().NonLazy();
            Container.Bind<Transform>().FromInstance(_gameGrid).WhenInjectedInto<GameGrid>();
            Container.Bind<GameGridData>().FromInstance(new GameGridData()
            {
                size = new Vector2Int(4, 5),
                objects = new()
            }).WhenInjectedInto<GameGrid>();
        }
    }
}