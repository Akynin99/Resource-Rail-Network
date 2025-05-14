using ResourceRailNetwork.Core;
using ResourceRailNetwork.Graph;
using UnityEngine;
using Zenject;

namespace ResourceRailNetwork.Train
{
    /// <summary>
    /// Main coordinator class for train management system
    /// Orchestrates interactions between subsystems
    /// </summary>
    public class TrainController : MonoBehaviour
    {
        [SerializeField] private TrainConfig[] trainConfigs;

        [Inject] private IRailNetworkGraph _graph;
        [Inject] private IResourceWallet _wallet;

        private TrainModel[] _trains;
        private readonly TrainSpawner _trainSpawner = new();
        private RouteManager _routeManager;
        private TrainStateController _stateController;
        private TrainPathUpdater _pathUpdater;
        private TrainDebugger _debugger;

        /// <summary>
        /// Initializes dependencies and spawns trains
        /// Subscribes to network change events
        /// </summary>
        private void Start()
        {
            InitializeDependencies();
            SpawnTrains();
            _graph.OnGraphUpdated += RefreshBestRoutes;
        }

        private void InitializeDependencies()
        {
            _routeManager = new RouteManager(_graph);
            _stateController = new TrainStateController(_graph, _wallet);
            _pathUpdater = new TrainPathUpdater(_graph);
            _debugger = new TrainDebugger();
        }

        private void SpawnTrains()
        {
            _trains = _trainSpawner.SpawnTrains(trainConfigs, transform, _graph, this);
            RefreshBestRoutes();
        }

        /// <summary>
        /// Global update loop for all trains
        /// Delegates to state controller and debug systems
        /// </summary>
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            foreach (var train in _trains)
            {
                _stateController.UpdateState(train, deltaTime);
                _debugger.RefreshDebugInfo(train);
            }
        }

        private void OnDrawGizmos()
        {
            _debugger?.DrawGizmos(_trains);
        }

        private void OnDestroy()
        {
            _graph.OnGraphUpdated -= RefreshBestRoutes;
        }

        /// <summary>
        /// Forces recalculation of all train paths
        /// Triggered by network topology changes
        /// </summary>
        private void RefreshBestRoutes()
        {
            foreach (var train in _trains)
            {
                train.SetRoute(_routeManager.FindBestRoute(train));
                UpdateTrainPath(train);
            }
        }

        private void UpdateTrainPath(TrainModel train)
        {
            Node target;
            switch (train.State)
            {
                case TrainModel.TrainState.Moving:
                    target = train.BestRoute.Mine;
                    break;
                case TrainModel.TrainState.Delivering:
                    target = train.BestRoute.BaseStation;
                    break;
                default:
                    target = null;
                    break;
            }

            if (target != null)
            {
                _pathUpdater.RefreshNextNode(train, target);
            }
        }

        public void OnTrainSettingsChanged(TrainModel train)
        {
            train.SetRoute(_routeManager.FindBestRoute(train));
            UpdateTrainPath(train);
        }
    }
}