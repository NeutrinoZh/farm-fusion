using Zenject;
using UnityEngine;

namespace Game
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private Transform _gameGrid;
        [SerializeField] private GridPrefabs _gridPrefabs;
        [SerializeField] private GridPointer _gridPointer;

        public override void InstallBindings()
        {
            Container.Bind<GridPrefabs>().FromInstance(_gridPrefabs);

            Container.Bind<GameGrid>().AsSingle().NonLazy();
            Container.Bind<Transform>().FromInstance(_gameGrid).WhenInjectedInto<GameGrid>();
            Container.Bind<GridData>().FromInstance(new GridData()
            {
                size = new Vector2Int(4, 6),
                objects = new()
            }).WhenInjectedInto<GameGrid>();

            Container.Bind<GridPointer>().FromInstance(_gridPointer);
        }
    }
}