using ResourceRailNetwork.Graph;
using UnityEngine;

namespace ResourceRailNetwork.Train
{
    public class TrainModel
    {
        public TrainSettings TrainSettings { get; }
        public TrainDebugInfo DebugInfo { get; }
        public Node LastNode { get; private set; }
        public Mine LastMine { get; private set; }
        public Node NextNode { get; private set; }
        public float Progress { get; private set; }
        public TrainState State { get; private set; }
        public Route Route { get; private set; }
        public bool HasCargo { get; private set; }
        public float MiningTimer { get; private set; }
        private GameObject View { get; }
        public float Speed => TrainSettings.Speed;
        public float MiningDuration => TrainSettings.MiningTime;

        public enum TrainState
        {
            Moving,
            Mining,
            Delivering
        }

        public TrainModel(TrainSettings trainSettings, TrainDebugInfo debugInfo, GameObject trainView, Node startNode)
        {
            TrainSettings = trainSettings;
            DebugInfo = debugInfo;
            View = trainView;
            LastNode = startNode;
            State = TrainState.Moving;
            Progress = 0;
            HasCargo = false;
        }

        private void OnSettingsChanged()
        {
            
        }

        public void SetRoute(Route route)
        {
            Route = route;
        }

        public void SetNextNode(Node node)
        {
            NextNode = node;
        }
        
        public void SetLastNode(Node node)
        {
            LastNode = node;
        }
        
        public void ResetProgress()
        {
            Progress = 0;
        }
        
        public void IncrementProgress(float deltaTime)
        {
            Progress += deltaTime;
        }
        
        public void SetProgress(float progress)
        {
            Progress = progress;
        }
        
        public void ResetMiningTimer()
        {
            MiningTimer = 0;
        }
        
        public void IncrementMiningTimer(float deltaTime)
        {
            MiningTimer += deltaTime;
        }
        
        public void SetCargo(bool value)
        {
            HasCargo = value;
        }
        
        public void SetLastMine(Mine mine)
        {
            LastMine = mine;
        }

        public void RefreshViewPos()
        {
            if (NextNode == null)
            {
                View.transform.position = LastNode.Position;
                
                return;
            }
            
            Vector3 pos  = Vector3.Lerp(LastNode.Position, NextNode.Position, Progress);
            View.transform.position = pos;
        }

        public void SetState(TrainState state)
        {
            State = state;
        }
    }
}