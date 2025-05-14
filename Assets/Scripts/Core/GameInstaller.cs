using ResourceRailNetwork.Graph;
using ResourceRailNetwork.GraphRenderer;
using UnityEngine;
using Zenject;

namespace ResourceRailNetwork.Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GraphRenderer.GraphRenderer graphRenderer;
        [SerializeField] private RailNetworkGraph railNetworkGraph;
        
        public override void InstallBindings()
        {
            Container.Bind<IGraphRenderer>().FromInstance(graphRenderer).AsSingle();
            Container.Bind<IRailNetworkGraph>().FromInstance(railNetworkGraph).AsSingle().NonLazy();
        }
    }
}