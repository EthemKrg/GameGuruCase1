using UnityEngine;
using Zenject;

namespace Injection
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        [SerializeField] private GridMenuController gridMenuController;
        [SerializeField] private GridBuilder gridBuilder;
        [SerializeField] private GridController gridController;

        public override void InstallBindings()
        {
            //Signals
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<GridRebuildedSignal>();

            //Bindings
            Container.Bind<GridMenuController>().FromInstance(gridMenuController).AsSingle();
            Container.Bind<GridBuilder>().FromInstance(gridBuilder).AsSingle();
            Container.Bind<GridController>().FromInstance(gridController).AsSingle();
        }
    }
}