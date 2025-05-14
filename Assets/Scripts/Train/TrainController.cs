using System;
using System.Collections.Generic;
using ResourceRailNetwork.Graph;
using UnityEngine;
using Zenject;

namespace ResourceRailNetwork.Train
{
    public class TrainController : MonoBehaviour
    {
        [SerializeField] private TrainConfig[] trainConfigs;

        [Inject] private IRailNetworkGraph _graph;

        private TrainModel[] _trains;
        private Route[] _routes;
        private readonly TrainSpawner _trainSpawner = new();

        private void Start()
        {
            _trains = _trainSpawner.SpawnTrains(trainConfigs, transform, _graph);
            
            CreateRoutes();
            RefreshBestRoutes();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            
            foreach (var train in _trains)
            {
                switch (train.State)
                {
                    case TrainModel.TrainState.Moving:
                        MovingProcessing(train, deltaTime);
                        break;
                    
                    case TrainModel.TrainState.Mining:
                        MiningProcessing(train, deltaTime);
                        break;
                    
                    case TrainModel.TrainState.Delivering:
                        DeliveringProcessing(train, deltaTime);
                        break;
                }
                
                RefreshDebugInfo(train);
            }
            
            
        }

        private void MovingProcessing(TrainModel train, float deltaTime)
        {
            float dist = deltaTime * train.Speed;
            float diff = dist / _graph.GetDistance(train.LastNode, train.NextNode);
            train.IncrementProgress(diff);
            
            if (train.Progress >= 1)
            {
                train.ResetProgress();
                train.SetLastNode(train.NextNode);
                
                if (train.LastNode == train.Route.Mine)
                {
                    // start mining
                    train.SetState(TrainModel.TrainState.Mining);
                    train.ResetMiningTimer();
                    train.SetLastMine(train.Route.Mine);
                }
                else
                {
                    train.SetNextNode(_graph.GetNextNode(train.LastNode, train.Route.Mine));
                }
            }
            
            train.RefreshViewPos();
        }
        
        private void MiningProcessing(TrainModel train, float deltaTime)
        {
            float diff = deltaTime * train.LastMine.TimeMult;
            train.IncrementMiningTimer(diff);

            if (train.MiningTimer < train.MiningDuration) return;
            
            // get cargo, start delivering
            train.SetCargo(true);
            train.SetState(TrainModel.TrainState.Delivering);
            train.ResetProgress();
            train.SetNextNode(_graph.GetNextNode(train.LastNode, train.Route.BaseStation));
            train.SetLastMine(null);
        }
        
        private void DeliveringProcessing(TrainModel train, float deltaTime)
        {
            float dist = deltaTime * train.Speed;
            float diff = dist / _graph.GetDistance(train.LastNode, train.NextNode);
            train.IncrementProgress(diff);
            
            if (train.Progress >= 1)
            {
                train.ResetProgress();
                train.SetLastNode(train.NextNode);
                
                if (train.LastNode == train.Route.BaseStation)
                {
                    // cargo delivered, start moving to mine
                    train.SetState(TrainModel.TrainState.Moving);
                    train.SetCargo(false);
                    train.ResetProgress();
                }
                else
                {
                    train.SetNextNode(_graph.GetNextNode(train.LastNode, train.Route.BaseStation));
                }
            }
            
            train.RefreshViewPos();
        }
        
        private void RefreshDebugInfo(TrainModel train)
        {
            #if UNITY_EDITOR

            string str = train.State == TrainModel.TrainState.Mining
                ? $"{train.State}, Timer: {train.MiningTimer} / {train.MiningDuration}"
                : $"{train.State}, Progress: {train.Progress}.";
            
            train.DebugInfo.SetText(str);
                
            #endif
        }

        private void CreateRoutes()
        {
            List<BaseStation> baseStations = _graph.GetAllBaseStations();
            List<Mine> mines = _graph.GetAllMines();

            _routes = new Route[baseStations.Count * mines.Count];
            int idx = 0;

            foreach (var baseStation in baseStations)
            {
                foreach (var mine in mines)
                {
                    _routes[idx].BaseStation = baseStation;
                    _routes[idx].Mine = mine;
                    _routes[idx].Distance = _graph.GetDistance(baseStation, mine);
                    idx++;
                }
            }
        }

        private void RefreshBestRoutes()
        {
            foreach (var train in _trains)
            {
                FindBestRoute(train);
            }
        }

        private void FindBestRoute(TrainModel train)
        {
            Route bestRoute = _routes[0];
            float bestProfitPerSecond = float.MinValue;

            foreach (var route in _routes)
            {
                float profitPerSecond = CalculateProfitPerSecond(route, train);
                
                if (profitPerSecond <= bestProfitPerSecond) continue;

                bestProfitPerSecond = profitPerSecond;
                bestRoute = route;
            }
            
            train.SetRoute(bestRoute);

            if (train.State == TrainModel.TrainState.Moving)
            {
                RefreshNextNode(train, train.Route.Mine);
            }
            else if (train.State == TrainModel.TrainState.Delivering)
            {
                RefreshNextNode(train, train.Route.BaseStation);
            }
        }

        private void RefreshNextNode(TrainModel train, Node target)
        {
            if (train.NextNode == null)
            {
                // train is in the last node
                train.SetNextNode(_graph.GetNextNode(train.LastNode, target));
                return;
            }
            
            // train is between nodes

            float distBetweenLastAndNext = _graph.GetDistance(train.LastNode, train.NextNode);

            float distFromLastNodeToMine = _graph.GetDistance(train.LastNode, target);
            distFromLastNodeToMine += distBetweenLastAndNext * train.Progress;

            float distFromNextNodeToMine = _graph.GetDistance(train.NextNode, target);
            distFromNextNodeToMine += distBetweenLastAndNext * (1 - train.Progress);

            if (distFromLastNodeToMine < distFromNextNodeToMine)
            {
                // change direction
                Node temp = train.NextNode;
                train.SetNextNode(train.LastNode);
                train.SetLastNode(temp);
                train.SetProgress(1 - train.Progress);
            }
        }

        private float CalculateProfitPerSecond(Route route, TrainModel train)
        {
            float duration = route.Distance / train.Speed * 2 + train.MiningDuration / route.Mine.TimeMult;
            float profit = route.BaseStation.ResourceMult;

            return profit / duration;
        }
    }
}