using ResourceRailNetwork.Core;
using ResourceRailNetwork.Graph;

namespace ResourceRailNetwork.Train
{
    /// <summary>
    /// Manages dynamic path recalculations and mid-journey rerouting
    /// Optimizes train paths when network changes occur during movement
    /// </summary>
    public class TrainPathUpdater
    {
        private readonly IRailNetworkGraph _graph;

        public TrainPathUpdater(IRailNetworkGraph graph)
        {
            _graph = graph;
        }

        public void RefreshNextNode(TrainModel train, Node target)
        {
            if (train.NextNode == null)
            {
                HandleNullNextNode(train, target);
                return;
            }

            HandleMidJourneyReroute(train, target);
        }

        private void HandleNullNextNode(TrainModel train, Node target)
        {
            if (train.LastNode == target)
            {
                return;
            }

            train.SetNextNode(_graph.GetNextNode(train.LastNode, target, out var path));
            train.SetCurrentPath(path);
        }
        
        /// <summary>
        /// Decides whether to continue current path or reverse direction
        /// Compares remaining distance through both possible paths
        /// </summary>
        private void HandleMidJourneyReroute(TrainModel train, Node target)
        {
            float distBetweenNodes = _graph.GetDistance(train.LastNode, train.NextNode);
            float currentProgress = train.Progress;

            float distanceFromLast = _graph.GetDistance(train.LastNode, target) + distBetweenNodes * currentProgress;
            float distanceFromNext =
                _graph.GetDistance(train.NextNode, target) + distBetweenNodes * (1 - currentProgress);

            if (distanceFromLast < distanceFromNext)
            {
                ReverseTrainDirection(train);
            }
        }

        private void ReverseTrainDirection(TrainModel train)
        {
            Node temp = train.NextNode;
            train.SetNextNode(train.LastNode);
            train.SetLastNode(temp);
            train.SetProgress(1 - train.Progress);
        }
    }
}