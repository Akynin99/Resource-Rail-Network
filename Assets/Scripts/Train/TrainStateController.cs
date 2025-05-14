using ResourceRailNetwork.Core;
using ResourceRailNetwork.Graph;

namespace ResourceRailNetwork.Train
{
    /// <summary>
    /// Handles state transitions and behavior for train movement phases
    /// Implements core state machine logic for train operations
    /// </summary>
    public class TrainStateController
    {
        private readonly IRailNetworkGraph _graph;
        private readonly IResourceWallet _wallet;

        public TrainStateController(IRailNetworkGraph graph, IResourceWallet wallet)
        {
            _graph = graph;
            _wallet = wallet;
        }

        /// <summary>
        /// Main update loop for train states
        /// </summary>
        public void UpdateState(TrainModel train, float deltaTime)
        {
            switch (train.State)
            {
                case TrainModel.TrainState.Moving:
                    UpdateMovingState(train, deltaTime);
                    break;

                case TrainModel.TrainState.Mining:
                    UpdateMiningState(train, deltaTime);
                    break;

                case TrainModel.TrainState.Delivering:
                    UpdateDeliveringState(train, deltaTime);
                    break;
            }
        }

        private void UpdateMovingState(TrainModel train, float deltaTime)
        {
            if (!train.NextNode)
            {
                train.SetNextNode(train.LastNode);
                HandleMovementCompletion(train);
                train.RefreshViewPos();
            }

            float dist = deltaTime * train.Speed;
            float diff = dist / _graph.GetDistance(train.LastNode, train.NextNode);
            train.IncrementProgress(diff);

            if (train.Progress >= 1)
            {
                HandleMovementCompletion(train);
            }

            train.RefreshViewPos();
        }

        /// <summary>
        /// Handles completion of movement segment
        /// Either starts mining or continues to next node
        /// </summary>
        private void HandleMovementCompletion(TrainModel train)
        {
            train.ResetProgress();
            train.SetLastNode(train.NextNode);

            if (train.LastNode == train.BestRoute.Mine)
            {
                StartMining(train);
            }
            else
            {
                UpdateTrainPath(train, train.BestRoute.Mine);
            }
        }

        private void StartMining(TrainModel train)
        {
            train.SetState(TrainModel.TrainState.Mining);
            train.ResetMiningTimer();
            train.SetLastMine(train.BestRoute.Mine);
        }

        private void UpdateMiningState(TrainModel train, float deltaTime)
        {
            train.IncrementMiningTimer(deltaTime);

            if (train.MiningTimer >= train.MiningDuration * train.LastMine.TimeMult)
            {
                StartDelivery(train);
            }
        }

        private void StartDelivery(TrainModel train)
        {
            train.SetCargo(true);
            train.SetState(TrainModel.TrainState.Delivering);
            train.ResetProgress();
            UpdateTrainPath(train, train.BestRoute.BaseStation);
            train.SetLastMine(null);
        }

        private void UpdateDeliveringState(TrainModel train, float deltaTime)
        {
            float dist = deltaTime * train.Speed;
            float diff = dist / _graph.GetDistance(train.LastNode, train.NextNode);
            train.IncrementProgress(diff);

            if (train.Progress >= 1)
            {
                HandleDeliveryCompletion(train);
            }

            train.RefreshViewPos();
        }

        private void HandleDeliveryCompletion(TrainModel train)
        {
            train.ResetProgress();
            train.SetLastNode(train.NextNode);

            if (train.LastNode == train.BestRoute.BaseStation)
            {
                CompleteDelivery(train);
            }
            else
            {
                UpdateTrainPath(train, train.BestRoute.BaseStation);
            }
        }

        private void CompleteDelivery(TrainModel train)
        {
            if (train.HasCargo)
            {
                _wallet.AddResource(train.BestRoute.BaseStation.ResourceMult);
                train.SetCargo(false);
            }

            train.SetState(TrainModel.TrainState.Moving);
        }

        private void UpdateTrainPath(TrainModel train, Node target)
        {
            train.SetNextNode(_graph.GetNextNode(train.LastNode, target, out var path));
            train.SetCurrentPath(path);
    }
}
}